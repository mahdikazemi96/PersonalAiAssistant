using PersonalAiAssistant.Models;

namespace PersonalAiAssistant.Services;

public class PromptBuilder
{
    public List<ChatMessage> Build(
        List<ChatMessage> history,
        List<SearchResult> documents)
    {
        var messages = new List<ChatMessage>();

        messages.Add(new ChatMessage
        {
            Role = "system",
            Content = "You are a helpful AI assistant."
        });

        if (documents.Any())
        {
            var context = string.Join(
                Environment.NewLine + Environment.NewLine,
                documents.Select(x => x.Text));

            messages.Add(new ChatMessage
            {
                Role = "system",
                Content =
                    $"""
                    Use the following context to answer the user's question.
                    
                    Context:
                    
                    {context}
                    
                    If the answer cannot be found in the context,
                    you may answer using your own knowledge.
                    """
            });
        }

        messages.AddRange(history);

        return messages;
    }
}