using BrightProgramming.AiSupportAssistant.Api.Ai.Matcher.Factory;
using BrightProgramming.AiSupportAssistant.Api.Knowledge.Models;

namespace BrightProgramming.AiSupportAssistant.Api.Ai.Matcher.Processor;

public class AiKnowledgeProcessor : IAiKnowledgeProcessor
{
    private readonly IAiKnowledgeMatcherFactory _aiKnowledgeMatcherFactory;

    public AiKnowledgeProcessor(
        IAiKnowledgeMatcherFactory aiKnowledgeMatcherFactory)
    {
        _aiKnowledgeMatcherFactory = aiKnowledgeMatcherFactory;
    }

    public Task<IReadOnlyCollection<KnowledgeDocument>> MatchAsync(
        string question,
        IReadOnlyCollection<KnowledgeDocument> documents,
        CancellationToken cancellationToken)
    {
        var provider = _aiKnowledgeMatcherFactory.GetProvider();

        return provider.MatchAsync(
            question,
            documents,
            cancellationToken);
    }
}