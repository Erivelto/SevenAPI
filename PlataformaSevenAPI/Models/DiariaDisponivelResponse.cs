namespace PlataformaSeven.API.Models
{
    public class DiariaDisponivelResponse
    {
        public int Id { get; set; }
        public int QuantidadeDiaria { get; set; }
        public DateTime DataReferenciaInical { get; set; }
        public DateTime DataReferenciaFinal { get; set; }
        public string? Funcao { get; set; }
        public string? Supervisor { get; set; }
        public string? Posto { get; set; }
    }
}
