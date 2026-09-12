using BrightProgramming.AiSupportAssistant.Api.Knowledge.Models;

namespace BrightProgramming.AiSupportAssistant.Api.Ai.Matcher.Processor;

public interface IAiKnowledgeProcessor
{
    Task<KnowledgeContext> MatchAsync(
        string question,
        KnowledgeContext knowledge,
        CancellationToken cancellationToken);
}