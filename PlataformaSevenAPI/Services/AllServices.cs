using PlataformaSeven.API.Models;
using PlataformaSeven.API.Repositories;

namespace PlataformaSeven.API.Services
{
    // ==================== ColaboradorDetalhe ====================
    public interface IColaboradorDetalheService
    {
        Task<IEnumerable<ColaboradorDetalhe>> GetAllAsync();
        Task<ColaboradorDetalhe?> GetByIdAsync(int id);
        Task<IEnumerable<ColaboradorDetalhe>> GetByColaboradorIdAsync(int idColaborador);
        Task<IEnumerable<SelectItens>> GetSelectColaboradorDetalhe(int idColaborador);
        Task<int> CreateAsync(ColaboradorDetalhe detalhe);
        Task<bool> UpdateAsync(ColaboradorDetalhe detalhe);
        Task<bool> DeleteAsync(int id);
    }

    public class ColaboradorDetalheService : IColaboradorDetalheService
    {
        private readonly IColaboradorDetalheRepository _repository;

        public ColaboradorDetalheService(IColaboradorDetalheRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ColaboradorDetalhe>> GetAllAsync() => await _repository.GetAllAsync();
        public async Task<ColaboradorDetalhe?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);
        public async Task<IEnumerable<ColaboradorDetalhe>> GetByColaboradorIdAsync(int idColaborador) => await _repository.GetByColaboradorIdAsync(idColaborador);
        public async Task<IEnumerable<SelectItens>> GetSelectColaboradorDetalhe(int idColaborador) => await _repository.GetSelectColaboradorDetalhe(idColaborador);
        public async Task<int> CreateAsync(ColaboradorDetalhe detalhe) => await _repository.CreateAsync(detalhe);
        public async Task<bool> UpdateAsync(ColaboradorDetalhe detalhe) => await _repository.UpdateAsync(detalhe);
        public async Task<bool> DeleteAsync(int id) => await _repository.DeleteAsync(id);
    }

    // ==================== Funcao ====================
    public interface IFuncaoService
    {
        Task<IEnumerable<Funcao>> GetAllAsync();
        Task<Funcao?> GetByIdAsync(int id);
        Task<int> CreateAsync(Funcao funcao);
        Task<bool> UpdateAsync(Funcao funcao);
        Task<bool> DeleteAsync(int id);
    }

    public class FuncaoService : IFuncaoService
    {
        private readonly IFuncaoRepository _repository;

        public FuncaoService(IFuncaoRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Funcao>> GetAllAsync() => await _repository.GetAllAsync();
        public async Task<Funcao?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);
        public async Task<int> CreateAsync(Funcao funcao) => await _repository.CreateAsync(funcao);
        public async Task<bool> UpdateAsync(Funcao funcao) => await _repository.UpdateAsync(funcao);
        public async Task<bool> DeleteAsync(int id) => await _repository.DeleteAsync(id);
    }

    // ==================== Posto ====================
    public interface IPostoService
    {
        Task<IEnumerable<Posto>> GetAllAsync();
        Task<Posto?> GetByIdAsync(int id);
        Task<int> CreateAsync(Posto posto);
        Task<bool> UpdateAsync(Posto posto);
        Task<bool> DeleteAsync(int id);
    }

    public class PostoService : IPostoService
    {
        private readonly IPostoRepository _repository;

        public PostoService(IPostoRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Posto>> GetAllAsync() => await _repository.GetAllAsync();
        public async Task<Posto?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);
        public async Task<int> CreateAsync(Posto posto) => await _repository.CreateAsync(posto);
        public async Task<bool> UpdateAsync(Posto posto) => await _repository.UpdateAsync(posto);
        public async Task<bool> DeleteAsync(int id) => await _repository.DeleteAsync(id);
    }

    // ==================== Supervisor ====================
    public interface ISupervisorService
    {
        Task<IEnumerable<Supervisor>> GetAllAsync();
        Task<Supervisor?> GetByIdAsync(int id);
        Task<int> CreateAsync(Supervisor supervisor);
        Task<bool> UpdateAsync(Supervisor supervisor);
        Task<bool> DeleteAsync(int id);
    }

    public class SupervisorService : ISupervisorService
    {
        private readonly ISupervisorRepository _repository;

        public SupervisorService(ISupervisorRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Supervisor>> GetAllAsync() => await _repository.GetAllAsync();
        public async Task<Supervisor?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);
        public async Task<int> CreateAsync(Supervisor supervisor) => await _repository.CreateAsync(supervisor);
        public async Task<bool> UpdateAsync(Supervisor supervisor) => await _repository.UpdateAsync(supervisor);
        public async Task<bool> DeleteAsync(int id) => await _repository.DeleteAsync(id);
    }

    // ==================== Usuario ====================
    public interface IUsuarioService
    {
        Task<IEnumerable<Usuario>> GetAllAsync();
        Task<Usuario?> GetByIdAsync(int id);
        Task<Usuario?> GetByUsernameAsync(string username);
        Task<int> CreateAsync(Usuario usuario);
        Task<bool> UpdateAsync(Usuario usuario);
        Task<bool> DeleteAsync(int id);
    }

    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _repository;

        public UsuarioService(IUsuarioRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Usuario>> GetAllAsync() => await _repository.GetAllAsync();
        public async Task<Usuario?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);
        public async Task<Usuario?> GetByUsernameAsync(string username) => await _repository.GetByUsernameAsync(username);
        
        public async Task<int> CreateAsync(Usuario usuario)
        {
            usuario.DataCadastro = DateTime.Now;
            usuario.DataAtualizacao = DateTime.Now;
            return await _repository.CreateAsync(usuario);
        }
        
        public async Task<bool> UpdateAsync(Usuario usuario)
        {
            usuario.DataAtualizacao = DateTime.Now;
            return await _repository.UpdateAsync(usuario);
        }
        
        public async Task<bool> DeleteAsync(int id) => await _repository.DeleteAsync(id);
    }

    // ==================== Diaria ====================
    public interface IDiariaService
    {
        Task<IEnumerable<Diaria>> GetAllAsync();
        Task<Diaria?> GetByIdAsync(int id);
        Task<IEnumerable<Diaria>> GetByColaboradorDetalheIdAsync(int idColaboradorDetalhe);
        Task<IEnumerable<Diaria>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<int> CreateAsync(Diaria diaria);
        Task<bool> UpdateAsync(Diaria diaria);
        Task<bool> DeleteAsync(int id);
    }

    public class DiariaService : IDiariaService
    {
        private readonly IDiariaRepository _repository;

        public DiariaService(IDiariaRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Diaria>> GetAllAsync() => await _repository.GetAllAsync();
        public async Task<Diaria?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);
        public async Task<IEnumerable<Diaria>> GetByColaboradorDetalheIdAsync(int idColaboradorDetalhe) => await _repository.GetByColaboradorDetalheIdAsync(idColaboradorDetalhe);
        public async Task<IEnumerable<Diaria>> GetByDateRangeAsync(DateTime startDate, DateTime endDate) => await _repository.GetByDateRangeAsync(startDate, endDate);
        public async Task<int> CreateAsync(Diaria diaria) => await _repository.CreateAsync(diaria);
        public async Task<bool> UpdateAsync(Diaria diaria) => await _repository.UpdateAsync(diaria);
        public async Task<bool> DeleteAsync(int id) => await _repository.DeleteAsync(id);
    }

    // ==================== DiariaDisponivel ====================
    public interface IDiariaDisponivelService
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

    public class DiariaDisponivelService : IDiariaDisponivelService
    {
        private readonly IDiariaDisponivelRepository _repository;

        public DiariaDisponivelService(IDiariaDisponivelRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<DiariaDisponivel>> GetAllAsync() => await _repository.GetAllAsync();
        public async Task<DiariaDisponivel?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);
        public async Task<IEnumerable<DiariaDisponivel>> GetByFuncaoIdAsync(int idFuncao) => await _repository.GetByFuncaoIdAsync(idFuncao);
        public async Task<IEnumerable<DiariaDisponivel>> GetByPostoIdAsync(int idPosto) => await _repository.GetByPostoIdAsync(idPosto);
        public async Task<IEnumerable<DiariaDisponivel>> GetBySupervisorIdAsync(int idSupervisor) => await _repository.GetBySupervisorIdAsync(idSupervisor);
        public async Task<IEnumerable<DiariaDisponivel>> GetByPeriodoAsync(DateTime dataInicio, DateTime dataFim) => await _repository.GetByPeriodoAsync(dataInicio, dataFim);
        public async Task<IEnumerable<DiariaDisponivelResponse>> GetListaDisponivelAsync() => await _repository.GetListaDisponivelAsync();

        public async Task<int> CreateAsync(DiariaDisponivel diariaDisponivel)
        {
            diariaDisponivel.DataCadastro = DateTime.Now;
            diariaDisponivel.DataAlteracao = DateTime.Now;
            diariaDisponivel.Excluido = false;
            return await _repository.CreateAsync(diariaDisponivel);
        }

        public async Task<bool> UpdateAsync(DiariaDisponivel diariaDisponivel)
        {
            diariaDisponivel.DataAlteracao = DateTime.Now;
            return await _repository.UpdateAsync(diariaDisponivel);
        }

        public async Task<bool> DeleteAsync(int id) => await _repository.DeleteAsync(id);
    }

    // ==================== DiariaDesconto ====================
    public interface IDiariaDescontoService
    {
        Task<IEnumerable<DiariaDesconto>> GetAllAsync();
        Task<DiariaDesconto?> GetByIdAsync(int id);
        Task<IEnumerable<DiariaDesconto>> GetByColaboradorIdAsync(int idColaborador);
        Task<IEnumerable<DiariaDesconto>> GetByPeriodoAsync(int idColaborador, DateTime dataInicio, DateTime dataFim);
        Task<int> CreateAsync(DiariaDesconto diariaDesconto);
        Task<bool> UpdateAsync(DiariaDesconto diariaDesconto);
        Task<bool> DeleteAsync(int id);
    }

    public class DiariaDescontoService : IDiariaDescontoService
    {
        private readonly IDiariaDescontoRepository _repository;

        public DiariaDescontoService(IDiariaDescontoRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<DiariaDesconto>> GetAllAsync() => await _repository.GetAllAsync();
        public async Task<DiariaDesconto?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);
        public async Task<IEnumerable<DiariaDesconto>> GetByColaboradorIdAsync(int idColaborador) => await _repository.GetByColaboradorIdAsync(idColaborador);
        public async Task<IEnumerable<DiariaDesconto>> GetByPeriodoAsync(int idColaborador, DateTime dataInicio, DateTime dataFim) => await _repository.GetByPeriodoAsync(idColaborador, dataInicio, dataFim);
        public async Task<int> CreateAsync(DiariaDesconto diariaDesconto) => await _repository.CreateAsync(diariaDesconto);
        public async Task<bool> UpdateAsync(DiariaDesconto diariaDesconto) => await _repository.UpdateAsync(diariaDesconto);
        public async Task<bool> DeleteAsync(int id) => await _repository.DeleteAsync(id);
    }

    // ==================== Relatorio ====================
    public interface IRelatorioService
    {
        Task<IEnumerable<ListaDiariaColaboradorResponse>> ListaDiariaColaboradorAsync(DateTime inicial, DateTime final, int idColaboradorDetalhe);
        Task<IEnumerable<ListaDiariaRelatorio>> ListaDiariaRelatoriosAsync(DateTime inicial, DateTime final, int? colaborador, int? posto);
        Task<IEnumerable<DiariaConsolidado>> RelatorioConsolidadoAsync(DateTime inicial, DateTime final);
    }

    public class RelatorioService : IRelatorioService
    {
        private readonly IRelatorioRepository _repository;

        public RelatorioService(IRelatorioRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ListaDiariaColaboradorResponse>> ListaDiariaColaboradorAsync(DateTime inicial, DateTime final, int idColaboradorDetalhe)
            => await _repository.ListaDiariaColaboradorAsync(inicial, final, idColaboradorDetalhe);

        public async Task<IEnumerable<ListaDiariaRelatorio>> ListaDiariaRelatoriosAsync(DateTime inicial, DateTime final, int? colaborador, int? posto)
            => await _repository.ListaDiariaRelatoriosAsync(inicial, final, colaborador, posto);

        public async Task<IEnumerable<DiariaConsolidado>> RelatorioConsolidadoAsync(DateTime inicial, DateTime final)
            => await _repository.RelatorioConsolidadoAsync(inicial, final);
    }

    // ==================== Menu ====================
    public interface IMenuService
    {
        Task<IEnumerable<Models.Menu>> GetAllMenusAsync();
        Task<IEnumerable<SubMenu>> GetAllSubMenusAsync();
        Task<IEnumerable<SubMenu>> GetSubMenusByMenuAsync(int codigoMenu);
        Task<IEnumerable<MenuComSubMenus>> GetMenuAdminAsync();
        Task<IEnumerable<MenuComSubMenus>> GetMenuPorPermissaoAsync(int idUsuario);
    }

    public class MenuService : IMenuService
    {
        private readonly IMenuRepository _repository;

        public MenuService(IMenuRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Models.Menu>> GetAllMenusAsync() => await _repository.GetAllMenusAsync();
        public async Task<IEnumerable<SubMenu>> GetAllSubMenusAsync() => await _repository.GetAllSubMenusAsync();
        public async Task<IEnumerable<SubMenu>> GetSubMenusByMenuAsync(int codigoMenu) => await _repository.GetSubMenusByMenuAsync(codigoMenu);
        public async Task<IEnumerable<MenuComSubMenus>> GetMenuAdminAsync() => await _repository.GetMenuAdminAsync();
        public async Task<IEnumerable<MenuComSubMenus>> GetMenuPorPermissaoAsync(int idUsuario) => await _repository.GetMenuPorPermissaoAsync(idUsuario);
    }

    // ==================== UsuarioPermissao ====================
    public interface IUsuarioPermissaoService
    {
        Task<IEnumerable<UsuarioPermissaoDetalhe>> GetAllAsync();
        Task<UsuarioPermissao?> GetByIdAsync(int id);
        Task<IEnumerable<UsuarioPermissaoDetalhe>> GetByUsuarioIdAsync(int idUsuario);
        Task<UsuarioPermissao?> GetByUsuarioAndSubMenuAsync(int idUsuario, int codigoSubMenu);
        Task<int> CreateAsync(UsuarioPermissao permissao);
        Task<bool> SalvarPermissoesLoteAsync(UsuarioPermissaoRequest request);
        Task<bool> DeleteAsync(int id);
        Task<bool> DeleteByUsuarioIdAsync(int idUsuario);
    }

    public class UsuarioPermissaoService : IUsuarioPermissaoService
    {
        private readonly IUsuarioPermissaoRepository _repository;

        public UsuarioPermissaoService(IUsuarioPermissaoRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<UsuarioPermissaoDetalhe>> GetAllAsync() => await _repository.GetAllAsync();
        public async Task<UsuarioPermissao?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);
        public async Task<IEnumerable<UsuarioPermissaoDetalhe>> GetByUsuarioIdAsync(int idUsuario) => await _repository.GetByUsuarioIdAsync(idUsuario);
        public async Task<UsuarioPermissao?> GetByUsuarioAndSubMenuAsync(int idUsuario, int codigoSubMenu) => await _repository.GetByUsuarioAndSubMenuAsync(idUsuario, codigoSubMenu);

        public async Task<int> CreateAsync(UsuarioPermissao permissao)
        {
            permissao.DataCadastro = DateTime.Now;
            return await _repository.CreateAsync(permissao);
        }

        public async Task<bool> SalvarPermissoesLoteAsync(UsuarioPermissaoRequest request)
            => await _repository.SalvarPermissoesLoteAsync(request.IdUsuario, request.Permissoes);

        public async Task<bool> DeleteAsync(int id) => await _repository.DeleteAsync(id);
        public async Task<bool> DeleteByUsuarioIdAsync(int idUsuario) => await _repository.DeleteByUsuarioIdAsync(idUsuario);
    }
}

