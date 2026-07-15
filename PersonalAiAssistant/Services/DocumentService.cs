using PersonalAiAssistant.Clients;

namespace PersonalAiAssistant.Services;

public class DocumentService
{
    private readonly IEmbeddingClient _embeddingClient;
    private readonly QdrantService _qdrantService;
    private readonly TextChunker _chunker;

    public DocumentService(
        IEmbeddingClient embeddingClient,
        QdrantService qdrantService,
        TextChunker chunker)
    {
        _embeddingClient = embeddingClient;
        _qdrantService = qdrantService;
        _chunker = chunker;
    }

    public async Task<int> SaveAsync(
        string text,
        string fileName)
    {
        await _qdrantService.CreateCollectionIfNotExistsAsync();

        var chunks =
            _chunker.Split(text);

        for (int i = 0; i < chunks.Count; i++)
        {
            var embedding =
                await _embeddingClient.CreateEmbeddingAsync(chunks[i]);

            await _qdrantService.InsertDocumentAsync(
                chunks[i],
                embedding,
                fileName,
                i + 1);
        }

        return chunks.Count;
    }
}