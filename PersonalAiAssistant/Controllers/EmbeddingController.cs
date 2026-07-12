using Microsoft.AspNetCore.Mvc;
using PersonalAiAssistant.Clients;
using PersonalAiAssistant.Models;

namespace PersonalAiAssistant.Controllers;

[ApiController]
[Route("api/embedding")]
public class EmbeddingController : ControllerBase
{
    private readonly IEmbeddingClient _embeddingClient;

    public EmbeddingController(IEmbeddingClient embeddingClient)
    {
        _embeddingClient = embeddingClient;
    }

    [HttpPost]
    public async Task<ActionResult<EmbeddingResultResponse>> CreateEmbedding(
        EmbeddingTextRequest request)
    {
        var vector = await _embeddingClient.CreateEmbeddingAsync(request.Text);

        return Ok(new EmbeddingResultResponse
        {
            Dimension = vector.Length,
            Vector = vector
        });
    }
}