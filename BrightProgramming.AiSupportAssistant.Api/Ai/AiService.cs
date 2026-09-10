namespace BrightProgramming.AiSupportAssistant.Api.Ai;

public class AiService : IAiService
{
    public Task<string> GetAnswerAsync(string question)
    {
        return Task.FromResult(
            "This is where our AI-generated support answer will eventually go.");
    }
}