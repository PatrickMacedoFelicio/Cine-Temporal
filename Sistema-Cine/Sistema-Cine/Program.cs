using Sistema_Cine.Data;
using Sistema_Cine.Repositories;
using Microsoft.EntityFrameworkCore;
using Sistema_Cine.Services;
using Sistema_Cine.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// 1. Habilitar Cache em Memória
builder.Services.AddMemoryCache();

// 2. Configurar o Cliente HTTP para o TMDb
builder.Services.AddHttpClient<ITmdbApiService, TmdbApiService>(client =>
{
    client.BaseAddress = new Uri("https://api.themoviedb.org/3/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

// 3. Configurar o Cliente HTTP para o Open-Meteo
builder.Services.AddHttpClient<IWeatherApiService, WeatherApiService>(client =>
{
    client.BaseAddress = new Uri("https://api.open-meteo.com/v1/");
});

// 4) Configuração do SQLite (RF11)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=filmes.db"));

// 5) Registro do repositório (RF11)
builder.Services.AddScoped<IFilmeRepository, FilmeRepository>();

// 6) Registro dos serviços
builder.Services.AddScoped<IExportService, ExportService>();
builder.Services.AddScoped<ILogService, LogService>();

// 7) MVC
builder.Services.AddControllersWithViews();

// ==== TUDO ACIMA É REGISTRO ====
// ==== DAQUI PARA BAIXO É O PIPELINE ====

var app = builder.Build();

// 8) Criar banco automaticamente
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

// 9) Configure pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.MapStaticAssets();

app.UseRouting();
app.UseAuthorization();

// Rotas MVC
app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();