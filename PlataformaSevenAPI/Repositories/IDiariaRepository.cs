using PlataformaSeven.API.Models;

namespace PlataformaSeven.API.Repositories
{
    public interface IDiariaRepository
    {
        Task<IEnumerable<Diaria>> GetAllAsync();
        Task<Diaria?> GetByIdAsync(int id);
        Task<IEnumerable<Diaria>> GetByColaboradorDetalheIdAsync(int idColaboradorDetalhe);
        Task<IEnumerable<Diaria>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<int> CreateAsync(Diaria diaria);
        Task<bool> UpdateAsync(Diaria diaria);
        Task<bool> DeleteAsync(int id);
    }
}

