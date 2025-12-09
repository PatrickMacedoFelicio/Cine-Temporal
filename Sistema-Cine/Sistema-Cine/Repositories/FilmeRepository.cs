using Microsoft.EntityFrameworkCore;
using Sistema_Cine.Data;
using Sistema_Cine.Models;
using Sistema_Cine.Services.Interfaces;

namespace Sistema_Cine.Repositories
{
    public class FilmeRepository : IFilmeRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogService _log;

        public FilmeRepository(AppDbContext context, ILogService log)
        {
            _context = context;
            _log = log;
        }

        // LISTAR TODOS
        public async Task<IEnumerable<Filme>> ListAsync()
        {
            try
            {
                return await _context.Filmes
                    .AsNoTracking()
                    .OrderBy(f => f.Titulo)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERRO] ListAsync: {ex.Message}");
                return Enumerable.Empty<Filme>();
            }
        }

        // BUSCAR POR ID
        public async Task<Filme?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Filmes
                    .AsNoTracking()
                    .FirstOrDefaultAsync(f => f.Id == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERRO] GetByIdAsync: {ex.Message}");
                return null;
            }
        }

        // CRIAR
        public async Task CreateAsync(Filme filme)
        {
            try
            {
                filme.DataImportacao = DateTime.Now;
                await _context.Filmes.AddAsync(filme);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERRO] CreateAsync: {ex.Message}");
                await _log.RegistrarErroAsync("Erro ao criar filme.", ex);
                throw;
            }
        }

        // ATUALIZAR
        public async Task UpdateAsync(Filme filme)
        {
            try
            {
                var existente = await _context.Filmes.FindAsync(filme.Id);

                if (existente == null)
                    throw new Exception("Filme não encontrado no banco.");

                _context.Entry(existente).CurrentValues.SetValues(filme);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                await _log.RegistrarErroAsync($"Erro ao atualizar filme ID={filme.Id}.", ex);
                Console.WriteLine($"[ERRO] UpdateAsync: {ex.Message}");
                throw;
            }
        }

        // EXCLUIR
        public async Task DeleteAsync(int id)
        {
            try
            {
                var filme = await _context.Filmes.FindAsync(id);

                if (filme == null)
                    return;

                _context.Filmes.Remove(filme);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                await _log.RegistrarErroAsync($"Erro ao deletar filme ID={id}.", ex);
                Console.WriteLine($"[ERRO] DeleteAsync: {ex.Message}");
                throw;
            }
        }
    }
}
