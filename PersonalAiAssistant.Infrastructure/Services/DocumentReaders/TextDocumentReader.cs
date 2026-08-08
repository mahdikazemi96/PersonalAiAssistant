using PersonalAiAssistant.Contracts.Interfaces;

namespace PersonalAiAssistant.Infrastructure.Services.DocumentReaders
{
    public class TextDocumentReader : IDocumentReader
    {
        public bool CanRead(string extension)
        {
            return extension.Equals(".txt",
                StringComparison.OrdinalIgnoreCase);
        }

        public async Task<string> ExtractTextAsync(Stream stream)
        {
            using var reader = new StreamReader(stream);

            return await reader.ReadToEndAsync();
        }
    }
}
