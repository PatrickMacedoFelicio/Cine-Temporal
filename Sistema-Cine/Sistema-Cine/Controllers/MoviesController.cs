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

        // ----------------------------------------
        // INDEX (Catálogo Local)
        // ----------------------------------------
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            var filmes = (await _repo.ListAsync()).ToList();

            var paged = filmes
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return View(new MoviesIndexViewModel
            {
                Movies = paged.Select(MapToViewModel),
                Page = page,
                HasNext = filmes.Count > page * pageSize
            });
        }

        // ----------------------------------------
        // SEARCH (TMDb)
        // ----------------------------------------
        public async Task<IActionResult> Search(string q, int page = 1)
        {
            if (string.IsNullOrWhiteSpace(q))
                return View(new SearchViewModel());

            var api = await _tmdb.SearchMoviesAsync(q, page);

            var results = api.Resultados.Select(r =>
            {
                DateTime? data = null;
                if (DateTime.TryParse(r.DataLancamento, out var dt))
                    data = dt;

                return new FilmeViewModel
                {
                    TmdbId = r.Id,
                    Titulo = r.Titulo,
                    TituloOriginal = r.Titulo,
                    DataLancamento = data,
                    PosterFullPath = r.CaminhoPoster != null
                        ? $"https://image.tmdb.org/t/p/w500{r.CaminhoPoster}"
                        : "/img/no-poster.png"
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

        // ----------------------------------------
        // IMPORTAR FILME DO TMDB
        // ----------------------------------------
        [HttpPost]
        public async Task<IActionResult> Import(int tmdbId)
        {
            var detalhes = await _tmdb.GetMovieDetailsAsync(tmdbId);
            if (detalhes == null)
                return NotFound();

            DateTime? data = null;
            if (DateTime.TryParse(detalhes.DataLancamento, out var dt))
                data = dt;

            var filme = new Filme
            {
                TmdbId = detalhes.Id,
                Titulo = detalhes.Titulo,
                TituloOriginal = detalhes.Titulo,
                Descricao = detalhes.Sinopse,
                DataLancamento = data,
                Genero = detalhes.Generos.FirstOrDefault()?.Nome,
                Duracao = detalhes.DuracaoMinutos,
                NotaMedia = (decimal?)detalhes.NotaMedia,
                PosterPath = detalhes.CaminhoPoster
            };

            await _repo.CreateAsync(filme);
            return RedirectToAction(nameof(Index));
        }

        // ----------------------------------------
        // DETAILS
        // ----------------------------------------
        public async Task<IActionResult> Details(int id)
        {
            var filme = await _repo.GetByIdAsync(id);
            if (filme == null)
                return NotFound();

            var vm = new MovieDetailViewModel
            {
                Filme = MapToViewModel(filme),
                Forecast = new List<WeatherDayViewModel>()
            };

            // CLIMA
            if (filme.Latitude.HasValue && filme.Longitude.HasValue)
            {
                var clima = await _weather.GetForecastAsync(
                    filme.Latitude.Value,
                    filme.Longitude.Value);

                if (clima?.Diario?.Datas != null)
                {
                    vm.Forecast = clima.Diario.Datas.Select((dia, i) =>
                    {
                        var date = DateTime.Parse(dia);

                        return new WeatherDayViewModel
                        {
                            Date = date,
                            TempMin = clima.Diario.TemperaturaMinima?[i] ?? 0,
                            TempMax = clima.Diario.TemperaturaMaxima?[i] ?? 0,
                            Temperature =
                                (clima.Diario.TemperaturaMinima?[i] ?? 0).ToString("0.0") +
                                "° / " +
                                (clima.Diario.TemperaturaMaxima?[i] ?? 0).ToString("0.0") +
                                "°",
                            Description = "Clima estimado",
                            IconUrl = "/img/weather/default.png"
                        };
                    });
                }
            }

            return View(vm);
        }

        // ----------------------------------------
        // EXPORT CSV / EXCEL
        // ----------------------------------------
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

        // ----------------------------------------
        // MAP → FILMEVIEWMODEL
        // ----------------------------------------
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
                NotaMedia = f.NotaMedia,
                ElencoPrincipal = f.ElencoPrincipal,
                PosterFullPath = f.PosterCompleto,
                CidadeReferencia = f.Cidade,
                Latitude = f.Latitude,
                Longitude = f.Longitude
            };
        }
    }
}
