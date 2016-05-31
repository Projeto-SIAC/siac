namespace SIAC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PessoaFisica")]
    public partial class PessoaFisica
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PessoaFisica()
        {
            AvalAuto = new HashSet<AvalAuto>();
            AvalPessoaResultado = new HashSet<AvalPessoaResultado>();
            AvalQuesPessoaResposta = new HashSet<AvalQuesPessoaResposta>();
            AviQuestaoPessoaResposta = new HashSet<AviQuestaoPessoaResposta>();
            PessoaFormacao = new HashSet<PessoaFormacao>();
            Usuario = new HashSet<Usuario>();
            AvalCertificacao = new HashSet<AvalCertificacao>();
            AviPublico = new HashSet<AviPublico>();
            Categoria = new HashSet<Categoria>();
            Ocupacao = new HashSet<Ocupacao>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CodPessoa { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DtNascimento { get; set; }

        [StringLength(11)]
        public string Cpf { get; set; }

        [StringLength(1)]
        public string Sexo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AvalAuto> AvalAuto { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AvalPessoaResultado> AvalPessoaResultado { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AvalQuesPessoaResposta> AvalQuesPessoaResposta { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AviQuestaoPessoaResposta> AviQuestaoPessoaResposta { get; set; }

        public virtual Pessoa Pessoa { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PessoaFormacao> PessoaFormacao { get; set; }

        public virtual PessoaLocalTrabalho PessoaLocalTrabalho { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Usuario> Usuario { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AvalCertificacao> AvalCertificacao { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AviPublico> AviPublico { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Categoria> Categoria { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ocupacao> Ocupacao { get; set; }
    }
}
