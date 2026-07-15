namespace PersonalAiAssistant.Services;

public interface IDocumentReader
{
    bool CanRead(string extension);

    Task<string> ExtractTextAsync(Stream stream);
}