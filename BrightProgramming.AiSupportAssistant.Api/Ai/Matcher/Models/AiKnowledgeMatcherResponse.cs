namespace BrightProgramming.AiSupportAssistant.Api.Ai.Matcher.Models;

public class AiKnowledgeMatcherResponse
{
    public IReadOnlyCollection<string> RelevantDocuments { get; init; }
        = [];
}