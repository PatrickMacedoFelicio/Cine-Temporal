using Sistema_Cine.Services.Interfaces;

namespace Sistema_Cine.Services
{
    public class LogService : ILogService
    {
        private readonly string _logPath;

        public LogService()
        {
            // Pasta local "Logs"
            _logPath = Path.Combine(Directory.GetCurrentDirectory(), "Logs");

            if (!Directory.Exists(_logPath))
                Directory.CreateDirectory(_logPath);
        }

        public async Task RegistrarErroAsync(string mensagem, Exception? ex = null)
        {
            string file = Path.Combine(_logPath, "erros.log");

            string texto = $"[{DateTime.Now:dd/MM/yyyy HH:mm:ss}] {mensagem}";

            if (ex != null)
            {
                texto += Environment.NewLine + $"   EX: {ex.Message}";
                texto += Environment.NewLine + $"   STACK: {ex.StackTrace}";
            }

            texto += Environment.NewLine + "---------------------------------" + Environment.NewLine;

            await File.AppendAllTextAsync(file, texto);
        }
    }
}