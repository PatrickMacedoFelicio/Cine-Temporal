using System.Collections.Generic;
using System.Threading.Tasks;
using Sistema_Cine.Models;

namespace SeuProjeto.Repositories
{
    public interface IFilmeRepository
    {
        Task<IEnumerable<Filme>> ListAsync();
        Task<Filme> GetByIdAsync(int id);
        Task CreateAsync(Filme filme);
        Task UpdateAsync(Filme filme);
        Task DeleteAsync(int id);
    }
}