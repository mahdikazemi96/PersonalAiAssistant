using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PersonalAiAssistant.AgentTool
{
    public static class AgentToolDependency
    {
        public static IServiceCollection AddAgentTool(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<ToolRouter>();

            services.AddSingleton<ToolExecutionService>();

            return services;
        }
    }
}
