using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlataformaSeven.API.Models;
using PlataformaSeven.API.Services;

namespace PlataformaSeven.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ColaboradorController : ControllerBase
    {
        private readonly IColaboradorService _service;

        public ColaboradorController(IColaboradorService service)
        {
            _service = service;
        }

        /// <summary>
        /// Retorna todos os colaboradores
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Colaborador>>> GetAll()
        {
            var colaboradores = await _service.GetAllAsync();
            return Ok(colaboradores);
        }

        /// <summary>
        /// Retorna um colaborador por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Colaborador>> GetById(int id)
        {
            var colaborador = await _service.GetByIdAsync(id);
            
            if (colaborador == null)
                return NotFound(new { message = "Colaborador não encontrado" });
            
            return Ok(colaborador);
        }

        /// <summary>
        /// Cria um novo colaborador
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] Colaborador colaborador)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var id = await _service.CreateAsync(colaborador);
            return CreatedAtAction(nameof(GetById), new { id }, new { id, message = "Colaborador criado com sucesso" });
        }

        /// <summary>
        /// Atualiza um colaborador existente
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] Colaborador colaborador)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            if (id != colaborador.Id)
                return BadRequest(new { message = "ID do colaborador não corresponde" });
            
            var success = await _service.UpdateAsync(colaborador);
            
            if (!success)
                return NotFound(new { message = "Colaborador não encontrado" });
            
            return Ok(new { message = "Colaborador atualizado com sucesso" });
        }

        /// <summary>
        /// Deleta um colaborador (soft delete)
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);
            
            if (!success)
                return NotFound(new { message = "Colaborador não encontrado" });
            
            return Ok(new { message = "Colaborador excluído com sucesso" });
        }
    }
}

