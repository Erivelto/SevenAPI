using PlataformaSeven.API.Models;

namespace PlataformaSeven.API.Repositories
{
    public interface IRelatorioRepository
    {
        Task<IEnumerable<ListaDiariaColaboradorResponse>> ListaDiariaColaboradorAsync(DateTime inicial, DateTime final, int idColaboradorDetalhe);
        Task<IEnumerable<ListaDiariaRelatorio>> ListaDiariaRelatoriosAsync(DateTime inicial, DateTime final, int? colaborador, int? posto);
        Task<IEnumerable<DiariaConsolidado>> RelatorioConsolidadoAsync(DateTime inicial, DateTime final);
    }
}
