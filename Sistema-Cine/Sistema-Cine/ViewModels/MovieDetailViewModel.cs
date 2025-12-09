using System.Collections.Generic;

namespace Sistema_Cine.ViewModels
{
    public class MovieDetailViewModel
    {
        public MovieViewModel Movie { get; set; }
        public IEnumerable<WeatherDayViewModel> Forecast { get; set; }
    }
}