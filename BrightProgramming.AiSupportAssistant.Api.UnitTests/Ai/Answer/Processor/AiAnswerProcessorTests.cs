using BrightProgramming.AiSupportAssistant.Api.Ai.Answer.Factory;
using BrightProgramming.AiSupportAssistant.Api.Ai.Answer.Processor;
using BrightProgramming.AiSupportAssistant.Api.Ai.Answer.Providers;
using BrightProgramming.AiSupportAssistant.Api.Knowledge.Models;
using Moq;

namespace BrightProgramming.AiSupportAssistant.Api.UnitTests.Ai.Answer.Processor;

public class AiAnswerProcessorTests
{
    [Fact]
    public async Task GetAnswerAsync_ForwardsArgumentsAndReturnsProviderAnswer()
    {
        var knowledge = new KnowledgeContext();
        using var cancellation = new CancellationTokenSource();
        var provider = new Mock<IAiAnswerProvider>();
        provider.Setup(x => x.GetAnswerAsync("question", knowledge, cancellation.Token))
            .ReturnsAsync("answer");
        var factory = new Mock<IAiAnswerProviderFactory>();
        factory.Setup(x => x.GetProvider()).Returns(provider.Object);
        var processor = new AiAnswerProcessor(factory.Object);

        var answer = await processor.GetAnswerAsync("question", knowledge, cancellation.Token);

        Assert.Equal("answer", answer);
        factory.Verify(x => x.GetProvider(), Times.Once);
        provider.Verify(x => x.GetAnswerAsync("question", knowledge, cancellation.Token), Times.Once);
    }

    [Fact]
    public async Task GetAnswerAsync_WhenProviderFails_PropagatesException()
    {
        var knowledge = new KnowledgeContext();
        var failure = new InvalidOperationException("provider failed");
        var provider = new Mock<IAiAnswerProvider>();
        provider.Setup(x => x.GetAnswerAsync("question", knowledge, It.IsAny<CancellationToken>()))
            .ThrowsAsync(failure);
        var factory = new Mock<IAiAnswerProviderFactory>();
        factory.Setup(x => x.GetProvider()).Returns(provider.Object);
        var processor = new AiAnswerProcessor(factory.Object);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => processor.GetAnswerAsync(
            "question",
            knowledge,
            CancellationToken.None));

        Assert.Same(failure, exception);
    }

    [Fact]
    public async Task GetAnswerAsync_WhenProviderIsCancelled_PropagatesCancellation()
    {
        var knowledge = new KnowledgeContext();
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();
        var provider = new Mock<IAiAnswerProvider>();
        provider.Setup(x => x.GetAnswerAsync("question", knowledge, cancellation.Token))
            .ThrowsAsync(new OperationCanceledException(cancellation.Token));
        var factory = new Mock<IAiAnswerProviderFactory>();
        factory.Setup(x => x.GetProvider()).Returns(provider.Object);
        var processor = new AiAnswerProcessor(factory.Object);

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => processor.GetAnswerAsync(
            "question",
            knowledge,
            cancellation.Token));
    }
}
