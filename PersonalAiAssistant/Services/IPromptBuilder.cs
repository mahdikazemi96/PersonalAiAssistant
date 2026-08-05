namespace PersonalAiAssistant.Services
{
    public interface IPromptBuilder
    {
        Task<string> BuildAsync();
    }
}
