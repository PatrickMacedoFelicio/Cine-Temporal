using System;

namespace Sistema_Cine.ViewModels
{
    public class FilmeViewModel
    {
        // Identificador local
        public int Id { get; set; }

        // Identificador TMDb (nullable para filmes criados manualmente)
        public int? TmdbId { get; set; }

        // Títulos
        public string Titulo { get; set; } = "";
        public string TituloOriginal { get; set; } = "";

        // Conteúdo
        public string Sinopse { get; set; } = "";
        public string Genero { get; set; } = "";
        public string Lingua { get; set; } = "";

        // Dados técnicos
        public int? Duracao { get; set; }
        public double? NotaMedia { get; set; }

        // Mídia
        public string PosterPath { get; set; } = "";
        public string PosterUrl { get; set; } = ""; 

        // Elenco
        public string ElencoPrincipal { get; set; } = "";

        // Cidade & Coordenadas
        public string CidadeReferencia { get; set; } = "";
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        // Datas
        public DateTime? DataLancamento { get; set; }
    }
}