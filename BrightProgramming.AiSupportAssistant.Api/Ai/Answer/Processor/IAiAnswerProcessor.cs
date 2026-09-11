namespace BrightProgramming.AiSupportAssistant.Api.Ai.Answer.Processor;

public interface IAiAnswerProcessor
{
    Task<string> GetAnswerAsync(
        string question,
        CancellationToken cancellationToken);
}