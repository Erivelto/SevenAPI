using PlataformaSeven.API.Models;

namespace PlataformaSeven.API.Repositories
{
    public interface IDiariaDisponivelRepository
    {
        Task<IEnumerable<DiariaDisponivel>> GetAllAsync();
        Task<DiariaDisponivel?> GetByIdAsync(int id);
        Task<IEnumerable<DiariaDisponivel>> GetByFuncaoIdAsync(int idFuncao);
        Task<IEnumerable<DiariaDisponivel>> GetByPostoIdAsync(int idPosto);
        Task<IEnumerable<DiariaDisponivel>> GetBySupervisorIdAsync(int idSupervisor);
        Task<IEnumerable<DiariaDisponivel>> GetByPeriodoAsync(DateTime dataInicio, DateTime dataFim);
        Task<IEnumerable<DiariaDisponivelResponse>> GetListaDisponivelAsync();
        Task<int> CreateAsync(DiariaDisponivel diariaDisponivel);
        Task<bool> UpdateAsync(DiariaDisponivel diariaDisponivel);
        Task<bool> DeleteAsync(int id);
    }
}
