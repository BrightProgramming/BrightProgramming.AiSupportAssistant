using BrightProgramming.AiSupportAssistant.Api.Controllers;
using BrightProgramming.AiSupportAssistant.Api.Mappers;
using BrightProgramming.AiSupportAssistant.Api.Models;
using BrightProgramming.AiSupportAssistant.Api.Models.Api;
using BrightProgramming.AiSupportAssistant.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BrightProgramming.AiSupportAssistant.Api.UnitTests.Controllers;

public class SupportControllerTests
{
    [Fact]
    public async Task AskAsync_MapsRequestCallsServiceAndReturnsMappedResponse()
    {
        var request = new AskRequest { Question = "question" };
        var supportRequest = new SupportRequest { Question = "question" };
        var supportResponse = new SupportResponse { Question = "question", Answer = "answer" };
        var response = new AskResponse { Question = "question", Answer = "answer" };
        using var cancellation = new CancellationTokenSource();
        var mapper = new Mock<ISupportApiMapper>();
        mapper.Setup(x => x.ToSupportRequest(request)).Returns(supportRequest);
        mapper.Setup(x => x.ToAskResponse(supportResponse)).Returns(response);
        var service = new Mock<ISupportService>();
        service.Setup(x => x.GetAnswerAsync(supportRequest, cancellation.Token)).ReturnsAsync(supportResponse);
        var controller = new SupportController(service.Object, mapper.Object);

        var result = await controller.AskAsync(request, cancellation.Token);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Same(response, okResult.Value);
        mapper.Verify(x => x.ToSupportRequest(request), Times.Once);
        service.Verify(x => x.GetAnswerAsync(supportRequest, cancellation.Token), Times.Once);
        mapper.Verify(x => x.ToAskResponse(supportResponse), Times.Once);
    }

    [Fact]
    public async Task AskAsync_WhenServiceFails_PropagatesExceptionAndDoesNotMapResponse()
    {
        var request = new AskRequest { Question = "question" };
        var supportRequest = new SupportRequest { Question = "question" };
        var failure = new InvalidOperationException("service failed");
        using var cancellation = new CancellationTokenSource();
        var mapper = new Mock<ISupportApiMapper>();
        mapper.Setup(x => x.ToSupportRequest(request)).Returns(supportRequest);
        var service = new Mock<ISupportService>();
        service.Setup(x => x.GetAnswerAsync(supportRequest, cancellation.Token))
            .ThrowsAsync(failure);
        var controller = new SupportController(service.Object, mapper.Object);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => controller.AskAsync(
            request,
            cancellation.Token));

        Assert.Same(failure, exception);
        mapper.Verify(x => x.ToAskResponse(It.IsAny<SupportResponse>()), Times.Never);
    }
}
