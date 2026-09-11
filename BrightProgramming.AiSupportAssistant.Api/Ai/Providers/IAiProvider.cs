namespace BrightProgramming.AiSupportAssistant.Api.Ai.Providers;

public interface IAiProvider
{
    string Name { get; }

    Task<string> GetAnswerAsync(string question, CancellationToken cancellationToken);
}