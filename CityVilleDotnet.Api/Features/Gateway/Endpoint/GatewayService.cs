using FastEndpoints;
using FluorineFx.IO;
using FluorineFx;
using System.Collections;
using System.Reflection;
using Humanizer;
using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Common.Domain;
using Microsoft.AspNetCore.Identity;

namespace CityVilleDotnet.Api.Features.Gateway.Endpoint;

internal sealed class GatewayService(UserManager<ApplicationUser> _userManager, IServiceProvider _serviceProvider) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Post("/flashservices/gateway.php");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var user = await _userManager.GetUserAsync(User);

        if (user is null)
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        using var ms = new MemoryStream();
        await HttpContext.Request.Body.CopyToAsync(ms);
        ms.Position = 0;

        AMFDeserializer deserializer = new AMFDeserializer(ms);
        AMFMessage requestMessage = deserializer.ReadAMFMessage();

        if (requestMessage.Bodies.Count > 0)
        {
            AMFBody requestBody = requestMessage.Bodies[0];
            string responseURI = $"{requestBody.Response}/onResult";
            string targetURI = "null";

            Console.WriteLine($"Response URI: {responseURI}, Target URI: {targetURI}");

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

                Console.WriteLine($"Received request for function {functionName} sequence {sequence}");

                var className = functionName.Split('.')[1].Pascalize();
                var response = await InvokeHandlePacketAsync($"CityVilleDotnet.Api.Services.{functionName}.{className}", "HandlePacket", _params, Guid.Parse(user.Id));

                if (response is null)
                {
                    Console.WriteLine("Something went wrong while processing the request.");

                    response = CreateEmptyResponse();
                }

                responses.Add(response);
            }

            ASObject emsg = new ASObject();
            emsg["serverTime"] = DateTime.Now.ToUniversalTime().Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            emsg["errorType"] = 0;
            emsg["data"] = responses;

            AMFMessage responseMessage = new AMFMessage(0);
            responseMessage.AddBody(new AMFBody(responseURI, targetURI, emsg));

            using var outputStream = new MemoryStream();
            AMFSerializer serializer = new AMFSerializer(outputStream);
            serializer.WriteMessage(responseMessage);

            HttpContext.Response.ContentType = "application/x-amf";
            await HttpContext.Response.Body.WriteAsync(outputStream.ToArray(), ct);
        }
    }

    public async Task<ASObject> InvokeHandlePacketAsync(string className, string methodName, object parameter, Guid userId)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var classType = assembly.GetTypes()
            .FirstOrDefault(t => t.FullName == className);

        if (classType == null)
        {
            return null;
        }

        var instance = ActivatorUtilities.CreateInstance(_serviceProvider, classType);

        var method = classType.GetMethod(methodName,
            BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

        if (method == null)
        {
            throw new Exception("Method not found.");
        }

        if (method.ReturnType != typeof(Task<ASObject>))
        {
            throw new Exception("The method does not match the expected return type.");
        }

        return await (Task<ASObject>)method.Invoke(instance, new object[] { parameter, userId });
    }

    public static ASObject CreateEmptyResponse()
    {
        return new CityVilleResponse(0, 333);
    }
}
