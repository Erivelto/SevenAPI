namespace PlataformaSeven.API.Models
{
    public class ListaDiariaRelatorio
    {
        public int IdColaboradorDetalhe { get; set; }
        public int Quantidade { get; set; }
        public string Colaborador { get; set; } = string.Empty;
        public string Periodo { get; set; } = string.Empty;
        public string Funcao { get; set; } = string.Empty;
        public string Supervisor { get; set; } = string.Empty;
        public string Posto { get; set; } = string.Empty;

        /// <summary>
        /// Detalhamento diario de cada colaborador no periodo (dias trabalhados com posto).
        /// Populado apos a query principal, equivalente ao loop do legado.
        /// </summary>
        public IEnumerable<ListaDiariaColaboradorResponse> Detalhe { get; set; } = new List<ListaDiariaColaboradorResponse>();
    }
}
