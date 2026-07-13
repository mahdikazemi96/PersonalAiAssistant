using System.Text.Json.Serialization;

namespace PersonalAiAssistant.Qdrant;

public class SearchRequest
{
    [JsonPropertyName("vector")]
    public float[] Vector { get; set; } = Array.Empty<float>();

    [JsonPropertyName("limit")]
    public int Limit { get; set; } = 5;

    [JsonPropertyName("with_payload")]
    public bool WithPayload { get; set; } = true;
}