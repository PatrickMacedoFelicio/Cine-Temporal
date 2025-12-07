using System.Text.Json.Serialization;

namespace Sistema_Cine.DTOs
{
    public class WeatherResposta
    {
        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }

        [JsonPropertyName("generationtime_ms")]
        public double GenerationTimeMs { get; set; }

        [JsonPropertyName("utc_offset_seconds")]
        public int UtcOffsetSeconds { get; set; }

        [JsonPropertyName("timezone")]
        public string? Timezone { get; set; }

        [JsonPropertyName("timezone_abbreviation")]
        public string? TimezoneAbbreviation { get; set; }

        [JsonPropertyName("elevation")]
        public double Elevation { get; set; }

        [JsonPropertyName("daily_units")]
        public UnidadesDiarias? UnidadesDiarias { get; set; }

        [JsonPropertyName("daily")]
        public ClimaDiario? Diario { get; set; }
    }

    public class UnidadesDiarias
    {
        [JsonPropertyName("time")]
        public string? Time { get; set; }

        [JsonPropertyName("temperature_2m_max")]
        public string? TemperaturaMaxima { get; set; }

        [JsonPropertyName("temperature_2m_min")]
        public string? TemperaturaMinima { get; set; }
    }

    public class ClimaDiario
    {
        [JsonPropertyName("time")]
        public List<string>? Datas { get; set; }

        [JsonPropertyName("temperature_2m_max")]
        public List<double>? TemperaturaMaxima { get; set; }

        [JsonPropertyName("temperature_2m_min")]
        public List<double>? TemperaturaMinima { get; set; }
    }
}