using Microsoft.AspNetCore.Mvc;
using Sistema_Cine.Services.Interfaces;

namespace Sistema_Cine.Controllers
{
    public class TesteApiController : Controller
    {
        private readonly ITmdbApiService _tmdbService;
        private readonly IWeatherApiService _weatherService;
        //

        public TesteApiController(ITmdbApiService tmdbService, IWeatherApiService weatherService)
        {
            _tmdbService = tmdbService;
            _weatherService = weatherService;
        }

        // Teste: /TesteApi/Busca?termo=Batman
        public async Task<IActionResult> Busca(string termo)
        {
            var resultado = await _tmdbService.SearchMoviesAsync(termo, 1);
            return Json(resultado);
        }

        // Teste: /TesteApi/Clima
        public async Task<IActionResult> Clima()
        {
            // Teste com coordenadas de São Paulo
            var resultado = await _weatherService.GetForecastAsync(-23.55, -46.63);
            return Json(resultado);
        }
    }
}