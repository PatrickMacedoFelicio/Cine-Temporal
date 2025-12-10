namespace Sistema_Cine.ViewModels
{
    public class FilmeViewModel
    {
        public int Id { get; set; } // local id
        public int? TmdbId { get; set; }
        public string Titulo { get; set; }
        public string TituloOriginal { get; set; }
        public string Sinopse { get; set; }
        public DateTime? DataLancamento { get; set; }
        public string Genero { get; set; }
        public string PosterFullPath { get; set; } // URL completo do poster

        public string PosterUrl { get; set; }
        public string Lingua { get; set; }
        public int? Duracao { get; set; }
        public decimal? NotaMedia { get; set; }
        public string ElencoPrincipal { get; set; }
        public string CidadeReferencia { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }

}