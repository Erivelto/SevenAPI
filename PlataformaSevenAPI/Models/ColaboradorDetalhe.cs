using System.ComponentModel.DataAnnotations;

namespace PlataformaSeven.API.Models
{
    public class ColaboradorDetalhe
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int IdColaborador { get; set; }
        
        [Required]
        public decimal ValorDiaria { get; set; }
        
        [Required]
        public int IdFuncao { get; set; }
        
        [Required]
        public int IdSupervisor { get; set; }
        
        [Required]
        public int IdPosto { get; set; }

        [MaxLength(1)]
        public string? Periodo { get; set; }
    }
}

