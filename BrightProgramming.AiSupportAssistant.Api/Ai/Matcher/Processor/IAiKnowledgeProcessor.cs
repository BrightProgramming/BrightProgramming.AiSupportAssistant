using BrightProgramming.AiSupportAssistant.Api.Knowledge.Models;

namespace BrightProgramming.AiSupportAssistant.Api.Ai.Matcher.Processor;

public interface IAiKnowledgeProcessor
{
    Task<IReadOnlyCollection<KnowledgeDocument>> MatchAsync(
        string question,
        IReadOnlyCollection<KnowledgeDocument> documents,
        CancellationToken cancellationToken);
}