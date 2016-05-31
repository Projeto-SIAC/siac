namespace SIAC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Aluno")]
    public partial class Aluno
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Aluno()
        {
            TurmaDiscAluno = new HashSet<TurmaDiscAluno>();
        }

        [Key]
        public int CodAluno { get; set; }

        public int CodCurso { get; set; }

        [Required]
        [StringLength(20)]
        public string MatrAluno { get; set; }

        public virtual Curso Curso { get; set; }

        public virtual Usuario Usuario { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TurmaDiscAluno> TurmaDiscAluno { get; set; }
    }
}
