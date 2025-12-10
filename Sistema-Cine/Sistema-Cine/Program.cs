using Sistema_Cine.Data;
using Sistema_Cine.Repositories;
using Microsoft.EntityFrameworkCore;
using Sistema_Cine.Services;
using Sistema_Cine.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection; 

var builder = WebApplication.CreateBuilder(args);

// RF08: Habilitar Cache em Memória
builder.Services.AddMemoryCache();

// RF05: Configurar o Cliente HTTP para o TMDb
builder.Services.AddHttpClient<ITmdbApiService, TmdbApiService>(client =>
{
    client.BaseAddress = new Uri("https://api.themoviedb.org/3/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

// RF06: Configurar o Cliente HTTP para o Open-Meteo
builder.Services.AddHttpClient<IWeatherApiService, WeatherApiService>(client =>
{
    client.BaseAddress = new Uri("https://api.open-meteo.com/v1/");
});

// RF11: Configuração do SQLite e DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=filmes.db"));

// RF09: Serviço de Log (CORRIGIDO: Movido para CIMA)
builder.Services.AddScoped<ILogService, LogService>();

// RF12: Serviço de Exportação (CORRIGIDO: Movido para CIMA)
builder.Services.AddScoped<ExportService>();

// RF11: Repositório de Filmes
builder.Services.AddScoped<IFilmeRepository, FilmeRepository>();

builder.Services.AddControllersWithViews();


var app = builder.Build(); // O DI Container é finalizado AQUI.

// RF11: Criar banco automaticamente (Execução após o Build)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    // É uma boa prática usar EnsureCreated apenas para prototipagem/SQLite simples, 
    // mas atende ao seu RF11 (sem migrations).
    db.Database.EnsureCreated(); 
}

// 5) Configure pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// Adicionando o UseStaticFiles para garantir o carregamento de CSS, JS, etc.
app.UseStaticFiles(); 

app.UseRouting();
app.UseAuthorization();

// Rotas MVC
app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();