using BrightProgramming.AiSupportAssistant.Api.Ai.Exceptions;
using BrightProgramming.AiSupportAssistant.Api.Configuration;
using BrightProgramming.AiSupportAssistant.Api.Constants;
using Microsoft.Extensions.Options;
using OpenAI.Chat;
using System.ClientModel;

namespace BrightProgramming.AiSupportAssistant.Api.Ai.OpenAi;

public class OpenAiProvider : IAiProvider
{
    private readonly ChatClient _chatClient;
    private readonly OpenAiOptions _options;

    public string Name => AiProvider.OpenAI;

    private const string SystemPrompt = "You are a helpful technical support assistant. Provide clear, concise and practical answers.";

    public OpenAiProvider(
        ChatClient chatClient,
        IOptions<OpenAiOptions> options)
    {
        _chatClient = chatClient;
        _options = options.Value;
    }

    public async Task<string> GetAnswerAsync(string question)
    {
        try
        {
            var messages = new ChatMessage[]
            {
                new SystemChatMessage(SystemPrompt),
                new UserChatMessage(question)
            };

            var completion = await _chatClient.CompleteChatAsync(messages);

            var text = completion.Value.Content
                .FirstOrDefault()?.Text;

            if (string.IsNullOrWhiteSpace(text))
            {
                throw new AiProviderException(
                    "AI provider returned an empty response.");
            }

            return text;
        }
        catch (ClientResultException ex)
        {
            throw new AiProviderException(
                "The AI provider could not process the request.",
                ex);
        }
    }
}