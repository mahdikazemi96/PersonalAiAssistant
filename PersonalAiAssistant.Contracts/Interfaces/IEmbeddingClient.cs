namespace PersonalAiAssistant.Contracts.Interfaces
{
    public interface IEmbeddingClient
    {
        Task<float[]> CreateEmbeddingAsync(string text);
    }
}
