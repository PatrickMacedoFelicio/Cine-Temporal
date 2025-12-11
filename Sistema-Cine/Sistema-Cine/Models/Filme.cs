using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sistema_Cine.Models
{
    public class Filme
    {
        [Key]
        public int Id { get; set; }

        // TMDb
        public int? TmdbId { get; set; }

        [Required]
        public string Titulo { get; set; }

        public string? TituloOriginal { get; set; }

        public string? Descricao { get; set; }

        public DateTime? DataLancamento { get; set; }

        public string? Genero { get; set; }

        public string? Lingua { get; set; }

        public int? Duracao { get; set; }

        public decimal? NotaMedia { get; set; }

        public string? ElencoPrincipal { get; set; }

        // TMDb Poster
        public string? PosterPath { get; set; }

        [NotMapped]
        public string PosterCompleto =>
            string.IsNullOrWhiteSpace(PosterPath)
                ? "/img/no-poster.png"
                : $"https://image.tmdb.org/t/p/w500{PosterPath}";

        // Campos RF03 - localização manual
        public string? Cidade { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        // Controle interno
        public DateTime? DataImportacao { get; set; }

        public DateTime? DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }

    }
}