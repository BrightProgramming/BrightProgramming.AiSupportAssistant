namespace BrightProgramming.AiSupportAssistant.Api.Ai;

public interface IAiService
{
    Task<string> GetAnswerAsync(string question);
}