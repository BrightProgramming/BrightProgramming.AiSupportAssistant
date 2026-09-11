using BrightProgramming.AiSupportAssistant.Api.Ai.Matcher.Providers;

namespace BrightProgramming.AiSupportAssistant.Api.Ai.Matcher.Factory;

public interface IAiKnowledgeMatcherFactory
{
    IAiKnowledgeMatcherProvider GetProvider();
}