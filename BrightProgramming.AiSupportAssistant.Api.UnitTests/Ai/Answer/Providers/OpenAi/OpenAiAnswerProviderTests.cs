using BrightProgramming.AiSupportAssistant.Api.Ai.Answer.Providers.OpenAi;
using BrightProgramming.AiSupportAssistant.Api.Ai.Exceptions;
using BrightProgramming.AiSupportAssistant.Api.Constants;
using BrightProgramming.AiSupportAssistant.Api.Knowledge.Models;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using OpenAI.Chat;
using System.ClientModel;
using System.ClientModel.Primitives;

namespace BrightProgramming.AiSupportAssistant.Api.UnitTests.Ai.Answer.Providers.OpenAi;

public class OpenAiAnswerProviderTests
{
    [Fact]
    public void Name_IsOpenAi()
    {
        var provider = CreateProvider(new Mock<ChatClient>().Object);

        Assert.Equal(AiProvider.OpenAI, provider.Name);
    }

    [Fact]
    public async Task GetAnswerAsync_WhenClientRequestFails_WrapsProviderException()
    {
        var client = new Mock<ChatClient>();
        var clientException = CreateClientException();
        client.Setup(x => x.CompleteChatAsync(
                It.IsAny<IEnumerable<ChatMessage>>(),
                It.IsAny<ChatCompletionOptions>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(clientException);
        var provider = CreateProvider(client.Object);

        var exception = await Assert.ThrowsAsync<AiProviderException>(() => provider.GetAnswerAsync(
            "question",
            new KnowledgeContext(),
            CancellationToken.None));

        Assert.Equal("The AI provider could not process the request.", exception.Message);
        Assert.Same(clientException, exception.InnerException);
    }

    [Fact]
    public async Task GetAnswerAsync_WhenCallerCancels_RethrowsCancellation()
    {
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();
        var client = new Mock<ChatClient>();
        client.Setup(x => x.CompleteChatAsync(
                It.IsAny<IEnumerable<ChatMessage>>(),
                It.IsAny<ChatCompletionOptions>(),
                cancellation.Token))
            .ThrowsAsync(new OperationCanceledException(cancellation.Token));
        var provider = CreateProvider(client.Object);

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => provider.GetAnswerAsync(
            "question",
            new KnowledgeContext(),
            cancellation.Token));
    }

    private static OpenAiAnswerProvider CreateProvider(ChatClient client) => new(
        client,
        NullLogger<OpenAiAnswerProvider>.Instance);

    private static ClientResultException CreateClientException() => new(
        "provider failure",
        new Mock<PipelineResponse>().Object,
        null);
}
