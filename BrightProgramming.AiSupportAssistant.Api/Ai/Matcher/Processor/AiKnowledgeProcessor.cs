using BrightProgramming.AiSupportAssistant.Api.Ai.Matcher.Factory;
using BrightProgramming.AiSupportAssistant.Api.Ai.Matcher.Providers;
using BrightProgramming.AiSupportAssistant.Api.Knowledge.Models;

namespace BrightProgramming.AiSupportAssistant.Api.Ai.Matcher.Processor;

public class AiKnowledgeProcessor : IAiKnowledgeProcessor
{
    private readonly IAiKnowledgeMatcherProvider _aiKnowledgeMatcherProvider;

    public AiKnowledgeProcessor(
        IAiKnowledgeMatcherFactory aiKnowledgeMatcherFactory)
    {
        _aiKnowledgeMatcherProvider = aiKnowledgeMatcherFactory.GetProvider();
    }

    public Task<KnowledgeContext> MatchAsync(
        string question,
        KnowledgeContext knowledge,
        CancellationToken cancellationToken)
    {
        return _aiKnowledgeMatcherProvider.MatchAsync(
            question,
            knowledge,
            cancellationToken);
    }
}