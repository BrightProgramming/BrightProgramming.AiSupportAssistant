using BrightProgramming.AiSupportAssistant.Api.Ai.Exceptions;
using BrightProgramming.AiSupportAssistant.Api.Ai.Matcher.Providers.OpenAi;
using BrightProgramming.AiSupportAssistant.Api.Constants;
using BrightProgramming.AiSupportAssistant.Api.Knowledge.Models;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using OpenAI.Chat;
using System.ClientModel;
using System.ClientModel.Primitives;

namespace BrightProgramming.AiSupportAssistant.Api.UnitTests.Ai.Matcher.Providers.OpenAi;

public class OpenAiKnowledgeMatcherProviderTests
{
    [Fact]
    public void Name_IsOpenAi()
    {
        var provider = CreateProvider(new Mock<ChatClient>().Object);

        Assert.Equal(AiProvider.OpenAI, provider.Name);
    }

    [Fact]
    public async Task MatchAsync_WhenClientRequestFails_WrapsProviderException()
    {
        var client = new Mock<ChatClient>();
        var clientException = CreateClientException();
        client.Setup(x => x.CompleteChatAsync(
                It.IsAny<IEnumerable<ChatMessage>>(),
                It.IsAny<ChatCompletionOptions>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(clientException);
        var provider = CreateProvider(client.Object);

        var exception = await Assert.ThrowsAsync<AiProviderException>(() => provider.MatchAsync(
            "question",
            new KnowledgeContext(),
            CancellationToken.None));

        Assert.Equal("The AI provider could not process the knowledge matching request.", exception.Message);
        Assert.Same(clientException, exception.InnerException);
    }

    [Fact]
    public async Task MatchAsync_WhenCallerCancels_RethrowsCancellation()
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

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => provider.MatchAsync(
            "question",
            new KnowledgeContext(),
            cancellation.Token));
    }

    private static OpenAiKnowledgeMatcherProvider CreateProvider(ChatClient client) => new(
        client,
        NullLogger<OpenAiKnowledgeMatcherProvider>.Instance);

    private static ClientResultException CreateClientException() => new(
        "provider failure",
        new Mock<PipelineResponse>().Object,
        null);
}
