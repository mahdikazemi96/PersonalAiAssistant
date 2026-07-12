using PersonalAiAssistant.Models;

namespace PersonalAiAssistant.Clients;

public interface IChatClient
{
    Task<string> ChatAsync(List<ChatMessage> messages);
}