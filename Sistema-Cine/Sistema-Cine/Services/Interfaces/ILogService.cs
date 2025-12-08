namespace Sistema_Cine.Services.Interfaces
{
    public interface ILogService
    {
        Task RegistrarErroAsync(string mensagem, Exception? ex = null);
    }
}