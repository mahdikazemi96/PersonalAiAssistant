using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PersonalAiAssistant.Contracts.Interfaces;
using PersonalAiAssistant.Infrastructure.Models;
using PersonalAiAssistant.Infrastructure.Services.DocumentReaders;
using PersonalAiAssistant.Infrastructure.Services.LmStudio;
using PersonalAiAssistant.Infrastructure.Services.Prompts;
using PersonalAiAssistant.Infrastructure.Services.VectorDb;

namespace PersonalAiAssistant.Infrastructure
{
    public static class InfrastructureDependency
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient<IChatClient, LmStudioChatClient>(client =>
            {
                client.BaseAddress = new Uri("http://127.0.0.1:1234");
                client.Timeout = TimeSpan.FromMinutes(5);
            });

            services.AddHttpClient<IEmbeddingClient, LmStudioEmbeddingClient>(client =>
            {
                client.BaseAddress = new Uri("http://127.0.0.1:1234");
            });

            services.AddHttpClient<QdrantService>(client =>
            {
                client.BaseAddress = new Uri("http://127.0.0.1:6333");
            });

            services.Configure<LmStudioOptions>(configuration.GetSection("LmStudio"));

            services.AddSingleton<IDocumentReader, PdfDocumentReader>();

            services.AddSingleton<IDocumentReader, WordDocumentReader>();

            services.AddSingleton<IDocumentReader, TextDocumentReader>();

            services.AddSingleton<IDocumentReader, MarkdownDocumentReader>();

            services.AddSingleton<DocumentReaderFactory>();

            services.AddSingleton<IPromptLoader, PromptLoader>();

            return services;
        }
    }
}
