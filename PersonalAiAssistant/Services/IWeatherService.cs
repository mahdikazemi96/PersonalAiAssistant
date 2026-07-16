using PersonalAiAssistant.Models;

namespace PersonalAiAssistant.Services;

public interface IWeatherService
{
    Task<WeatherResult> GetCurrentWeatherAsync(
        string city);
}