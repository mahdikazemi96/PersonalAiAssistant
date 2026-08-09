using Microsoft.Extensions.Configuration;
using PersonalAiAssistant.Contracts.Interfaces;
using PersonalAiAssistant.Infrastructure.Models;
using System.Net.Http.Json;

namespace PersonalAiAssistant.Infrastructure.Services.LmStudio
{
    public class LmStudioEmbeddingClient : IEmbeddingClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public LmStudioEmbeddingClient(
            HttpClient httpClient,
            IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<float[]> CreateEmbeddingAsync(string text)
        {
            var request = new EmbeddingRequest
            {
                Model = _configuration["LmStudio:EmbeddingModel"]!,
                Input = text
            };

            var response = await _httpClient.PostAsJsonAsync(
                "/v1/embeddings",
                request);

            response.EnsureSuccessStatusCode();

            var embedding =
                await response.Content.ReadFromJsonAsync<EmbeddingResponse>();

            return embedding!.Data.First().Embedding;
        }
    }
}
