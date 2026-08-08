using System.Text.Json.Serialization;

namespace PersonalAiAssistant.Infrastructure.Models
{
    public class ChatCompletionResponse
    {
        public string Id { get; set; } = string.Empty;

        public string Object { get; set; } = string.Empty;

        public long Created { get; set; }

        public string Model { get; set; } = string.Empty;

        public List<Choice> Choices { get; set; } = new();

        [JsonPropertyName("system_fingerprint")]
        public string? SystemFingerprint { get; set; }
    }

    public class Choice
    {
        public int Index { get; set; }

        public ChatCompletionMessage Message { get; set; } = new();

        [JsonPropertyName("finish_reason")]
        public string FinishReason { get; set; } = string.Empty;
    }
}
