using BrightProgramming.AiSupportAssistant.Api.Constants;
using BrightProgramming.AiSupportAssistant.Api.Knowledge.Models;
using BrightProgramming.AiSupportAssistant.Api.Knowledge.Repository;

namespace BrightProgramming.AiSupportAssistant.Api.Knowledge.Providers.Markdown;

public class MarkdownKnowledgeProvider : IKnowledgeProvider
{
    private readonly IKnowledgeRepository _repository;
    private readonly Lazy<KnowledgeContext> _knowledge;

    public string Name => KnowledgeProvider.Markdown;

    public MarkdownKnowledgeProvider(
        IKnowledgeRepository repository)
    {
        _repository = repository;

        _knowledge = new Lazy<KnowledgeContext>(LoadKnowledge);
    }

    public Task<KnowledgeContext> GetKnowledgeAsync(
        CancellationToken cancellationToken)
    {
        return Task.FromResult(_knowledge.Value);
    }

    private KnowledgeContext LoadKnowledge()
    {
        return new KnowledgeContext
        {
            Documents = _repository.GetDocuments()
        };
    }
}