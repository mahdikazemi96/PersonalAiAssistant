using PersonalAiAssistant.Models;

namespace PersonalAiAssistant.Tools;

public interface ITool
{
    string Name { get; }

    string Description { get; }

    Task<string?> GetContextAsync();

    Task<ToolExecutionResult> ExecuteAsync(
        string arguments);
}