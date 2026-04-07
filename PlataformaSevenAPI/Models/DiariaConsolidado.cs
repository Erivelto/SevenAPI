namespace PlataformaSeven.API.Models
{
	public class DiariaConsolidado
	{
		public int IdColaborador { get; set; }
		public string Nome { get; set; }
		public decimal ValorTotal { get; set; }
		public string ValorDiaria { get; set; }
		public int QuantidadeDiaria { get; set; }
		public string Pix { get; set; }
		public decimal Adiantamento { get; set; }

	}
}
