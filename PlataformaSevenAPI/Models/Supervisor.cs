using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PlataformaSeven.API.Models
{
    public class Supervisor
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(150, ErrorMessage = "Máximo é de 150 caracteres!")]
        [Required(ErrorMessage = "Preenchimento obrigatório")]
        [DisplayName("Nome")]
        public string Nome { get; set; } = string.Empty;
    }
}

