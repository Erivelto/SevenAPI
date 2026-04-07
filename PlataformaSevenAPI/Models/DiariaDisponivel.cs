using System.ComponentModel.DataAnnotations;

namespace PlataformaSeven.API.Models
{
	public class DiariaDisponivel
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public int QuantidadeDiaria { get; set; }

		[Required]
		public DateTime DataReferenciaInical { get; set; }
		
		public DateTime DataReferenciaFinal { get; set; }

		[Required]
		public int IdFuncao { get; set; }

		[Required]
		public int IdSupervisor { get; set; }

		[Required]
		public int IdPosto { get; set; }

		[Required]
		[MaxLength(50)]
		public string UsuarioCadAlt { get; set; } = string.Empty;

		public DateTime DataCadastro { get; set; } = DateTime.Now;

		public DateTime DataAlteracao { get; set; } = DateTime.Now;

		public bool Excluido { get; set; } = false;
	}
}
