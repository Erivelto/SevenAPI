using PlataformaSeven.API.Models;

namespace PlataformaSeven.API.Repositories
{
    public interface IUsuarioPermissaoRepository
    {
        Task<IEnumerable<UsuarioPermissaoDetalhe>> GetAllAsync();
        Task<UsuarioPermissao?> GetByIdAsync(int id);
        Task<IEnumerable<UsuarioPermissaoDetalhe>> GetByUsuarioIdAsync(int idUsuario);
        Task<UsuarioPermissao?> GetByUsuarioAndSubMenuAsync(int idUsuario, int codigoSubMenu);
        Task<UsuarioPermissao?> GetByUsuarioAndSubMenuUrlAsync(int idUsuario, string subMenuUrl);
        Task<int> CreateAsync(UsuarioPermissao permissao);
        Task<bool> SalvarPermissoesLoteAsync(int idUsuario, List<UsuarioPermissaoItem> permissoes);
        Task<bool> DeleteAsync(int id);
        Task<bool> DeleteByUsuarioIdAsync(int idUsuario);
    }
}
