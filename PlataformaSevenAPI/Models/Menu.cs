using System.ComponentModel.DataAnnotations;

namespace PlataformaSeven.API.Models
{
    public class Menu
    {
        [Key]
        public int Codigo { get; set; }

        [Required]
        public string Descricao { get; set; } = string.Empty;

        public string? Icone { get; set; }

        public string? Url { get; set; }

        public int Ordem { get; set; } = 0;

        public bool Ativo { get; set; } = true;

        public DateTime DataCriacao { get; set; } = DateTime.Now;
    }

    public class SubMenu
    {
        [Key]
        public int Codigo { get; set; }

        [Required]
        public int CodigoMenu { get; set; }

        [Required]
        public string Descricao { get; set; } = string.Empty;

        public string? Icone { get; set; }

        public string? Url { get; set; }

        public int Ordem { get; set; } = 0;

        public bool Ativo { get; set; } = true;

        public string? PerfilAcesso { get; set; }

        public DateTime DataCriacao { get; set; } = DateTime.Now;
    }

    /// <summary>
    /// DTO de resposta com o menu e seus submenus aninhados,
    /// já filtrado pelo perfil do usuário autenticado.
    /// </summary>
    public class MenuComSubMenus
    {
        public int Codigo { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public string? Icone { get; set; }
        public string? Url { get; set; }
        public int Ordem { get; set; }
        public List<SubMenu> SubMenus { get; set; } = new();
    }
}
