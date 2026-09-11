namespace BrightProgramming.AiSupportAssistant.Api.Ai;

public interface IAiProvider
{
    string Name { get; }

    Task<string> GetAnswerAsync(string question);
}