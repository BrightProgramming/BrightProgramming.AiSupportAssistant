using BrightProgramming.AiSupportAssistant.Api.Configuration;
using BrightProgramming.AiSupportAssistant.Api.Constants;
using BrightProgramming.AiSupportAssistant.Api.Knowledge.Models;
using Microsoft.Extensions.Options;

namespace BrightProgramming.AiSupportAssistant.Api.Knowledge.Providers.Markdown;

public class MarkdownKnowledgeProvider : IKnowledgeProvider
{
    private readonly KnowledgeSourceOptions _options;
    private readonly ILogger<MarkdownKnowledgeProvider> _logger;
    private readonly Lazy<KnowledgeContext> _knowledge;

    public string Name => KnowledgeProvider.Markdown;

    public MarkdownKnowledgeProvider(
        IOptions<KnowledgeSourceOptions> options,
        ILogger<MarkdownKnowledgeProvider> logger)
    {
        _options = options.Value;
        _logger = logger;

        _knowledge = new Lazy<KnowledgeContext>(LoadDocuments);
    }

    public Task<KnowledgeContext> GetKnowledgeAsync(
        CancellationToken cancellationToken)
    {
        return Task.FromResult(_knowledge.Value);
    }

    private KnowledgeContext LoadDocuments()
    {
        var path = Path.GetFullPath(_options.Path);

        if (!Directory.Exists(path))
        {
            _logger.LogWarning(
                "Knowledge directory does not exist: {Path}",
                path);

            return new KnowledgeContext
            {
                Documents = []
            };
        }

        var documents = new List<KnowledgeDocument>();

        var files = Directory.GetFiles(
            path,
            "*.md",
            SearchOption.AllDirectories);

        foreach (var file in files)
        {
            var content = File.ReadAllText(file);

            documents.Add(new KnowledgeDocument
            {
                Name = Path.GetFileName(file),
                Content = content
            });
        }

        _logger.LogInformation(
            "Loaded {DocumentCount} knowledge documents from {Path}",
            documents.Count,
            path);

        return new KnowledgeContext
        {
            Documents = documents
        };
    }
}