using System.Collections.Generic;

namespace Sistema_Cine.ViewModels
{
    public class MovieDetailViewModel
    {
        // Dados do filme vindos do banco local (com poster, sinopse, elenco etc.)
        public FilmeViewModel Filme { get; set; } = new FilmeViewModel();

        // Previsão do tempo (lista diária)
        public IEnumerable<WeatherDayViewModel> Forecast { get; set; } = new List<WeatherDayViewModel>();

        // Flag auxiliar para exibição de clima
        public bool HasCoordinates { get; set; }
    }
}