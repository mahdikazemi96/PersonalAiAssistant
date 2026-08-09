using System.Threading.Tasks;

namespace PersonalAiAssistant.AgentTool.SolutionAnalyzer
{
    public interface ISolutionAnalyzerService
    {
        Task<SolutionInfo> GetSolutionAsync();

        Task<string> AnalyzeAsync();
    }
}
