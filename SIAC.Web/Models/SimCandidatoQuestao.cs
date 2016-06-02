namespace SIAC.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("SimCandidatoQuestao")]
    public partial class SimCandidatoQuestao
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Ano { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int NumIdentificador { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CodCandidato { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CodDiaRealizacao { get; set; }

        [Key]
        [Column(Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CodProva { get; set; }

        [Key]
        [Column(Order = 5)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CodQuestao { get; set; }

        public int? RespAlternativa { get; set; }

        [Column(TypeName = "text")]
        public string RespDiscursiva { get; set; }

        public decimal? NotaDiscursiva { get; set; }

        public virtual SimCandidatoProva SimCandidatoProva { get; set; }

        public virtual SimProvaQuestao SimProvaQuestao { get; set; }
    }
}