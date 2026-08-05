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

    public async Task<string> ExecuteAsync(
        IEnumerable<ChatMessage> conversation,
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

        return result.Content;
    }
}