using BrightProgramming.AiSupportAssistant.Api.Ai.Answer.Processor;
using BrightProgramming.AiSupportAssistant.Api.Models;
using BrightProgramming.AiSupportAssistant.Api.Knowledge.Processor;

namespace BrightProgramming.AiSupportAssistant.Api.Services;

public class SupportService : ISupportService
{
    private readonly IAiAnswerProcessor _aiAnswerProcessor;
    private readonly IKnowledgeProcessor _knowledgeProcessor;

    public SupportService(
        IAiAnswerProcessor aiAnswerProcessor,
        IKnowledgeProcessor knowledgeProcessor)
    {
        _aiAnswerProcessor = aiAnswerProcessor;
        _knowledgeProcessor = knowledgeProcessor;
    }

    public async Task<SupportResponse> GetAnswerAsync(
        SupportRequest request,
        CancellationToken cancellationToken)
    {
        var knowledge = await _knowledgeProcessor.GetRelevantKnowledgeAsync(
            request.Question,
            cancellationToken);

        var answer = await _aiAnswerProcessor.GetAnswerAsync(
            request.Question,
            knowledge,
            cancellationToken);

        return new SupportResponse
        {
            Question = request.Question,
            Answer = answer
        };
    }
}