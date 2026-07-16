using PersonalAiAssistant.Models;
using PersonalAiAssistant.Models.Llm;

namespace PersonalAiAssistant.Clients;

public interface IChatClient
{
    Task<string> ChatAsync(
        List<ChatMessage> messages,
        ResponseFormat? responseFormat = null);
}