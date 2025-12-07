using Sistema_Cine.DTOs;

namespace Sistema_Cine.Services.Interfaces
{
    public interface IWeatherApiService
    {
        Task<WeatherResposta> GetForecastAsync(double latitude, double longitude);
    }
}