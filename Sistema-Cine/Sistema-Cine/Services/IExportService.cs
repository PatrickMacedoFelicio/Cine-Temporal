using Sistema_Cine.Models;

namespace Sistema_Cine.Services
{
    public interface IExportService
    {
        byte[] ExportFilmesToExcel(List<Filme> filmes);
        byte[] ExportFilmesToCsv(List<Filme> filmes);
    }
}