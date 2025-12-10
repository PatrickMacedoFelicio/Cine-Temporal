using System.Collections.Generic;

namespace Sistema_Cine.ViewModels
{
    public class SearchViewModel
    {
        public string Query { get; set; }
        public int Page { get; set; }
        public int TotalPages { get; set; }
        
        public IEnumerable<FilmeViewModel> Results { get; set; }
    }
}