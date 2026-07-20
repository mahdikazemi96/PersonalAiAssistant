using System.Text.Json;
using PersonalAiAssistant.Clients;
using PersonalAiAssistant.Models;

namespace PersonalAiAssistant.Services;

public class PlannerService
{
    private readonly ToolAgentPromptBuilder _promptBuilder;
    private readonly ToolRouter _toolRouter;
    private readonly IChatClient _chatClient;

    public PlannerService(
        ToolAgentPromptBuilder promptBuilder,
        ToolRouter toolRouter,
        IChatClient chatClient)
    {
        _promptBuilder = promptBuilder;
        _toolRouter = toolRouter;
        _chatClient = chatClient;
    }

    public async Task<AgentAction> PlanAsync(
        AgentContext context)
    {
        //------------------------------------------
        // Build Prompt
        //------------------------------------------

        var messages =
            await _promptBuilder.BuildMessagesAsync(
                context.Conversation,
                _toolRouter.GetTools().ToList());

        //------------------------------------------
        // Ask LLM
        //------------------------------------------

        var response =
            await _chatClient.ChatAsync(
                messages,
                AgentAction.ResponseFormat);

        //------------------------------------------
        // Deserialize
        //------------------------------------------

        AgentAction? action;

        try
            {
            action =
                JsonSerializer.Deserialize<AgentAction>(
                    response,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
        }
        catch
        {
            return new AgentAction
            {
                Action = AgentActionType.Answer
            };
        }

        //------------------------------------------
        // Invalid Response
        //------------------------------------------

        if (action == null)
        {
            return new AgentAction
            {
                Action = AgentActionType.Answer
            };
        }

        return action;
    }
}