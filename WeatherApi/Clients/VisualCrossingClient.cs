using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using WeatherApi.Configuration;
using WeatherApi.Models;

namespace WeatherApi.Clients
{
    public class VisualCrossingClient : IWeatherApiClient
    {
        private readonly VisualCrossingOptions _options;
        private readonly HttpClient _httpClient;
        // Meh why not add some logging as well
        private readonly ILogger<VisualCrossingClient> _logger;


        public VisualCrossingClient(HttpClient httpClient, IOptions<VisualCrossingOptions> options, ILogger<VisualCrossingClient> logger)
        {
            _httpClient = httpClient;
            _options = options.Value;
            _logger = logger;
        }
        public async Task<VisualCrossingResponse> GetWeatherForecastAsync(string location)
        {
            string requestUrl = $"{_options.BaseUrl}{location}?key={_options.ApiKey}&include=current";
            VisualCrossingResponse visualCrossingResponse = await _httpClient.GetFromJsonAsync<VisualCrossingResponse>(requestUrl) ?? throw new InvalidOperationException("Failed to retrieve weather forecast.");
            _logger.LogInformation("Successfully retrieved weather forecast for location: {Location}", location);
            return visualCrossingResponse;
        }
    }
}
