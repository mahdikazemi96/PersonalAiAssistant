using PersonalAiAssistant.Models;

namespace PersonalAiAssistant.Services;

public class AssistantService
{
    private readonly ToolAgentService _toolAgentService;
    private readonly RagService _ragService;

    public AssistantService(
        ToolAgentService toolAgentService,
        RagService ragService)
    {
        _toolAgentService = toolAgentService;
        _ragService = ragService;
    }

    public async Task<ChatResponse> AskAsync(
        string question)
    {
        var toolResponse =
            await _toolAgentService.TryHandleAsync(question);

        if (toolResponse != null)
            return toolResponse;

        return await _ragService.AskAsync(question);
    }
}