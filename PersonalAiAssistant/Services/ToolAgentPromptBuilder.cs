using PersonalAiAssistant.Models;
using PersonalAiAssistant.Tools;
using System.Text;

namespace PersonalAiAssistant.Services;

public class ToolAgentPromptBuilder
{
    public string BuildSelectionPrompt(
        string question,
        IReadOnlyCollection<ITool> tools)
    {
        var builder = new StringBuilder();

        builder.AppendLine(
            "You are an AI agent.");

        builder.AppendLine();

        builder.AppendLine(
            "Your task is ONLY to decide whether a tool should be used.");

        builder.AppendLine();

        builder.AppendLine(
            "Available tools:");

        builder.AppendLine();

        foreach (var tool in tools)
        {
            builder.AppendLine($"Name: {tool.Name}");

            builder.AppendLine($"Description: {tool.Description}");

            builder.AppendLine();
        }

        builder.AppendLine(
            "If no tool is needed respond exactly with:");

        builder.AppendLine();

        builder.AppendLine("NONE");

        builder.AppendLine();

        builder.AppendLine(
            "Otherwise respond ONLY with valid JSON in this format:");

        builder.AppendLine();

        builder.AppendLine(
@"{
  ""tool"":""calculator"",
  ""arguments"":""25*17""
}");

        builder.AppendLine();

        builder.AppendLine(
            $"User Question: {question}");

        return builder.ToString();
    }

    public string BuildResultPrompt(
        string question,
        string toolName,
        ToolExecutionResult toolResult)
    {
        var builder = new StringBuilder();

        builder.AppendLine(
            "You are an AI assistant.");

        builder.AppendLine();

        builder.AppendLine(
            "A tool has already been executed successfully.");

        builder.AppendLine();

        builder.AppendLine(
            "Generate the final answer for the user.");

        builder.AppendLine();

        builder.AppendLine(
            "Do not mention internal implementation details.");

        builder.AppendLine();

        builder.AppendLine(
            "Do not say that you executed a tool unless the user explicitly asks.");

        builder.AppendLine();

        builder.AppendLine(
            "Use the tool result as the source of truth.");

        builder.AppendLine();

        builder.AppendLine("User Question:");

        builder.AppendLine(question);

        builder.AppendLine();

        builder.AppendLine("Tool:");

        builder.AppendLine(toolName);

        builder.AppendLine();

        builder.AppendLine("Tool Result:");

        builder.AppendLine(toolResult.Content);

        builder.AppendLine();

        builder.AppendLine(
            "Generate the final answer.");

        return builder.ToString();
    }
}