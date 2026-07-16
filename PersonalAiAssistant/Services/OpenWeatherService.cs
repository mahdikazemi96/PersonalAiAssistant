using PersonalAiAssistant.Models;

namespace PersonalAiAssistant.Services;

public class OpenWeatherService : IWeatherService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    public OpenWeatherService(
        HttpClient httpClient,
        IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<WeatherResult> GetCurrentWeatherAsync(
        string city)
    {
        if (string.IsNullOrWhiteSpace(_configuration["OpenWeather:ApiKey"]))
        {
            throw new InvalidOperationException(
                "OpenWeather API key is not configured.");
        }

        var response =
            await _httpClient.GetAsync(
                $"weather?q={Uri.EscapeDataString(city)}" +
                $"&units=metric" +
                $"&appid={_configuration["OpenWeather:ApiKey"]}");

        response.EnsureSuccessStatusCode();

        var weather =
            await response.Content.ReadFromJsonAsync<OpenWeatherResponse>();

        if (weather == null)
        {
            throw new InvalidOperationException(
                "Weather service returned an invalid response.");
        }

        return new WeatherResult
        {
            City = weather.Name,
            Temperature = weather.Main.Temperature,
            FeelsLike = weather.Main.FeelsLike,
            Humidity = weather.Main.Humidity,
            Description =
                weather.Weather.FirstOrDefault()?.Description
                ?? string.Empty
        };
    }
}