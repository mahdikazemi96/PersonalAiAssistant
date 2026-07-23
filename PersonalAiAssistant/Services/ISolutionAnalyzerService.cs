using PersonalAiAssistant.Models;

namespace PersonalAiAssistant.Services;

public interface ISolutionAnalyzerService
{
    Task<SolutionInfo> GetSolutionAsync();

    Task<string> AnalyzeAsync();
}