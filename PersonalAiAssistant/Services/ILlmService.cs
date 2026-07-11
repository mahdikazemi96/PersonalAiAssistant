namespace PersonalAiAssistant.Services;

public interface ILlmService
{
    Task<string> AskAsync(string message);
}