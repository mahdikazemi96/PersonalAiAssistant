using PersonalAiAssistant.Models;

namespace PersonalAiAssistant.Services;

public class PromptBuilder
{
    public List<ChatMessage> Build(List<ChatMessage> history, List<string> documents)
    {
        var messages = new List<ChatMessage>();

        // System Prompt اصلی
        messages.Add(new ChatMessage
        {
            Role = "system",
            Content = "You are a helpful AI assistant."
        });

        // Context
        if (documents.Any())
        {
            messages.Add(new ChatMessage
            {
                Role = "system",
                Content =
                    $"""
                    Use the following information when answering.
                    
                    Context:
                    
                    {string.Join(Environment.NewLine, documents)}
                    
                    If the answer is not in the context,
                    you may answer from your own knowledge.
                    """
            });
        }

        // Conversation History
        messages.AddRange(
            history.Where(x => x.Role != "system"));

        return messages;
    }
}