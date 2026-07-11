using System.Net.Http.Json;
using System.Text.Json;

namespace PersonalAiAssistant.Services;

public class LmStudioService : ILlmService
{
    private readonly HttpClient _httpClient;

    public LmStudioService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> AskAsync(string message)
    {
        var request = new
        {
            model = "phi-3-mini-4k-instruct",
            temperature = 0.7,
            messages = new object[]
            {
                new
                {
                    role = "system",
                    content = "You are a helpful AI assistant."
                },
                new
                {
                    role = "user",
                    content = message
                }
            }
        };

        var response = await _httpClient.PostAsJsonAsync(
            "/v1/chat/completions",
            request);

        response.EnsureSuccessStatusCode();

        using var stream = await response.Content.ReadAsStreamAsync();

        using var document = await JsonDocument.ParseAsync(stream);

        return document
            .RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString()!;
    }
}