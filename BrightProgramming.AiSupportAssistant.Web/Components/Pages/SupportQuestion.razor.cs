using Microsoft.AspNetCore.Components;

namespace BrightProgramming.AiSupportAssistant.Web.Components.Pages;

public partial class SupportQuestion
{
    [Parameter]
    public string Question { get; set; } = string.Empty;

    [Parameter]
    public EventCallback<string> QuestionChanged { get; set; }

    [Parameter]
    public bool IsBusy { get; set; }

    [Parameter]
    public EventCallback OnAsk { get; set; }

    private async Task AskAsync()
    {
        await QuestionChanged.InvokeAsync(Question);
        await OnAsk.InvokeAsync();
    }
}