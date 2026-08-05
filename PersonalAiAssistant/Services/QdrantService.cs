using PersonalAiAssistant.Qdrant;
using System.Net;
using System.Text.Json;

namespace PersonalAiAssistant.Services;

public class QdrantService
{
    private readonly HttpClient _httpClient;

    private const string CollectionName = "documents";

    private bool _collectionInitialized;

    public QdrantService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task CreateCollectionIfNotExistsAsync()
    {
        if (_collectionInitialized)
            return;

        var response =
            await _httpClient.GetAsync($"/collections/{CollectionName}");

        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode != HttpStatusCode.NotFound)
            {
                response.EnsureSuccessStatusCode();
            }
            else
            {
                var request = new CreateCollectionRequest
                {
                    Vectors = new VectorConfiguration
                    {
                        Size = 768,
                        Distance = "Cosine"
                    }
                };

                var createResponse =
                    await _httpClient.PutAsJsonAsync(
                        $"/collections/{CollectionName}",
                        request);

                createResponse.EnsureSuccessStatusCode();
            }
        }

        _collectionInitialized = true;
    }

    public async Task InsertDocumentAsync(
        string text,
        float[] vector,
        string fileName,
        int chunkNumber)
    {
        var request = new UpsertPointRequest();

        request.Points.Add(new QdrantPoint
        {
            Vector = vector,
            Payload =
            {
                ["text"] = text,
                ["fileName"] = fileName,
                ["chunkNumber"] = chunkNumber
            }
        });

        var response =
            await _httpClient.PutAsJsonAsync(
                $"/collections/{CollectionName}/points",
                request);

        response.EnsureSuccessStatusCode();
    }

    public async Task<List<Models.SearchResult>> SearchAsync(
        float[] vector,
        int limit = 5)
    {
        var request = new SearchRequest
        {
            Vector = vector,
            Limit = limit
        };

        var response =
            await _httpClient.PostAsJsonAsync(
                $"/collections/{CollectionName}/points/search",
                request);

        response.EnsureSuccessStatusCode();

        var result =
            await response.Content.ReadFromJsonAsync<SearchResponse>();

        var documents = new List<Models.SearchResult>();

        if (result == null)
            return documents;

        foreach (var point in result.Result)
        {
            var text = GetString(point.Payload, "text");

            if (string.IsNullOrWhiteSpace(text))
                continue;

            documents.Add(new Models.SearchResult
            {
                Text = text,
                FileName = GetString(point.Payload, "fileName", "Unknown"),
                ChunkNumber = GetInt(point.Payload, "chunkNumber", 1),
                Score = point.Score
            });
        }

        return documents;
    }

    private static string GetString(
        Dictionary<string, object> payload,
        string key,
        string defaultValue = "")
    {
        if (!payload.TryGetValue(key, out var value))
            return defaultValue;

        if (value == null)
            return defaultValue;

        if (value is JsonElement json)
        {
            if (json.ValueKind == JsonValueKind.String)
                return json.GetString() ?? defaultValue;

            return json.ToString();
        }

        return value.ToString() ?? defaultValue;
    }

    private static int GetInt(
        Dictionary<string, object> payload,
        string key,
        int defaultValue = 0)
    {
        if (!payload.TryGetValue(key, out var value))
            return defaultValue;

        if (value == null)
            return defaultValue;

        if (value is JsonElement json)
        {
            if (json.ValueKind == JsonValueKind.Number)
                return json.GetInt32();

            if (json.ValueKind == JsonValueKind.String &&
                int.TryParse(json.GetString(), out var number))
            {
                return number;
            }

            return defaultValue;
        }

        if (value is int numberValue)
            return numberValue;

        if (int.TryParse(value.ToString(), out var result))
            return result;

        return defaultValue;
    }
}