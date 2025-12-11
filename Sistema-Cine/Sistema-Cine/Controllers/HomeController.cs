using Microsoft.AspNetCore.Mvc;
using Sistema_Cine.Services.Interfaces;
using Sistema_Cine.ViewModels;

namespace Sistema_Cine.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWeatherApiService _weatherService;

        public HomeController(IWeatherApiService weatherService)
        {
            _weatherService = weatherService;
        }

        public async Task<IActionResult> Index()
        {
            // São Paulo como exemplo
            double lat = -23.5505;
            double lon = -46.6333;

            var clima = await _weatherService.GetForecastAsync(lat, lon);

            var vm = new WeatherHomeViewModel
            {
                Cidade = "São Paulo",
                TemperaturaMinima = clima.Diario?.TemperaturaMinima?.FirstOrDefault(),
                TemperaturaMaxima = clima.Diario?.TemperaturaMaxima?.FirstOrDefault(),
                Data = clima.Diario?.Datas?.FirstOrDefault()
            };

            return View(vm);
        }
    }
}