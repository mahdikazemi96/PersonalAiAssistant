using PersonalAiAssistant.Models;
using System.Data;

namespace PersonalAiAssistant.Tools;

public class CalculatorTool : ITool
{
    public string Name => "calculator";

    public string Description =>
        "Evaluates simple mathematical expressions.";
    public Task<string?> GetContextAsync()
    {
        return Task.FromResult<string?>(null);
    }
    public Task<ToolExecutionResult> ExecuteAsync(
        string arguments)
    {
        var table = new DataTable();

        var value =
            table.Compute(arguments, null);

        return Task.FromResult(
            new ToolExecutionResult
            {
                Content = value.ToString() ?? string.Empty,
                ContentType = "text/plain"
            });
    }
}