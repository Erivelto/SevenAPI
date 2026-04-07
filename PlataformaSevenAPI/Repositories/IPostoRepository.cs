using PlataformaSeven.API.Models;

namespace PlataformaSeven.API.Repositories
{
    public interface IPostoRepository
    {
        Task<IEnumerable<Posto>> GetAllAsync();
        Task<Posto?> GetByIdAsync(int id);
        Task<int> CreateAsync(Posto posto);
        Task<bool> UpdateAsync(Posto posto);
        Task<bool> DeleteAsync(int id);
    }
}

