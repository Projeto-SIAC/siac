namespace SIAC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("SimCandidato")]
    public partial class SimCandidato
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SimCandidato()
        {
            SimCandidatoProva = new HashSet<SimCandidatoProva>();
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

        public int NumInscricao { get; set; }

        public DateTime DtInscricao { get; set; }

        public int? CodSala { get; set; }

        public decimal? EscorePadronizadoFinal { get; set; }

        [StringLength(50)]
        public string NumeroMascara { get; set; }

        public virtual Candidato Candidato { get; set; }

        public virtual Simulado Simulado { get; set; }

        public virtual SimSala SimSala { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SimCandidatoProva> SimCandidatoProva { get; set; }
    }
}