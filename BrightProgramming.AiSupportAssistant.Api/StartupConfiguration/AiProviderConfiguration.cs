using BrightProgramming.AiSupportAssistant.Api.Ai.Providers.OpenAi;
using BrightProgramming.AiSupportAssistant.Api.Ai.Factory;
using BrightProgramming.AiSupportAssistant.Api.Configuration;
using Microsoft.Extensions.Options;
using OpenAI.Chat;
using BrightProgramming.AiSupportAssistant.Api.Ai.Providers;

namespace BrightProgramming.AiSupportAssistant.Api.StartupConfiguration;

public static class AiProviderConfiguration
{
    public static IServiceCollection AddAiProviders(
        this IServiceCollection services)
    {
        services.AddSingleton<ChatClient>(serviceProvider =>
        {
            var options = serviceProvider
                .GetRequiredService<IOptions<OpenAiOptions>>()
                .Value;

            return new ChatClient(
                options.Model,
                options.ApiKey);
        });

        services.AddSingleton<IAiProvider, OpenAiProvider>();
        services.AddSingleton<IAiProviderFactory, AiProviderFactory>();

        return services;
    }
}