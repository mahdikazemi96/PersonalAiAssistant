using PersonalAiAssistant.Contracts.Models;
using System.Text.Json.Serialization;

namespace PersonalAiAssistant.Infrastructure.Models
{
    public class ChatCompletionRequest
    {
        public string Model { get; set; } = string.Empty;

        public double Temperature { get; set; }

        public List<ChatCompletionMessage> Messages { get; set; } = new();

        [JsonPropertyName("response_format")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ResponseFormat? ResponseFormat { get; set; }
    }

    public class ChatCompletionMessage
    {
        public string Role { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;
    }
}
