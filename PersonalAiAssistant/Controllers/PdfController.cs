using Microsoft.AspNetCore.Mvc;
using PersonalAiAssistant.Models;
using PersonalAiAssistant.Services;

namespace PersonalAiAssistant.Controllers;

[ApiController]
[Route("api/pdf")]
public class PdfController : ControllerBase
{
    private readonly IDocumentReader _documentReader;
    private readonly DocumentService _documentService;
    private readonly TextChunker _chunker;

    public PdfController(
        IDocumentReader documentReader,
        DocumentService documentService,
        TextChunker chunker)
    {
        _documentReader = documentReader;
        _documentService = documentService;
        _chunker = chunker;
    }

    [HttpPost]
    public async Task<ActionResult<UploadPdfResponse>> Upload(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest();

        using var stream = file.OpenReadStream();

        var text = await _documentReader.ExtractTextAsync(stream);

        var chunks = _chunker.Split(text);

        const int maxConcurrency = 4;

        using var semaphore = new SemaphoreSlim(maxConcurrency);

        var tasks = chunks
            .Select(async (chunk, index) =>
            {
                await semaphore.WaitAsync();

                try
                {
                    await _documentService.SaveAsync(
                        chunk,
                        file.FileName,
                        index + 1);
                }
                finally
                {
                    semaphore.Release();
                }
            });

        await Task.WhenAll(tasks);

        return Ok(new UploadPdfResponse
        {
            Characters = text.Length,
            Chunks = chunks.Count
        });
    }
}