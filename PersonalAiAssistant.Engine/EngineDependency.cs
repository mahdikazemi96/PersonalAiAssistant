using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PersonalAiAssistant.Contracts.Interfaces;

namespace PersonalAiAssistant.Engine
{
    public static class EngineDependency
    {
        public static IServiceCollection AddEngine(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<ConversationService>();

            services.AddSingleton<TextChunker>();

            services.AddSingleton<AnswerGenerationService>();

            services.AddSingleton<DocumentService>();

            services.AddSingleton<AssistantService>();

            services.AddSingleton<PlannerService>();

            services.AddSingleton<AssistantService>();

            services.AddSingleton<RagService>();

            services.AddKeyedSingleton<IPromptBuilder, ToolAgentPromptBuilder>("ToolAgentPromptBuilder");
            services.AddKeyedSingleton<IPromptBuilder, RagPromptBuilder>("RagPromptBuilder");

            return services;
        }
    }
}
