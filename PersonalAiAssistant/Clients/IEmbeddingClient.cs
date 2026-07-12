namespace PersonalAiAssistant.Clients;

public interface IEmbeddingClient
{
    Task<float[]> CreateEmbeddingAsync(string text);
}