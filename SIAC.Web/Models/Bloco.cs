namespace SIAC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Bloco")]
    public partial class Bloco
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Bloco()
        {
            Sala = new HashSet<Sala>();
        }

        [Key]
        public int CodBloco { get; set; }

        public int? CodInstituicao { get; set; }

        public int? CodCampus { get; set; }

        [StringLength(100)]
        public string Descricao { get; set; }

        [StringLength(15)]
        public string Sigla { get; set; }

        [StringLength(255)]
        public string RefLocal { get; set; }

        [StringLength(140)]
        public string Observacao { get; set; }

        public virtual Campus Campus { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sala> Sala { get; set; }
    }
}
