namespace PersonalAiAssistant.Models;

public class ToolSelectionResult
{
    public bool UseTool { get; set; }

    public string Tool { get; set; } = string.Empty;

    public string Arguments { get; set; } = string.Empty;
}