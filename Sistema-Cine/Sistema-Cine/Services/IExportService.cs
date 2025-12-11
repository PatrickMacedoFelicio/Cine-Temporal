using Sistema_Cine.Models;

namespace Sistema_Cine.Services.Interfaces
{
    public interface IExportService
    {
        byte[] ExportarCsv(IEnumerable<Filme> filmes);
        byte[] ExportarExcel(IEnumerable<Filme> filmes);
    }
}