using BrightProgramming.AiSupportAssistant.Api.Configuration;
using BrightProgramming.AiSupportAssistant.Api.Knowledge.Models;
using Microsoft.Extensions.Options;

namespace BrightProgramming.AiSupportAssistant.Api.Knowledge.Repository.Markdown;

public class MarkdownKnowledgeRepository : IKnowledgeRepository
{
    private readonly KnowledgeSourceOptions _options;
    private readonly ILogger<MarkdownKnowledgeRepository> _logger;

    public MarkdownKnowledgeRepository(
        IOptions<KnowledgeSourceOptions> options,
        ILogger<MarkdownKnowledgeRepository> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    public IReadOnlyCollection<KnowledgeDocument> GetDocuments()
    {
        var path = Path.GetFullPath(_options.Path);

        if (!Directory.Exists(path))
        {
            _logger.LogWarning(
                "Knowledge directory does not exist: {Path}",
                path);

            return [];
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

        return documents;
    }
}