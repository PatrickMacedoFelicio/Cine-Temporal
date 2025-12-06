using Microsoft.Extensions.Caching.Memory;
using Sistema_Cine.DTOs;
using Sistema_Cine.Services.Interfaces;
using System.Text.Json;

namespace SeuProjeto.Services
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

        public async Task<WeatherResposta> GetForecastAsync(double latitude, double longitude)
        {
            string chaveCache = $"clima_{latitude}_{longitude}";

            // RF08: Cache Weather (10 min)
            if (_cache.TryGetValue(chaveCache, out WeatherResposta cached)) return cached;

            _logger.LogInformation($"[API Clima] Buscando previsão Lat:{latitude} Lon:{longitude}");

            // URL formatada conforme RF06
            // &timezone=auto garante que a data venha certa para o local
            string url = $"forecast?latitude={latitude}&longitude={longitude}&daily=temperature_2m_max,temperature_2m_min&timezone=auto";

            var resposta = await _httpClient.GetAsync(url);
            resposta.EnsureSuccessStatusCode();

            var resultado = JsonSerializer.Deserialize<WeatherResposta>(await resposta.Content.ReadAsStringAsync());

            // Salva no cache
            _cache.Set(chaveCache, resultado, TimeSpan.FromMinutes(10));
            
            return resultado;
        }
    }
}