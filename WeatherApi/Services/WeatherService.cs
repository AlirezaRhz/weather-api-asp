using StackExchange.Redis;
using System.Text.Json;
using WeatherApi.Clients;
using WeatherApi.Models;

namespace WeatherApi.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly IWeatherApiClient _client;
        private readonly IDatabase _redis;
        public WeatherService(IWeatherApiClient client, ConnectionMultiplexer redis)
        {
            _client = client;
            _redis = redis.GetDatabase();
        }

        public async Task<WeatherForecast> GetWeatherForecastAsync(string location)
        {
            // Making a RedisKey and checking if data is in cache or not
            // I did this all with 0 help from ai or even internet btw. all intellisense and definitions especially intellisense
            RedisKey redisKey = new RedisKey(location);
            RedisValue cachedValue = await _redis.StringGetAsync(redisKey);
            if (cachedValue == RedisValue.Null)
            {
                VisualCrossingResponse response = await _client.GetWeatherForecastAsync(location);
                // Map VisualCrossingResponse to WeatherForecast
                WeatherForecast weather = new WeatherForecast
                {
                    City = location,
                    TimeZone = response.TimeZone,
                    CurrentDateTime = response.CurrentConditions.DateTime,
                    Temperature = response.CurrentConditions.Temp,
                    FeelsLike = response.CurrentConditions.FeelsLike,
                    Humidity = response.CurrentConditions.Humidity,
                    WindSpeed = response.CurrentConditions.WindSpeed
                };
                await _redis.StringSetAsync(
                    redisKey,
                    JsonSerializer.Serialize(weather),
                    TimeSpan.FromMinutes(10));

                return weather;
            }
            else
            {
                // Since We already Map VisualCrossingResponse to WeatherForecast before saving it to redis, We can just return this here :)
                WeatherForecast weather = JsonSerializer.Deserialize<WeatherForecast>(cachedValue.ToString())!;
                return weather;
            }


        }
    }
}
