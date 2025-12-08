using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sistema_Cine.Models
{
    public class Filme
    {
        [Key]
        public int Id { get; set; } 
        
        // TMDb

        [Required]
        [Display(Name = "Título")]
        public string Titulo { get; set; }

        [Display(Name = "Descrição")]
        [DataType(DataType.MultilineText)]
        public string? Descricao { get; set; }

        [Display(Name = "Data de Lançamento")]
        public string? DataLancamento { get; set; } // O TMDb envia como string yyyy-mm-dd

        [Display(Name = "Nota (TMDb)")]
        public double? Nota { get; set; }

        [Display(Name = "ID do TMDb")]
        public int? TmdbId { get; set; }

        // Caminho parcial recebido da API
        [Display(Name = "Poster TMDb")]
        public string? PosterPath { get; set; }

        // Caminho completo montado no Controller
        [NotMapped]
        public string PosterCompleto =>
            string.IsNullOrWhiteSpace(PosterPath)
                ? "/img/no-poster.png"
                : $"https://image.tmdb.org/t/p/w500{PosterPath}";
        
        // Campos adicionados manualmente (RF03)


        [Display(Name = "Cidade")]
        public string? Cidade { get; set; }

        [Display(Name = "Latitude")]
        public double? Latitude { get; set; }

        [Display(Name = "Longitude")]
        public double? Longitude { get; set; }
        
        // Campos auxiliares para organização

        [Display(Name = "Data de Importação")]
        public DateTime DataImportacao { get; set; } = DateTime.Now;
    }
}
