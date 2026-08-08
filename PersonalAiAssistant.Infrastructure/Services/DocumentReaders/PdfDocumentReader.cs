using PersonalAiAssistant.Contracts.Interfaces;
using UglyToad.PdfPig;

namespace PersonalAiAssistant.Infrastructure.Services.DocumentReaders
{
    public class PdfDocumentReader : IDocumentReader
    {
        public bool CanRead(string extension)
        {
            return extension.Equals(".pdf",
                StringComparison.OrdinalIgnoreCase);
        }

        public Task<string> ExtractTextAsync(Stream stream)
        {
            using var document =
                PdfDocument.Open(stream);

            var text = string.Empty;

            foreach (var page in document.GetPages())
            {
                text += page.Text;
                text += Environment.NewLine;
            }

            return Task.FromResult(text);
        }
    }
}
