using System.Text;
using PersonalAiAssistant.Models;

namespace PersonalAiAssistant.Services;

public class ToolExecutionService
{
    private readonly ToolRouter _toolRouter;

    public ToolExecutionService(
        ToolRouter toolRouter)
    {
        _toolRouter = toolRouter;
    }

    public async Task ExecuteAsync(
        AgentContext context,
        AgentAction action)
    {
        //------------------------------------------
        // Validation
        //------------------------------------------

        if (action.Action != AgentActionType.Tool)
        {
            throw new InvalidOperationException(
                "Planner returned a non-tool action.");
        }

        if (string.IsNullOrWhiteSpace(action.Tool))
        {
            throw new InvalidOperationException(
                "Planner did not specify a tool.");
        }

        //------------------------------------------
        // Execute Tool
        //------------------------------------------

        var result =
            await _toolRouter.ExecuteAsync(
                action.Tool,
                action.Arguments ?? string.Empty);

        //------------------------------------------
        // Save Tool Call
        //------------------------------------------

        context.Conversation.Add(
            new ChatMessage
            {
                Role = "assistant",
                Content = BuildToolCallMessage(action)
            });

        //------------------------------------------
        // Save Tool Result
        //------------------------------------------

        context.Conversation.Add(
            new ChatMessage
            {
                Role = "tool",
                Content = BuildToolResultMessage(
                    action.Tool,
                    result.Content)
            });
    }

    private static string BuildToolCallMessage(
        AgentAction action)
    {
        var builder = new StringBuilder();

        builder.AppendLine("Tool Executed");

        builder.AppendLine();

        builder.AppendLine($"Tool: {action.Tool}");

        if (!string.IsNullOrWhiteSpace(action.Arguments))
        {
            builder.AppendLine();

            builder.AppendLine("Arguments:");

            builder.AppendLine(action.Arguments);
        }

        return builder.ToString();
    }

    private static string BuildToolResultMessage(
        string tool,
        string result)
    {
        var builder = new StringBuilder();

        builder.AppendLine("Tool Result");

        builder.AppendLine();

        builder.AppendLine($"Tool: {tool}");

        builder.AppendLine();

        builder.AppendLine(result);

        return builder.ToString();
    }
}