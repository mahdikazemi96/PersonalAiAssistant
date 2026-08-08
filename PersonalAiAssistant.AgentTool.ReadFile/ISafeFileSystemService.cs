using System.Collections.Generic;
using System.Threading.Tasks;

namespace PersonalAiAssistant.AgentTool.ReadFile
{
    public interface ISafeFileSystemService
    {
        Task<List<string>> GetFilesAsync(
            string relativePath);

        Task<string> ReadFileAsync(
            string relativePath);
    }
}
