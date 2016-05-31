namespace SIAC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AviIndicador")]
    public partial class AviIndicador
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AviIndicador()
        {
            AviQuestao = new HashSet<AviQuestao>();
        }

        [Key]
        public int CodAviIndicador { get; set; }

        [Required]
        [StringLength(100)]
        public string Descricao { get; set; }

        [StringLength(255)]
        public string Observacao { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AviQuestao> AviQuestao { get; set; }
    }
}
