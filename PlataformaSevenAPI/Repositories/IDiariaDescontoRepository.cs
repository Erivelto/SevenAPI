using PlataformaSeven.API.Models;

namespace PlataformaSeven.API.Repositories
{
    public interface IDiariaDescontoRepository
    {
        Task<IEnumerable<DiariaDesconto>> GetAllAsync();
        Task<DiariaDesconto?> GetByIdAsync(int id);
        Task<IEnumerable<DiariaDesconto>> GetByColaboradorIdAsync(int idColaborador);
        Task<IEnumerable<DiariaDesconto>> GetByPeriodoAsync(int idColaborador, DateTime dataInicio, DateTime dataFim);
        Task<int> CreateAsync(DiariaDesconto diariaDesconto);
        Task<bool> UpdateAsync(DiariaDesconto diariaDesconto);
        Task<bool> DeleteAsync(int id);
    }
}
