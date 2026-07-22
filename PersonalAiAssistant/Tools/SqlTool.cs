using PersonalAiAssistant.Models;
using PersonalAiAssistant.Services;

namespace PersonalAiAssistant.Tools;

public class SqlTool : ITool
{
    private readonly ISqlToolService _sqlToolService;
    private readonly IDatabaseSchemaReader _schemaReader;

    public SqlTool(
        ISqlToolService sqlToolService,
        IDatabaseSchemaReader schemaReader)
    {
        _sqlToolService = sqlToolService;
        _schemaReader = schemaReader;
    }

    public string Name => "sql";

    public string Description =>
        "Execute read-only SQL queries against SQL Server.";
    public async Task<string?> GetContextAsync()
    {
        var schema =
            await _schemaReader.ReadAsync();

        return DatabaseSchemaFormatter.Format(schema);
    }
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