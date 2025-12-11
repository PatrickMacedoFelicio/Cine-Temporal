namespace Sistema_Cine.ViewModels
{
    public class WeatherDayViewModel
    {
        public DateTime Date { get; set; }
        public double TempMin { get; set; }
        public double TempMax { get; set; }
        public string Temperature { get; set; } = "";
        public string Description { get; set; } = "";
        public string IconUrl { get; set; } = "";
    }

}