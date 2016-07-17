namespace SIAC.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("SimProva")]
    public partial class SimProva
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SimProva()
        {
            SimCandidatoProva = new HashSet<SimCandidatoProva>();
            SimProvaQuestao = new HashSet<SimProvaQuestao>();
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
        public int CodDiaRealizacao { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CodProva { get; set; }

        public int? CodProfessor { get; set; }

        public int CodDisciplina { get; set; }

        public int QteQuestoes { get; set; }

        [StringLength(200)]
        public string Titulo { get; set; }

        [StringLength(500)]
        public string Descricao { get; set; }

        public decimal? MediaAritmeticaAcerto { get; set; }

        public decimal? DesvioPadraoAcerto { get; set; }

        public float Peso { get; set; }

        public int TipoQuestoes { get; set; }

        public bool FlagRedacao { get; set; }

        public int OrdemDesempate { get; set; }

        public virtual Disciplina Disciplina { get; set; }

        public virtual Professor Professor { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SimCandidatoProva> SimCandidatoProva { get; set; }

        public virtual SimDiaRealizacao SimDiaRealizacao { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SimProvaQuestao> SimProvaQuestao { get; set; }
    }
}