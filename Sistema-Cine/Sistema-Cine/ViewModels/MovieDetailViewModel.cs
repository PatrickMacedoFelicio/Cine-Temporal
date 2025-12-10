using System.Collections.Generic;

namespace Sistema_Cine.ViewModels
{
    public class MovieDetailViewModel
    {
        public FilmeViewModel Filme { get; set; }
        public IEnumerable<WeatherDayViewModel> Forecast { get; set; }
    }
}