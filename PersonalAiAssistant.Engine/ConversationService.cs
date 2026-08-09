using PersonalAiAssistant.Contracts.Models;

namespace PersonalAiAssistant.Engine;

public class ConversationService
{
    public IReadOnlyList<ChatMessage> Conversation => _conversation;

    private readonly List<ChatMessage> _conversation = new();

    public void AddSystemMessage(string message)
    {
        _conversation.Add(new ChatMessage
        {
            Role = "system",
            Content = message
        });
    }
    public void AddUserMessage(string message)
    {
        _conversation.Add(new ChatMessage
        {
            Role = "user",
            Content = message
        });
    }
    public void AddAssistantMessage(string message)
    {
        _conversation.Add(new ChatMessage
        {
            Role = "assistant",
            Content = message
        });
    }
    public void AddToolMessage(string message)
    {
        _conversation.Add(new ChatMessage
        {
            Role = "tool",
            Content = message
        });
    }

    public void Clear()
    {
        _conversation.RemoveRange(1, _conversation.Count - 1);
    }
}