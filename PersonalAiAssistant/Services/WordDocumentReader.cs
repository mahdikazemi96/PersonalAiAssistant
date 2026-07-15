using DocumentFormat.OpenXml.Packaging;

namespace PersonalAiAssistant.Services;

public class WordDocumentReader : IDocumentReader
{
    public bool CanRead(string extension)
    {
        return extension.Equals(".docx",
            StringComparison.OrdinalIgnoreCase);
    }

    public Task<string> ExtractTextAsync(Stream stream)
    {
        using var document =
            WordprocessingDocument.Open(stream, false);

        var body =
            document.MainDocumentPart?
                .Document
                .Body;

        if (body == null)
            return Task.FromResult(string.Empty);

        return Task.FromResult(body.InnerText);
    }
}