using BrightProgramming.AiSupportAssistant.Api.Configuration;
using BrightProgramming.AiSupportAssistant.Api.Constants;
using Microsoft.Extensions.Options;
using OpenAI.Chat;

namespace BrightProgramming.AiSupportAssistant.Api.Ai.OpenAi;

public class OpenAiProvider : IAiProvider
{
    private readonly ChatClient _chatClient;
    private readonly OpenAiOptions _options;

    public string Name => AiProvider.OpenAI;

    public OpenAiProvider(
        ChatClient chatClient,
        IOptions<OpenAiOptions> options)
    {
        _chatClient = chatClient;
        _options = options.Value;
    }

    public async Task<string> GetAnswerAsync(string question)
    {
        var completion = await _chatClient.CompleteChatAsync(question);

        return completion.Value.Content[0].Text;
    }
}