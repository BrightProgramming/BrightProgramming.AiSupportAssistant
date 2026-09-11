using BrightProgramming.AiSupportAssistant.Api.Models;
using BrightProgramming.AiSupportAssistant.Api.Models.Api;

namespace BrightProgramming.AiSupportAssistant.Api.Mappers;

public interface ISupportApiMapper
{
    SupportRequest ToSupportRequest(AskRequest request);

    AskResponse ToAskResponse(SupportResponse response);
}