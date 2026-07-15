using Microsoft.AspNetCore.Mvc;
using PersonalAiAssistant.Models;
using PersonalAiAssistant.Services;

namespace PersonalAiAssistant.Controllers;

[ApiController]
[Route("api/chat")]
public class ChatController : ControllerBase
{
    private readonly AssistantService _assistantService;
    private readonly ConversationService _conversationService;

    public ChatController(
        AssistantService assistantService,
        ConversationService conversationService)
    {
        _assistantService = assistantService;
        _conversationService = conversationService;
    }

    [HttpPost]
    public async Task<ActionResult<ChatResponse>> Chat(ChatRequest request)
    {
        var answer =
            await _assistantService.AskAsync(request.Message);

        return Ok(new ChatResponse
        {
            Answer = answer.Answer,
            Sources = answer.Sources
        });
    }

    [HttpDelete("history")]
    public IActionResult ClearHistory()
    {
        _conversationService.Clear();

        return Ok();
    }
}