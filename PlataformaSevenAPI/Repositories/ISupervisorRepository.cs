using PlataformaSeven.API.Models;

namespace PlataformaSeven.API.Repositories
{
    public interface ISupervisorRepository
    {
        Task<IEnumerable<Supervisor>> GetAllAsync();
        Task<Supervisor?> GetByIdAsync(int id);
        Task<int> CreateAsync(Supervisor supervisor);
        Task<bool> UpdateAsync(Supervisor supervisor);
        Task<bool> DeleteAsync(int id);
    }
}

