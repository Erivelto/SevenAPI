using PlataformaSeven.API.Models;

namespace PlataformaSeven.API.Repositories
{
    public interface IColaboradorDetalheRepository
    {
        Task<IEnumerable<ColaboradorDetalhe>> GetAllAsync();
        Task<ColaboradorDetalhe?> GetByIdAsync(int id);
        Task<IEnumerable<ColaboradorDetalhe>> GetByColaboradorIdAsync(int idColaborador);
        Task<IEnumerable<SelectItens>> GetSelectColaboradorDetalhe(int idColaborador);
        Task<int> CreateAsync(ColaboradorDetalhe detalhe);
        Task<bool> UpdateAsync(ColaboradorDetalhe detalhe);
        Task<bool> DeleteAsync(int id);
    }
}

