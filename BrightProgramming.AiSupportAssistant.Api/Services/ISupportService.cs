using BrightProgramming.AiSupportAssistant.Api.Models;

namespace BrightProgramming.AiSupportAssistant.Api.Services;

public interface ISupportService
{
    Task<SupportResponse> GetAnswerAsync(SupportRequest request, CancellationToken cancellationToken);
}