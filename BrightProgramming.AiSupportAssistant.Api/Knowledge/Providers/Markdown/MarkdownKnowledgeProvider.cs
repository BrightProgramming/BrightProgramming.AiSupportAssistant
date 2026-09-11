using BrightProgramming.AiSupportAssistant.Api.Configuration;
using BrightProgramming.AiSupportAssistant.Api.Constants;
using BrightProgramming.AiSupportAssistant.Api.Knowledge.Models;
using Microsoft.Extensions.Options;

namespace BrightProgramming.AiSupportAssistant.Api.Knowledge.Providers.Markdown;

public class MarkdownKnowledgeProvider : IKnowledgeProvider
{
    private readonly KnowledgeOptions _options;
    private readonly ILogger<MarkdownKnowledgeProvider> _logger;
    private readonly List<KnowledgeDocument> _documents = [];

    public string Name => KnowledgeProvider.Markdown;

    public MarkdownKnowledgeProvider(
        IOptions<KnowledgeOptions> options,
        ILogger<MarkdownKnowledgeProvider> logger)
    {
        _options = options.Value;
        _logger = logger;

        LoadDocuments();
    }

    public Task<KnowledgeContext> GetRelevantKnowledgeAsync(
        string question,
        CancellationToken cancellationToken)
    {
        var context = new KnowledgeContext
        {
            Documents = _documents
        };

        return Task.FromResult(context);
    }

    private void LoadDocuments()
    {
        var path = Path.GetFullPath(_options.Path);

        if (!Directory.Exists(path))
        {
            _logger.LogWarning("Knowledge directory does not exist: {Path}", path);

            return;
        }

        var files = Directory.GetFiles(path, "*.md", SearchOption.AllDirectories);

        foreach (var file in files)
        {
            var content = File.ReadAllText(file);

            _documents.Add(new KnowledgeDocument
            {
                Name = Path.GetFileName(file),
                Content = content
            });
        }

        _logger.LogInformation("Loaded {DocumentCount} knowledge documents from {Path}", _documents.Count, path);
    }
}