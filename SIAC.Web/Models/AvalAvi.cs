namespace SIAC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("AvalAvi")]
    public partial class AvalAvi
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AvalAvi()
        {
            AviPublico = new HashSet<AviPublico>();
            AviQuestao = new HashSet<AviQuestao>();
        }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Ano { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Semestre { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CodTipoAvaliacao { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int NumIdentificador { get; set; }

        public int CodColabCoordenador { get; set; }

        [Required]
        [StringLength(250)]
        public string Titulo { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string Objetivo { get; set; }

        public DateTime? DtTermino { get; set; }

        public virtual Avaliacao Avaliacao { get; set; }

        public virtual Colaborador Colaborador { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AviPublico> AviPublico { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AviQuestao> AviQuestao { get; set; }
    }
}