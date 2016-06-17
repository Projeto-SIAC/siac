namespace SIAC.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("AviModulo")]
    public partial class AviModulo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AviModulo()
        {
            AviQuestao = new HashSet<AviQuestao>();
        }

        [Key]
        public int CodAviModulo { get; set; }

        [Required]
        [StringLength(100)]
        public string Descricao { get; set; }

        [Column(TypeName = "text")]
        public string Objetivo { get; set; }

        [StringLength(255)]
        public string Observacao { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AviQuestao> AviQuestao { get; set; }
    }
}