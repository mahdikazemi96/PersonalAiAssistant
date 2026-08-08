using PersonalAiAssistant.Contracts;
using System.Threading.Tasks;

namespace PersonalAiAssistant.AgentTool
{
    public interface ITool
    {
        string Name { get; }

        string Description { get; }

        Task<string?> GetContextAsync();

        Task<ToolExecutionResult> ExecuteAsync(
            string arguments);
    }
}
