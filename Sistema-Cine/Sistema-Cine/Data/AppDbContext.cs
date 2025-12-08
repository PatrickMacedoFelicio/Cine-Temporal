using Microsoft.EntityFrameworkCore;
using Sistema_Cine.Models;

namespace Sistema_Cine.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // Tabela Filmes (principal)
        public DbSet<Filme> Filmes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração opcional para melhorar o SQLite
            modelBuilder.Entity<Filme>(entity =>
            {
                entity.ToTable("Filmes");

                entity.Property(f => f.Titulo)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(f => f.Descricao)
                    .HasColumnType("TEXT");

                entity.Property(f => f.Cidade)
                    .HasMaxLength(120);

                entity.Property(f => f.Latitude);
                entity.Property(f => f.Longitude);
            });
        }
    }
}