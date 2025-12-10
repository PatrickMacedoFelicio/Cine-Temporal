using System.Collections.Generic;

namespace Sistema_Cine.ViewModels
{
    public class MoviesIndexViewModel
    {
        public IEnumerable<FilmeViewModel> Movies { get; set; }

        public int Page { get; set; }
        public bool HasNext { get; set; }
    }
}