using Microsoft.EntityFrameworkCore;
using Sistema_Cine.Models;

namespace Sistema_Cine.Data;    

public class AppDbContext : DbContext
{
    public DbSet<Filme> Filmes { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
}