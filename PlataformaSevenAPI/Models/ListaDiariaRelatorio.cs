namespace PlataformaSeven.API.Models
{
	public class ListaDiariaRelatorio
	{
		public int IdColaboradorDetalhe { get; set; }
		public int Quantidade { get; set; }
		public string Colaborador { get; set; }
		public string Periodo { get; set; }
		public string Funcao { get; set; }
		public string Posto { get; set; }
		public string Supervisor { get; set; }
		public string Detalhe { get; set; }
	}
}
