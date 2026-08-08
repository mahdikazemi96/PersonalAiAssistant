using Microsoft.AspNetCore.Mvc;
using PersonalAiAssistant.Contracts.Models;
using PersonalAiAssistant.Engine;
using System.Threading.Tasks;

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
    public async Task<ActionResult<ChatResponse>> Chat(string message)
    {
        var answer =
            await _assistantService.AskAsync(message);

        return Ok(answer);
    }

    [HttpDelete("history")]
    public IActionResult ClearHistory()
    {
        _conversationService.Clear();

        return Ok();
    }
}