using Microsoft.Extensions.Caching.Memory;
using Sistema_Cine.DTOs;
using Sistema_Cine.Services.Interfaces;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sistema_Cine.Services
{
    public class WeatherApiService : IWeatherApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;
        private readonly ILogger<WeatherApiService> _logger;

        public WeatherApiService(HttpClient httpClient, IMemoryCache cache, ILogger<WeatherApiService> logger)
        {
            _httpClient = httpClient;
            _cache = cache;
            _logger = logger;
        }

        public async Task<WeatherResposta?> GetForecastAsync(double latitude, double longitude)
        {
            string chaveCache = $"clima_{latitude}_{longitude}";

            if (_cache.TryGetValue(chaveCache, out WeatherResposta cached))
                return cached;

            // IMPORTANTÍSSIMO: força "." como separador decimal
            string lat = latitude.ToString(CultureInfo.InvariantCulture);
            string lon = longitude.ToString(CultureInfo.InvariantCulture);

            string url =
                $"forecast?latitude={lat}" +
                $"&longitude={lon}" +
                $"&daily=temperature_2m_max,temperature_2m_min" +
                $"&timezone=auto";

            _logger.LogInformation($"[WEATHER API] Chamando: {_httpClient.BaseAddress}{url}");

            try
            {
                var resposta = await _httpClient.GetAsync(url);
                resposta.EnsureSuccessStatusCode();

                var json = await resposta.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                // A Open-Meteo RETORNA UM OBJETO, NÃO ARRAY!
                var resultado = JsonSerializer.Deserialize<WeatherResposta>(json, options);

                if (resultado == null)
                    throw new Exception("JSON retornado pela API é nulo.");

                _cache.Set(chaveCache, resultado, TimeSpan.FromMinutes(10));

                return resultado;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao consultar previsão do tempo para {latitude},{longitude}");
                throw;
            }
        }
    }
}
