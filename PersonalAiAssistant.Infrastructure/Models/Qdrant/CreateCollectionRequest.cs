using System.Text.Json.Serialization;

namespace PersonalAiAssistant.Infrastructure.Models.Qdrant;

public class CreateCollectionRequest
{
    [JsonPropertyName("vectors")]
    public VectorConfiguration Vectors { get; set; } = new();
}

public class VectorConfiguration
{
    [JsonPropertyName("size")]
    public int Size { get; set; }

    [JsonPropertyName("distance")]
    public string Distance { get; set; } = "Cosine";
}