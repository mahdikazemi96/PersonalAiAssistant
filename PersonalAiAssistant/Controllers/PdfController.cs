using Microsoft.AspNetCore.Mvc;
using PersonalAiAssistant.Clients;
using PersonalAiAssistant.Models;
using PersonalAiAssistant.Services;

namespace PersonalAiAssistant.Controllers;

[ApiController]
[Route("api/pdf")]
public class PdfController : ControllerBase
{
    private readonly IDocumentReader _documentReader;
    private readonly IEmbeddingClient _embeddingClient;
    private readonly QdrantService _qdrantService;
    private readonly TextChunker _chunker;

    public PdfController(
        IDocumentReader documentReader,
        IEmbeddingClient embeddingClient,
        QdrantService qdrantService,
        TextChunker chunker)
    {
        _documentReader = documentReader;
        _embeddingClient = embeddingClient;
        _qdrantService = qdrantService;
        _chunker = chunker;
    }

    [HttpPost]
    public async Task<ActionResult<UploadPdfResponse>> Upload(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest();

        using var stream = file.OpenReadStream();

        var text = await _documentReader.ExtractTextAsync(stream);

        await _qdrantService.CreateCollectionIfNotExistsAsync();

        var chunks = _chunker.Split(text);

        foreach (var chunk in chunks)
        {
            var embedding =
                await _embeddingClient.CreateEmbeddingAsync(chunk);

            await _qdrantService.InsertDocumentAsync(
                chunk,
                embedding);
        }

        return Ok(new UploadPdfResponse
        {
            Characters = text.Length,
            Chunks = chunks.Count
        });
    }
}