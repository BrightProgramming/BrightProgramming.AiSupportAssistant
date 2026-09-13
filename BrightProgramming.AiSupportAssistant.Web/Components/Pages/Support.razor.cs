using BrightProgramming.AiSupportAssistant.Web.Services;
using Microsoft.AspNetCore.Components;

namespace BrightProgramming.AiSupportAssistant.Web.Components.Pages;

public partial class Support
{
    [Inject]
    private SupportService SupportService { get; set; } = default!;

    protected string Question { get; set; } = string.Empty;

    protected string Answer { get; set; } = string.Empty;

    protected string Error { get; set; } = string.Empty;

    protected bool IsBusy { get; set; }

    protected async Task AskAsync()
    {
        if (string.IsNullOrWhiteSpace(Question))
        {
            Error = "Please enter a question.";
            Answer = string.Empty;
            return;
        }

        IsBusy = true;
        Answer = string.Empty;
        Error = string.Empty;

        try
        {
            var result = await SupportService.AskAsync(Question);

            Answer = result?.Answer ?? "No answer was returned.";
        }
        catch
        {
            Error = "Unable to get an answer from the AI Support Assistant.";
        }
        finally
        {
            IsBusy = false;
        }
    }
}