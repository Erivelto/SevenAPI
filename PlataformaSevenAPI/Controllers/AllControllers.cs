using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlataformaSeven.API.Models;
using PlataformaSeven.API.Services;

namespace PlataformaSeven.API.Controllers
{
    // ==================== ColaboradorDetalhe Controller ====================
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ColaboradorDetalheController : ControllerBase
    {
        private readonly IColaboradorDetalheService _service;

        public ColaboradorDetalheController(IColaboradorDetalheService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ColaboradorDetalhe>>> GetAll()
        {
            var detalhes = await _service.GetAllAsync();
            return Ok(detalhes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ColaboradorDetalhe>> GetById(int id)
        {
            var detalhe = await _service.GetByIdAsync(id);
            if (detalhe == null) return NotFound();
            return Ok(detalhe);
        }

        [HttpGet("colaborador/{idColaborador}")]
        public async Task<ActionResult<IEnumerable<ColaboradorDetalhe>>> GetByColaboradorId(int idColaborador)
        {
            var detalhes = await _service.GetByColaboradorIdAsync(idColaborador);
            return Ok(detalhes);
        }

        [HttpGet("select/{idColaborador}")]
        public async Task<ActionResult<IEnumerable<SelectItens>>> GetSelectColaboradorDetalhe(int idColaborador)
        {
            var itens = await _service.GetSelectColaboradorDetalhe(idColaborador);
            return Ok(itens);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] ColaboradorDetalhe detalhe)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var id = await _service.CreateAsync(detalhe);
            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] ColaboradorDetalhe detalhe)
        {
            if (id != detalhe.Id) return BadRequest();
            var success = await _service.UpdateAsync(detalhe);
            if (!success) return NotFound();
            return Ok(new { message = "Atualizado com sucesso" });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);
            if (!success) return NotFound();
            return Ok(new { message = "Excluído com sucesso" });
        }
    }

    // ==================== Funcao Controller ====================
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FuncaoController : ControllerBase
    {
        private readonly IFuncaoService _service;

        public FuncaoController(IFuncaoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Funcao>>> GetAll()
        {
            var funcoes = await _service.GetAllAsync();
            return Ok(funcoes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Funcao>> GetById(int id)
        {
            var funcao = await _service.GetByIdAsync(id);
            if (funcao == null) return NotFound();
            return Ok(funcao);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] Funcao funcao)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var id = await _service.CreateAsync(funcao);
            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] Funcao funcao)
        {
            if (id != funcao.Id) return BadRequest();
            var success = await _service.UpdateAsync(funcao);
            if (!success) return NotFound();
            return Ok(new { message = "Atualizado com sucesso" });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);
            if (!success) return NotFound();
            return Ok(new { message = "Excluído com sucesso" });
        }
    }

    // ==================== Posto Controller ====================
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PostoController : ControllerBase
    {
        private readonly IPostoService _service;

        public PostoController(IPostoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Posto>>> GetAll()
        {
            var postos = await _service.GetAllAsync();
            return Ok(postos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Posto>> GetById(int id)
        {
            var posto = await _service.GetByIdAsync(id);
            if (posto == null) return NotFound();
            return Ok(posto);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] Posto posto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var id = await _service.CreateAsync(posto);
            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] Posto posto)
        {
            if (id != posto.Id) return BadRequest();
            var success = await _service.UpdateAsync(posto);
            if (!success) return NotFound();
            return Ok(new { message = "Atualizado com sucesso" });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);
            if (!success) return NotFound();
            return Ok(new { message = "Excluído com sucesso" });
        }
    }

    // ==================== Supervisor Controller ====================
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SupervisorController : ControllerBase
    {
        private readonly ISupervisorService _service;

        public SupervisorController(ISupervisorService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Supervisor>>> GetAll()
        {
            var supervisores = await _service.GetAllAsync();
            return Ok(supervisores);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Supervisor>> GetById(int id)
        {
            var supervisor = await _service.GetByIdAsync(id);
            if (supervisor == null) return NotFound();
            return Ok(supervisor);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] Supervisor supervisor)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var id = await _service.CreateAsync(supervisor);
            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] Supervisor supervisor)
        {
            if (id != supervisor.Id) return BadRequest();
            var success = await _service.UpdateAsync(supervisor);
            if (!success) return NotFound();
            return Ok(new { message = "Atualizado com sucesso" });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);
            if (!success) return NotFound();
            return Ok(new { message = "Excluído com sucesso" });
        }
    }

    // ==================== Usuario Controller ====================
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _service;

        public UsuarioController(IUsuarioService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetAll()
        {
            var usuarios = await _service.GetAllAsync();
            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetById(int id)
        {
            var usuario = await _service.GetByIdAsync(id);
            if (usuario == null) return NotFound();
            return Ok(usuario);
        }

        [HttpGet("username/{username}")]
        public async Task<ActionResult<Usuario>> GetByUsername(string username)
        {
            var usuario = await _service.GetByUsernameAsync(username);
            if (usuario == null) return NotFound();
            return Ok(usuario);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] Usuario usuario)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var id = await _service.CreateAsync(usuario);
            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] Usuario usuario)
        {
            if (id != usuario.Id) return BadRequest();
            var success = await _service.UpdateAsync(usuario);
            if (!success) return NotFound();
            return Ok(new { message = "Atualizado com sucesso" });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);
            if (!success) return NotFound();
            return Ok(new { message = "Excluído com sucesso" });
        }
    }

    // ==================== Diaria Controller ====================
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DiariaController : ControllerBase
    {
        private readonly IDiariaService _service;

        public DiariaController(IDiariaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Diaria>>> GetAll()
        {
            var diarias = await _service.GetAllAsync();
            return Ok(diarias);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Diaria>> GetById(int id)
        {
            var diaria = await _service.GetByIdAsync(id);
            if (diaria == null) return NotFound();
            return Ok(diaria);
        }

        [HttpGet("colaborador-detalhe/{idColaboradorDetalhe}")]
        public async Task<ActionResult<IEnumerable<Diaria>>> GetByColaboradorDetalheId(int idColaboradorDetalhe)
        {
            var diarias = await _service.GetByColaboradorDetalheIdAsync(idColaboradorDetalhe);
            return Ok(diarias);
        }

        [HttpGet("periodo")]
        public async Task<ActionResult<IEnumerable<Diaria>>> GetByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var diarias = await _service.GetByDateRangeAsync(startDate, endDate);
            return Ok(diarias);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] Diaria diaria)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var id = await _service.CreateAsync(diaria);
            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] Diaria diaria)
        {
            if (id != diaria.Id) return BadRequest();
            var success = await _service.UpdateAsync(diaria);
            if (!success) return NotFound();
            return Ok(new { message = "Atualizado com sucesso" });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);
            if (!success) return NotFound();
            return Ok(new { message = "Excluído com sucesso" });
        }
    }

    // ==================== DiariaDisponivel Controller ====================
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DiariaDisponivelController : ControllerBase
    {
        private readonly IDiariaDisponivelService _service;

        public DiariaDisponivelController(IDiariaDisponivelService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DiariaDisponivel>>> GetAll()
        {
            var itens = await _service.GetAllAsync();
            return Ok(itens);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DiariaDisponivel>> GetById(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpGet("periodo")]
        public async Task<ActionResult<IEnumerable<DiariaDisponivel>>> GetByPeriodo([FromQuery] DateTime dataInicio, [FromQuery] DateTime dataFim)
        {
            if (dataInicio > dataFim)
                return BadRequest(new { message = "A data inicial não pode ser maior que a data final" });

            var itens = await _service.GetByPeriodoAsync(dataInicio, dataFim);
            return Ok(itens);
        }

        [HttpGet("lista")]
        public async Task<ActionResult<IEnumerable<DiariaDisponivelResponse>>> GetListaDisponivel()
        {
            var itens = await _service.GetListaDisponivelAsync();
            return Ok(itens);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] DiariaDisponivel diariaDisponivel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var id = await _service.CreateAsync(diariaDisponivel);
            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] DiariaDisponivel diariaDisponivel)
        {
            if (id != diariaDisponivel.Id) return BadRequest();
            var success = await _service.UpdateAsync(diariaDisponivel);
            if (!success) return NotFound();
            return Ok(new { message = "Atualizado com sucesso" });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);
            if (!success) return NotFound();
            return Ok(new { message = "Excluído com sucesso" });
        }
    }

    // ==================== DiariaDesconto Controller ====================
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DiariaDescontoController : ControllerBase
    {
        private readonly IDiariaDescontoService _service;

        public DiariaDescontoController(IDiariaDescontoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DiariaDesconto>>> GetAll()
        {
            var itens = await _service.GetAllAsync();
            return Ok(itens);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DiariaDesconto>> GetById(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpGet("colaborador/{idColaborador}")]
        public async Task<ActionResult<IEnumerable<DiariaDesconto>>> GetByColaboradorId(int idColaborador)
        {
            var itens = await _service.GetByColaboradorIdAsync(idColaborador);
            return Ok(itens);
        }

        [HttpGet("periodo")]
        public async Task<ActionResult<IEnumerable<DiariaDesconto>>> GetByPeriodo([FromQuery] int idColaborador, [FromQuery] DateTime dataInicio, [FromQuery] DateTime dataFim)
        {
            if (dataInicio > dataFim)
                return BadRequest(new { message = "A data inicial não pode ser maior que a data final" });

            var itens = await _service.GetByPeriodoAsync(idColaborador, dataInicio, dataFim);
            return Ok(itens);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] DiariaDesconto diariaDesconto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var id = await _service.CreateAsync(diariaDesconto);
            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] DiariaDesconto diariaDesconto)
        {
            if (id != diariaDesconto.Id) return BadRequest();
            var success = await _service.UpdateAsync(diariaDesconto);
            if (!success) return NotFound();
            return Ok(new { message = "Atualizado com sucesso" });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);
            if (!success) return NotFound();
            return Ok(new { message = "Excluído com sucesso" });
        }
    }

    // ==================== Relatorio Controller ====================
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RelatorioController : ControllerBase
    {
        private readonly IRelatorioService _service;

        public RelatorioController(IRelatorioService service)
        {
            _service = service;
        }

        /// <summary>
        /// Lista diárias de um colaborador detalhe por período
        /// </summary>
        [HttpGet("lista-diaria-colaborador")]
        public async Task<ActionResult<IEnumerable<ListaDiariaColaboradorResponse>>> ListaDiariaColaborador(
            [FromQuery] DateTime inicial,
            [FromQuery] DateTime final,
            [FromQuery] int idColaboradorDetalhe)
        {
            if (inicial > final)
                return BadRequest(new { message = "A data inicial não pode ser maior que a data final" });

            var result = await _service.ListaDiariaColaboradorAsync(inicial, final, idColaboradorDetalhe);
            return Ok(result);
        }

        /// <summary>
        /// Relatório de diárias agrupado por colaborador, com filtros opcionais de colaborador e posto
        /// </summary>
        [HttpGet("lista-diaria-relatorio")]
        public async Task<ActionResult<IEnumerable<ListaDiariaRelatorio>>> ListaDiariaRelatorio(
            [FromQuery] DateTime inicial,
            [FromQuery] DateTime final,
            [FromQuery] int? colaborador = null,
            [FromQuery] int? posto = null)
        {
            if (inicial > final)
                return BadRequest(new { message = "A data inicial não pode ser maior que a data final" });

            var result = await _service.ListaDiariaRelatoriosAsync(inicial, final, colaborador, posto);
            return Ok(result);
        }

        /// <summary>
        /// Relatório consolidado de diárias com totais e adiantamentos por colaborador
        /// </summary>
        [HttpGet("consolidado")]
        public async Task<ActionResult<IEnumerable<DiariaConsolidado>>> RelatorioConsolidado(
            [FromQuery] DateTime inicial,
            [FromQuery] DateTime final)
        {
            if (inicial > final)
                return BadRequest(new { message = "A data inicial não pode ser maior que a data final" });

            var result = await _service.RelatorioConsolidadoAsync(inicial, final);
            return Ok(result);
        }
    }

    // ==================== UsuarioPermissao Controller ====================
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioPermissaoController : ControllerBase
    {
        private readonly IUsuarioPermissaoService _service;

        public UsuarioPermissaoController(IUsuarioPermissaoService service)
        {
            _service = service;
        }

        /// <summary>
        /// Lista todas as permissoes com detalhes de Menu e SubMenu.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioPermissaoDetalhe>>> GetAll()
        {
            var permissoes = await _service.GetAllAsync();
            return Ok(permissoes);
        }

        /// <summary>
        /// Retorna uma permissao pelo Id.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioPermissao>> GetById(int id)
        {
            var permissao = await _service.GetByIdAsync(id);
            if (permissao == null) return NotFound();
            return Ok(permissao);
        }

        /// <summary>
        /// Lista todas as permissoes de um usuario com detalhes de Menu e SubMenu.
        /// </summary>
        [HttpGet("usuario/{idUsuario}")]
        public async Task<ActionResult<IEnumerable<UsuarioPermissaoDetalhe>>> GetByUsuarioId(int idUsuario)
        {
            var permissoes = await _service.GetByUsuarioIdAsync(idUsuario);
            return Ok(permissoes);
        }

        /// <summary>
        /// Salva as permissoes de um usuario em lote.
        /// Substitui todas as permissoes existentes pelos itens informados (com flag ApenasLeitura por SubMenu).
        /// </summary>
        [HttpPost("salvar-lote")]
        public async Task<ActionResult> SalvarLote([FromBody] UsuarioPermissaoRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var success = await _service.SalvarPermissoesLoteAsync(request);
            if (!success) return BadRequest(new { message = "Erro ao salvar permissoes" });
            return Ok(new { message = "Permissoes salvas com sucesso" });
        }

        /// <summary>
        /// Adiciona uma unica permissao de SubMenu para um usuario.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] UsuarioPermissao permissao)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var id = await _service.CreateAsync(permissao);
            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        /// <summary>
        /// Remove uma permissao pelo Id.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);
            if (!success) return NotFound();
            return Ok(new { message = "Permissao removida com sucesso" });
        }

        /// <summary>
        /// Remove todas as permissoes de um usuario.
        /// </summary>
        [HttpDelete("usuario/{idUsuario}")]
        public async Task<ActionResult> DeleteByUsuarioId(int idUsuario)
        {
            var success = await _service.DeleteByUsuarioIdAsync(idUsuario);
            if (!success) return NotFound();
            return Ok(new { message = "Permissoes do usuario removidas com sucesso" });
        }
    }

    // ==================== Menu Controller ====================
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _service;

        public MenuController(IMenuService service)
        {
            _service = service;
        }

        /// <summary>
        /// Retorna o menu do usuario autenticado.
        /// Admin recebe todos os menus e submenus.
        /// Demais usuarios recebem apenas os submenus liberados em UsuarioPermissao.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MenuComSubMenus>>> GetMenus()
        {
            var tipo = User.FindFirst("Tipo")?.Value;

            if (string.Equals(tipo, "A", StringComparison.OrdinalIgnoreCase))
            {
                var menuAdmin = await _service.GetMenuAdminAsync();
                return Ok(menuAdmin);
            }

            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
                return Unauthorized(new { message = "Usuario nao identificado" });

            var menus = await _service.GetMenuPorPermissaoAsync(userId);
            return Ok(menus);
        }

        /// <summary>
        /// Retorna todos os menus ativos sem submenus (util para telas de cadastro de permissao).
        /// </summary>
        [HttpGet("menus")]
        public async Task<ActionResult<IEnumerable<Models.Menu>>> GetAllMenus()
        {
            var menus = await _service.GetAllMenusAsync();
            return Ok(menus);
        }

        /// <summary>
        /// Retorna todos os submenus ativos de um menu especifico.
        /// </summary>
        [HttpGet("{codigoMenu}/submenus")]
        public async Task<ActionResult<IEnumerable<SubMenu>>> GetSubMenusByMenu(int codigoMenu)
        {
            var subMenus = await _service.GetSubMenusByMenuAsync(codigoMenu);
            return Ok(subMenus);
        }

        /// <summary>
        /// Retorna todos os submenus ativos (util para montar tela de selecao de permissoes).
        /// </summary>
        [HttpGet("submenus")]
        public async Task<ActionResult<IEnumerable<SubMenu>>> GetAllSubMenus()
        {
            var subMenus = await _service.GetAllSubMenusAsync();
            return Ok(subMenus);
        }
    }
}


