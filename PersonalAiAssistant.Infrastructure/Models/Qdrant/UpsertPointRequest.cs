using System.Text.Json.Serialization;

namespace PersonalAiAssistant.Infrastructure.Models.Qdrant;

public class UpsertPointRequest
{
    [JsonPropertyName("points")]
    public List<QdrantPoint> Points { get; set; } = new();
}

public class QdrantPoint
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [JsonPropertyName("vector")]
    public float[] Vector { get; set; } = Array.Empty<float>();

    [JsonPropertyName("payload")]
    public Dictionary<string, object> Payload { get; set; } = new();
}