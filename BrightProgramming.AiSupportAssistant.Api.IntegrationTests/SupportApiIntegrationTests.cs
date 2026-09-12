using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using BrightProgramming.AiSupportAssistant.Api.Ai.Answer.Processor;
using BrightProgramming.AiSupportAssistant.Api.Ai.Exceptions;
using BrightProgramming.AiSupportAssistant.Api.Knowledge.Models;
using BrightProgramming.AiSupportAssistant.Api.Knowledge.Processor;
using BrightProgramming.AiSupportAssistant.Api.Mappers;
using BrightProgramming.AiSupportAssistant.Api.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace BrightProgramming.AiSupportAssistant.Api.IntegrationTests;

public class SupportApiIntegrationTests
{
    [Fact]
    public async Task PostSupport_WhenDependenciesSucceed_ReturnsMappedAnswer()
    {
        using var factory = new TestApplicationFactory(
            new StubKnowledgeProcessor(),
            new StubAnswerProcessor("Use dependency injection."));

        using var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync(
            "/Support",
            new { Question = "How does DI work?" });

        var body = await response.Content.ReadFromJsonAsync<JsonElement>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(
            "How does DI work?",
            body.GetProperty("question").GetString());
        Assert.Equal(
            "Use dependency injection.",
            body.GetProperty("answer").GetString());
    }

    [Fact]
    public async Task PostSupport_WhenRequestIsInvalid_ReturnsBadRequestProblemDetails()
    {
        using var factory = new TestApplicationFactory(
            new StubKnowledgeProcessor(),
            new StubAnswerProcessor("unused"));

        using var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync(
            "/Support",
            new { });

        var body = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Contains("Question", body, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task PostSupport_WhenAiProviderFails_ReturnsServiceUnavailableProblemDetails()
    {
        using var factory = new TestApplicationFactory(
            new StubKnowledgeProcessor(),
            new StubAnswerProcessor(
                new AiProviderException("provider failed")));

        using var client = factory.CreateClient();

        client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

        var response = await client.PostAsJsonAsync(
            "/Support",
            new { Question = "How does DI work?" });

        var body = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.ServiceUnavailable, response.StatusCode);
        Assert.Contains("AI provider unavailable", body);
        Assert.Contains(
            "The AI service is currently unavailable.",
            body);
    }

    [Fact]
    public void ApplicationStartup_ResolvesApplicationServicesFromDI()
    {
        using var factory = new TestApplicationFactory(
            new StubKnowledgeProcessor(),
            new StubAnswerProcessor("answer"));

        Assert.NotNull(
            factory.Services.GetRequiredService<ISupportService>());

        Assert.NotNull(
            factory.Services.GetRequiredService<ISupportApiMapper>());
    }

    private sealed class TestApplicationFactory(
        IKnowledgeProcessor knowledgeProcessor,
        IAiAnswerProcessor answerProcessor)
        : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureLogging(logging =>
                logging.ClearProviders());

            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll<IKnowledgeProcessor>();
                services.RemoveAll<IAiAnswerProcessor>();

                services.AddSingleton(knowledgeProcessor);
                services.AddSingleton(answerProcessor);
            });
        }
    }

    private sealed class StubKnowledgeProcessor : IKnowledgeProcessor
    {
        public Task<KnowledgeContext> GetRelevantKnowledgeAsync(
            string question,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(new KnowledgeContext());
        }
    }

    private sealed class StubAnswerProcessor : IAiAnswerProcessor
    {
        private readonly string? _answer;
        private readonly Exception? _exception;

        public StubAnswerProcessor(string answer)
        {
            _answer = answer;
        }

        public StubAnswerProcessor(Exception exception)
        {
            _exception = exception;
        }

        public Task<string> GetAnswerAsync(
            string question,
            KnowledgeContext knowledge,
            CancellationToken cancellationToken)
        {
            if (_exception is not null)
            {
                return Task.FromException<string>(_exception);
            }

            return Task.FromResult(_answer!);
        }
    }
}