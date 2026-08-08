using System.Threading.Tasks;

namespace PersonalAiAssistant.AgentTool.Weather
{
    public interface IWeatherService
    {
        Task<WeatherResult> GetCurrentWeatherAsync(
            string city);
    }
}
