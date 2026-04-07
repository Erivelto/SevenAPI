using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlataformaSeven.API.Models;
using PlataformaSeven.API.Services;
using System.Security.Claims;

namespace PlataformaSeven.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AutenticacaoController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IJwtService _jwtService;
        private readonly IMenuService _menuService;

        public AutenticacaoController(IUsuarioService usuarioService, IJwtService jwtService, IMenuService menuService)
        {
            _usuarioService = usuarioService;
            _jwtService = jwtService;
            _menuService = menuService;
        }

        /// <summary>
        /// Endpoint de login com autenticação JWT
        /// </summary>
        /// <param name="request">Credenciais de login (user e password)</param>
        /// <returns>Token JWT e informações do usuário</returns>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new LoginResponse
                {
                    Success = false,
                    Message = "Dados inválidos"
                });
            }

            // Buscar usuário no banco
            var usuarios = await _usuarioService.GetAllAsync();
            var usuario = usuarios.FirstOrDefault(x => 
                x.User == request.User && 
                x.Password == request.Password);

            if (usuario == null)
            {
                return Unauthorized(new LoginResponse
                {
                    Success = false,
                    Message = "Usuário ou senha inválidos"
                });
            }

            // Gerar tokens
            var token = _jwtService.GenerateToken(usuario);
            var refreshToken = _jwtService.GenerateRefreshToken();
            var expiresAt = DateTime.UtcNow.AddMinutes(480); // 8 horas

            // Carregar menu conforme o perfil do usuario
            var menu = string.Equals(usuario.Tipo, "A", StringComparison.OrdinalIgnoreCase)
                ? await _menuService.GetMenuAdminAsync()
                : await _menuService.GetMenuPorPermissaoAsync(usuario.Id);

            return Ok(new LoginResponse
            {
                Success = true,
                Message = "Login realizado com sucesso",
                Token = token,
                RefreshToken = refreshToken,
                ExpiresAt = expiresAt,
                User = new UserInfo
                {
                    Id = usuario.Id,
                    User = usuario.User,
                    Tipo = usuario.Tipo,
                    DataCadastro = usuario.DataCadastro
                },
                Menu = menu
            });
        }

        /// <summary>
        /// Registra um novo usuário
        /// </summary>
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<LoginResponse>> Register([FromBody] Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new LoginResponse
                {
                    Success = false,
                    Message = "Dados inválidos"
                });
            }

            // Verificar se usuário já existe
            var usuarioExistente = await _usuarioService.GetByUsernameAsync(usuario.User);
            
            if (usuarioExistente != null)
            {
                return BadRequest(new LoginResponse
                {
                    Success = false,
                    Message = "Usuário já existe"
                });
            }

            // Criar usuário
            usuario.DataCadastro = DateTime.Now;
            usuario.DataAtualizacao = DateTime.Now;
            
            var id = await _usuarioService.CreateAsync(usuario);
            usuario.Id = id;

            // Gerar tokens
            var token = _jwtService.GenerateToken(usuario);
            var refreshToken = _jwtService.GenerateRefreshToken();
            var expiresAt = DateTime.UtcNow.AddMinutes(480);

            return CreatedAtAction(nameof(GetCurrentUser), new { id }, new LoginResponse
            {
                Success = true,
                Message = "Usuário registrado com sucesso",
                Token = token,
                RefreshToken = refreshToken,
                ExpiresAt = expiresAt,
                User = new UserInfo
                {
                    Id = usuario.Id,
                    User = usuario.User,
                    Tipo = usuario.Tipo,
                    DataCadastro = usuario.DataCadastro
                }
            });
        }

        /// <summary>
        /// Retorna informações do usuário autenticado
        /// </summary>
        [HttpGet("me")]
        [Authorize]
        public async Task<ActionResult<UserInfo>> GetCurrentUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int id))
            {
                return BadRequest(new { message = "ID de usuário inválido" });
            }

            var usuario = await _usuarioService.GetByIdAsync(id);

            if (usuario == null)
            {
                return NotFound(new { message = "Usuário não encontrado" });
            }

            return Ok(new UserInfo
            {
                Id = usuario.Id,
                User = usuario.User,
                Tipo = usuario.Tipo,
                DataCadastro = usuario.DataCadastro
            });
        }

        /// <summary>
        /// Altera a senha do usuário autenticado
        /// </summary>
        [HttpPost("change-password")]
        [Authorize]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "Dados inválidos" });

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int id))
            {
                return BadRequest(new { success = false, message = "ID de usuário inválido" });
            }

            var usuario = await _usuarioService.GetByIdAsync(id);

            if (usuario == null)
            {
                return NotFound(new { success = false, message = "Usuário não encontrado" });
            }

            // Verificar senha atual
            if (usuario.Password != request.CurrentPassword)
            {
                return BadRequest(new { success = false, message = "Senha atual incorreta" });
            }

            // Atualizar senha
            usuario.Password = request.NewPassword;
            usuario.DataAtualizacao = DateTime.Now;

            var success = await _usuarioService.UpdateAsync(usuario);

            if (!success)
            {
                return BadRequest(new { success = false, message = "Erro ao atualizar senha" });
            }

            return Ok(new { success = true, message = "Senha alterada com sucesso" });
        }

        /// <summary>
        /// Renova o token JWT usando refresh token
        /// </summary>
        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<ActionResult<LoginResponse>> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            if (string.IsNullOrEmpty(request.Token) || string.IsNullOrEmpty(request.RefreshToken))
            {
                return BadRequest(new LoginResponse
                {
                    Success = false,
                    Message = "Token ou refresh token inválido"
                });
            }

            var principal = await _jwtService.GetPrincipalFromExpiredToken(request.Token);
            
            if (principal == null)
            {
                return BadRequest(new LoginResponse
                {
                    Success = false,
                    Message = "Token inválido"
                });
            }

            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int id))
            {
                return BadRequest(new LoginResponse
                {
                    Success = false,
                    Message = "Token inválido"
                });
            }

            var usuario = await _usuarioService.GetByIdAsync(id);

            if (usuario == null)
            {
                return NotFound(new LoginResponse
                {
                    Success = false,
                    Message = "Usuário não encontrado"
                });
            }

            // Gerar novos tokens
            var newToken = _jwtService.GenerateToken(usuario);
            var newRefreshToken = _jwtService.GenerateRefreshToken();
            var expiresAt = DateTime.UtcNow.AddMinutes(480);

            return Ok(new LoginResponse
            {
                Success = true,
                Message = "Token renovado com sucesso",
                Token = newToken,
                RefreshToken = newRefreshToken,
                ExpiresAt = expiresAt,
                User = new UserInfo
                {
                    Id = usuario.Id,
                    User = usuario.User,
                    Tipo = usuario.Tipo,
                    DataCadastro = usuario.DataCadastro
                }
            });
        }

        /// <summary>
        /// Valida se o token JWT é válido
        /// </summary>
        [HttpGet("validate")]
        [Authorize]
        public ActionResult ValidateToken()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            return Ok(new
            {
                valid = true,
                user = new
                {
                    id = userId,
                    username = username,
                    role = role
                }
            });
        }
    }
}

