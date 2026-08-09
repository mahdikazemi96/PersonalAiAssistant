namespace PersonalAiAssistant.Contracts.Models
{
    public class SearchResult
    {
        public string Text { get; set; } = string.Empty;

        public string FileName { get; set; } = string.Empty;

        public int ChunkNumber { get; set; }

        public float Score { get; set; }
    }
}
