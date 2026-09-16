using Microsoft.AspNetCore.Mvc;
using PersonalAiAssistant.Contracts.Models;
using PersonalAiAssistant.Engine;
using System.Threading.Tasks;

namespace PersonalAiAssistant.Controllers;

[ApiController]
[Route("api/chat")]
public class ChatController : ControllerBase
{
    private readonly RagService _ragService;
    private readonly AssistantService _assistantService;
    private readonly ConversationService _conversationService;

    public ChatController(
        RagService ragService,
        AssistantService assistantService,
        ConversationService conversationService)
    {
        _ragService = ragService;
        _assistantService = assistantService;
        _conversationService = conversationService;
    }

    [HttpPost]
    public async Task<ActionResult<ChatResponse>> Chat(string message)
    {
        var ragAnswer = await _ragService.AskAsync(message);

        if (ragAnswer != null)
            return Ok(ragAnswer);

        var toolAgentAswer =
            await _assistantService.AskAsync(message);

        return Ok(toolAgentAswer);
    }

    [HttpDelete("history")]
    public IActionResult ClearHistory()
    {
        _conversationService.Clear();

        return Ok();
    }
}