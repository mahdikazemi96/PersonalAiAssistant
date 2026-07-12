using Microsoft.AspNetCore.Mvc;
using PersonalAiAssistant.Clients;
using PersonalAiAssistant.Models;

namespace PersonalAiAssistant.Controllers;

[ApiController]
[Route("api/chat")]
public class ChatController : ControllerBase
{
    private readonly IChatClient _chatClient;

    public ChatController(IChatClient chatClient)
    {
        _chatClient = chatClient;
    }

    [HttpPost]
    public async Task<ActionResult<ChatResponse>> Chat(ChatRequest request)
    {
        var messages = new List<ChatMessage>
        {
            new()
            {
                Role = "system",
                Content = "You are a helpful AI assistant."
            },
            new()
            {
                Role = "user",
                Content = request.Message
            }
        };

        var answer = await _chatClient.ChatAsync(messages);

        return Ok(new ChatResponse
        {
            Answer = answer
        });
    }
}