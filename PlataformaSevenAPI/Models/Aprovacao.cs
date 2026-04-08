using System.ComponentModel.DataAnnotations;

namespace PlataformaSeven.API.Models
{
    /// <summary>
    /// Define qual usuario e o aprovador de CREATE para uma entidade especifica.
    /// Gerenciado pelo usuario master. Pode ter multiplos aprovadores por entidade.
    /// </summary>
    public class AprovacaoConfig
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Nome da entidade controlada (ex: "Posto", "Funcao", "Supervisor", "Colaborador").
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Entidade { get; set; } = string.Empty;

        /// <summary>
        /// Id do usuario que tem permissao para aprovar solicitacoes desta entidade.
        /// </summary>
        [Required]
        public int IdUsuarioAprovador { get; set; }

        public bool Ativo { get; set; } = true;

        public DateTime DataCadastro { get; set; } = DateTime.Now;
    }

    /// <summary>
    /// Registro de stage (fila) de uma solicitacao de CREATE pendente de aprovacao.
    /// </summary>
    public class AprovacaoStage
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Nome da entidade alvo (ex: "Posto").
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Entidade { get; set; } = string.Empty;

        /// <summary>
        /// Payload JSON com os dados a serem inseridos apos aprovacao.
        /// </summary>
        [Required]
        public string PayloadJson { get; set; } = string.Empty;

        [Required]
        public int IdUsuarioSolicitante { get; set; }

        [Required]
        public int IdUsuarioAprovador { get; set; }

        /// <summary>
        /// Pendente | Aprovado | Rejeitado
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = AprovacaoStatus.Pendente;

        public DateTime DataSolicitacao { get; set; } = DateTime.Now;

        public DateTime? DataAprovacao { get; set; }

        /// <summary>
        /// Observacao do aprovador (ex: motivo de rejeicao).
        /// </summary>
        [MaxLength(500)]
        public string? Observacao { get; set; }

        /// <summary>
        /// Id do registro efetivamente criado apos aprovacao.
        /// </summary>
        public int? IdRegistroGerado { get; set; }
    }

    public static class AprovacaoStatus
    {
        public const string Pendente  = "Pendente";
        public const string Aprovado  = "Aprovado";
        public const string Rejeitado = "Rejeitado";
    }

    // ==================== DTOs ====================

    public class AprovacaoStageDetalhe
    {
        public int Id { get; set; }
        public string Entidade { get; set; } = string.Empty;
        public string PayloadJson { get; set; } = string.Empty;
        public int IdUsuarioSolicitante { get; set; }
        public string NomeSolicitante { get; set; } = string.Empty;
        public int IdUsuarioAprovador { get; set; }
        public string NomeAprovador { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime DataSolicitacao { get; set; }
        public DateTime? DataAprovacao { get; set; }
        public string? Observacao { get; set; }
        public int? IdRegistroGerado { get; set; }
    }

    public class SolicitarAprovacaoRequest
    {
        /// <summary>
        /// Nome da entidade alvo (ex: "Posto").
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Entidade { get; set; } = string.Empty;

        /// <summary>
        /// Payload JSON com os dados a serem inseridos. Ex: {"Nome":"Posto Central"}
        /// </summary>
        [Required]
        public string PayloadJson { get; set; } = string.Empty;
    }

    public class AprovarRequest
    {
        [MaxLength(500)]
        public string? Observacao { get; set; }
    }

    public class RejeitarRequest
    {
        [Required(ErrorMessage = "Informe o motivo da rejeicao.")]
        [MaxLength(500)]
        public string Observacao { get; set; } = string.Empty;
    }

    public class AprovacaoConfigRequest
    {
        [Required]
        [MaxLength(100)]
        public string Entidade { get; set; } = string.Empty;

        [Required]
        public int IdUsuarioAprovador { get; set; }
    }
}
