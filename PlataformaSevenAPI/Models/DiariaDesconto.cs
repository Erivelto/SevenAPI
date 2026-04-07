using System.ComponentModel.DataAnnotations;

namespace PlataformaSeven.API.Models
{
	public class DiariaDesconto
	{
		[Key]
		public int Id { get; set; }
		public int IdColaborador { get; set; }
		public DateTime Data { get; set; }
		public string? UserCadastro { get; set; }
		public decimal Valor { get; set; }
	}
}
