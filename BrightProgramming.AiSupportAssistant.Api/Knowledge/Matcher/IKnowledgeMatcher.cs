using BrightProgramming.AiSupportAssistant.Api.Knowledge.Models;

namespace BrightProgramming.AiSupportAssistant.Api.Knowledge;

public interface IKnowledgeMatcher
{
    Task<KnowledgeContext> MatchAsync(
        string question,
        KnowledgeContext knowledge,
        CancellationToken cancellationToken);
}