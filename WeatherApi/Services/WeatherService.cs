using WeatherApi.Clients;
using WeatherApi.Models;

namespace WeatherApi.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly IWeatherApiClient _client;
        public WeatherService(IWeatherApiClient client)
        {
            _client = client;
        }

        public async Task<WeatherForecast> GetWeatherForecastAsync(string location)
        {
            VisualCrossingResponse response = await _client.GetWeatherForecastAsync(location);

            // Map VisualCrossingResponse to WeatherForecast
            return new WeatherForecast
            {
                City = location,
                TimeZone = response.TimeZone,
                CurrentDateTime = response.CurrentConditions.DateTime,
                Temperature = response.CurrentConditions.Temp,
                FeelsLike = response.CurrentConditions.FeelsLike,
                Humidity = response.CurrentConditions.Humidity,
                WindSpeed = response.CurrentConditions.WindSpeed
            };
        }
    }
}
