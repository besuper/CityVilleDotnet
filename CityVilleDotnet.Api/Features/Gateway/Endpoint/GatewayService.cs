using FastEndpoints;
using FluorineFx.IO;
using FluorineFx;
using System.Collections;
using System.Reflection;
using Humanizer;
using CityVilleDotnet.Api.Common.Amf;
using Microsoft.AspNetCore.Identity;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Domain.Enums;

namespace CityVilleDotnet.Api.Features.Gateway.Endpoint;

internal sealed class GatewayService(UserManager<ApplicationUser> userManager, IServiceProvider serviceProvider, ILogger<GatewayService> logger) : EndpointWithoutRequest
{
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

            var header = content[0] as ASObject;
            var amfContent = content[1] as object[];

            if (amfContent is null)
            {
                logger.LogWarning("Received empty AMF content in request.");
                return;
            }

            logger.LogDebug("Request header received: {Header}", header);

            var responses = new ArrayList();
            var objectUid = header?["zyUid"];

            if (objectUid is null)
            {
                await Send.UnauthorizedAsync(ct);
                return;
            }

            var uid = int.Parse((string)objectUid);

            ASObject? errorResponse = null;

            foreach (ASObject item in amfContent)
            {
                var @params = item["params"] as object[];
                var functionName = item["functionName"] as string;
                var sequence = item["sequence"];

                if (@params is null || functionName is null || sequence is null)
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

                    @params = taskParams.Append(@params).ToArray();

                    packageName = "QuestService";
                    upperClassName = "HandleQuestProgress";
                }

                ASObject? response = null;

                try
                {
                    response = await InvokeHandlePacketAsync($"CityVilleDotnet.Api.Services.{packageName}.{upperClassName}", "HandlePacket", @params, Guid.Parse(user.Id), ct);

                    if (response is null)
                    {
                        logger.LogDebug("Something went wrong while processing the request.");

                        response = CreateEmptyResponse();
                    }
                }
                catch (Exception e)
                {
                    logger.LogError(e, "Error processing request for function {FunctionName} with params {@Params}", functionName, @params);

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

            var responseMessage = new AMFMessage(0);
            responseMessage.AddBody(new AMFBody(responseUri, targetUri, emsg));

            using var outputStream = new MemoryStream();
            var serializer = new AMFSerializer(outputStream);
            serializer.WriteMessage(responseMessage);

            HttpContext.Response.ContentType = "application/x-amf";
            await HttpContext.Response.Body.WriteAsync(outputStream.ToArray(), ct);
        }
    }

    private async Task<ASObject?> InvokeHandlePacketAsync(string className, string methodName, object parameter, Guid userId, CancellationToken cancellationToken)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var classType = assembly.GetTypes()
            .FirstOrDefault(t => t.FullName == className);

        if (classType is null)
            return null;

        var instance = ActivatorUtilities.CreateInstance(serviceProvider, classType);

        var method = classType.GetMethod(methodName,
            BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

        if (method is null)
            throw new Exception("Method not found.");

        if (method.ReturnType != typeof(Task<ASObject>))
            throw new Exception("The method does not match the expected return type.");

        return await (Task<ASObject>)method.Invoke(instance, [parameter, userId, cancellationToken])!;
    }

    public static ASObject CreateEmptyResponse()
    {
        return new CityVilleResponse();
    }
}