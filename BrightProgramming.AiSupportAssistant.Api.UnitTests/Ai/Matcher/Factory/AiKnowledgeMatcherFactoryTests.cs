using BrightProgramming.AiSupportAssistant.Api.Ai.Matcher.Factory;
using BrightProgramming.AiSupportAssistant.Api.Ai.Matcher.Providers;
using BrightProgramming.AiSupportAssistant.Api.Configuration;
using Microsoft.Extensions.Options;
using Moq;

namespace BrightProgramming.AiSupportAssistant.Api.UnitTests.Ai.Matcher.Factory;

public class AiKnowledgeMatcherFactoryTests
{
    [Fact]
    public void GetProvider_WhenProviderNameDiffersOnlyByCase_ReturnsMatchingProvider()
    {
        var provider = new Mock<IAiKnowledgeMatcherProvider>();
        provider.SetupGet(x => x.Name).Returns("OpenAI");
        var factory = new AiKnowledgeMatcherFactory(
            [provider.Object],
            Options.Create(new AiKnowledgeMatcherOptions { Provider = "openai" }));

        Assert.Same(provider.Object, factory.GetProvider());
    }

    [Fact]
    public void Constructor_WhenConfiguredProviderIsNotRegistered_Throws()
    {
        var exception = Assert.Throws<InvalidOperationException>(() => new AiKnowledgeMatcherFactory(
            [],
            Options.Create(new AiKnowledgeMatcherOptions { Provider = "Other" })));

        Assert.Equal("AI knowledge matcher provider 'Other' is not registered.", exception.Message);
    }

    [Fact]
    public void Constructor_WhenMultipleProvidersMatch_Throws()
    {
        var first = new Mock<IAiKnowledgeMatcherProvider>();
        first.SetupGet(x => x.Name).Returns("OpenAI");
        var second = new Mock<IAiKnowledgeMatcherProvider>();
        second.SetupGet(x => x.Name).Returns("openai");

        Assert.Throws<InvalidOperationException>(() => new AiKnowledgeMatcherFactory(
            [first.Object, second.Object],
            Options.Create(new AiKnowledgeMatcherOptions { Provider = "OpenAI" })));
    }
}
