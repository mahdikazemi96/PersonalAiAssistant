using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace PersonalAiAssistant.AgentTool.Weather
{
    public static class WeatherToolDependency
    {
        public static IServiceCollection AddWeatherTool(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<ITool, WeatherTool>();

            services.AddHttpClient<IWeatherService, OpenWeatherService>(client =>
            {
                client.BaseAddress =
                    new Uri("https://api.openweathermap.org/data/2.5/");
            });

            return services;
        }
    }
}
