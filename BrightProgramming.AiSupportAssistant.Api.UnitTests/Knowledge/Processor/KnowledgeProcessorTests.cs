using BrightProgramming.AiSupportAssistant.Api.Ai.Matcher.Processor;
using BrightProgramming.AiSupportAssistant.Api.Knowledge.Factory;
using BrightProgramming.AiSupportAssistant.Api.Knowledge.Models;
using BrightProgramming.AiSupportAssistant.Api.Knowledge.Processor;
using BrightProgramming.AiSupportAssistant.Api.Knowledge.Providers;
using Moq;

namespace BrightProgramming.AiSupportAssistant.Api.UnitTests.Knowledge.Processor;

public class KnowledgeProcessorTests
{
    [Fact]
    public async Task GetRelevantKnowledgeAsync_LoadsKnowledgeThenMatchesIt()
    {
        var allKnowledge = new KnowledgeContext();
        var relevantKnowledge = new KnowledgeContext();
        using var cancellation = new CancellationTokenSource();
        var sequence = new MockSequence();
        var provider = new Mock<IKnowledgeProvider>(MockBehavior.Strict);
        provider.InSequence(sequence)
            .Setup(x => x.GetKnowledgeAsync(cancellation.Token))
            .ReturnsAsync(allKnowledge);
        var matcher = new Mock<IAiKnowledgeProcessor>(MockBehavior.Strict);
        matcher.InSequence(sequence)
            .Setup(x => x.MatchAsync("question", allKnowledge, cancellation.Token))
            .ReturnsAsync(relevantKnowledge);
        var factory = new Mock<IKnowledgeProviderFactory>();
        factory.Setup(x => x.GetProvider()).Returns(provider.Object);
        var processor = new KnowledgeProcessor(factory.Object, matcher.Object);

        var result = await processor.GetRelevantKnowledgeAsync("question", cancellation.Token);

        Assert.Same(relevantKnowledge, result);
        provider.VerifyAll();
        matcher.VerifyAll();
    }

    [Fact]
    public async Task GetRelevantKnowledgeAsync_WhenProviderFails_DoesNotCallMatcher()
    {
        var failure = new InvalidOperationException("provider failed");
        var provider = new Mock<IKnowledgeProvider>();
        provider.Setup(x => x.GetKnowledgeAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(failure);
        var matcher = new Mock<IAiKnowledgeProcessor>();
        var factory = new Mock<IKnowledgeProviderFactory>();
        factory.Setup(x => x.GetProvider()).Returns(provider.Object);
        var processor = new KnowledgeProcessor(factory.Object, matcher.Object);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => processor.GetRelevantKnowledgeAsync(
            "question",
            CancellationToken.None));

        Assert.Same(failure, exception);
        matcher.Verify(
            x => x.MatchAsync(It.IsAny<string>(), It.IsAny<KnowledgeContext>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task GetRelevantKnowledgeAsync_WhenMatcherFails_PropagatesException()
    {
        var knowledge = new KnowledgeContext();
        var failure = new InvalidOperationException("matcher failed");
        var provider = new Mock<IKnowledgeProvider>();
        provider.Setup(x => x.GetKnowledgeAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(knowledge);
        var matcher = new Mock<IAiKnowledgeProcessor>();
        matcher.Setup(x => x.MatchAsync("question", knowledge, It.IsAny<CancellationToken>()))
            .ThrowsAsync(failure);
        var factory = new Mock<IKnowledgeProviderFactory>();
        factory.Setup(x => x.GetProvider()).Returns(provider.Object);
        var processor = new KnowledgeProcessor(factory.Object, matcher.Object);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => processor.GetRelevantKnowledgeAsync(
            "question",
            CancellationToken.None));

        Assert.Same(failure, exception);
    }
}
