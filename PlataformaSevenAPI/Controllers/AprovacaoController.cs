using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlataformaSeven.API.Models;
using PlataformaSeven.API.Services;

namespace PlataformaSeven.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AprovacaoController : ControllerBase
    {
        private readonly IAprovacaoService _service;

        public AprovacaoController(IAprovacaoService service)
        {
            _service = service;
        }

        // ==================== Config (master) ====================

        /// <summary>
        /// Lista todas as configuracoes de aprovacao.
        /// </summary>
        [HttpGet("config")]
        public async Task<ActionResult<IEnumerable<AprovacaoConfig>>> GetConfigs()
        {
            var configs = await _service.GetAllConfigsAsync();
            return Ok(configs);
        }

        /// <summary>
        /// Lista aprovadores de uma entidade especifica.
        /// </summary>
        [HttpGet("config/{entidade}")]
        public async Task<ActionResult<IEnumerable<AprovacaoConfig>>> GetConfigByEntidade(string entidade)
        {
            var configs = await _service.GetConfigsByEntidadeAsync(entidade);
            return Ok(configs);
        }

        /// <summary>
        /// Adiciona um aprovador para uma entidade. Uso exclusivo do master.
        /// </summary>
        [HttpPost("config")]
        public async Task<ActionResult> CreateConfig([FromBody] AprovacaoConfigRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var id = await _service.CreateConfigAsync(request);
            return Ok(new { id, message = "Configuracao de aprovacao criada com sucesso." });
        }

        /// <summary>
        /// Remove (desativa) um aprovador de uma entidade. Uso exclusivo do master.
        /// </summary>
        [HttpDelete("config/{id}")]
        public async Task<ActionResult> DeleteConfig(int id)
        {
            var success = await _service.DeleteConfigAsync(id);
            if (!success) return NotFound(new { message = "Configuracao nao encontrada." });
            return Ok(new { message = "Configuracao removida com sucesso." });
        }

        // ==================== Solicitacao (usuario comum) ====================

        /// <summary>
        /// Envia uma solicitacao de criacao para aprovacao.
        /// O payload deve ser o JSON do objeto a ser criado (ex: {"Nome":"Posto Central"}).
        /// Retorna 202 com os ids de stage gerados (um por aprovador configurado).
        /// Retorna 422 se a entidade nao tiver aprovacao configurada.
        /// </summary>
        [HttpPost("solicitar")]
        public async Task<ActionResult> Solicitar([FromBody] SolicitarAprovacaoRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
                return Unauthorized(new { message = "Usuario nao identificado." });

            var ids = (await _service.SolicitarAsync(request, userId)).ToList();

            if (ids.Count == 0)
                return UnprocessableEntity(new
                {
                    message = $"A entidade '{request.Entidade}' nao possui aprovacao configurada. Use o endpoint direto de criacao."
                });

            return Accepted(new
            {
                message = $"Solicitacao enviada para {ids.Count} aprovador(es). Aguarde a aprovacao.",
                ids
            });
        }

        /// <summary>
        /// Lista as proprias solicitacoes do usuario autenticado com o status atual.
        /// </summary>
        [HttpGet("minhas-solicitacoes")]
        public async Task<ActionResult<IEnumerable<AprovacaoStageDetalhe>>> GetMinhasSolicitacoes()
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
                return Unauthorized(new { message = "Usuario nao identificado." });

            var lista = await _service.GetMinhasSolicitacoesAsync(userId);
            return Ok(lista);
        }

        // ==================== Fila do aprovador ====================

        /// <summary>
        /// Lista as solicitacoes pendentes para o usuario autenticado aprovar.
        /// </summary>
        [HttpGet("pendentes")]
        public async Task<ActionResult<IEnumerable<AprovacaoStageDetalhe>>> GetPendentes()
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
                return Unauthorized(new { message = "Usuario nao identificado." });

            var lista = await _service.GetPendentesPorAprovadorAsync(userId);
            return Ok(lista);
        }

        /// <summary>
        /// Aprova uma solicitacao: cria o registro definitivo na tabela de destino.
        /// </summary>
        [HttpPost("{id}/aprovar")]
        public async Task<ActionResult> Aprovar(int id, [FromBody] AprovarRequest request)
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
                return Unauthorized(new { message = "Usuario nao identificado." });

            var (sucesso, mensagem, idGerado) = await _service.AprovarAsync(id, userId, request.Observacao);

            if (!sucesso) return BadRequest(new { message = mensagem });

            return Ok(new { message = mensagem, idRegistroGerado = idGerado });
        }

        /// <summary>
        /// Rejeita uma solicitacao informando o motivo.
        /// </summary>
        [HttpPost("{id}/rejeitar")]
        public async Task<ActionResult> Rejeitar(int id, [FromBody] RejeitarRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
                return Unauthorized(new { message = "Usuario nao identificado." });

            var (sucesso, mensagem) = await _service.RejeitarAsync(id, userId, request.Observacao);

            if (!sucesso) return BadRequest(new { message = mensagem });

            return Ok(new { message = mensagem });
        }

        // ==================== Visao geral (master) ====================

        /// <summary>
        /// Lista todos os stages com filtros opcionais de entidade e status.
        /// Uso do usuario master para auditoria.
        /// </summary>
        [HttpGet("stages")]
        public async Task<ActionResult<IEnumerable<AprovacaoStageDetalhe>>> GetAllStages(
            [FromQuery] string? entidade,
            [FromQuery] string? status)
        {
            var lista = await _service.GetAllStagesAsync(entidade, status);
            return Ok(lista);
        }
    }
}
