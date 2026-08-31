using System.Text.Json.Serialization;

namespace WeatherApi.Models
{
    public class VisualCrossingResponse
    {
        [JsonPropertyName("timezone")]
        public required string TimeZone { get; set; }
        [JsonPropertyName("currentConditions")]
        public CurrentConditions CurrentConditions { get; set; }
    }

    public class CurrentConditions
    {
        [JsonPropertyName("datetime")]
        public string DateTime { get; set; } = string.Empty;
        [JsonPropertyName("temp")]
        public decimal Temp { get; set; }
        [JsonPropertyName("feelslike")]
        public decimal FeelsLike { get; set; }
        [JsonPropertyName("humidity")]
        public decimal Humidity { get; set; }
        [JsonPropertyName("windspeed")]
        public decimal WindSpeed { get; set; }


    }
}
