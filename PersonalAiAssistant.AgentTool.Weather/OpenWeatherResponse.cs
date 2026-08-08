using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PersonalAiAssistant.AgentTool.Weather
{
    public class OpenWeatherResponse
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("weather")]
        public List<OpenWeatherDescription> Weather { get; set; } = new();

        [JsonPropertyName("main")]
        public OpenWeatherMain Main { get; set; } = new();
    }

    public class OpenWeatherMain
    {
        [JsonPropertyName("temp")]
        public double Temperature { get; set; }

        [JsonPropertyName("feels_like")]
        public double FeelsLike { get; set; }

        [JsonPropertyName("humidity")]
        public int Humidity { get; set; }
    }

    public class OpenWeatherDescription
    {
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
    }
}
