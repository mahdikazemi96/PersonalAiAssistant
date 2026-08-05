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
        IEnumerable<ChatMessage> conversation)
    {
        //------------------------------------------
        // Ask LLM
        //------------------------------------------

        var answer =
            await _chatClient.ChatAsync(
                conversation);

        //------------------------------------------
        // Response
        //------------------------------------------

        return new ChatResponse
        {
            Answer = answer
        };
    }
}