using BrightProgramming.AiSupportAssistant.Api.Knowledge.Models;

namespace BrightProgramming.AiSupportAssistant.Api.Ai.Matcher.Providers;

public interface IAiKnowledgeMatcherProvider
{
    string Name { get; }

    Task<IReadOnlyCollection<KnowledgeDocument>> MatchAsync(
        string question,
        IReadOnlyCollection<KnowledgeDocument> documents,
        CancellationToken cancellationToken);
}