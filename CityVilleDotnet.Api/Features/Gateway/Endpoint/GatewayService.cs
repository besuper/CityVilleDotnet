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
            await SendUnauthorizedAsync(ct);
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

            logger.LogDebug($"Response URI: {responseUri}, Target URI: {targetUri}");

            var content = requestBody.Content as object[];

            var header = content[0] as ASObject;
            var _content = content[1] as object[];

            var responses = new ArrayList();
            var _uid = header["zyUid"];

            if (_uid is null)
            {
                await SendUnauthorizedAsync(ct);
                return;
            }

            var uid = int.Parse((string)_uid);

            foreach (ASObject item in _content)
            {
                var _params = item["params"] as object[];
                var functionName = item["functionName"] as string;
                var sequence = item["sequence"];

                logger.LogDebug($"Received request for function {functionName} sequence {sequence}");

                var packageName = functionName.Split('.')[0];
                var _className = functionName.Split('.')[1];
                var className = _className.Pascalize();

                if (QuestSettingsManager.TaskActions.Contains(_className))
                {
                    logger.LogDebug($"Handling task quest action {_className}");

                    var taskParams = new object[] { _className };
                    taskParams.Append(_params);

                    _params = taskParams;

                    packageName = "QuestService";
                    className = "HandleQuestProgress";
                }

                var response = await InvokeHandlePacketAsync($"CityVilleDotnet.Api.Services.{packageName}.{className}", "HandlePacket", _params, Guid.Parse(user.Id), ct);

                if (response is null)
                {
                    logger.LogDebug("Something went wrong while processing the request.");

                    response = CreateEmptyResponse();
                }

                responses.Add(response);
            }

            var emsg = new ASObject
            {
                ["serverTime"] = DateTime.Now.ToUniversalTime().Subtract(new DateTime(1970, 1, 1)).TotalSeconds,
                ["errorType"] = 0,
                ["data"] = responses
            };

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
        return new CityVilleResponse(0, 333);
    }
}