namespace PersonalAiAssistant.Models;

public class LmStudioOptions
{
    public string BaseUrl { get; set; } = string.Empty;

    public string Model { get; set; } = string.Empty;

    public double Temperature { get; set; }
}