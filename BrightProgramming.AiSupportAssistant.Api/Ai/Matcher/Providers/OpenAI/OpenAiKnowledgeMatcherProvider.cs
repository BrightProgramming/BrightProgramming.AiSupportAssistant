using BrightProgramming.AiSupportAssistant.Api.Ai.Matcher.Models;
using BrightProgramming.AiSupportAssistant.Api.Ai.Matcher.Providers;
using BrightProgramming.AiSupportAssistant.Api.Ai.Exceptions;
using BrightProgramming.AiSupportAssistant.Api.Constants;
using BrightProgramming.AiSupportAssistant.Api.Knowledge.Models;
using OpenAI.Chat;
using System.ClientModel;
using System.Diagnostics;
using System.Text.Json;

namespace BrightProgramming.AiSupportAssistant.Api.Ai.Matcher.Providers.OpenAi;

public class OpenAiKnowledgeMatcherProvider : IAiKnowledgeMatcherProvider
{
    private const string SystemPrompt =
        """
        You identify which knowledge documents are relevant to a technical support question.
        Return only valid JSON matching the requested response format.
        """;

    private readonly ChatClient _chatClient;
    private readonly ILogger<OpenAiKnowledgeMatcherProvider> _logger;

    public string Name => AiProvider.OpenAI;

    public OpenAiKnowledgeMatcherProvider(
        [FromKeyedServices(AiClient.KnowledgeMatcher)] ChatClient chatClient,
        ILogger<OpenAiKnowledgeMatcherProvider> logger)
    {
        _chatClient = chatClient;
        _logger = logger;
    }

    public async Task<KnowledgeContext> MatchAsync(
        string question,
        KnowledgeContext knowledge,
        CancellationToken cancellationToken)
    {
        var stopwatch = Stopwatch.StartNew();

        _logger.LogInformation(
            "Sending AI knowledge matching request using provider {Provider}. TraceId: {TraceId}",
            Name,
            Activity.Current?.Id);

        try
        {
            var documentList = string.Join(
                Environment.NewLine + Environment.NewLine,
                knowledge.Documents.Select(document =>
                    $"DOCUMENT: {document.Name}{Environment.NewLine}{document.Content}"));

            var userPrompt =
                $$"""
                Technical support question:
                {{question}}

                Available knowledge documents:

                {{documentList}}

                Identify the documents that contain knowledge relevant to answering the question.

                Return JSON in exactly this form:
                {
                  "relevantDocuments": [
                    "document-name.md"
                  ]
                }

                Only include document names from the supplied list.
                """;

            var messages = new ChatMessage[]
            {
                new SystemChatMessage(SystemPrompt),
                new UserChatMessage(userPrompt)
            };

            var completion = await _chatClient.CompleteChatAsync(
                messages,
                cancellationToken: cancellationToken);

            var text = completion.Value.Content
                .FirstOrDefault()?.Text;

            if (string.IsNullOrWhiteSpace(text))
            {
                throw new AiProviderException(
                    "AI provider returned an empty response.");
            }

            var response = JsonSerializer.Deserialize<AiKnowledgeMatcherResponse>(
                text,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (response is null)
            {
                throw new AiProviderException(
                    "AI provider returned an invalid knowledge matching response.");
            }

            var matchedDocuments = knowledge.Documents
                .Where(document => response.RelevantDocuments.Contains(
                    document.Name,
                    StringComparer.OrdinalIgnoreCase))
                .ToArray();

            _logger.LogInformation(
                "AI knowledge matching request completed successfully using provider {Provider} in {ElapsedMilliseconds} ms. Matched {DocumentCount} documents. TraceId: {TraceId}",
                Name,
                stopwatch.ElapsedMilliseconds,
                matchedDocuments.Length,
                Activity.Current?.Id);

            return new KnowledgeContext
            {
                Documents = matchedDocuments
            };
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            _logger.LogInformation(
                "AI knowledge matching request was cancelled by the caller. Provider: {Provider}. TraceId: {TraceId}",
                Name,
                Activity.Current?.Id);

            throw;
        }
        catch (ClientResultException ex)
        {
            _logger.LogError(
                ex,
                "AI knowledge matching request failed using provider {Provider} after {ElapsedMilliseconds} ms. TraceId: {TraceId}",
                Name,
                stopwatch.ElapsedMilliseconds,
                Activity.Current?.Id);

            throw new AiProviderException(
                "The AI provider could not process the knowledge matching request.",
                ex);
        }
    }
}