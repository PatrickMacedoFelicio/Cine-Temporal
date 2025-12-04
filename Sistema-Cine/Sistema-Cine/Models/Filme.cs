namespace Sistema_Cine.Models;

public class Filme
{
    public int Id { get; set; }
    public int TmdbId { get; set; }

    public string Titulo { get; set; }
    public string TituloOriginal { get; set; }
    public string Sinopse { get; set; }
    public DateTime? DataLancamento { get; set; }

    public string Genero { get; set; }
    public string PosterPath { get; set; }
    public string Lingua { get; set; }
    public int? Duracao { get; set; }
    public double? NotaMedia { get; set; }

    public string ElencoPrincipal { get; set; }

    // Dados da cidade (da API Open-Meteo)
    public string CidadeReferencia { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }

    // Metadados para logs no futuro
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    public DateTime DataAtualizacao { get; set; } = DateTime.UtcNow;
}