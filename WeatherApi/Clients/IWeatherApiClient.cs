using WeatherApi.Models;

namespace WeatherApi.Clients
{
    public interface IWeatherApiClient
    {
        public Task<VisualCrossingResponse> GetWeatherForecastAsync(string location);
    }
}
