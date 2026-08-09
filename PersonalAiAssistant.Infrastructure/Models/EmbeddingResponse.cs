using System.Text.Json.Serialization;

namespace PersonalAiAssistant.Infrastructure.Models
{
    public class EmbeddingResponse
    {
        public List<EmbeddingData> Data { get; set; } = new();
    }

    public class EmbeddingData
    {
        [JsonPropertyName("embedding")]
        public float[] Embedding { get; set; } = Array.Empty<float>();

        [JsonPropertyName("index")]
        public int Index { get; set; }
    }
}
