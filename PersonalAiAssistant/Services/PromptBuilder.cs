using System.Text;
using PersonalAiAssistant.Models;

namespace PersonalAiAssistant.Services;

public class PromptBuilder
{
    public List<ChatMessage> Build(
        List<ChatMessage> history,
        List<SearchResult> documents)
    {
        var messages = new List<ChatMessage>();

        var systemPrompt = BuildSystemPrompt(documents);

        messages.Add(new ChatMessage
        {
            Role = "system",
            Content = systemPrompt
        });

        messages.AddRange(history);

        return messages;
    }

    private static string BuildSystemPrompt(
    List<SearchResult> documents)
    {
        var builder = new StringBuilder();

        builder.AppendLine("You are a helpful AI assistant.");

        builder.AppendLine();

        builder.AppendLine("Rules:");

        builder.AppendLine("- Use the provided context as your primary source of information.");

        builder.AppendLine("- If the context contains the answer, base your response on it.");

        builder.AppendLine("- If the context does not contain enough information, clearly tell the user that the uploaded documents do not contain the answer, then answer using your general knowledge.");

        builder.AppendLine("- Clearly distinguish between information taken from the uploaded documents and information based on your own knowledge.");

        builder.AppendLine("- Never claim that information came from the documents if it did not.");

        builder.AppendLine("- If you are uncertain about information outside the provided context, explicitly say so.");

        builder.AppendLine("- Do not mention the context or uploaded documents unless it is relevant to your answer.");

        builder.AppendLine("- Keep your answers clear, accurate and concise.");

        builder.AppendLine();

        builder.AppendLine("Context:");

        builder.AppendLine();

        if (documents.Count == 0)
        {
            builder.AppendLine("No relevant documents were found.");

            return builder.ToString();
        }

        for (var i = 0; i < documents.Count; i++)
        {
            builder.AppendLine(
                $"========== Document {i + 1} ==========");

            builder.AppendLine();

            builder.AppendLine(documents[i].Text);

            builder.AppendLine();
        }

        return builder.ToString();
    }
}