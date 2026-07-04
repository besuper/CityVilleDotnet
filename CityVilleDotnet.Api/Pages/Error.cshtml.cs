using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CityVilleDotnet.Api.Pages;

[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
public class ErrorModel(IWebHostEnvironment environment) : PageModel
{
    public bool ShowExceptionDetails => environment.IsDevelopment();

    public string? ExceptionMessage { get; private set; }

    public string? ExceptionStackTrace { get; private set; }

    public string? ExceptionPath { get; private set; }

    public void OnGet()
    {
        var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

        if (exceptionFeature is null) return;

        ExceptionPath = exceptionFeature.Path;
        ExceptionMessage = exceptionFeature.Error.Message;
        ExceptionStackTrace = exceptionFeature.Error.ToString();
    }
}
