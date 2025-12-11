
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
        
        
        public async Task<TmdbRespostaBusca> GetPopularMoviesAsync(int pagina)
        {
            string chaveCache = $"filmes_populares_pagina_{pagina}";
    
            if (_cache.TryGetValue(chaveCache, out TmdbRespostaBusca resultadoCache))
            {
                _logger.LogInformation($"[CACHE] Filmes populares recuperados da página {pagina}");
                return resultadoCache;
            }

            try
            {
                _logger.LogInformation($"[API TMDb] Buscando filmes populares, Página: {pagina}");

                var resposta = await _httpClient.GetAsync($"movie/popular?api_key={_apiKey}&page={pagina}&language=pt-BR");
                resposta.EnsureSuccessStatusCode();

                var conteudo = await resposta.Content.ReadAsStringAsync();
                var resultado = JsonSerializer.Deserialize<TmdbRespostaBusca>(conteudo);

                _cache.Set(chaveCache, resultado, TimeSpan.FromMinutes(5));

                return resultado;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao buscar filmes populares na página {pagina}");
                throw;
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
        
        
        public async Task<TmdbRespostaBusca> GetPopularMoviesAsync(int pagina = 1)
        {
            string chaveCache = $"filmes_populares_pagina_{pagina}";
    
            if (_cache.TryGetValue(chaveCache, out TmdbRespostaBusca resultadoCache))
            {
                _logger.LogInformation($"[CACHE] Filmes populares recuperados da página: {pagina}");
                return resultadoCache;
            }

            try
            {
                _logger.LogInformation($"[API TMDb] Buscando filmes populares, Página: {pagina}");

                var resposta = await _httpClient.GetAsync($"movie/popular?api_key={_apiKey}&page={pagina}&language=pt-BR");
                resposta.EnsureSuccessStatusCode();

                var conteudo = await resposta.Content.ReadAsStringAsync();
                var resultado = JsonSerializer.Deserialize<TmdbRespostaBusca>(conteudo);

                _cache.Set(chaveCache, resultado, TimeSpan.FromMinutes(5));

                return resultado;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao buscar filmes populares no TMDb");
                throw;
            }
        }
        
        
// Filmes em cartaz nos cinemas
public async Task<TmdbRespostaBusca> GetNowPlayingMoviesAsync(int pagina = 1)
{
    string chaveCache = $"filmes_em_cartaz_pagina_{pagina}";
    
    if (_cache.TryGetValue(chaveCache, out TmdbRespostaBusca resultadoCache))
    {
        _logger.LogInformation($"[CACHE] Filmes em cartaz recuperados da página: {pagina}");
        return resultadoCache;
    }

    try
    {
        var resposta = await _httpClient.GetAsync($"movie/now_playing?api_key={_apiKey}&page={pagina}&language=pt-BR");
        resposta.EnsureSuccessStatusCode();

        var resultado = JsonSerializer.Deserialize<TmdbRespostaBusca>(
            await resposta.Content.ReadAsStringAsync());

        _cache.Set(chaveCache, resultado, TimeSpan.FromMinutes(5));
        return resultado;
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Erro ao buscar filmes em cartaz no TMDb");
        throw;
    }
}

// Próximos lançamentos
public async Task<TmdbRespostaBusca> GetUpcomingMoviesAsync(int pagina = 1)
{
    string chaveCache = $"proximos_lancamentos_pagina_{pagina}";
    
    if (_cache.TryGetValue(chaveCache, out TmdbRespostaBusca resultadoCache))
    {
        _logger.LogInformation($"[CACHE] Próximos lançamentos recuperados da página: {pagina}");
        return resultadoCache;
    }

    try
    {
        var resposta = await _httpClient.GetAsync($"movie/upcoming?api_key={_apiKey}&page={pagina}&language=pt-BR");
        resposta.EnsureSuccessStatusCode();

        var resultado = JsonSerializer.Deserialize<TmdbRespostaBusca>(
            await resposta.Content.ReadAsStringAsync());

        _cache.Set(chaveCache, resultado, TimeSpan.FromMinutes(5));
        return resultado;
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Erro ao buscar próximos lançamentos no TMDb");
        throw;
    }
}

// Filmes mais bem avaliados
public async Task<TmdbRespostaBusca> GetTopRatedMoviesAsync(int pagina = 1)
{
    string chaveCache = $"filmes_top_rated_pagina_{pagina}";
    
    if (_cache.TryGetValue(chaveCache, out TmdbRespostaBusca resultadoCache))
    {
        _logger.LogInformation($"[CACHE] Filmes mais bem avaliados recuperados da página: {pagina}");
        return resultadoCache;
    }

    try
    {
        var resposta = await _httpClient.GetAsync($"movie/top_rated?api_key={_apiKey}&page={pagina}&language=pt-BR");
        resposta.EnsureSuccessStatusCode();

        var resultado = JsonSerializer.Deserialize<TmdbRespostaBusca>(
            await resposta.Content.ReadAsStringAsync());

        _cache.Set(chaveCache, resultado, TimeSpan.FromMinutes(5));
        return resultado;
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Erro ao buscar filmes mais bem avaliados no TMDb");
        throw;
    }
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
