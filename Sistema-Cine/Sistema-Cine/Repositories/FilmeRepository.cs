using Microsoft.EntityFrameworkCore;
using SeuProjeto.Repositories;
using Sistema_Cine.Data;
using Sistema_Cine.Models;

namespace Sistema_Cine.Repositories
{
    public class FilmeRepository : IFilmeRepository
    {
        private readonly AppDbContext _context;

        public FilmeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Filme>> ListAsync()
        {
            return await _context.Filmes
                .OrderBy(f => f.Titulo)
                .ToListAsync();
        }

        public async Task<Filme> GetByIdAsync(int id)
        {
            return await _context.Filmes.FindAsync(id);
        }

        public async Task CreateAsync(Filme filme)
        {
            filme.DataCriacao = DateTime.UtcNow;
            filme.DataAtualizacao = DateTime.UtcNow;

            await _context.Filmes.AddAsync(filme);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Filme filme)
        {
            filme.DataAtualizacao = DateTime.UtcNow;

            _context.Filmes.Update(filme);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var filme = await GetByIdAsync(id);

            if (filme != null)
            {
                _context.Filmes.Remove(filme);
                await _context.SaveChangesAsync();
            }
        }
    }
}