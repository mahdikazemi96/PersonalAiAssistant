using PersonalAiAssistant.Contracts;
using System.Threading.Tasks;

namespace PersonalAiAssistant.AgentTool.SolutionAnalyzer
{
    public class SolutionAnalyzerTool : ITool
    {
        private readonly ISolutionAnalyzerService
            _solutionAnalyzerService;

        public SolutionAnalyzerTool(
            ISolutionAnalyzerService solutionAnalyzerService)
        {
            _solutionAnalyzerService =
                solutionAnalyzerService;
        }

        public string Name => "solution";

        public string Description => "Analyzes the current solution structure.";

        public Task<string?> GetContextAsync()
        {
            return Task.FromResult<string?>(
                """
            Use this tool when the user asks about:

            - solution
            - project
            - workspace
            - folder structure
            - project structure
            - file structure
            - analyze solution
            - analyze project
            - show project tree

            Do NOT use this tool for:

            - code review
            - architecture explanation
            - documentation generation

            This tool only returns the solution structure.
            """);
        }

        public async Task<ToolExecutionResult> ExecuteAsync(
            string arguments)
        {
            var result =
                await _solutionAnalyzerService
                    .AnalyzeAsync();

            return new ToolExecutionResult
            {
                Content = result,
                ContentType = "text/plain"
            };
        }
    }
}
