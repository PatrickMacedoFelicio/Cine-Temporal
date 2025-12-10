using Sistema_Cine.DTOs;

namespace Sistema_Cine.Services.Interfaces
{
    public interface ITmdbApiService
    {
        Task<TmdbRespostaBusca> SearchMoviesAsync(string termoBusca, int pagina);
        Task<TmdbDetalhesFilme> GetMovieDetailsAsync(int id);
        Task<TmdbRespostaConfig> GetConfigurationAsync();
        Task<TmdbRespostaBusca> GetPopularMoviesAsync(int pagina);
        Task<TmdbRespostaBusca> GetNowPlayingMoviesAsync(int pagina);
        Task<TmdbRespostaBusca> GetUpcomingMoviesAsync(int pagina);
        Task<TmdbRespostaBusca> GetTopRatedMoviesAsync(int pagina);

        
    }
}