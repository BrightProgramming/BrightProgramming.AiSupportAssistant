using BrightProgramming.AiSupportAssistant.Api.Configuration;
using BrightProgramming.AiSupportAssistant.Api.Constants;
using Microsoft.Extensions.Options;

namespace BrightProgramming.AiSupportAssistant.Api.Ai.OpenAi;

public class OpenAiProvider : IAiProvider
{
    private readonly AiOptions _options;

    public string Name => AiProvider.OpenAI;

    public OpenAiProvider(IOptions<AiOptions> options)
    {
        _options = options.Value;
    }

    public Task<string> GetAnswerAsync(string question)
    {
        return Task.FromResult(
            $"AI provider configured: {_options.Provider}");
    }
}