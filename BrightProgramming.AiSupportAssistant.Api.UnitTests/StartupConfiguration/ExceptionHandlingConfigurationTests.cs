using BrightProgramming.AiSupportAssistant.Api.Ai.Exceptions;
using BrightProgramming.AiSupportAssistant.Api.StartupConfiguration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BrightProgramming.AiSupportAssistant.Api.UnitTests.StartupConfiguration;

public class ExceptionHandlingConfigurationTests
{
    [Fact]
    public void AddExceptionHandling_WhenAiProviderException_Returns503StatusCode()
    {
        var options = CreateExceptionHandlerOptions();

        var statusCode = options.StatusCodeSelector!(
            new AiProviderException("provider failed"));

        Assert.Equal(
            StatusCodes.Status503ServiceUnavailable,
            statusCode);
    }

    [Fact]
    public void AddExceptionHandling_WhenOtherException_Returns500StatusCode()
    {
        var options = CreateExceptionHandlerOptions();

        var statusCode = options.StatusCodeSelector!(
            new InvalidOperationException("failure"));

        Assert.Equal(
            StatusCodes.Status500InternalServerError,
            statusCode);
    }

    [Fact]
    public void AddExceptionHandling_WhenAiProviderException_CustomizesProblemDetails()
    {
        var options = CreateProblemDetailsOptions();
        var context = CreateProblemDetailsContext(
            new AiProviderException("provider failed"));

        options.CustomizeProblemDetails!(context);

        Assert.Equal("AI provider unavailable", context.ProblemDetails.Title);
        Assert.Equal(
            "The AI service is currently unavailable.",
            context.ProblemDetails.Detail);
    }

    [Fact]
    public void AddExceptionHandling_WhenOtherException_LeavesDefaultProblemDetailsUnchanged()
    {
        var options = CreateProblemDetailsOptions();

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Internal Server Error",
            Detail = "default detail"
        };

        var context = CreateProblemDetailsContext(
            new InvalidOperationException("failure"),
            problemDetails);

        options.CustomizeProblemDetails!(context);

        Assert.Equal(
            StatusCodes.Status500InternalServerError,
            context.ProblemDetails.Status);

        Assert.Equal(
            "Internal Server Error",
            context.ProblemDetails.Title);

        Assert.Equal(
            "default detail",
            context.ProblemDetails.Detail);
    }

    private static ProblemDetailsOptions CreateProblemDetailsOptions()
    {
        var services = new ServiceCollection();

        services.AddExceptionHandling();

        using var provider = services.BuildServiceProvider();

        return provider
            .GetRequiredService<IOptions<ProblemDetailsOptions>>()
            .Value;
    }

    private static ExceptionHandlerOptions CreateExceptionHandlerOptions()
    {
        var services = new ServiceCollection();

        services.AddExceptionHandling();

        using var provider = services.BuildServiceProvider();

        return provider
            .GetRequiredService<IOptions<ExceptionHandlerOptions>>()
            .Value;
    }

    private static ProblemDetailsContext CreateProblemDetailsContext(
        Exception exception,
        ProblemDetails? problemDetails = null)
    {
        var httpContext = new DefaultHttpContext();

        httpContext.Features.Set<IExceptionHandlerFeature>(
            new ExceptionHandlerFeature
            {
                Error = exception
            });

        return new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = problemDetails ?? new ProblemDetails()
        };
    }
}