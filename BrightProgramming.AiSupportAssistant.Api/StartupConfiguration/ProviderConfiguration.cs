using BrightProgramming.AiSupportAssistant.Api.Ai;
using BrightProgramming.AiSupportAssistant.Api.Ai.Factories;
using BrightProgramming.AiSupportAssistant.Api.Ai.OpenAi;
using BrightProgramming.AiSupportAssistant.Api.Configuration;
using Microsoft.Extensions.Options;
using OpenAI.Chat;

namespace BrightProgramming.AiSupportAssistant.Api.StartupConfiguration;

public static class ProviderConfiguration
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