namespace PersonalAiAssistant.Services;

public interface ISqlToolService
{
    Task<string> ExecuteAsync(string userQuestion);
}