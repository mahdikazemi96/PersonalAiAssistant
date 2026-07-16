using System.Text.Json.Serialization;

namespace PersonalAiAssistant.Models.Llm;

public class ResponseFormat
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "json_schema";

    [JsonPropertyName("json_schema")]
    public JsonSchemaDefinition JsonSchema { get; set; } = new();
}

public class JsonSchemaDefinition
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("strict")]
    public bool Strict { get; set; } = true;

    [JsonPropertyName("schema")]
    public object Schema { get; set; } = default!;
}