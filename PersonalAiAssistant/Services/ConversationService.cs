using PersonalAiAssistant.Models;

namespace PersonalAiAssistant.Services;

public class ConversationService
{
    private readonly List<ChatMessage> _messages = new();

    public void AddUserMessage(string message)
    {
        _messages.Add(new ChatMessage
        {
            Role = "user",
            Content = message
        });
    }

    public void AddAssistantMessage(string message)
    {
        _messages.Add(new ChatMessage
        {
            Role = "assistant",
            Content = message
        });
    }

    public List<ChatMessage> GetMessages()
    {
        return _messages.ToList();
    }

    public void Clear()
    {
        _messages.RemoveRange(1, _messages.Count - 1);
    }
}