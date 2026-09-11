using BrightProgramming.AiSupportAssistant.Api.Knowledge;
using BrightProgramming.AiSupportAssistant.Api.Knowledge.Factory;
using BrightProgramming.AiSupportAssistant.Api.Knowledge.Providers;
using BrightProgramming.AiSupportAssistant.Api.Knowledge.Providers.Markdown;

namespace BrightProgramming.AiSupportAssistant.Api.StartupConfiguration;

public static class KnowledgeConfiguration
{
    public static IServiceCollection AddKnowledge(
        this IServiceCollection services)
    {
        services.AddSingleton<IKnowledgeProvider, MarkdownKnowledgeProvider>();
        services.AddSingleton<IKnowledgeProviderFactory, KnowledgeProviderFactory>();
        
        services.AddSingleton<IKnowledgeMatcher, KnowledgeMatcher>();

        return services;
    }
}