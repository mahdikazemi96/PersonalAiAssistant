using PersonalAiAssistant.Clients;
using PersonalAiAssistant.Models;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PersonalAiAssistant.Services;

public class PlannerService
{
    private readonly IChatClient _chatClient;

    public PlannerService(
        IChatClient chatClient)
    {
        _chatClient = chatClient;
    }

    public async Task<AgentAction> PlanAsync(
        IEnumerable<ChatMessage> conversation)
    {
        //------------------------------------------
        // Ask LLM
        //------------------------------------------

        var response =
            await _chatClient.ChatAsync(
                conversation,
                AgentAction.ResponseFormat);

        //------------------------------------------
        // Deserialize
        //------------------------------------------

        var deserializedAction = Desrialize(response);

        //------------------------------------------
        // Invalid Response
        //------------------------------------------

        var action = HandleNullAction(deserializedAction);

        return action;
    }

    private AgentAction Desrialize(string obj)
    {
        AgentAction? action;

        try
        {
            action =
                JsonSerializer.Deserialize<AgentAction>(
                    obj,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        Converters =
                        {
                            new JsonStringEnumConverter()
                        }
                    });
        }
        catch
        {
            return new AgentAction
            {
                Action = AgentActionType.Answer
            };
        }

        return action;
    }

    private AgentAction HandleNullAction(AgentAction action)
    {
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