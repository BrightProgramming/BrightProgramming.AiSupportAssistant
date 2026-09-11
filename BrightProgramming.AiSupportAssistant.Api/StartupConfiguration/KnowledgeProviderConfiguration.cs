using BrightProgramming.AiSupportAssistant.Api.Knowledge.Factory;
using BrightProgramming.AiSupportAssistant.Api.Knowledge.Providers;
using BrightProgramming.AiSupportAssistant.Api.Knowledge.Providers.Markdown;

namespace BrightProgramming.AiSupportAssistant.Api.StartupConfiguration;

public static class KnowledgeProviderConfiguration
{
    public static IServiceCollection AddKnowledgeProviders(
        this IServiceCollection services)
    {
        services.AddSingleton<IKnowledgeProvider, MarkdownKnowledgeProvider>();
        services.AddSingleton<IKnowledgeProviderFactory, KnowledgeProviderFactory>();

        return services;
    }
}