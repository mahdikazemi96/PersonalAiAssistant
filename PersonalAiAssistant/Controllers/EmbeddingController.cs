using Microsoft.AspNetCore.Mvc;
using PersonalAiAssistant.Contracts.Interfaces;
using PersonalAiAssistant.Models;
using System.Threading.Tasks;

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
        string text)
    {
        var vector = await _embeddingClient.CreateEmbeddingAsync(text);

        return Ok(new EmbeddingResultResponse
        {
            Dimension = vector.Length,
            Vector = vector
        });
    }
}