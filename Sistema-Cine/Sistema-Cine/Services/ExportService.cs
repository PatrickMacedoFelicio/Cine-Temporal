using ClosedXML.Excel;
using Sistema_Cine.Models;
using Sistema_Cine.Services.Interfaces;
using System.Text;

namespace Sistema_Cine.Services
{
    public class ExportService : IExportService
    {
        public byte[] ExportarCsv(IEnumerable<Filme> filmes)
        {
            var sb = new StringBuilder();

            sb.AppendLine("Id;Título;Descrição;Cidade;Latitude;Longitude;DataImportacao");

            foreach (var f in filmes)
            {
                sb.AppendLine($"{f.Id};{f.Titulo};{f.Descricao};{f.Cidade};{f.Latitude};{f.Longitude};{f.DataImportacao}");
            }

            return Encoding.UTF8.GetBytes(sb.ToString());
        }

        public byte[] ExportarExcel(IEnumerable<Filme> filmes)
        {
            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("Filmes");

            ws.Cell(1, 1).Value = "ID";
            ws.Cell(1, 2).Value = "Título";
            ws.Cell(1, 3).Value = "Descrição";
            ws.Cell(1, 4).Value = "Cidade";
            ws.Cell(1, 5).Value = "Latitude";
            ws.Cell(1, 6).Value = "Longitude";
            ws.Cell(1, 7).Value = "Data de Importação";

            int row = 2;

            foreach (var f in filmes)
            {
                ws.Cell(row, 1).Value = f.Id;
                ws.Cell(row, 2).Value = f.Titulo;
                ws.Cell(row, 3).Value = f.Descricao;
                ws.Cell(row, 4).Value = f.Cidade;
                ws.Cell(row, 5).Value = f.Latitude;
                ws.Cell(row, 6).Value = f.Longitude;
                ws.Cell(row, 7).Value = f.DataImportacao;

                row++;
            }

            ws.Columns().AdjustToContents();

            using var ms = new MemoryStream();
            workbook.SaveAs(ms);
            return ms.ToArray();
        }
    }
}