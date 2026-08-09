using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PersonalAiAssistant.AgentTool.Calculator
{
    public static class CalculatorToolDependency
    {
        public static IServiceCollection AddCalculatorTool(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<ITool, CalculatorTool>();

            return services;
        }
    }
}
