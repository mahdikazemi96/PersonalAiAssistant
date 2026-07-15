using Microsoft.AspNetCore.Mvc;
using PersonalAiAssistant.Models;
using PersonalAiAssistant.Services;

namespace PersonalAiAssistant.Controllers;

[ApiController]
[Route("api/document")]
public class DocumentController : ControllerBase
{
    private readonly DocumentService _documentService;

    public DocumentController(DocumentService documentService)
    {
        _documentService = documentService;
    }

    [HttpPost]
    public async Task<IActionResult> Save(SaveDocumentRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Text))
            return BadRequest();

        await _documentService.SaveAsync(
            request.Text,
            "Manual",
            1);

        return Ok();
    }

    [HttpPost("search")]
    public async Task<ActionResult<List<SearchResult>>> Search(
        SaveDocumentRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Text))
            return BadRequest();

        var documents =
            await _documentService.SearchAsync(request.Text);

        return Ok(documents);
    }
}