using BrightProgramming.AiSupportAssistant.Api.Ai.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace BrightProgramming.AiSupportAssistant.Api.StartupConfiguration;

public static class ExceptionHandlingConfiguration
{
    public static IServiceCollection AddExceptionHandling(
        this IServiceCollection services)
    {
        services.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = context =>
            {
                var exception = context.HttpContext
                    .Features
                    .Get<IExceptionHandlerFeature>()
                    ?.Error;

                if (exception is AiProviderException)
                {
                    context.ProblemDetails.Title = "AI provider unavailable";
                    context.ProblemDetails.Detail =
                        "The AI service is currently unavailable.";
                }
            };
        });

        services.Configure<ExceptionHandlerOptions>(options =>
        {
            options.StatusCodeSelector = exception =>
                exception is AiProviderException
                    ? StatusCodes.Status503ServiceUnavailable
                    : StatusCodes.Status500InternalServerError;
        });

        return services;
    }
}