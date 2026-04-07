using System.ComponentModel.DataAnnotations;

namespace PlataformaSeven.API.Models
{
    public class Diaria
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int IdColaboradorDetalhe { get; set; }

        [Required]
        public DateTime DataDiaria { get; set; } = DateTime.Now;

        public string? UserCadastro { get; set; }
    }
}

