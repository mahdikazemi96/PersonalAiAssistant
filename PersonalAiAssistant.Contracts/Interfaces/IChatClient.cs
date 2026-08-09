using PersonalAiAssistant.Contracts.Models;

namespace PersonalAiAssistant.Contracts.Interfaces
{
    public interface IChatClient
    {
        Task<string> ChatAsync(
            IEnumerable<ChatMessage> messages,
            ResponseFormat? responseFormat = null);
    }
}
