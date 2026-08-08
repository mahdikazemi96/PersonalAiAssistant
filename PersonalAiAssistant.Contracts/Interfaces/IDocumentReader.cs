namespace PersonalAiAssistant.Contracts.Interfaces
{
    public interface IDocumentReader
    {
        bool CanRead(string extension);

        Task<string> ExtractTextAsync(Stream stream);
    }
}
