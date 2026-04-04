using FastEndpoints;
using FluorineFx.IO;
using FluorineFx;
using System.Collections.Frozen;
using System.Reflection;
using Humanizer;
using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Services.QuestService;
using Microsoft.AspNetCore.Identity;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Features.Gateway.Endpoint;

internal sealed class GatewayService(UserManager<ApplicationUser> userManager, IServiceProvider serviceProvider, ILogger<GatewayService> logger, CityVilleDbContext context) : EndpointWithoutRequest
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
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var user = await userManager.GetUserAsync(User);

        if (user is null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        var pUser = await context.Set<Player>()
            .AsNoTracking()
            .Where(x => x.AppUser.Id == user.Id)
            .Select(x => x.Id)
            .FirstOrDefaultAsync(ct);

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

            if (content is null)
            {
                logger.LogWarning("Received empty content in request.");
                return;
            }

            object[]? parsedAmfContent = content[1] as object[];

            if (parsedAmfContent is null || parsedAmfContent.Length == 0)
            {
                logger.LogWarning("Received empty AMF content in request.");
                return;
            }

            var responses = new List<ASObject>();

            ASObject? errorResponse = null;

            foreach (ASObject item in parsedAmfContent)
            {
                var parameters = item["params"] as object[];
                var functionName = item["functionName"] as string;
                var sequence = item["sequence"];

                if (parameters is null || functionName is null || sequence is null)
                {
                    logger.LogWarning("Received incomplete request item: {Item}", item);
                    continue;
                }

                logger.LogDebug("Received request for function {FunctionName} sequence {Sequence}", functionName, sequence);

                var packageName = functionName.Split('.')[0];
                var className = functionName.Split('.')[1];
                var upperClassName = className.Pascalize();

                if (QuestSettingsManager.TaskActions.Contains(className))
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

                    logger.LogDebug("Parameters {Objects}", (object?)parameters);
                }

                if (packageName == "GameMechanicService" && upperClassName == "PerformMechanicAction")
                {
                    var mechanicType = (string)parameters[1];

                    upperClassName = mechanicType.Pascalize();

                    logger.LogDebug("Parameters {Objects}", (object?)parameters);
                }

                ASObject? response = null;

                try
                {
                    response = await InvokeHandlePacketAsync($"CityVilleDotnet.Api.Services.{packageName}.{upperClassName}", parameters, pUser, ct);

                    if (response is null)
                    {
                        logger.LogError("Something went wrong while processing the request. {PackageName}.{UpperClassName}", packageName, upperClassName);
                        logger.LogDebug("Parameters {Objects}", (object?)parameters);

                        response = CreateEmptyResponse();
                    }
                }
                catch (ValidationException ve)
                {
                    var errors = string.Join("; ", ve.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}"));
                    logger.LogWarning("Validation failed for {FunctionName}: {Errors}", functionName, errors);

                    response = new CityVilleResponse().Error(GameErrorType.InvalidData).ErrorMessage(errors).ToObject();

                    errorResponse = response;
                }
                catch (Exception e)
                {
                    logger.LogError(e, "Error processing request for function {FunctionName} with params {@Params}", functionName, parameters);

                    response = new CityVilleResponse().Error(GameErrorType.InvalidData).ErrorMessage(e.Message).ToObject();

                    errorResponse = response;
                }

                responses.Add(response);
            }

            var emsg = new CityVilleResponse().Data(responses).ToObject();

            if (errorResponse is not null)
            {
                emsg = errorResponse;
            }

            var responseMessage = new AMFMessage(3);
            responseMessage.AddBody(new AMFBody(responseUri, targetUri, emsg));

            using var outputStream = new MemoryStream();
            var serializer = new AMFSerializer(outputStream);
            serializer.WriteMessage(responseMessage);

            HttpContext.Response.ContentType = "application/x-amf";
            await HttpContext.Response.Body.WriteAsync(outputStream.ToArray(), ct);
        }
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