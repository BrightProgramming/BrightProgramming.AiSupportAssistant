using BrightProgramming.AiSupportAssistant.Api.Ai.Matcher.Factory;
using BrightProgramming.AiSupportAssistant.Api.Ai.Matcher.Processor;
using BrightProgramming.AiSupportAssistant.Api.Ai.Matcher.Providers;
using BrightProgramming.AiSupportAssistant.Api.Knowledge.Models;
using Moq;

namespace BrightProgramming.AiSupportAssistant.Api.UnitTests.Ai.Matcher.Processor;

public class AiKnowledgeProcessorTests
{
    [Fact]
    public async Task MatchAsync_ForwardsArgumentsAndReturnsProviderResult()
    {
        var knowledge = new KnowledgeContext();
        var matchedKnowledge = new KnowledgeContext();
        using var cancellation = new CancellationTokenSource();
        var provider = new Mock<IAiKnowledgeMatcherProvider>();
        provider.Setup(x => x.MatchAsync("question", knowledge, cancellation.Token))
            .ReturnsAsync(matchedKnowledge);
        var factory = new Mock<IAiKnowledgeMatcherFactory>();
        factory.Setup(x => x.GetProvider()).Returns(provider.Object);
        var processor = new AiKnowledgeProcessor(factory.Object);

        var result = await processor.MatchAsync("question", knowledge, cancellation.Token);

        Assert.Same(matchedKnowledge, result);
        factory.Verify(x => x.GetProvider(), Times.Once);
        provider.Verify(x => x.MatchAsync("question", knowledge, cancellation.Token), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_WhenProviderFails_PropagatesException()
    {
        var knowledge = new KnowledgeContext();
        var failure = new InvalidOperationException("provider failed");
        var provider = new Mock<IAiKnowledgeMatcherProvider>();
        provider.Setup(x => x.MatchAsync("question", knowledge, It.IsAny<CancellationToken>()))
            .ThrowsAsync(failure);
        var factory = new Mock<IAiKnowledgeMatcherFactory>();
        factory.Setup(x => x.GetProvider()).Returns(provider.Object);
        var processor = new AiKnowledgeProcessor(factory.Object);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => processor.MatchAsync(
            "question",
            knowledge,
            CancellationToken.None));

        Assert.Same(failure, exception);
    }

    [Fact]
    public async Task MatchAsync_WhenProviderIsCancelled_PropagatesCancellation()
    {
        var knowledge = new KnowledgeContext();
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();
        var provider = new Mock<IAiKnowledgeMatcherProvider>();
        provider.Setup(x => x.MatchAsync("question", knowledge, cancellation.Token))
            .ThrowsAsync(new OperationCanceledException(cancellation.Token));
        var factory = new Mock<IAiKnowledgeMatcherFactory>();
        factory.Setup(x => x.GetProvider()).Returns(provider.Object);
        var processor = new AiKnowledgeProcessor(factory.Object);

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => processor.MatchAsync(
            "question",
            knowledge,
            cancellation.Token));
    }
}
