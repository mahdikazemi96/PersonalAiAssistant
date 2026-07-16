using PersonalAiAssistant.Models;
using PersonalAiAssistant.Services;

namespace PersonalAiAssistant.Tools;

public class FileSystemTool : ITool
{
    private readonly ISafeFileSystemService _fileSystemService;

    public FileSystemTool(
        ISafeFileSystemService fileSystemService)
    {
        _fileSystemService = fileSystemService;
    }

    public string Name => "filesystem";

    public string Description =>
        "Reads the content of a file inside the workspace.";

    public async Task<ToolExecutionResult> ExecuteAsync(
        string arguments)
    {
        try
        {
            var content =
                await _fileSystemService.ReadFileAsync(arguments);

            return new ToolExecutionResult
            {
                Content = content,
                ContentType = "text/plain"
            };
        }
        catch (Exception ex)
        {
            return new ToolExecutionResult
            {
                Content = ex.Message,
                ContentType = "text/plain"
            };
        }
    }
}