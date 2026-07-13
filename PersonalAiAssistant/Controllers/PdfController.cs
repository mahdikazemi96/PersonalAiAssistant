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

    public PdfController(
        IDocumentReader documentReader,
        IEmbeddingClient embeddingClient,
        QdrantService qdrantService)
    {
        _documentReader = documentReader;
        _embeddingClient = embeddingClient;
        _qdrantService = qdrantService;
    }

    [HttpPost]
    public async Task<ActionResult<UploadPdfResponse>> Upload(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest();

        using var stream = file.OpenReadStream();

        var text = await _documentReader.ExtractTextAsync(stream);

        var embedding =
            await _embeddingClient.CreateEmbeddingAsync(text);

        await _qdrantService.CreateCollectionIfNotExistsAsync();

        await _qdrantService.InsertDocumentAsync(
            text,
            embedding);

        return Ok(new UploadPdfResponse
        {
            Characters = text.Length
        });
    }
}