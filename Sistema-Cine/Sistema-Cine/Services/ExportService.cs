using ClosedXML.Excel;
using Sistema_Cine.Models;
using System.Text;

namespace Sistema_Cine.Services
{
    public class ExportService
    {
        // ===========================================================
        //  EXPORTAR PARA EXCEL (ClosedXML) - JÁ PRONTO
        // ===========================================================
        public byte[] ExportFilmesToExcel(List<Filme> filmes)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Filmes");

            // Cabeçalho
            worksheet.Cell(1, 1).Value = "ID";
            worksheet.Cell(1, 2).Value = "Título";
            worksheet.Cell(1, 3).Value = "Descrição";
            worksheet.Cell(1, 4).Value = "Cidade";
            worksheet.Cell(1, 5).Value = "Latitude";
            worksheet.Cell(1, 6).Value = "Longitude";
            worksheet.Cell(1, 7).Value = "Data de Importação";

            // Estiliza
            var headerRange = worksheet.Range("A1:G1");
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;

            // Conteúdo
            int row = 2;
            foreach (var f in filmes)
            {
                worksheet.Cell(row, 1).Value = f.Id;
                worksheet.Cell(row, 2).Value = f.Titulo;
                worksheet.Cell(row, 3).Value = f.Descricao;
                worksheet.Cell(row, 4).Value = f.Cidade;
                worksheet.Cell(row, 5).Value = f.Latitude;
                worksheet.Cell(row, 6).Value = f.Longitude;
                worksheet.Cell(row, 7).Value = f.DataImportacao;
                row++;
            }

            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }

        // ===========================================================
        //  EXPORTAR PARA CSV (RF12)
        // ===========================================================
        public byte[] ExportFilmesToCsv(List<Filme> filmes)
        {
            var sb = new StringBuilder();

            // Cabeçalho CSV
            sb.AppendLine("Id;Titulo;Descricao;Cidade;Latitude;Longitude;DataImportacao");

            // Linhas
            foreach (var f in filmes)
            {
                sb.AppendLine(
                    $"{f.Id};" +
                    $"{EscapeCsv(f.Titulo)};" +
                    $"{EscapeCsv(f.Descricao)};" +
                    $"{EscapeCsv(f.Cidade)};" +
                    $"{f.Latitude};" +
                    $"{f.Longitude};" +
                    $"{f.DataImportacao:yyyy-MM-dd HH:mm:ss}"
                );
            }

            return Encoding.UTF8.GetBytes(sb.ToString());
        }

        // Evita quebra de CSV quando há ; , " ou quebra de linha
        private string EscapeCsv(string? value)
        {
            if (string.IsNullOrEmpty(value))
                return "";

            if (value.Contains(';') || value.Contains('"') || value.Contains('\n'))
                return $"\"{value.Replace("\"", "\"\"")}\"";

            return value;
        }
    }
}
