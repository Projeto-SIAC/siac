namespace SIAC.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Parametro")]
    public partial class Parametro
    {
        [Key]
        public int CodParametro { get; set; }

        public int TempoInatividade { get; set; }

        public int NumeracaoQuestao { get; set; }

        public int NumeracaoAlternativa { get; set; }

        public int QteSemestres { get; set; }

        public int PeriodoLetivoAnoAtual { get; set; }

        public int PeriodoLetivoSemestreAtual { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string TermoResponsabilidade { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string NotaUsoAcademica { get; set; }

        public double ValorNotaMedia { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string NotaUsoReposicao { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string NotaUsoCertificacao { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string NotaUsoInstitucional { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string CoordenadorAVI { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string NotaUsoSimulado { get; set; }

        [Required]
        [StringLength(200)]
        public string SmtpEnderecoHost { get; set; }

        public int SmtpPorta { get; set; }

        [Required]
        [StringLength(200)]
        public string SmtpUsuario { get; set; }

        [Required]
        [StringLength(200)]
        public string SmtpSenha { get; set; }

        public bool SmtpFlagSSL { get; set; }
    }
}