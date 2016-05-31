namespace SIAC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Instituicao")]
    public partial class Instituicao
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Instituicao()
        {
            Campus = new HashSet<Campus>();
            PessoaLocalTrabalho = new HashSet<PessoaLocalTrabalho>();
            ProReitoria = new HashSet<ProReitoria>();
            Reitoria = new HashSet<Reitoria>();
            AviPublico = new HashSet<AviPublico>();
        }

        [Key]
        public int CodInstituicao { get; set; }

        public int CodPessoaJuridica { get; set; }

        [Required]
        [StringLength(10)]
        public string Sigla { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Campus> Campus { get; set; }

        public virtual PessoaJuridica PessoaJuridica { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PessoaLocalTrabalho> PessoaLocalTrabalho { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProReitoria> ProReitoria { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reitoria> Reitoria { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AviPublico> AviPublico { get; set; }
    }
}
