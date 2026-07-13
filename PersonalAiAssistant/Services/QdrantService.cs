using System.Net;
using System.Net.Http.Json;
using PersonalAiAssistant.Qdrant;

namespace PersonalAiAssistant.Services;

public class QdrantService
{
    private readonly HttpClient _httpClient;

    private const string CollectionName = "documents";

    public QdrantService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task CreateCollectionIfNotExistsAsync()
    {
        var response =
            await _httpClient.GetAsync($"/collections/{CollectionName}");

        if (response.IsSuccessStatusCode)
            return;

        if (response.StatusCode != HttpStatusCode.NotFound)
        {
            response.EnsureSuccessStatusCode();
        }

        var request = new CreateCollectionRequest
        {
            Vectors = new VectorConfiguration
            {
                Size = 768,
                Distance = "Cosine"
            }
        };

        var createResponse = await _httpClient.PutAsJsonAsync(
            $"/collections/{CollectionName}",
            request);

        createResponse.EnsureSuccessStatusCode();
    }

    public async Task InsertDocumentAsync(
        string text,
        float[] vector)
    {
        var request = new UpsertPointRequest();

        request.Points.Add(new QdrantPoint
        {
            Vector = vector,
            Payload =
            {
                ["text"] = text
            }
        });

        var response = await _httpClient.PutAsJsonAsync(
            $"/collections/{CollectionName}/points",
            request);

        response.EnsureSuccessStatusCode();
    }

    public async Task<List<string>> SearchAsync(
    float[] vector,
    int limit = 5)
    {
        var request = new SearchRequest
        {
            Vector = vector,
            Limit = limit
        };

        var response = await _httpClient.PostAsJsonAsync(
            $"/collections/{CollectionName}/points/search",
            request);

        response.EnsureSuccessStatusCode();

        var result =
            await response.Content.ReadFromJsonAsync<SearchResponse>();

        return result!.Result
            .Select(x => x.Payload["text"].ToString()!)
            .ToList();
    }
}