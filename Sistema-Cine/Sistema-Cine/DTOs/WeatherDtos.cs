using System.Text.Json.Serialization;

namespace Sistema_Cine.DTOs
{
    public class WeatherResposta
    {
        [JsonPropertyName("daily")]
        public ClimaDiario Diario { get; set; }
    }

    public class ClimaDiario
    {
        // O Open-Meteo retorna listas (arrays) para cada dia
        [JsonPropertyName("time")]
        public List<string> Datas { get; set; } 

        [JsonPropertyName("temperature_2m_max")]
        public List<double> TemperaturaMaxima { get; set; }

        [JsonPropertyName("temperature_2m_min")]
        public List<double> TemperaturaMinima { get; set; }
    }
}