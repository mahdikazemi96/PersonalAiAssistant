using System.Text;
using PersonalAiAssistant.Models;
using PersonalAiAssistant.Tools;

namespace PersonalAiAssistant.Services;

public class ToolAgentPromptBuilder
{
    public async Task<List<ChatMessage>> BuildMessagesAsync(
        IReadOnlyList<ChatMessage> conversation,
        IReadOnlyList<ITool> tools)
    {
        var messages = new List<ChatMessage>();

        messages.Add(new ChatMessage
        {
            Role = "system",
            Content = await BuildSystemPrompt(tools)
        });

        messages.AddRange(conversation);

        return messages;
    }

    private async Task<string> BuildSystemPrompt(
        IReadOnlyList<ITool> tools)
    {
        var builder = new StringBuilder();

        builder.AppendLine("You are an AI Planning Agent.");

        builder.AppendLine();

        builder.AppendLine("Your responsibility is to decide ONLY the next action.");

        builder.AppendLine();

        builder.AppendLine("The conversation already contains:");

        builder.AppendLine("- User requests");

        builder.AppendLine("- Previous tool executions");

        builder.AppendLine("- Previous tool results");

        builder.AppendLine();

        builder.AppendLine("You may execute multiple tools.");

        builder.AppendLine();

        builder.AppendLine("Never execute the same tool with the same arguments twice unless it is absolutely necessary.");

        builder.AppendLine();

        builder.AppendLine("If another tool is required return:");

        builder.AppendLine();

        builder.AppendLine("{");

        builder.AppendLine("  \"action\":\"Tool\",");

        builder.AppendLine("  \"tool\":\"tool-name\",");

        builder.AppendLine("  \"arguments\":\"tool arguments\"");

        builder.AppendLine("}");

        builder.AppendLine();

        builder.AppendLine("If enough information has been gathered return:");

        builder.AppendLine();

        builder.AppendLine("{");

        builder.AppendLine("  \"action\":\"Answer\",");

        builder.AppendLine("  \"tool\":null,");

        builder.AppendLine("  \"arguments\":null");

        builder.AppendLine("}");

        builder.AppendLine();

        builder.AppendLine("Return ONLY the JSON object.");

        builder.AppendLine();

        builder.AppendLine("Available tools:");

        builder.AppendLine();

        foreach (var tool in tools)
        {
            builder.AppendLine($"- {tool.Name}");

            builder.AppendLine($"  {tool.Description}");

            var context = await tool.GetContextAsync();

            if (!string.IsNullOrWhiteSpace(context))
            {
                builder.AppendLine();

                builder.AppendLine("Context:");

                builder.AppendLine(context);
            }

            builder.AppendLine();
        }

        return builder.ToString();
    }
}