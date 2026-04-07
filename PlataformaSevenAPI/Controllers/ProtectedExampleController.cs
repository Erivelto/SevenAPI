using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace PlataformaSeven.API.Controllers
{
    /// <summary>
    /// Exemplo de controller protegido com autorização
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Requer autenticação para todos os endpoints
    public class ProtectedExampleController : ControllerBase
    {
        /// <summary>
        /// Endpoint acessível por qualquer usuário autenticado
        /// </summary>
        [HttpGet("authenticated")]
        public ActionResult GetAuthenticatedData()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            return Ok(new
            {
                message = "Você está autenticado!",
                userId = userId,
                username = username,
                role = role
            });
        }

        /// <summary>
        /// Endpoint acessível apenas por Administradores
        /// </summary>
        [HttpGet("admin-only")]
        [Authorize(Roles = "Administrador")]
        public ActionResult GetAdminData()
        {
            return Ok(new
            {
                message = "Você é um administrador!",
                data = "Dados sensíveis apenas para admins"
            });
        }

        /// <summary>
        /// Endpoint acessível por Administrador ou Supervisor
        /// </summary>
        [HttpGet("supervisor-or-admin")]
        [Authorize(Roles = "Administrador,Supervisor")]
        public ActionResult GetSupervisorData()
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            return Ok(new
            {
                message = $"Você é {role}!",
                data = "Dados para supervisores e admins"
            });
        }

        /// <summary>
        /// Endpoint público (não requer autenticação)
        /// </summary>
        [HttpGet("public")]
        [AllowAnonymous]
        public ActionResult GetPublicData()
        {
            return Ok(new
            {
                message = "Este endpoint é público",
                data = "Qualquer um pode acessar"
            });
        }
    }
}

