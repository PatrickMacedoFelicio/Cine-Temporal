using System.Collections.Generic;

namespace Sistema_Cine.ViewModels
{
    public class MoviesIndexViewModel
    {
        public IEnumerable<MovieViewModel> Movies { get; set; }

        public int Page { get; set; }
        public bool HasNext { get; set; }
    }
}