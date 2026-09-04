# WeatherApi

A small ASP.NET Core Web API that returns current weather data by location using the [Visual Crossing Weather API](https://www.visualcrossing.com/), with Redis caching and per-client rate limiting.

## Features

- **Weather lookups by location** via `GET /WeatherForecast/{location}`
- **Redis caching** to reduce repeated calls to the Visual Crossing API
- **Per-client rate limiting** using a fixed-window limiter
- **OpenAPI** support for API documentation and testing
- **Dependency injection** for application services and external API clients

## Tech Stack

- [.NET 10](https://dotnet.microsoft.com/) / ASP.NET Core Web API
- [StackExchange.Redis](https://github.com/StackExchange/StackExchange.Redis)
- [Visual Crossing Weather API](https://www.visualcrossing.com/)
- `Microsoft.AspNetCore.RateLimiting`

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- A running Redis instance (local via Docker or a hosted Redis instance)
- A [Visual Crossing API key](https://www.visualcrossing.com/weather-api)

## Configuration

Configure the Visual Crossing base URL in `appsettings.json`:

```json
{
  "VisualCrossing": {
    "BaseUrl": "https://weather.visualcrossing.com/VisualCrossingWebServices/rest/services/timeline/"
  },
  "Redis": {
    "ConnectionString": "localhost:6379"
  }
}
```

### Run Redis locally with Docker

```bash
docker run -d --name weatherapi-redis -p 6379:6379 redis
```

## Caching

Weather data is cached in Redis to reduce unnecessary calls to the Visual Crossing Weather API.

- Forecasts are cached by location.
- Cached responses are returned when available.
- Cache entries expire after the configured TTL (Default is 10 minutes).
- When a cached value is missing or expired, the API fetches fresh data and updates Redis.

## Running the API

```bash
cd WeatherApi
dotnet restore
dotnet run
```

In development, the application exposes an OpenAPI document. Check the console output for the exact URL and port.

## API

### `GET /WeatherForecast/{location}`

Returns current weather data for the specified location.

#### Example

```bash
curl "https://localhost:<port>/WeatherForecast/London"
```

#### Response

On success, the endpoint returns `200 OK` with a `WeatherForecast` object.

```json
{
  "city": "London",
  "timeZone": "Europe/London",
  "currentDateTime": "20:37:00",
  "temperature": 68.5,
  "feelsLike": 68.5,
  "humidity": 59.7,
  "windSpeed": 6.9
}
```

## Rate Limiting

Requests are rate-limited per client using a fixed-window policy.

- Clients are identified by the `X-Client-Id` request header.
- If `X-Client-Id` is not provided, the caller's remote IP address is used.
- Each client is currently limited to **20 requests per minute**.
- Requests exceeding the limit receive `429 Too Many Requests`.

The rate limit is configured by the `ClientLimit` policy in `Program.cs`.

Project Idea From : https://roadmap.sh/projects/weather-api-wrapper-service
