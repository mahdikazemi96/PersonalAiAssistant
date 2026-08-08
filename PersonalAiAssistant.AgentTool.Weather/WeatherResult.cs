namespace PersonalAiAssistant.AgentTool.Weather
{
    public class WeatherResult
    {
        public string City { get; set; } = string.Empty;

        public double Temperature { get; set; }

        public double FeelsLike { get; set; }

        public int Humidity { get; set; }

        public string Description { get; set; } = string.Empty;
    }
}
