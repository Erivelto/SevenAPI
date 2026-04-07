using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PlataformaSeven.API.Models
{
    public class Colaborador
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(150, ErrorMessage = "Máximo é de 150 caracteres!")]
        [Required(ErrorMessage = "Preenchimento obrigatório")]
        [DisplayName("Nome Completo")]
        public string Nome { get; set; } = string.Empty;

        public string? Pix { get; set; }
        
        public string? Referencia { get; set; }

        [DisplayName("Logradouro(Avenida,Rua,Travessa...)")]
        [MaxLength(250, ErrorMessage = "Máximo é de 250 caracteres!")]
        public string? Endereco { get; set; }

        [DisplayName("N°")]
        [MaxLength(10, ErrorMessage = "Máximo é de 10 caracteres!")]
        public string? Numero { get; set; }

        [DisplayName("Complemento(Apta,sala,casa...)")]
        [MaxLength(30, ErrorMessage = "Máximo é de 30 caracteres!")]
        public string? Complemento { get; set; }

        [DisplayName("Bairro")]
        [MaxLength(30, ErrorMessage = "Máximo é de 30 caracteres!")]
        public string? Bairro { get; set; }

        [DisplayName("Cidade")]
        [MaxLength(30, ErrorMessage = "Máximo é de 30 caracteres!")]
        public string? Cidade { get; set; }

        [DisplayName("Estado")]
        [MaxLength(2, ErrorMessage = "Máximo é de 2 caracteres!")]
        public string? UF { get; set; }

        [DisplayName("CEP")]
        [MaxLength(10, ErrorMessage = "Máximo é de 10 caracteres!")]
        public string? CEP { get; set; }

        public DateTime DataCadastro { get; set; } = DateTime.Now;
        
        public DateTime DataAlteracao { get; set; } = DateTime.Now;
        
        public string? UserCad { get; set; }
        
        public string? UserAlt { get; set; }
        
        public bool Excluido { get; set; } = false;
    }
}

