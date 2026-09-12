using BrightProgramming.AiSupportAssistant.Api.Mappers;
using BrightProgramming.AiSupportAssistant.Api.Models;
using BrightProgramming.AiSupportAssistant.Api.Models.Api;

namespace BrightProgramming.AiSupportAssistant.Api.UnitTests.Mappers;

public class SupportApiMapperTests
{
    private readonly SupportApiMapper _mapper = new();

    [Fact]
    public void ToSupportRequest_MapsQuestion()
    {
        var result = _mapper.ToSupportRequest(new AskRequest { Question = "How does logging work?" });

        Assert.Equal("How does logging work?", result.Question);
    }

    [Fact]
    public void ToAskResponse_MapsQuestionAndAnswer()
    {
        var result = _mapper.ToAskResponse(new SupportResponse
        {
            Question = "How does logging work?",
            Answer = "Use ILogger."
        });

        Assert.Equal("How does logging work?", result.Question);
        Assert.Equal("Use ILogger.", result.Answer);
    }
}
