using System.ComponentModel.DataAnnotations;

namespace BrightProgramming.AiSupportAssistant.Api.Models.Api;

public sealed class AskRequest
{
    [Required]
    public string Question { get; set; } = string.Empty;
}