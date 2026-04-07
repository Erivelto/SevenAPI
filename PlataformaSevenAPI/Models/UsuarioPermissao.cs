using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations;

namespace PlataformaSeven.API.Models
{
    /// <summary>
    /// Permissao de acesso de um usuario a um SubMenu especifico.
    /// </summary>
    public class UsuarioPermissao
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int IdUsuario { get; set; }

        [Required]
        public int CodigoSubMenu { get; set; }

        /// <summary>
        /// Quando true, o usuario so pode consumir metodos GET neste SubMenu.
        /// </summary>
        public bool ApenasLeitura { get; set; } = false;

        public bool Ativo { get; set; } = true;

        public DateTime? DataCadastro { get; set; }
    }

    /// <summary>
    /// DTO de resposta com permissao enriquecida com dados do SubMenu e do Menu pai.
    /// </summary>
    public class UsuarioPermissaoDetalhe
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public int CodigoSubMenu { get; set; }
        public string SubMenuDescricao { get; set; } = string.Empty;
        public string? SubMenuIcone { get; set; }
        public string? SubMenuUrl { get; set; }
        public int SubMenuOrdem { get; set; }
        public int CodigoMenu { get; set; }
        public string MenuDescricao { get; set; } = string.Empty;
        public string? MenuIcone { get; set; }
        public int MenuOrdem { get; set; }
        public bool ApenasLeitura { get; set; }
        public bool Ativo { get; set; }
        public DateTime? DataCadastro { get; set; }
    }

    /// <summary>
    /// Item de permissao para envio em lote.
    /// </summary>
    public class UsuarioPermissaoItem
    {
        [Required]
        public int CodigoSubMenu { get; set; }

        public bool ApenasLeitura { get; set; } = false;
    }

    /// <summary>
    /// Request para salvar permissoes de um usuario em lote.
    /// Substitui todas as permissoes existentes do usuario.
    /// </summary>
    public class UsuarioPermissaoRequest
    {
        [Required]
        public int IdUsuario { get; set; }

        [Required]
        public List<UsuarioPermissaoItem> Permissoes { get; set; } = new();
    }
}
