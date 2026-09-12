using BrightProgramming.AiSupportAssistant.Api.Configuration;
using BrightProgramming.AiSupportAssistant.Api.Knowledge.Factory;
using BrightProgramming.AiSupportAssistant.Api.Knowledge.Providers;
using Microsoft.Extensions.Options;
using Moq;

namespace BrightProgramming.AiSupportAssistant.Api.UnitTests.Knowledge.Factory;

public class KnowledgeProviderFactoryTests
{
    [Fact]
    public void GetProvider_WhenProviderNameDiffersOnlyByCase_ReturnsMatchingProvider()
    {
        var provider = new Mock<IKnowledgeProvider>();
        provider.SetupGet(x => x.Name).Returns("Markdown");
        var factory = new KnowledgeProviderFactory(
            [provider.Object],
            Options.Create(new KnowledgeSourceOptions { Provider = "markdown" }));

        Assert.Same(provider.Object, factory.GetProvider());
    }

    [Fact]
    public void Constructor_WhenConfiguredProviderIsNotRegistered_Throws()
    {
        var provider = new Mock<IKnowledgeProvider>();
        provider.SetupGet(x => x.Name).Returns("Markdown");

        var exception = Assert.Throws<InvalidOperationException>(() => new KnowledgeProviderFactory(
            [provider.Object],
            Options.Create(new KnowledgeSourceOptions { Provider = "Other" })));

        Assert.Equal("Knowledge provider 'Other' is not registered.", exception.Message);
    }

    [Fact]
    public void Constructor_WhenMultipleProvidersMatch_Throws()
    {
        var first = new Mock<IKnowledgeProvider>();
        first.SetupGet(x => x.Name).Returns("Markdown");
        var second = new Mock<IKnowledgeProvider>();
        second.SetupGet(x => x.Name).Returns("markdown");

        Assert.Throws<InvalidOperationException>(() => new KnowledgeProviderFactory(
            [first.Object, second.Object],
            Options.Create(new KnowledgeSourceOptions { Provider = "Markdown" })));
    }
}
