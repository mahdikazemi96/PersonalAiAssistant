using System.Text;
using PersonalAiAssistant.Models;

namespace PersonalAiAssistant.Services;

public class SqlPromptBuilder
{
    public string Build(
        string question,
        DatabaseSchema schema)
    {
        var builder = new StringBuilder();

        //------------------------------------
        // Role
        //------------------------------------

        builder.AppendLine(
            "You are an expert SQL Server developer.");

        builder.AppendLine();

        builder.AppendLine(
            "Generate a SQL Server query.");

        builder.AppendLine();

        //------------------------------------
        // Rules
        //------------------------------------

        builder.AppendLine(
            "Rules:");

        builder.AppendLine(
            "- Generate a SQL Server SELECT statement.");

        builder.AppendLine(
            "- Return a normal SELECT statement only.");

        builder.AppendLine(
            "- Generate ONLY one query.");

        builder.AppendLine(
            "- Use only SELECT statements.");

        builder.AppendLine(
            "- Never generate INSERT.");

        builder.AppendLine(
            "- Never generate UPDATE.");

        builder.AppendLine(
            "- Never generate DELETE.");

        builder.AppendLine(
            "- Never generate DROP.");

        builder.AppendLine(
            "- Never generate ALTER.");

        builder.AppendLine(
            "- Never generate CREATE.");

        builder.AppendLine(
            "- Never generate EXEC.");

        builder.AppendLine(
            "- Never use FOR JSON.");

        builder.AppendLine(
            "- Never use FOR XML.");

        builder.AppendLine(
            "- Use only tables and columns from the schema below.");

        builder.AppendLine();

        //------------------------------------
        // Schema
        //------------------------------------

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

        //------------------------------------
        // User Question
        //------------------------------------

        builder.AppendLine(
            "User Question:");

        builder.AppendLine(question);

        return builder.ToString();
    }
}