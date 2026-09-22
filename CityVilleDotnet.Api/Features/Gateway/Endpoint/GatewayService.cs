using FastEndpoints;
using FluorineFx.IO;
using FluorineFx;
using System.Collections.Frozen;
using System.Reflection;
using Humanizer;
using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Common.Identity;
using CityVilleDotnet.Api.Services.QuestService;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Exceptions;
using CityVilleDotnet.Common.Settings;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace CityVilleDotnet.Api.Features.Gateway.Endpoint;

internal sealed class GatewayService(IServiceProvider serviceProvider, ILogger<GatewayService> logger) : EndpointWithoutRequest
{
    private static FrozenDictionary<string, Type> _handlerTypes = FrozenDictionary<string, Type>.Empty;

    public static void InitializeHandlers(Assembly assembly)
    {
        _handlerTypes = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(AmfService)))
            .Where(t => t.FullName is not null)
            .ToFrozenDictionary(t => t.FullName!);
    }

    public override void Configure()
    {
        Post("/flashservices/gateway.php");
        Options(x => x.WithMetadata(new RequestSizeLimitAttribute(1 * 1024 * 1024)));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        if (User.GetPlayerId() is not { } playerId)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        using var ms = new MemoryStream();
        await HttpContext.Request.Body.CopyToAsync(ms, ct);
        ms.Position = 0;

        var deserializer = new AMFDeserializer(ms);
        var requestMessage = deserializer.ReadAMFMessage();

        if (requestMessage.Bodies.Count > 0)
        {
            var requestBody = requestMessage.Bodies[0];
            var responseUri = $"{requestBody.Response}/onResult";
            var targetUri = "null";

            logger.LogDebug("Response URI: {ResponseUri}, Target URI: {TargetUri}", responseUri, targetUri);

            var content = requestBody.Content as object[];

            if (content is null || content.Length < 2)
            {
                logger.LogWarning("Received empty content in request.");
                await WriteAmfResponseAsync(responseUri, targetUri, new CityVilleResponse().Error(GameErrorType.MissingData).ToObject(), ct);
                return;
            }

            object[]? parsedAmfContent = content[1] as object[];

            if (parsedAmfContent is null || parsedAmfContent.Length == 0)
            {
                logger.LogWarning("Received empty AMF content in request.");
                await WriteAmfResponseAsync(responseUri, targetUri, new CityVilleResponse().Error(GameErrorType.MissingData).ToObject(), ct);
                return;
            }

            var responses = new List<ASObject>();

            foreach (ASObject item in parsedAmfContent)
            {
                var parameters = item["params"] as object[];
                var functionName = item["functionName"] as string;
                var sequence = item["sequence"];

                if (parameters is null || functionName is null || sequence is null)
                {
                    logger.LogWarning("Received incomplete request item: {Item}", item);
                    responses.Add(new CityVilleResponse().Error(GameErrorType.MissingData).ToObject());
                    continue;
                }

                logger.LogInformation("Received request from {player} for {FunctionName} sequence {Sequence} parameters {parameters}", playerId, functionName, sequence, parameters);

                ASObject? response;

                try
                {
                    var nameParts = functionName.Split('.');
                    var packageName = nameParts[0];
                    var className = nameParts[1];
                    var upperClassName = className.Pascalize();

                    if (!_handlerTypes.ContainsKey($"CityVilleDotnet.Api.Services.{packageName}.{upperClassName}") && QuestSettingsManager.TaskActions.Contains(className))
                    {
                        logger.LogDebug("Handling task quest action {ClassName}", className);

                        var taskParams = new object[] { className };

                        parameters = taskParams.Append(parameters).ToArray();

                        packageName = "QuestService";
                        upperClassName = nameof(HandleQuestProgress);
                    }

                    if (packageName == "WorldService" && upperClassName == "PerformAction")
                    {
                        var actionType = (string)parameters[0];

                        upperClassName = actionType.Pascalize();
                    }

                    if (packageName == "GameMechanicService" && upperClassName == "PerformMechanicAction")
                    {
                        var mechanicType = (string)parameters[1];

                        upperClassName = mechanicType.Pascalize();
                    }

                    response = await InvokeHandlePacketAsync($"CityVilleDotnet.Api.Services.{packageName}.{upperClassName}", parameters, playerId, ct);

                    if (response is null)
                    {
                        logger.LogError("Something went wrong while processing the request. {PackageName}.{UpperClassName}", packageName, upperClassName);
                        logger.LogDebug("Parameters {Objects}", (object?)parameters);

                        response = CreateEmptyResponse();
                    }
                }
                catch (DomainException de)
                {
                    logger.LogWarning("Domain exception for {FunctionName}: {Errors}", functionName, de.Reason);

                    response = new CityVilleResponse().Error(de.Reason).ToObject();
                }
                catch (ValidationException ve)
                {
                    var errors = string.Join("; ", ve.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}"));
                    logger.LogWarning("Validation failed for {FunctionName}: {Errors}", functionName, errors);

                    response = new CityVilleResponse().Error(GameErrorType.InvalidData).ErrorMessage(errors).ToObject();
                }
                catch (Exception e)
                {
                    logger.LogError(e, "Error processing request for function {FunctionName} with params {@Params}", functionName, parameters);

                    response = new CityVilleResponse().Error(GameErrorType.InvalidData).ToObject();
                }

                responses.Add(response);
            }

            var emsg = new CityVilleResponse().Data(responses).ToObject();

            await WriteAmfResponseAsync(responseUri, targetUri, emsg, ct);
        }
    }

    private async Task WriteAmfResponseAsync(string responseUri, string targetUri, ASObject emsg, CancellationToken ct)
    {
        var responseMessage = new AMFMessage(3);
        responseMessage.AddBody(new AMFBody(responseUri, targetUri, emsg));

        using var outputStream = new MemoryStream();
        var serializer = new AMFSerializer(outputStream);
        serializer.WriteMessage(responseMessage);

        HttpContext.Response.ContentType = "application/x-amf";
        await HttpContext.Response.Body.WriteAsync(outputStream.ToArray(), ct);
    }

    private async Task<ASObject?> InvokeHandlePacketAsync(string className, object parameter, Guid userId, CancellationToken cancellationToken)
    {
        if (!_handlerTypes.TryGetValue(className, out var classType))
            return null;

        var instance = (AmfService)serviceProvider.GetRequiredService(classType);
        instance.ServiceProvider = serviceProvider;

        return await instance.HandlePacket((object[])parameter, userId, cancellationToken);
    }

    public static ASObject CreateEmptyResponse()
    {
        return new CityVilleResponse();
    }
}