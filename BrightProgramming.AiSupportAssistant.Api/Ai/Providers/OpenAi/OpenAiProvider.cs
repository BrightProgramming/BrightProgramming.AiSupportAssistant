using BrightProgramming.AiSupportAssistant.Api.Ai.Exceptions;
using BrightProgramming.AiSupportAssistant.Api.Constants;
using OpenAI.Chat;
using System.ClientModel;
using System.Diagnostics;

namespace BrightProgramming.AiSupportAssistant.Api.Ai.Providers.OpenAi;

public class OpenAiProvider : IAiProvider
{
    private const string SystemPrompt = "You are a helpful technical support assistant. Provide clear, concise and practical answers.";

    private readonly ChatClient _chatClient;
    private readonly ILogger<OpenAiProvider> _logger;

    public string Name => AiProvider.OpenAI;

    public OpenAiProvider(
        ChatClient chatClient,
        ILogger<OpenAiProvider> logger)
    {
        _chatClient = chatClient;
        _logger = logger;
    }

    public async Task<string> GetAnswerAsync(
        string question,
        CancellationToken cancellationToken)
    {
        var stopwatch = Stopwatch.StartNew();
        _logger.LogInformation("Sending AI request using provider {Provider}. TraceId: {TraceId}", Name, Activity.Current?.Id);

        try
        {
            var messages = new ChatMessage[]
            {
                new SystemChatMessage(SystemPrompt),
                new UserChatMessage(question)
            };

            var completion = await _chatClient.CompleteChatAsync(messages, cancellationToken: cancellationToken);

            _logger.LogInformation(
                "AI request completed successfully using provider {Provider} in {ElapsedMilliseconds} ms. TraceId: {TraceId}",
                Name,
                stopwatch.ElapsedMilliseconds,
                Activity.Current?.Id);

            var text = completion.Value.Content
                .FirstOrDefault()?.Text;

            if (string.IsNullOrWhiteSpace(text))
            {
                throw new AiProviderException(
                    "AI provider returned an empty response.");
            }

            return text;
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            _logger.LogInformation(
                "AI request was cancelled by the caller. Provider: {Provider}. TraceId: {TraceId}",
                Name,
                Activity.Current?.Id);

            throw;
        }
        catch (ClientResultException ex)
        {
            _logger.LogError(
                ex,
                "AI provider request failed using provider {Provider} after {ElapsedMilliseconds} ms. TraceId: {TraceId}",
                Name,
                stopwatch.ElapsedMilliseconds,
                Activity.Current?.Id);

            throw new AiProviderException(
                "The AI provider could not process the request.",
                ex);
        }
    }
}