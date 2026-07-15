namespace PersonalAiAssistant.Services;

public class DocumentReaderFactory
{
    private readonly IEnumerable<IDocumentReader> _readers;

    public DocumentReaderFactory(
        IEnumerable<IDocumentReader> readers)
    {
        _readers = readers;
    }

    public IDocumentReader GetReader(string fileName)
    {
        var extension =
            Path.GetExtension(fileName);

        var reader =
            _readers.FirstOrDefault(x =>
                x.CanRead(extension));

        if (reader == null)
        {
            throw new NotSupportedException(
                $"File type '{extension}' is not supported.");
        }

        return reader;
    }
}