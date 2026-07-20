using PersonalAiAssistant.Clients;
using PersonalAiAssistant.Models;

namespace PersonalAiAssistant.Services;

public class AnswerGenerationService
{
    private readonly IChatClient _chatClient;

    public AnswerGenerationService(
        IChatClient chatClient)
    {
        _chatClient = chatClient;
    }

    public async Task<ChatResponse> GenerateAsync(
        AgentContext context)
    {
        //------------------------------------------
        // Ask LLM
        //------------------------------------------

        var answer =
            await _chatClient.ChatAsync(
                context.Conversation);

        //------------------------------------------
        // Response
        //------------------------------------------

        return new ChatResponse
        {
            Answer = answer
        };
    }
}