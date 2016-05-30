namespace SIAC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NivelEnsino")]
    public partial class NivelEnsino
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NivelEnsino()
        {
            Curso = new HashSet<Curso>();
            PessoaFormacao = new HashSet<PessoaFormacao>();
        }

        [Key]
        public int CodNivelEnsino { get; set; }

        [Required]
        [StringLength(40)]
        public string Descricao { get; set; }

        [StringLength(5)]
        public string Sigla { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Curso> Curso { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PessoaFormacao> PessoaFormacao { get; set; }
    }
}
