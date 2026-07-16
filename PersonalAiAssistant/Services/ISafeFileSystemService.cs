namespace PersonalAiAssistant.Services;

public interface ISafeFileSystemService
{
    Task<List<string>> GetFilesAsync(
        string relativePath);

    Task<string> ReadFileAsync(
        string relativePath);
}