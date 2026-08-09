using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PersonalAiAssistant.AgentTool.ReadFile
{
    public static class ReadFileToolDependency
    {
        public static IServiceCollection AddReadFileTool(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<ITool, FileSystemTool>();

            services.AddSingleton<ISafeFileSystemService, SafeFileSystemService>();

            return services;
        }
    }
}
