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

        // Campo ausente que gerava erro no Controller
        [JsonPropertyName("total_results")]
        public int TotalResultados { get; set; }
    }


    // RESUMO DE FILME
    public class TmdbResumoFilme
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Titulo { get; set; } = "";

        [JsonPropertyName("poster_path")]
        public string? CaminhoPoster { get; set; }

        // release_date pode vir vazio ou nulo → manter como string?
        [JsonPropertyName("release_date")]
        public string? DataLancamento { get; set; }
    }


    // DETALHES DO FILME
    public class TmdbDetalhesFilme : TmdbResumoFilme
    {
        [JsonPropertyName("overview")]
        public string Sinopse { get; set; } = "";

        // O TMDb retorna double, mas seu banco usa decimal? → conversão precisa ser explícita
        [JsonPropertyName("vote_average")]
        public double? NotaMedia { get; set; }

        [JsonPropertyName("genres")]
        public List<Genero> Generos { get; set; } = new();

        [JsonPropertyName("runtime")]
        public int? DuracaoMinutos { get; set; }
    }


    public class Genero
    {
        [JsonPropertyName("name")]
        public string Nome { get; set; } = "";
    }


    // CONFIGURAÇÃO DE IMAGENS
    public class TmdbRespostaConfig
    {
        [JsonPropertyName("images")]
        public TmdbConfigImagens Imagens { get; set; } = new();
    }


    public class TmdbConfigImagens
    {
        [JsonPropertyName("secure_base_url")]
        public string UrlBaseSegura { get; set; } = "";

        [JsonPropertyName("poster_sizes")]
        public List<string> TamanhosPoster { get; set; } = new();
    }
}
