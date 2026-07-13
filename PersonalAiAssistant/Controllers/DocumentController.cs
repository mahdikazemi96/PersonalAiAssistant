using Microsoft.AspNetCore.Mvc;
using PersonalAiAssistant.Clients;
using PersonalAiAssistant.Models;
using PersonalAiAssistant.Services;

namespace PersonalAiAssistant.Controllers;

[ApiController]
[Route("api/document")]
public class DocumentController : ControllerBase
{
    private readonly IEmbeddingClient _embeddingClient;
    private readonly QdrantService _qdrantService;

    public DocumentController(
        IEmbeddingClient embeddingClient,
        QdrantService qdrantService)
    {
        _embeddingClient = embeddingClient;
        _qdrantService = qdrantService;
    }

    [HttpPost]
    public async Task<IActionResult> Save(SaveDocumentRequest request)
    {
        var embedding =
            await _embeddingClient.CreateEmbeddingAsync(request.Text);

        await _qdrantService.CreateCollectionIfNotExistsAsync();

        await _qdrantService.InsertDocumentAsync(
            request.Text,
            embedding);

        return Ok();
    }

    [HttpPost("search")]
    public async Task<ActionResult<List<string>>> Search(SaveDocumentRequest request)
    {
        var embedding =
            await _embeddingClient.CreateEmbeddingAsync(request.Text);

        var documents =
            await _qdrantService.SearchAsync(embedding);

        return Ok(documents);
    }
}