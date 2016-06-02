namespace SIAC.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("SimCandidatoProva")]
    public partial class SimCandidatoProva
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SimCandidatoProva()
        {
            SimCandidatoQuestao = new HashSet<SimCandidatoQuestao>();
        }

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

        public decimal? NotaDiscursiva { get; set; }

        public int? QteAcertos { get; set; }

        public bool? FlagPresente { get; set; }

        public decimal? EscorePadronizado { get; set; }

        [Column(TypeName = "text")]
        public string Observacoes { get; set; }

        public virtual SimCandidato SimCandidato { get; set; }

        public virtual SimProva SimProva { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SimCandidatoQuestao> SimCandidatoQuestao { get; set; }
    }
}