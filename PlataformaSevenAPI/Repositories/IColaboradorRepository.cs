using PlataformaSeven.API.Models;

namespace PlataformaSeven.API.Repositories
{
    public interface IColaboradorRepository
    {
        Task<IEnumerable<Colaborador>> GetAllAsync();
        Task<Colaborador?> GetByIdAsync(int id);
        Task<int> CreateAsync(Colaborador colaborador);
        Task<bool> UpdateAsync(Colaborador colaborador);
        Task<bool> DeleteAsync(int id);
    }
}

