using Microsoft.Extensions.Caching.Memory;
using Sistema_Cine.DTOs;
using Sistema_Cine.Services.Interfaces;
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
        
       
        public async Task<WeatherResposta> GetForecastAsync(double latitude, double longitude)
        {
            string chaveCache = $"clima_{latitude}_{longitude}";

            if (_cache.TryGetValue(chaveCache, out WeatherResposta cached)) return cached;

            _logger.LogInformation($"[API Clima] Buscando previsão Lat:{latitude} Lon:{longitude}");

            string url = $"forecast?latitude={latitude}&longitude={longitude}&daily=temperature_2m_max,temperature_2m_min&timezone=auto";

            try 
            {
                var resposta = await _httpClient.GetAsync(url);
                resposta.EnsureSuccessStatusCode();

                var jsonString = await resposta.Content.ReadAsStringAsync();
                _logger.LogInformation($"[API Clima] URL: {_httpClient.BaseAddress}{url}");

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                };

                // Deserializa como array e pega o primeiro resultado
                var resultados = JsonSerializer.Deserialize<WeatherResposta[]>(jsonString, options);
        
                if (resultados == null || resultados.Length == 0)
                {
                    throw new Exception("Nenhum resultado retornado pela API");
                }

                var resultado = resultados[0]; // Pega o primeiro resultado
                _cache.Set(chaveCache, resultado, TimeSpan.FromMinutes(10));
                return resultado;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Erro na requisição HTTP: {ex.Message}");
                throw;
            }
            catch (JsonException ex)
            {
                _logger.LogError($"Erro ao deserializar JSON: {ex.Message}");
                _logger.LogError($"Path: {ex.Path}, LineNumber: {ex.LineNumber}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro não esperado: {ex.Message}");
                throw;
            }
        }
        

    }
}