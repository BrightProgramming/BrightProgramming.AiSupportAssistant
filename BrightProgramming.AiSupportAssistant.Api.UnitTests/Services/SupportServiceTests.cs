using BrightProgramming.AiSupportAssistant.Api.Ai.Answer.Processor;
using BrightProgramming.AiSupportAssistant.Api.Knowledge.Models;
using BrightProgramming.AiSupportAssistant.Api.Knowledge.Processor;
using BrightProgramming.AiSupportAssistant.Api.Models;
using BrightProgramming.AiSupportAssistant.Api.Services;
using Moq;

namespace BrightProgramming.AiSupportAssistant.Api.UnitTests.Services;

public class SupportServiceTests
{
    [Fact]
    public async Task GetAnswerAsync_GetsRelevantKnowledgeThenReturnsAnswerForOriginalQuestion()
    {
        var request = new SupportRequest { Question = "How does DI work?" };
        var knowledge = new KnowledgeContext();
        using var cancellation = new CancellationTokenSource();
        var sequence = new MockSequence();
        var knowledgeProcessor = new Mock<IKnowledgeProcessor>(MockBehavior.Strict);
        knowledgeProcessor.InSequence(sequence)
            .Setup(x => x.GetRelevantKnowledgeAsync(request.Question, cancellation.Token))
            .ReturnsAsync(knowledge);
        var answerProcessor = new Mock<IAiAnswerProcessor>(MockBehavior.Strict);
        answerProcessor.InSequence(sequence)
            .Setup(x => x.GetAnswerAsync(request.Question, knowledge, cancellation.Token))
            .ReturnsAsync("Register services in the container.");
        var service = new SupportService(answerProcessor.Object, knowledgeProcessor.Object);

        var response = await service.GetAnswerAsync(request, cancellation.Token);

        Assert.Equal(request.Question, response.Question);
        Assert.Equal("Register services in the container.", response.Answer);
        knowledgeProcessor.VerifyAll();
        answerProcessor.VerifyAll();
    }

    [Fact]
    public async Task GetAnswerAsync_WhenKnowledgeProcessingFails_DoesNotCallAnswerProcessor()
    {
        var knowledgeProcessor = new Mock<IKnowledgeProcessor>();
        knowledgeProcessor.Setup(x => x.GetRelevantKnowledgeAsync("question", It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("knowledge failed"));
        var answerProcessor = new Mock<IAiAnswerProcessor>();
        var service = new SupportService(answerProcessor.Object, knowledgeProcessor.Object);

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.GetAnswerAsync(
            new SupportRequest { Question = "question" },
            CancellationToken.None));

        answerProcessor.Verify(
            x => x.GetAnswerAsync(It.IsAny<string>(), It.IsAny<KnowledgeContext>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task GetAnswerAsync_WhenAnswerProcessingFails_PropagatesException()
    {
        var knowledge = new KnowledgeContext();
        var failure = new InvalidOperationException("answer failed");
        var knowledgeProcessor = new Mock<IKnowledgeProcessor>();
        knowledgeProcessor
            .Setup(x => x.GetRelevantKnowledgeAsync("question", It.IsAny<CancellationToken>()))
            .ReturnsAsync(knowledge);
        var answerProcessor = new Mock<IAiAnswerProcessor>();
        answerProcessor
            .Setup(x => x.GetAnswerAsync("question", knowledge, It.IsAny<CancellationToken>()))
            .ThrowsAsync(failure);
        var service = new SupportService(answerProcessor.Object, knowledgeProcessor.Object);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => service.GetAnswerAsync(
            new SupportRequest { Question = "question" },
            CancellationToken.None));

        Assert.Same(failure, exception);
    }
}
