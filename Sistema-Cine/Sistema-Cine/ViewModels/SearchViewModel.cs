using System.Collections.Generic;

namespace Sistema_Cine.ViewModels
{
    public class SearchViewModel
    {
        // Consulta
        public string Query { get; set; }

        // Resultados
        public IEnumerable<FilmeViewModel> Results { get; set; } = new List<FilmeViewModel>();

        // Paginação TMDb (RF13)
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public int TotalResults { get; set; }    // novo
        public int PageSize { get; set; } = 20;  // novo

        // Conveniência
        public bool HasResults => Results != null && Results.Any();

        // Tratamento de erros (RF09)
        public string ErrorMessage { get; set; }
    }
}