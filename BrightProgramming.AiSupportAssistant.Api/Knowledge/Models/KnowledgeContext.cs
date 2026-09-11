namespace BrightProgramming.AiSupportAssistant.Api.Knowledge.Models;

public class KnowledgeContext
{
    public IReadOnlyCollection<KnowledgeDocument> Documents { get; init; }
        = [];
}