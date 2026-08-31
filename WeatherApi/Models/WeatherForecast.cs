namespace WeatherApi.Models;

public class WeatherForecast
{
    public string City { get; set; } = string.Empty;
    public string TimeZone { get; set; } = string.Empty;
    public string CurrentDateTime { get; set; } = string.Empty;
    public decimal Temperature { get; set; }
    public decimal FeelsLike { get; set; }
    public decimal Humidity { get; set; }
    public decimal WindSpeed { get; set; }
}
