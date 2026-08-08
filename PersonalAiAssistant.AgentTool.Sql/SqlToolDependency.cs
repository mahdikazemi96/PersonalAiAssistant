using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PersonalAiAssistant.Contracts.Interfaces;

namespace PersonalAiAssistant.AgentTool.Sql
{
    public static class SqlToolDependency
    {
        public static IServiceCollection AddSqlTool(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<ITool, SqlTool>();

            services.AddSingleton<IDatabaseSchemaReader, SqlServerSchemaReader>();

            services.AddSingleton<ISqlToolService, SqlToolService>();

            services.AddSingleton<SqlValidator>();

            services.AddKeyedSingleton<IPromptBuilder, SqlPromptBuilder>("SqlPromptBuilder");

            return services;
        }
    }
}
