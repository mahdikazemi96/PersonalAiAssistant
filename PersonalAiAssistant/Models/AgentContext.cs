using PersonalAiAssistant.Models;

namespace PersonalAiAssistant.Services;

public class AgentContext
{
    public List<ChatMessage> Conversation { get; } = new();

    public int Iteration { get; set; }
}