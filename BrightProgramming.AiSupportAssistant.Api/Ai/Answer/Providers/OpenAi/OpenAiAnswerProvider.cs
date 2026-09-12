using BrightProgramming.AiSupportAssistant.Api.Ai.Exceptions;
using BrightProgramming.AiSupportAssistant.Api.Constants;
using BrightProgramming.AiSupportAssistant.Api.Knowledge.Models;
using OpenAI.Chat;
using System.ClientModel;
using System.Diagnostics;

namespace BrightProgramming.AiSupportAssistant.Api.Ai.Answer.Providers.OpenAi;

public class OpenAiAnswerProvider : IAiAnswerProvider
{
    private const string SystemPrompt =
        "You are a helpful technical support assistant. " +
        "Provide clear, concise and practical answers using only the supplied knowledge. " +
        "If the supplied knowledge does not contain enough information to answer the question, " +
        "say so clearly rather than inventing an answer.";

    private readonly ChatClient _chatClient;
    private readonly ILogger<OpenAiAnswerProvider> _logger;

    public string Name => AiProvider.OpenAI;

    public OpenAiAnswerProvider(
        [FromKeyedServices(AiClient.Answer)] ChatClient chatClient,
        ILogger<OpenAiAnswerProvider> logger)
    {
        _chatClient = chatClient;
        _logger = logger;
    }

    public async Task<string> GetAnswerAsync(
        string question,
        KnowledgeContext knowledge,
        CancellationToken cancellationToken)
    {
        var stopwatch = Stopwatch.StartNew();

        _logger.LogInformation(
            "Sending AI request using provider {Provider}. TraceId: {TraceId}",
            Name,
            Activity.Current?.Id);

        try
        {
            var knowledgeText = string.Join(
                "\n\n",
                knowledge.Documents.Select(document =>
                    $"## {document.Name}\n{document.Content}"));

            var messages = new ChatMessage[]
            {
                new SystemChatMessage(SystemPrompt),

                new UserChatMessage(
                    $"""
                    Knowledge:

                    {knowledgeText}

                    Question:

                    {question}
                    """)
            };

            var completion = await _chatClient.CompleteChatAsync(
                messages,
                cancellationToken: cancellationToken);

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
        catch (OperationCanceledException)
            when (cancellationToken.IsCancellationRequested)
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