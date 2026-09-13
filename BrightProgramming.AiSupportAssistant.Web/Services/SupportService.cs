using System.Net.Http.Json;
using BrightProgramming.AiSupportAssistant.Web.Models;

namespace BrightProgramming.AiSupportAssistant.Web.Services;

public class SupportService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public SupportService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<SupportResponse?> AskAsync(string question)
    {
        var client = _httpClientFactory.CreateClient(
            "AiSupportAssistantApi");

        var response = await client.PostAsJsonAsync(
            "/Support",
            new SupportRequest
            {
                Question = question
            });

        response.EnsureSuccessStatusCode();

        return await response.Content
            .ReadFromJsonAsync<SupportResponse>();
    }
}