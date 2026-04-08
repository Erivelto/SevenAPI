using System.Text.Json;
using PlataformaSeven.API.Models;
using PlataformaSeven.API.Repositories;

namespace PlataformaSeven.API.Services
{
    public interface IAprovacaoService
    {
        // ---- Config ----
        Task<IEnumerable<AprovacaoConfig>> GetAllConfigsAsync();
        Task<IEnumerable<AprovacaoConfig>> GetConfigsByEntidadeAsync(string entidade);
        Task<int> CreateConfigAsync(AprovacaoConfigRequest request);
        Task<bool> DeleteConfigAsync(int id);

        // ---- Stage ----

        /// <summary>
        /// Recebe um payload JSON e cria um registro na fila para cada aprovador configurado.
        /// Retorna os Ids gerados. Se a entidade nao tiver aprovacao configurada, retorna lista vazia.
        /// </summary>
        Task<IEnumerable<int>> SolicitarAsync(SolicitarAprovacaoRequest request, int idUsuarioSolicitante);

        Task<IEnumerable<AprovacaoStageDetalhe>> GetPendentesPorAprovadorAsync(int idUsuarioAprovador);
        Task<IEnumerable<AprovacaoStageDetalhe>> GetMinhasSolicitacoesAsync(int idUsuarioSolicitante);
        Task<IEnumerable<AprovacaoStageDetalhe>> GetAllStagesAsync(string? entidade, string? status);

        /// <summary>
        /// Aprova o stage: desserializa o payload, chama o repository correto e grava o registro definitivo.
        /// </summary>
        Task<(bool sucesso, string mensagem, int? idGerado)> AprovarAsync(int idStage, int idUsuarioAprovador, string? observacao);

        Task<(bool sucesso, string mensagem)> RejeitarAsync(int idStage, int idUsuarioAprovador, string observacao);
    }

    public class AprovacaoService : IAprovacaoService
    {
        private readonly IAprovacaoRepository _aprovacaoRepository;
        private readonly IPostoRepository     _postoRepository;
        private readonly IFuncaoRepository    _funcaoRepository;
        private readonly ISupervisorRepository _supervisorRepository;
        private readonly IColaboradorRepository _colaboradorRepository;

        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public AprovacaoService(
            IAprovacaoRepository    aprovacaoRepository,
            IPostoRepository        postoRepository,
            IFuncaoRepository       funcaoRepository,
            ISupervisorRepository   supervisorRepository,
            IColaboradorRepository  colaboradorRepository)
        {
            _aprovacaoRepository   = aprovacaoRepository;
            _postoRepository       = postoRepository;
            _funcaoRepository      = funcaoRepository;
            _supervisorRepository  = supervisorRepository;
            _colaboradorRepository = colaboradorRepository;
        }

        // ==================== Config ====================

        public async Task<IEnumerable<AprovacaoConfig>> GetAllConfigsAsync()
            => await _aprovacaoRepository.GetAllConfigsAsync();

        public async Task<IEnumerable<AprovacaoConfig>> GetConfigsByEntidadeAsync(string entidade)
            => await _aprovacaoRepository.GetConfigsByEntidadeAsync(entidade);

        public async Task<int> CreateConfigAsync(AprovacaoConfigRequest request)
        {
            var config = new AprovacaoConfig
            {
                Entidade           = request.Entidade,
                IdUsuarioAprovador = request.IdUsuarioAprovador,
                Ativo              = true,
                DataCadastro       = DateTime.Now
            };
            return await _aprovacaoRepository.CreateConfigAsync(config);
        }

        public async Task<bool> DeleteConfigAsync(int id)
            => await _aprovacaoRepository.DeleteConfigAsync(id);

        // ==================== Stage ====================

        public async Task<IEnumerable<int>> SolicitarAsync(SolicitarAprovacaoRequest request, int idUsuarioSolicitante)
        {
            var aprovadores = (await _aprovacaoRepository.GetAprovadoresAtivosAsync(request.Entidade)).ToList();

            if (aprovadores.Count == 0)
                return Enumerable.Empty<int>();

            var ids = new List<int>();

            foreach (var aprovador in aprovadores)
            {
                var stage = new AprovacaoStage
                {
                    Entidade              = request.Entidade,
                    PayloadJson           = request.PayloadJson,
                    IdUsuarioSolicitante  = idUsuarioSolicitante,
                    IdUsuarioAprovador    = aprovador.IdUsuarioAprovador,
                    Status                = AprovacaoStatus.Pendente,
                    DataSolicitacao       = DateTime.Now
                };
                var id = await _aprovacaoRepository.CreateStageAsync(stage);
                ids.Add(id);
            }

            return ids;
        }

        public async Task<IEnumerable<AprovacaoStageDetalhe>> GetPendentesPorAprovadorAsync(int idUsuarioAprovador)
            => await _aprovacaoRepository.GetPendentesPorAprovadorAsync(idUsuarioAprovador);

        public async Task<IEnumerable<AprovacaoStageDetalhe>> GetMinhasSolicitacoesAsync(int idUsuarioSolicitante)
            => await _aprovacaoRepository.GetPorSolicitanteAsync(idUsuarioSolicitante);

        public async Task<IEnumerable<AprovacaoStageDetalhe>> GetAllStagesAsync(string? entidade, string? status)
            => await _aprovacaoRepository.GetAllStagesAsync(entidade, status);

        public async Task<(bool sucesso, string mensagem, int? idGerado)> AprovarAsync(
            int idStage, int idUsuarioAprovador, string? observacao)
        {
            var stage = await _aprovacaoRepository.GetStageByIdAsync(idStage);

            if (stage == null)
                return (false, "Solicitacao nao encontrada.", null);

            if (stage.Status != AprovacaoStatus.Pendente)
                return (false, $"Solicitacao ja esta com status '{stage.Status}'.", null);

            if (stage.IdUsuarioAprovador != idUsuarioAprovador)
                return (false, "Voce nao e o aprovador desta solicitacao.", null);

            int idGerado;

            try
            {
                idGerado = await DespacharCriacaoAsync(stage.Entidade, stage.PayloadJson);
            }
            catch (Exception ex)
            {
                return (false, $"Erro ao criar registro: {ex.Message}", null);
            }

            await _aprovacaoRepository.AprovarAsync(idStage, idGerado, observacao);
            return (true, "Aprovado com sucesso.", idGerado);
        }

        public async Task<(bool sucesso, string mensagem)> RejeitarAsync(
            int idStage, int idUsuarioAprovador, string observacao)
        {
            var stage = await _aprovacaoRepository.GetStageByIdAsync(idStage);

            if (stage == null)
                return (false, "Solicitacao nao encontrada.");

            if (stage.Status != AprovacaoStatus.Pendente)
                return (false, $"Solicitacao ja esta com status '{stage.Status}'.");

            if (stage.IdUsuarioAprovador != idUsuarioAprovador)
                return (false, "Voce nao e o aprovador desta solicitacao.");

            await _aprovacaoRepository.RejeitarAsync(idStage, observacao);
            return (true, "Solicitacao rejeitada.");
        }

        // ==================== Despacho por entidade ====================

        /// <summary>
        /// Desserializa o payload e chama o repository correto conforme a entidade.
        /// Para adicionar uma nova entidade no fluxo basta incluir um novo case aqui.
        /// </summary>
        private async Task<int> DespacharCriacaoAsync(string entidade, string payloadJson)
        {
            return entidade.ToLower() switch
            {
                "posto" => await CriarPostoAsync(payloadJson),
                "funcao" => await CriarFuncaoAsync(payloadJson),
                "supervisor" => await CriarSupervisorAsync(payloadJson),
                "colaborador" => await CriarColaboradorAsync(payloadJson),
                _ => throw new NotSupportedException($"Entidade '{entidade}' nao possui handler de criacao configurado.")
            };
        }

        private async Task<int> CriarPostoAsync(string json)
        {
            var posto = JsonSerializer.Deserialize<Posto>(json, _jsonOptions)
                ?? throw new InvalidOperationException("Payload invalido para Posto.");
            return await _postoRepository.CreateAsync(posto);
        }

        private async Task<int> CriarFuncaoAsync(string json)
        {
            var funcao = JsonSerializer.Deserialize<Funcao>(json, _jsonOptions)
                ?? throw new InvalidOperationException("Payload invalido para Funcao.");
            return await _funcaoRepository.CreateAsync(funcao);
        }

        private async Task<int> CriarSupervisorAsync(string json)
        {
            var supervisor = JsonSerializer.Deserialize<Supervisor>(json, _jsonOptions)
                ?? throw new InvalidOperationException("Payload invalido para Supervisor.");
            return await _supervisorRepository.CreateAsync(supervisor);
        }

        private async Task<int> CriarColaboradorAsync(string json)
        {
            var colaborador = JsonSerializer.Deserialize<Colaborador>(json, _jsonOptions)
                ?? throw new InvalidOperationException("Payload invalido para Colaborador.");
            colaborador.DataCadastro  = DateTime.Now;
            colaborador.DataAlteracao = DateTime.Now;
            return await _colaboradorRepository.CreateAsync(colaborador);
        }
    }
}
