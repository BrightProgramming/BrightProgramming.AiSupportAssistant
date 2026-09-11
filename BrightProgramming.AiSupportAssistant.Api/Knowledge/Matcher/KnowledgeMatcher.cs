using BrightProgramming.AiSupportAssistant.Api.Ai.Matcher.Processor;
using BrightProgramming.AiSupportAssistant.Api.Knowledge.Models;

namespace BrightProgramming.AiSupportAssistant.Api.Knowledge;

public class KnowledgeMatcher : IKnowledgeMatcher
{
    private readonly IAiKnowledgeProcessor _aiKnowledgeProcessor;

    public KnowledgeMatcher(
        IAiKnowledgeProcessor aiKnowledgeProcessor)
    {
        _aiKnowledgeProcessor = aiKnowledgeProcessor;
    }

    public async Task<KnowledgeContext> MatchAsync(
        string question,
        KnowledgeContext knowledge,
        CancellationToken cancellationToken)
    {
        var documents = await _aiKnowledgeProcessor.MatchAsync(
            question,
            knowledge.Documents,
            cancellationToken);

        return new KnowledgeContext
        {
            Documents = documents
        };
    }
}