using PersonalAiAssistant.Models;

public class TableSchema
{
    public string Schema { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public List<ColumnSchema> Columns { get; set; } = new();
}