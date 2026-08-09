namespace PersonalAiAssistant.Infrastructure.Models
{
    public class EmbeddingRequest
    {
        public string Model { get; set; } = string.Empty;

        public string Input { get; set; } = string.Empty;
    }
}
