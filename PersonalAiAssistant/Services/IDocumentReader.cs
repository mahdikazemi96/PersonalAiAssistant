namespace PersonalAiAssistant.Services;

public interface IDocumentReader
{
    Task<string> ExtractTextAsync(Stream stream);
}