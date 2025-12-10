using Microsoft.AspNetCore.Mvc;
using Sistema_Cine.Models;
using Sistema_Cine.Repositories;
using Sistema_Cine.Services.Interfaces;
using Sistema_Cine.ViewModels;

namespace Sistema_Cine.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IFilmeRepository _repo;
        private readonly ITmdbApiService _tmdb;
        private readonly IWeatherApiService _weather;
        private readonly IExportService _export;

        public MoviesController(
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

        // ------------------------------------------------------------
        // MAP → MODEL
        // ------------------------------------------------------------
        private Filme MapToModel(FilmeViewModel vm)
        {
            return new Filme
            {
                Id = vm.Id,
                TmdbId = vm.TmdbId,
                Titulo = vm.Titulo,
                TituloOriginal = vm.TituloOriginal,
                Descricao = vm.Sinopse,
                DataLancamento = vm.DataLancamento,
                Genero = vm.Genero,
                Lingua = vm.Lingua,
                Duracao = vm.Duracao,
                // CONVERSÃO CORRIGIDA:
                NotaMedia = vm.NotaMedia.HasValue 
                    ? (decimal?)vm.NotaMedia.Value 
                    : null,
                ElencoPrincipal = vm.ElencoPrincipal,
                PosterPath = vm.PosterPath,
                Cidade = vm.CidadeReferencia,
                Latitude = vm.Latitude,
                Longitude = vm.Longitude
            };
        }


        // ------------------------------------------------------------
        // MAP → VIEWMODEL
        // ------------------------------------------------------------
        private FilmeViewModel MapToViewModel(Filme f)
        {
            return new FilmeViewModel
            {
                Id = f.Id,
                TmdbId = f.TmdbId,
                Titulo = f.Titulo,
                TituloOriginal = f.TituloOriginal ?? f.Titulo,
                Sinopse = f.Descricao,
                DataLancamento = f.DataLancamento,
                Genero = f.Genero,
                Lingua = f.Lingua,
                Duracao = f.Duracao,
                // CONVERSÃO CORRIGIDA:
                NotaMedia = f.NotaMedia.HasValue
                    ? (double?)f.NotaMedia.Value
                    : null,
                ElencoPrincipal = f.ElencoPrincipal,
                PosterPath = f.PosterPath,
                PosterUrl = f.PosterCompleto,
                CidadeReferencia = f.Cidade,
                Latitude = f.Latitude,
                Longitude = f.Longitude
            };
        }

        // ------------------------------------------------------------
        // CREATE
        // ------------------------------------------------------------
        public IActionResult Create() => View(new FilmeViewModel());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FilmeViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            try
            {
                var filme = MapToModel(viewModel);

                filme.DataAtualizacao = DateTime.UtcNow;

                await _repo.CreateAsync(filme);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "Erro ao salvar filme.");
                return View(viewModel);
            }
        }

        // ------------------------------------------------------------
        // EDIT
        // ------------------------------------------------------------
        public async Task<IActionResult> Edit(int id)
        {
            var filme = await _repo.GetByIdAsync(id);
            if (filme == null) return NotFound();

            return View(MapToViewModel(filme));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, FilmeViewModel vm)
        {
            if (id != vm.Id) return NotFound();
            if (!ModelState.IsValid) return View(vm);

            try
            {
                var filme = await _repo.GetByIdAsync(id);
                if (filme == null) return NotFound();

                filme.Titulo = vm.Titulo;
                filme.TituloOriginal = vm.TituloOriginal;
                filme.Descricao = vm.Sinopse;
                filme.DataLancamento = vm.DataLancamento;
                filme.Genero = vm.Genero;
                filme.Duracao = vm.Duracao;
                filme.NotaMedia = vm.NotaMedia;
                filme.ElencoPrincipal = vm.ElencoPrincipal;
                filme.Cidade = vm.CidadeReferencia;
                filme.Latitude = vm.Latitude;
                filme.Longitude = vm.Longitude;
                filme.DataAtualizacao = DateTime.UtcNow;

                await _repo.UpdateAsync(filme);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "Erro ao atualizar filme.");
                return View(vm);
            }
        }

        // ------------------------------------------------------------
        // DELETE (MODAL)
        // ------------------------------------------------------------
        public async Task<IActionResult> Delete(int id)
        {
            var filme = await _repo.GetByIdAsync(id);
            if (filme == null) return NotFound();

            var vm = MapToViewModel(filme);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return PartialView("_ModalDeleteFilme", vm);

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _repo.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // ------------------------------------------------------------
        // INDEX (CATÁLOGO LOCAL)
        // ------------------------------------------------------------
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            var filmes = (await _repo.ListAsync()).ToList();

            var paged = filmes.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return View(new MoviesIndexViewModel
            {
                Movies = paged.Select(MapToViewModel),
                Page = page,
                PageSize = pageSize,
                TotalMovies = filmes.Count,
                TotalPages = (int)Math.Ceiling(filmes.Count / (double)pageSize)
            });
        }

        // ------------------------------------------------------------
        // SEARCH (TMDb)
        // ------------------------------------------------------------
        public async Task<IActionResult> Search(string q, int page = 1)
        {
            if (string.IsNullOrWhiteSpace(q))
                return View(new SearchViewModel());

            var api = await _tmdb.SearchMoviesAsync(q, page);

            var results = api.Resultados.Select(r =>
            {
                DateTime? data = DateTime.TryParse(r.DataLancamento, out var dt) ? dt : null;

                return new FilmeViewModel
                {
                    TmdbId = r.Id,
                    Titulo = r.Titulo,
                    TituloOriginal = r.Titulo,
                    PosterUrl = r.CaminhoPoster != null
                        ? $"https://image.tmdb.org/t/p/w500{r.CaminhoPoster}"
                        : "/img/no-poster.png",
                    DataLancamento = data
                };
            });

            return View(new SearchViewModel
            {
                Query = q,
                Page = page,
                TotalPages = api.TotalPaginas,
                Results = results
            });
        }

        // ------------------------------------------------------------
        // IMPORT (TMDb → Local)
        // ------------------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> Import(int tmdbId)
        {
            var detalhes = await _tmdb.GetMovieDetailsAsync(tmdbId);
            if (detalhes == null) return NotFound();

            DateTime? data = DateTime.TryParse(detalhes.DataLancamento, out var dt) ? dt : null;

            var filme = new Filme
            {
                TmdbId = detalhes.Id,
                Titulo = detalhes.Titulo,
                TituloOriginal = detalhes.Titulo,
                Descricao = detalhes.Sinopse,
                DataLancamento = data,
                Genero = detalhes.Generos.FirstOrDefault()?.Nome,
                Duracao = detalhes.DuracaoMinutos ?? 0,
                NotaMedia = detalhes.NotaMedia.HasValue
                    ? (decimal)detalhes.NotaMedia.Value
                    : null,
                PosterPath = detalhes.CaminhoPoster,
                DataAtualizacao = DateTime.UtcNow
            };

            await _repo.CreateAsync(filme);
            return RedirectToAction(nameof(Index));
        }

        // ------------------------------------------------------------
        // DETAILS (MODAL)
        // ------------------------------------------------------------
        public async Task<IActionResult> Details(int id)
        {
            var filme = await _repo.GetByIdAsync(id);
            if (filme == null) return NotFound();

            var vm = new MovieDetailViewModel
            {
                Filme = MapToViewModel(filme),
                Forecast = new List<WeatherDayViewModel>(),
                HasCoordinates = filme.Latitude != null && filme.Longitude != null
            };

            if (vm.HasCoordinates)
            {
                var clima = await _weather.GetForecastAsync(filme.Latitude!.Value, filme.Longitude!.Value);

                if (clima?.Diario?.Datas != null)
                {
                    vm.Forecast = clima.Diario.Datas.Select((dia, i) => new WeatherDayViewModel
                    {
                        Date = DateTime.Parse(dia),
                        TempMin = clima.Diario.TemperaturaMinima?[i] ?? 0,
                        TempMax = clima.Diario.TemperaturaMaxima?[i] ?? 0,
                        Temperature = $"{clima.Diario.TemperaturaMinima?[i]:0.0}° / {clima.Diario.TemperaturaMaxima?[i]:0.0}°",
                        Description = "Clima estimado",
                        IconUrl = "/img/weather/default.png"
                    }).ToList();
                }
            }

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return PartialView("_ModalMovieDetails", vm);

            return View(vm);
        }

        // ------------------------------------------------------------
        // EXPORTAÇÃO
        // ------------------------------------------------------------
        public async Task<FileResult> ExportCsv()
        {
            var filmes = (await _repo.ListAsync()).ToList();
            var bytes = _export.ExportarCsv(filmes);
            return File(bytes, "text/csv", "filmes.csv");
        }

        public async Task<FileResult> ExportExcel()
        {
            var filmes = (await _repo.ListAsync()).ToList();
            var bytes = _export.ExportarExcel(filmes);
            return File(bytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "filmes.xlsx");
        }
    }
}
