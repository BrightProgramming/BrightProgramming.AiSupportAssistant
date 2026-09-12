using BrightProgramming.AiSupportAssistant.Api.Ai.Answer.Factory;
using BrightProgramming.AiSupportAssistant.Api.Ai.Answer.Providers;
using BrightProgramming.AiSupportAssistant.Api.Configuration;
using Microsoft.Extensions.Options;
using Moq;

namespace BrightProgramming.AiSupportAssistant.Api.UnitTests.Ai.Answer.Factory;

public class AiAnswerProviderFactoryTests
{
    [Fact]
    public void GetProvider_WhenProviderNameDiffersOnlyByCase_ReturnsMatchingProvider()
    {
        var provider = new Mock<IAiAnswerProvider>();
        provider.SetupGet(x => x.Name).Returns("OpenAI");
        var factory = new AiAnswerProviderFactory(
            [provider.Object],
            Options.Create(new AiAnswerOptions { Provider = "openai" }));

        Assert.Same(provider.Object, factory.GetProvider());
    }

    [Fact]
    public void Constructor_WhenConfiguredProviderIsNotRegistered_Throws()
    {
        var exception = Assert.Throws<InvalidOperationException>(() => new AiAnswerProviderFactory(
            [],
            Options.Create(new AiAnswerOptions { Provider = "Other" })));

        Assert.Equal("AI provider 'Other' is not registered.", exception.Message);
    }

    [Fact]
    public void Constructor_WhenMultipleProvidersMatch_Throws()
    {
        var first = new Mock<IAiAnswerProvider>();
        first.SetupGet(x => x.Name).Returns("OpenAI");
        var second = new Mock<IAiAnswerProvider>();
        second.SetupGet(x => x.Name).Returns("openai");

        Assert.Throws<InvalidOperationException>(() => new AiAnswerProviderFactory(
            [first.Object, second.Object],
            Options.Create(new AiAnswerOptions { Provider = "OpenAI" })));
    }
}
