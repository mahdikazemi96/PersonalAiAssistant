using Microsoft.AspNetCore.Mvc;
using PersonalAiAssistant.Clients;
using PersonalAiAssistant.Models;
using PersonalAiAssistant.Services;

namespace PersonalAiAssistant.Controllers;

[ApiController]
[Route("api/chat")]
public class ChatController : ControllerBase
{
    private readonly IChatClient _chatClient;
    private readonly ConversationService _conversationService;

    public ChatController(
        IChatClient chatClient,
        ConversationService conversationService)
    {
        _chatClient = chatClient;
        _conversationService = conversationService;
    }

    [HttpPost]
    public async Task<ActionResult<ChatResponse>> Chat(ChatRequest request)
    {
        _conversationService.AddUserMessage(request.Message);

        var answer = await _chatClient.ChatAsync(
            _conversationService.GetMessages());

        _conversationService.AddAssistantMessage(answer);

        return Ok(new ChatResponse
        {
            Answer = answer
        });
    }

    [HttpDelete("history")]
    public IActionResult ClearHistory()
    {
        _conversationService.Clear();

        return Ok();
    }
}