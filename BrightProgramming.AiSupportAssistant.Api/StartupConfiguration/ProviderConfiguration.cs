using BrightProgramming.AiSupportAssistant.Api.Ai;
using BrightProgramming.AiSupportAssistant.Api.Ai.Factory;
using BrightProgramming.AiSupportAssistant.Api.Ai.OpenAi;

public static class ProviderConfiguration
{
    public static IServiceCollection AddAiProviders(
        this IServiceCollection services)
    {
        services.AddSingleton<IAiProvider, OpenAiProvider>();
        services.AddSingleton<IAiProviderFactory, AiProviderFactory>();

        return services;
    }
}