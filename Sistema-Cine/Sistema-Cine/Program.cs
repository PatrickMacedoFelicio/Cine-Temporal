using Sistema_Cine.Data;
using Sistema_Cine.Repositories;
using Microsoft.EntityFrameworkCore;
using Sistema_Cine.Services;
using Sistema_Cine.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Cache
builder.Services.AddMemoryCache();

// TMDb HTTP Client
builder.Services.AddHttpClient<ITmdbApiService, TmdbApiService>(client =>
{
    client.BaseAddress = new Uri("https://api.themoviedb.org/3/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

// Weather HTTP Client
builder.Services.AddHttpClient<IWeatherApiService, WeatherApiService>(client =>
{
    client.BaseAddress = new Uri("https://api.open-meteo.com/v1/");
});

// SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=filmes.db"));

// Log Service
builder.Services.AddScoped<ILogService, LogService>();

// Export Service  ✅ CORRIGIDO
builder.Services.AddScoped<IExportService, ExportService>();

// Repository
builder.Services.AddScoped<IFilmeRepository, FilmeRepository>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Criar banco automaticamente
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();