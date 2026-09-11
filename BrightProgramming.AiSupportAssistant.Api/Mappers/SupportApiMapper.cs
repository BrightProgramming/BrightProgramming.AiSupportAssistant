using BrightProgramming.AiSupportAssistant.Api.Models;
using BrightProgramming.AiSupportAssistant.Api.Models.Api;

namespace BrightProgramming.AiSupportAssistant.Api.Mappers;

public class SupportApiMapper : ISupportApiMapper
{
    public SupportRequest ToSupportRequest(AskRequest request)
    {
        return new SupportRequest
        {
            Question = request.Question
        };
    }

    public AskResponse ToAskResponse(SupportResponse response)
    {
        return new AskResponse
        {
            Question = response.Question,
            Answer = response.Answer
        };
    }
}