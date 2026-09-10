using BrightProgramming.AiSupportAssistant.Api.Ai;
using BrightProgramming.AiSupportAssistant.Api.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BrightProgramming.AiSupportAssistant.Api.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services)
    {
        services.AddSingleton<ISupportService, SupportService>();
        services.AddSingleton<IAiService, AiService>();

        return services;
    }
}