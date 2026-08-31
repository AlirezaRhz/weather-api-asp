using StackExchange.Redis;
using WeatherApi.Clients;
using WeatherApi.Configuration;
using WeatherApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddOpenApi();

// Configure Visual Crossing Options (Adding to DI and binding)
builder.Services.Configure<VisualCrossingOptions>(builder.Configuration.GetSection("VisualCrossing"));

// Automatic Dependency Injection for VisualCrossingClient (Read: if someone wants IWeatherApiClient, give them an instance of VisualCrossingClient)
// Literally Similar to something like AddScoped except it automatically knows to give VisualCrossingClient an HttpClient if it wants. The name is literally "AddHttpClient"
builder.Services.AddHttpClient<IWeatherApiClient, VisualCrossingClient>();

// If someone asks DI for IWeatherService, give them an instance of WeatherService.
// Unlike AddHttpClient, AddScoped does not configure/provide a special HttpClient for the class. Otherwise It's literally the same as the one above.
builder.Services.AddScoped<IWeatherService, WeatherService>();

Console.WriteLine(
    builder.Configuration.GetSection("Redis:ConnectionString").Value
);

// Add Redis to the Dependency injection container :
builder.Services.AddSingleton<ConnectionMultiplexer>(
    ConnectionMultiplexer.Connect(builder.Configuration.GetSection("Redis:ConnectionString").Value!)
    );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
