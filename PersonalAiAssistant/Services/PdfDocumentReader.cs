using UglyToad.PdfPig;

namespace PersonalAiAssistant.Services;

public class PdfDocumentReader : IDocumentReader
{
    public Task<string> ExtractTextAsync(Stream stream)
    {
        using var document = PdfDocument.Open(stream);

        var text = "";

        foreach (var page in document.GetPages())
        {
            text += page.Text;
            text += Environment.NewLine;
        }

        return Task.FromResult(text);
    }
}