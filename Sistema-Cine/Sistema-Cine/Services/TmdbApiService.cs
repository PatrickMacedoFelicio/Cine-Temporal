
using Microsoft.Extensions.Caching.Memory;
using Sistema_Cine.DTOs;
using Sistema_Cine.Services.Interfaces;
using System.Text.Json;

namespace Sistema_Cine.Services
{
    public class TmdbApiService : ITmdbApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;
        private readonly ILogger<TmdbApiService> _logger;
        private readonly string _apiKey;

        public TmdbApiService(HttpClient httpClient, IMemoryCache cache, IConfiguration config, ILogger<TmdbApiService> logger)
        {
            _httpClient = httpClient;
            _cache = cache;
            _logger = logger;
            // Busca a chave configurada no User Secrets (RF07)
            _apiKey = config["TMDb:ApiKey"]; 
        }

        public async Task<TmdbRespostaBusca> SearchMoviesAsync(string termoBusca, int pagina)
        {
            // RF08: Cache de busca (TTL 5 min)
            // Cria uma chave única para essa busca específica
            string chaveCache = $"busca_{termoBusca}_{pagina}";
            
            if (_cache.TryGetValue(chaveCache, out TmdbRespostaBusca resultadoCache))
            {
                _logger.LogInformation($"[CACHE] Resultado recuperado para: {termoBusca}");
                return resultadoCache;
            }

            try
            {
                // RF09: Log da requisição externa
                _logger.LogInformation($"[API TMDb] Buscando: {termoBusca}, Página: {pagina}");

                var resposta = await _httpClient.GetAsync($"search/movie?api_key={_apiKey}&query={termoBusca}&page={pagina}&language=pt-BR");
                resposta.EnsureSuccessStatusCode();

                var conteudo = await resposta.Content.ReadAsStringAsync();
                var resultado = JsonSerializer.Deserialize<TmdbRespostaBusca>(conteudo);

                // Salva no Cache por 5 minutos
                _cache.Set(chaveCache, resultado, TimeSpan.FromMinutes(5));

                return resultado;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao buscar filmes no TMDb com o termo: {termoBusca}");
                throw; // Repassa o erro para o Controller tratar
            }
        }

        public async Task<TmdbDetalhesFilme> GetMovieDetailsAsync(int id)
        {
            string chaveCache = $"filme_detalhes_{id}";

            // Tenta pegar do cache primeiro
            if (_cache.TryGetValue(chaveCache, out TmdbDetalhesFilme filmeCache)) 
                return filmeCache;

            _logger.LogInformation($"[API TMDb] Buscando detalhes do ID: {id}");
            
            var resposta = await _httpClient.GetAsync($"movie/{id}?api_key={_apiKey}&language=pt-BR");
            resposta.EnsureSuccessStatusCode();

            var resultado = JsonSerializer.Deserialize<TmdbDetalhesFilme>(await resposta.Content.ReadAsStringAsync());
            
            // RF08: Cache de detalhes (TTL 10 min)
            _cache.Set(chaveCache, resultado, TimeSpan.FromMinutes(10));
            
            return resultado;
        }

        public async Task<TmdbRespostaConfig> GetConfigurationAsync()
        {
            // Configuração muda raramente, cache longo de 24h
            return await _cache.GetOrCreateAsync("tmdb_configuracao", async entrada =>
            {
                entrada.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24);
                
                var resposta = await _httpClient.GetAsync($"configuration?api_key={_apiKey}");
                return JsonSerializer.Deserialize<TmdbRespostaConfig>(await resposta.Content.ReadAsStringAsync());
            });
        }
    }
}