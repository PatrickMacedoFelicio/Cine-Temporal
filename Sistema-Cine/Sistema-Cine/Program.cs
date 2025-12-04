using Sistema_Cine.Data;
using Sistema_Cine.Repositories;
using Microsoft.EntityFrameworkCore;
using SeuProjeto.Repositories;

var builder = WebApplication.CreateBuilder(args);

// 1) Configuração do SQLite (RF11)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=filmes.db"));

// 2) Registro do repositório (RF11)
builder.Services.AddScoped<IFilmeRepository, FilmeRepository>();

// 3) MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

// 4) Criar banco automaticamente
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated(); // cria o banco e tabelas se não existirem
}

// 5) Configure pipeline
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