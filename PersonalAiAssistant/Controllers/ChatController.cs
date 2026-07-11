using Microsoft.AspNetCore.Mvc;
using PersonalAiAssistant.Models;
using PersonalAiAssistant.Services;

namespace PersonalAiAssistant.Controllers;

[ApiController]
[Route("api/chat")]
public class ChatController : ControllerBase
{
    private readonly ILlmService _llmService;

    public ChatController(ILlmService llmService)
    {
        _llmService = llmService;
    }

    [HttpPost]
    public async Task<ActionResult<ChatResponse>> Chat(ChatRequest request)
    {
        var answer = await _llmService.AskAsync(request.Message);

        return Ok(new ChatResponse
        {
            Answer = answer
        });
    }
}