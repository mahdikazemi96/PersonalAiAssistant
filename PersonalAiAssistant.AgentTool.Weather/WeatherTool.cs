using PersonalAiAssistant.Contracts;
using System.Net.Http;
using System.Threading.Tasks;

namespace PersonalAiAssistant.AgentTool.Weather
{
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
            return Task.FromResult<string?>(
                """
            Weather Tool

            Input:
            Only the city name.

            Extract the city name from the user's request.

            Return ONLY the city name.

            Examples

            User:
            What is today's Tehran temperature?

            Argument:
            Tehran

            ----------------------------------------

            User:
            Tell me today's weather in London.

            Argument:
            London

            ----------------------------------------

            User:
            How is the weather in Paris today?

            Argument:
            Paris

            ----------------------------------------

            User:
            What is the current weather in New York?

            Argument:
            New York
            """);
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
}
