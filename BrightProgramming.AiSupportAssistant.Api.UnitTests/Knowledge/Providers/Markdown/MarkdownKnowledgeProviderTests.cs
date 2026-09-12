using BrightProgramming.AiSupportAssistant.Api.Configuration;
using BrightProgramming.AiSupportAssistant.Api.Knowledge.Providers.Markdown;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace BrightProgramming.AiSupportAssistant.Api.UnitTests.Knowledge.Providers.Markdown;

public class MarkdownKnowledgeProviderTests
{
    [Fact]
    public async Task GetKnowledgeAsync_WhenDirectoryDoesNotExist_ReturnsEmptyContext()
    {
        using var directory = new TemporaryDirectory();
        var missingPath = Path.Combine(directory.Path, "missing");
        var provider = CreateProvider(missingPath);

        var result = await provider.GetKnowledgeAsync(CancellationToken.None);

        Assert.Empty(result.Documents);
    }

    [Fact]
    public async Task GetKnowledgeAsync_LoadsMarkdownFilesRecursivelyAndExcludesOtherFiles()
    {
        using var directory = new TemporaryDirectory();
        File.WriteAllText(Path.Combine(directory.Path, "top-level.md"), "top-level content");
        var nestedDirectory = Directory.CreateDirectory(Path.Combine(directory.Path, "nested"));
        File.WriteAllText(Path.Combine(nestedDirectory.FullName, "nested.md"), "nested content");
        File.WriteAllText(Path.Combine(nestedDirectory.FullName, "ignored.txt"), "ignore me");
        var provider = CreateProvider(directory.Path);

        var result = await provider.GetKnowledgeAsync(CancellationToken.None);

        Assert.Equal(2, result.Documents.Count);
        Assert.Contains(result.Documents, x => x.Name == "top-level.md" && x.Content == "top-level content");
        Assert.Contains(result.Documents, x => x.Name == "nested.md" && x.Content == "nested content");
    }

    [Fact]
    public async Task GetKnowledgeAsync_CachesDocumentsAfterFirstLoad()
    {
        using var directory = new TemporaryDirectory();
        File.WriteAllText(Path.Combine(directory.Path, "first.md"), "first");
        var provider = CreateProvider(directory.Path);

        var first = await provider.GetKnowledgeAsync(CancellationToken.None);
        File.WriteAllText(Path.Combine(directory.Path, "second.md"), "second");
        var second = await provider.GetKnowledgeAsync(CancellationToken.None);

        Assert.Same(first, second);
        Assert.Single(second.Documents);
        Assert.Equal("first.md", Assert.Single(second.Documents).Name);
    }

    private static MarkdownKnowledgeProvider CreateProvider(string path) => new(
        Options.Create(new KnowledgeSourceOptions { Path = path }),
        NullLogger<MarkdownKnowledgeProvider>.Instance);

    private sealed class TemporaryDirectory : IDisposable
    {
        public TemporaryDirectory()
        {
            Path = System.IO.Path.Combine(System.IO.Path.GetTempPath(), Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(Path);
        }

        public string Path { get; }

        public void Dispose()
        {
            if (Directory.Exists(Path))
            {
                Directory.Delete(Path, recursive: true);
            }
        }
    }
}
