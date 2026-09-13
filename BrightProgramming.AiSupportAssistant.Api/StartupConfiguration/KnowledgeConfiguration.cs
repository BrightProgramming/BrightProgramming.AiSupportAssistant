using BrightProgramming.AiSupportAssistant.Api.Knowledge.Factory;
using BrightProgramming.AiSupportAssistant.Api.Knowledge.Providers;
using BrightProgramming.AiSupportAssistant.Api.Knowledge.Providers.Markdown;
using BrightProgramming.AiSupportAssistant.Api.Knowledge.Processor;
using BrightProgramming.AiSupportAssistant.Api.Knowledge.Repository;
using BrightProgramming.AiSupportAssistant.Api.Knowledge.Repository.Markdown;

namespace BrightProgramming.AiSupportAssistant.Api.StartupConfiguration;

public static class KnowledgeConfiguration
{
    public static IServiceCollection AddKnowledge(
        this IServiceCollection services)
    {
        services.AddSingleton<IKnowledgeRepository, MarkdownKnowledgeRepository>();

        services.AddSingleton<IKnowledgeProvider, MarkdownKnowledgeProvider>();
        services.AddSingleton<IKnowledgeProviderFactory, KnowledgeProviderFactory>();

        services.AddSingleton<IKnowledgeProcessor, KnowledgeProcessor>();

        return services;
    }
}