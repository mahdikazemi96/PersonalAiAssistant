using Microsoft.Extensions.Options;
using PersonalAiAssistant.Contracts.Interfaces;
using PersonalAiAssistant.Contracts.Models;
using PersonalAiAssistant.Infrastructure.Models;
using System.Net.Http.Json;

namespace PersonalAiAssistant.Infrastructure.Services.LmStudio
{
    public class LmStudioChatClient : IChatClient
    {
        private readonly HttpClient _httpClient;
        private readonly LmStudioOptions _options;

        public LmStudioChatClient(
            HttpClient httpClient,
            IOptions<LmStudioOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
        }

        public async Task<string> ChatAsync(
            IEnumerable<ChatMessage> messages,
            ResponseFormat? responseFormat = null)
        {
            var request = new ChatCompletionRequest
            {
                Model = _options.Model!,
                Temperature = _options.Temperature!,
                ResponseFormat = responseFormat,
                Messages = messages
                    .Select(x => new ChatCompletionMessage
                    {
                        Role = x.Role,
                        Content = x.Content
                    })
                    .ToList()
            };

            var response =
                await _httpClient.PostAsJsonAsync(
                    "/v1/chat/completions",
                    request);

            response.EnsureSuccessStatusCode();

            var completion =
                await response.Content.ReadFromJsonAsync<ChatCompletionResponse>();

            if (completion == null)
                throw new Exception("LM Studio returned an empty response.");

            if (completion.Choices.Count == 0)
                throw new Exception("LM Studio returned no choices.");

            return completion
                .Choices
                .First()
                .Message
                .Content;
        }
    }
}
