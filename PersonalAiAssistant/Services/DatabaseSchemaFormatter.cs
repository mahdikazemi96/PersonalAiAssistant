using PersonalAiAssistant.Models;
using System.Text;

namespace PersonalAiAssistant.Services;

public static class DatabaseSchemaFormatter
{
    public static string Format(
        DatabaseSchema schema)
    {
        var builder = new StringBuilder();

        builder.AppendLine("Database Schema");

        builder.AppendLine();

        foreach (var table in schema.Tables)
        {
            builder.AppendLine(
                "==================================");

            builder.AppendLine();

            builder.AppendLine(
                $"Table: {table.Name}");

            builder.AppendLine();

            builder.AppendLine("Columns:");

            builder.AppendLine();

            foreach (var column in table.Columns)
            {
                builder.AppendLine(
                    $"- {column.Name} ({column.DataType})");
            }

            builder.AppendLine();
        }

        return builder.ToString();
    }
}