using System.ComponentModel.DataAnnotations;

namespace PlataformaSeven.API.Models
{
    /// <summary>
    /// Modelo de requisição de login
    /// </summary>
    public class LoginRequest
    {
        [Required(ErrorMessage = "Usuário é obrigatório")]
        public string User { get; set; } = string.Empty;

        [Required(ErrorMessage = "Senha é obrigatória")]
        public string Password { get; set; } = string.Empty;
    }

    /// <summary>
    /// Modelo de resposta de login com JWT
    /// </summary>
    public class LoginResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public UserInfo? User { get; set; }
        public IEnumerable<MenuComSubMenus> Menu { get; set; } = new List<MenuComSubMenus>();
    }

    /// <summary>
    /// Informações do usuário autenticado
    /// </summary>
    public class UserInfo
    {
        public int Id { get; set; }
        public string User { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public DateTime? DataCadastro { get; set; }
    }

    /// <summary>
    /// Modelo para alteração de senha
    /// </summary>
    public class ChangePasswordRequest
    {
        [Required(ErrorMessage = "Senha atual é obrigatória")]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nova senha é obrigatória")]
        [MinLength(6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres")]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirmação de senha é obrigatória")]
        [Compare("NewPassword", ErrorMessage = "As senhas não coincidem")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }

    /// <summary>
    /// Modelo para refresh token
    /// </summary>
    public class RefreshTokenRequest
    {
        [Required]
        public string Token { get; set; } = string.Empty;

        [Required]
        public string RefreshToken { get; set; } = string.Empty;
    }
}

