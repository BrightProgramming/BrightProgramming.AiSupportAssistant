using BrightProgramming.AiSupportAssistant.Api.Knowledge.Models;

namespace BrightProgramming.AiSupportAssistant.Api.Knowledge.Repository;

public interface IKnowledgeRepository
{
    IReadOnlyCollection<KnowledgeDocument> GetDocuments();
}