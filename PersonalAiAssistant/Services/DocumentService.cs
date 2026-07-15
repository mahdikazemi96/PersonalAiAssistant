using PersonalAiAssistant.Clients;
using PersonalAiAssistant.Models;

namespace PersonalAiAssistant.Services;

public class DocumentService
{
    private readonly IEmbeddingClient _embeddingClient;
    private readonly QdrantService _qdrantService;

    public DocumentService(
        IEmbeddingClient embeddingClient,
        QdrantService qdrantService)
    {
        _embeddingClient = embeddingClient;
        _qdrantService = qdrantService;
    }

    public async Task SaveAsync(
        string text,
        string fileName,
        int chunkNumber)
    {
        if (string.IsNullOrWhiteSpace(text))
            return;

        await _qdrantService.CreateCollectionIfNotExistsAsync();

        var embedding =
            await _embeddingClient.CreateEmbeddingAsync(text);

        await _qdrantService.InsertDocumentAsync(
            text,
            embedding,
            fileName,
            chunkNumber);
    }

    public async Task<List<SearchResult>> SearchAsync(
        string text,
        int limit = 5)
    {
        var embedding =
            await _embeddingClient.CreateEmbeddingAsync(text);

        return await _qdrantService.SearchAsync(
            embedding,
            limit);
    }
}