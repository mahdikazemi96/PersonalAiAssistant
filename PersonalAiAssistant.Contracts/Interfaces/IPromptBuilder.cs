namespace PersonalAiAssistant.Contracts.Interfaces
{
    public interface IPromptBuilder
    {
        Task<string> BuildAsync();
    }
}
