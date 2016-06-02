namespace SIAC.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PessoaJuridica")]
    public partial class PessoaJuridica
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PessoaJuridica()
        {
            Campus = new HashSet<Campus>();
            Diretoria = new HashSet<Diretoria>();
            Instituicao = new HashSet<Instituicao>();
            ProReitoria = new HashSet<ProReitoria>();
            Reitoria = new HashSet<Reitoria>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CodPessoa { get; set; }

        [StringLength(255)]
        public string RazaoSocial { get; set; }

        [Required]
        [StringLength(100)]
        public string NomeFantasia { get; set; }

        [StringLength(15)]
        public string Cnpj { get; set; }

        [StringLength(100)]
        public string Portal { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Campus> Campus { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Diretoria> Diretoria { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Instituicao> Instituicao { get; set; }

        public virtual Pessoa Pessoa { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProReitoria> ProReitoria { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reitoria> Reitoria { get; set; }
    }
}