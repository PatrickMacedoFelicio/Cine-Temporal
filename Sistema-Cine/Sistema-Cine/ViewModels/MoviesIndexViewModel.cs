using System.Collections.Generic;

namespace Sistema_Cine.ViewModels
{
    public class MoviesIndexViewModel
    {
        // Lista de filmes do catálogo local
        public IEnumerable<FilmeViewModel> Movies { get; set; } = new List<FilmeViewModel>();

        // Paginação
        public int Page { get; set; }
        public int TotalPages { get; set; }  // novo
        public int PageSize { get; set; } = 10;  // novo
        public int TotalMovies { get; set; }  // novo

        public bool HasNext => Page < TotalPages;
        public bool HasPrevious => Page > 1;

        // Apenas visual
        public string PersistenciaUsada { get; set; }
    }
}