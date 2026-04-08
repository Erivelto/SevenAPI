using PlataformaSeven.API.Models;

namespace PlataformaSeven.API.Repositories
{
    public interface IAprovacaoRepository
    {
        // ---- Config ----
        Task<IEnumerable<AprovacaoConfig>> GetAllConfigsAsync();
        Task<IEnumerable<AprovacaoConfig>> GetConfigsByEntidadeAsync(string entidade);
        Task<AprovacaoConfig?> GetConfigByIdAsync(int id);
        Task<int> CreateConfigAsync(AprovacaoConfig config);
        Task<bool> DeleteConfigAsync(int id);

        // ---- Stage ----
        Task<int> CreateStageAsync(AprovacaoStage stage);
        Task<AprovacaoStage?> GetStageByIdAsync(int id);

        /// <summary>Pendentes que o aprovador informado precisa avaliar.</summary>
        Task<IEnumerable<AprovacaoStageDetalhe>> GetPendentesPorAprovadorAsync(int idUsuarioAprovador);

        /// <summary>Todas as solicitacoes feitas por um usuario (para o proprio acompanhar).</summary>
        Task<IEnumerable<AprovacaoStageDetalhe>> GetPorSolicitanteAsync(int idUsuarioSolicitante);

        /// <summary>Lista geral com filtros opcionais (uso do master).</summary>
        Task<IEnumerable<AprovacaoStageDetalhe>> GetAllStagesAsync(string? entidade, string? status);

        Task<bool> AprovarAsync(int id, int idRegistroGerado, string? observacao);
        Task<bool> RejeitarAsync(int id, string observacao);

        /// <summary>Verifica se a entidade tem aprovacao configurada e retorna o(s) aprovador(es).</summary>
        Task<IEnumerable<AprovacaoConfig>> GetAprovadoresAtivosAsync(string entidade);
    }
}
