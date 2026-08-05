namespace PersonalAiAssistant.Services
{
    public interface IPromptLoader
    {
        string Load(string promptName);
    }
}
