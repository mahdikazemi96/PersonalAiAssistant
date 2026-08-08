using PersonalAiAssistant.Contracts;
using System;
using System.Threading.Tasks;

namespace PersonalAiAssistant.AgentTool.ReadFile
{
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
        public Task<string?> GetContextAsync()
        {
            return Task.FromResult<string?>(null);
        }
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
}
