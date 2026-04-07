using PlataformaSeven.API.Models;

namespace PlataformaSeven.API.Repositories
{
    public interface IFuncaoRepository
    {
        Task<IEnumerable<Funcao>> GetAllAsync();
        Task<Funcao?> GetByIdAsync(int id);
        Task<int> CreateAsync(Funcao funcao);
        Task<bool> UpdateAsync(Funcao funcao);
        Task<bool> DeleteAsync(int id);
    }
}

