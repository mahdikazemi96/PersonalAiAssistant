using PersonalAiAssistant.Models;

namespace PersonalAiAssistant.Services;

public class AssistantService
{
    private readonly AgentService _agentService;

    public AssistantService(
       AgentService agentService)
    {
        _agentService = agentService;
    }

    public async Task<ChatResponse> AskAsync(
        string question)
    {
        return await _agentService.AskAsync(question);
    }
}