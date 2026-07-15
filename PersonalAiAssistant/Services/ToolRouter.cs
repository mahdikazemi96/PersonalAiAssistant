using PersonalAiAssistant.Models;
using PersonalAiAssistant.Tools;

namespace PersonalAiAssistant.Services;

public class ToolRouter
{
    private readonly Dictionary<string, ITool> _tools;

    public ToolRouter(
        IEnumerable<ITool> tools)
    {
        _tools =
            tools.ToDictionary(
                x => x.Name,
                StringComparer.OrdinalIgnoreCase);
    }

    public IReadOnlyCollection<ITool> GetTools()
    {
        return _tools.Values.ToList();
    }

    public async Task<ToolExecutionResult> ExecuteAsync(
        string toolName,
        string arguments)
    {
        if (!_tools.TryGetValue(toolName, out var tool))
        {
            throw new InvalidOperationException(
                $"Tool '{toolName}' not found.");
        }

        return await tool.ExecuteAsync(arguments);
    }
}