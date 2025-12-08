using Microsoft.AspNetCore.Mvc;
using Sistema_Cine.Models;
using Sistema_Cine.Repositories;
using Sistema_Cine.Services;
using Sistema_Cine.Services.Interfaces;
using System.Globalization;
using System.Text;

namespace Sistema_Cine.Controllers
{
    public class FilmesController : Controller
    {
        private readonly IFilmeRepository _repo;
        private readonly ITmdbApiService _tmdb;
        private readonly IWeatherApiService _weather;
        private readonly IExportService _export;

        public FilmesController(
            IFilmeRepository repo,
            ITmdbApiService tmdb,
            IWeatherApiService weather,
            IExportService export)
        {
            _repo = repo;
            _tmdb = tmdb;
            _weather = weather;
            _export = export;
        }

        // GET: /Filmes
        public async Task<IActionResult> Index()
        {
            var filmes = await _repo.ListAsync();
            return View(filmes);
        }

        // GET: /Filmes/Buscar?termo=batman&page=1
        public async Task<IActionResult> Buscar(string termo, int page = 1)
        {
            if (string.IsNullOrWhiteSpace(termo))
                return View(new Sistema_Cine.DTOs.TmdbRespostaBusca());

            var resultado = await _tmdb.SearchMoviesAsync(termo, page);

            ViewBag.Termo = termo;
            return View(resultado);
        }

        // POST: /Filmes/ImportarFromApi/123
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ImportarFromApi(int tmdbId)
        {
            var detalhes = await _tmdb.GetMovieDetailsAsync(tmdbId);
            if (detalhes == null) return NotFound();

            var config = await _tmdb.GetConfigurationAsync();

            string posterCompleto = detalhes.CaminhoPoster;

            if (!string.IsNullOrEmpty(detalhes.CaminhoPoster) && config?.Imagens != null)
            {
                var baseUrl = config.Imagens.UrlBaseSegura?.TrimEnd('/');
                var tamanho = config.Imagens.TamanhosPoster?.FirstOrDefault() ?? "w500";
                posterCompleto = $"{baseUrl}/{tamanho}{detalhes.CaminhoPoster}";
            }

            var filme = new Filme
            {
                TmdbId = detalhes.Id,
                Titulo = detalhes.Titulo,
                Descricao = detalhes.Sinopse,
                DataLancamento = detalhes.DataLancamento,
                Nota = detalhes.NotaMedia,
                PosterPath = detalhes.CaminhoPoster,
                Cidade = null,             // RF03: preenchido manualmente
                Latitude = null,
                Longitude = null
            };

            await _repo.CreateAsync(filme);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Filmes/Create
        public IActionResult Create()
        {
            return View(new Filme());
        }

        // POST: /Filmes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Filme filme)
        {
            if (!ModelState.IsValid)
                return View(filme);

            await _repo.CreateAsync(filme);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Filmes/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var filme = await _repo.GetByIdAsync(id);

            if (filme == null)
                return NotFound();

            return View(filme);
        }

        // POST: /Filmes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Filme filme)
        {
            if (id != filme.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(filme);

            await _repo.UpdateAsync(filme);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Filmes/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var filme = await _repo.GetByIdAsync(id);

            if (filme == null)
                return NotFound();

            if (filme.Latitude.HasValue && filme.Longitude.HasValue)
            {
                try
                {
                    ViewBag.Clima = await _weather.GetForecastAsync(
                        filme.Latitude.Value,
                        filme.Longitude.Value
                    );
                }
                catch
                {
                    ViewBag.Clima = null;
                }
            }

            return View(filme);
        }

        // GET: /Filmes/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var filme = await _repo.GetByIdAsync(id);

            if (filme == null) return NotFound();

            return View(filme);
        }

        // POST: /Filmes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _repo.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Filmes/ExportCsv
        public async Task<FileResult> ExportCsv()
        {
            var filmes = (await _repo.ListAsync()).ToList();
            var bytes = _export.ExportFilmesToCsv(filmes);

            return File(bytes, "text/csv", "filmes.csv");
        }


        // GET: /Filmes/ExportExcel
        public async Task<FileResult> ExportExcel()
        {
            var filmes = (await _repo.ListAsync()).ToList();
            var bytes = _export.ExportFilmesToExcel(filmes);

            return File(bytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "filmes.xlsx");
        }

    }
}
