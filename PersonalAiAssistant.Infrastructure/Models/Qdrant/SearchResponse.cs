using System.Text.Json.Serialization;

namespace PersonalAiAssistant.Infrastructure.Models.Qdrant;

public class SearchResponse
{
    [JsonPropertyName("result")]
    public List<SearchResult> Result { get; set; } = new();
}

public class SearchResult
{
    [JsonPropertyName("score")]
    public float Score { get; set; }

    [JsonPropertyName("payload")]
    public Dictionary<string, object> Payload { get; set; } = new();
}