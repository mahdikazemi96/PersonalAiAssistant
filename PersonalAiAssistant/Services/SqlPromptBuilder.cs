using System.Text;

namespace PersonalAiAssistant.Services;

public class SqlPromptBuilder : IPromptBuilder
{
    private readonly IPromptLoader _promptLoader;
    private readonly IDatabaseSchemaReader _schemaReader;
    public SqlPromptBuilder(IPromptLoader promptLoader, IDatabaseSchemaReader schemaReader)
    {
        _promptLoader = promptLoader;
        _schemaReader = schemaReader;
    }

    public async Task<string> BuildAsync()
    {
        //----------------------------------------------------
        // Main Prmpt
        //----------------------------------------------------

        var prompt = _promptLoader.Load("SqlSystem.txt");

        var builder = new StringBuilder(prompt);

        builder.AppendLine();

        //------------------------------------
        // Schema
        //------------------------------------

        var schema = await _schemaReader.ReadAsync();

        builder.AppendLine(
            "Database Schema:");

        builder.AppendLine();

        foreach (var table in schema.Tables)
        {
            builder.AppendLine(
                $"Table: {table.Schema}.{table.Name}");

            foreach (var column in table.Columns)
            {
                builder.AppendLine(
                    $"    - {column.Name} ({column.DataType})");
            }

            builder.AppendLine();
        }

        return builder.ToString();
    }
}