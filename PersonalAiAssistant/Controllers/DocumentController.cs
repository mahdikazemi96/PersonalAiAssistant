using Microsoft.AspNetCore.Mvc;
using PersonalAiAssistant.Models;
using PersonalAiAssistant.Services;

namespace PersonalAiAssistant.Controllers;

[ApiController]
[Route("api/document-upload")]
public class DocumentUploadController : ControllerBase
{
    private readonly DocumentReaderFactory _readerFactory;
    private readonly DocumentService _documentService;

    public DocumentUploadController(
        DocumentReaderFactory readerFactory,
        DocumentService documentService)
    {
        _readerFactory = readerFactory;
        _documentService = documentService;
    }

    [HttpPost]
    public async Task<ActionResult<UploadPdfResponse>> Upload(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("File is required.");

        var reader =
            _readerFactory.GetReader(file.FileName);

        using var stream =
            file.OpenReadStream();

        var text =
            await reader.ExtractTextAsync(stream);

        var chunkCount =
            await _documentService.SaveAsync(
                text,
                file.FileName);

        return Ok(new UploadPdfResponse
        {
            Characters = text.Length,
            Chunks = chunkCount
        });
    }
}