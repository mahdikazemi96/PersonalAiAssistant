namespace PersonalAiAssistant.Contracts
{
    public class ToolExecutionResult
    {
        public string Content { get; set; } = string.Empty;

        public string ContentType { get; set; } = "text/plain";
    }
}
