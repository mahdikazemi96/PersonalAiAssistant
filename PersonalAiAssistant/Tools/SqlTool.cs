using PersonalAiAssistant.Models;
using PersonalAiAssistant.Services;

namespace PersonalAiAssistant.Tools;

public class SqlTool : ITool
{
    private readonly ISqlToolService _sqlToolService;

    public SqlTool(
        ISqlToolService sqlToolService)
    {
        _sqlToolService = sqlToolService;
    }

    public string Name => "sql";

    public string Description =>
        "Execute read-only SQL queries against SQL Server.";

    public async Task<ToolExecutionResult> ExecuteAsync(
        string arguments)
    {
        var result = await _sqlToolService.ExecuteAsync(arguments);

        return new ToolExecutionResult
        {
            Content = result.ToString() ?? string.Empty,
            ContentType = "text/plain"
        };
    }
}