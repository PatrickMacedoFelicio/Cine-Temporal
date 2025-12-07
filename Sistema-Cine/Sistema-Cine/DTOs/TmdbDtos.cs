using System.Text.Json.Serialization;

namespace Sistema_Cine.DTOs
{
    // Resposta da busca (/search/movie)
    public class TmdbRespostaBusca
    {
        [JsonPropertyName("results")]
        public List<TmdbResumoFilme> Resultados { get; set; } = new();

        [JsonPropertyName("page")]
        public int PaginaAtual { get; set; }

        [JsonPropertyName("total_pages")]
        public int TotalPaginas { get; set; }
    }

    public class TmdbResumoFilme
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Titulo { get; set; }

        [JsonPropertyName("poster_path")]
        public string? CaminhoPoster { get; set; } 

        [JsonPropertyName("release_date")]
        public string DataLancamento { get; set; }
    }

    // Detalhes completos (/movie/{id})
    // Herda do Resumo para reaproveitar ID e Título
    public class TmdbDetalhesFilme : TmdbResumoFilme
    {
        [JsonPropertyName("overview")]
        public string Sinopse { get; set; }

        [JsonPropertyName("vote_average")]
        public double NotaMedia { get; set; }

        [JsonPropertyName("genres")]
        public List<Genero> Generos { get; set; }

        [JsonPropertyName("runtime")]
        public int? DuracaoMinutos { get; set; }
    }

    public class Genero
    {
        [JsonPropertyName("name")]
        public string Nome { get; set; }
    }

    // Configuração para montar URL de imagens (/configuration)
    public class TmdbRespostaConfig
    {
        [JsonPropertyName("images")]
        public TmdbConfigImagens Imagens { get; set; }
    }

    public class TmdbConfigImagens
    {
        [JsonPropertyName("secure_base_url")]
        public string UrlBaseSegura { get; set; }

        [JsonPropertyName("poster_sizes")]
        public List<string> TamanhosPoster { get; set; }
    }
}