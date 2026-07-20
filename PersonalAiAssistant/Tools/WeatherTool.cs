using PersonalAiAssistant.Models;
using PersonalAiAssistant.Services;

namespace PersonalAiAssistant.Tools;

public class WeatherTool : ITool
{
    private readonly IWeatherService _weatherService;

    public WeatherTool(
        IWeatherService weatherService)
    {
        _weatherService = weatherService;
    }

    public string Name => "weather";

    public string Description =>
        "Gets the current weather of a city.";
    public Task<string?> GetContextAsync()
    {
        return Task.FromResult<string?>(null);
    }
    public async Task<ToolExecutionResult> ExecuteAsync(
        string arguments)
    {
        if (string.IsNullOrWhiteSpace(arguments))
        {
            return new ToolExecutionResult
            {
                Content = "City name is required.",
                ContentType = "text/plain"
            };
        }

        try
        {
            var weather =
                await _weatherService.GetCurrentWeatherAsync(
                    arguments);

            var result =
$"""
City: {weather.City}

Temperature: {weather.Temperature} °C

Feels Like: {weather.FeelsLike} °C

Humidity: {weather.Humidity}%

Description: {weather.Description}
""";

            return new ToolExecutionResult
            {
                Content = result,
                ContentType = "text/plain"
            };
        }
        catch (HttpRequestException)
        {
            return new ToolExecutionResult
            {
                Content = "Unable to retrieve weather information.",
                ContentType = "text/plain"
            };
        }
    }
}