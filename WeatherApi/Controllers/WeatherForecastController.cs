using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using WeatherApi.Models;
using WeatherApi.Services;

namespace WeatherApi
{
    [Route("[controller]")]
    [ApiController]
    [EnableRateLimiting("ClientLimit")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IWeatherService _weatherService;

        public WeatherForecastController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        [HttpGet("{location}")]
        public async Task<ActionResult<WeatherForecast>> GetWeatherForecast(string location)
        {
            WeatherForecast forecast = await _weatherService.GetWeatherForecastAsync(location);
            return Ok(forecast);
        }
    }

}
