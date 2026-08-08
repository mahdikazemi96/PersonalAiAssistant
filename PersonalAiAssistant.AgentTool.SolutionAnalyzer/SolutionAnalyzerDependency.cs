using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PersonalAiAssistant.AgentTool.SolutionAnalyzer
{
    public static class SolutionAnalyzerDependency
    {
        public static IServiceCollection AddSolutionAnalyzerTool(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<ITool, SolutionAnalyzerTool>();

            services.AddSingleton<ISolutionAnalyzerService, SolutionAnalyzerService>();

            return services;
        }
    }
}
