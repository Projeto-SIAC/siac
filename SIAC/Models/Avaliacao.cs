namespace SIAC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Avaliacao")]
    public partial class Avaliacao
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Avaliacao()
        {
            AvaliacaoProrrogacao = new HashSet<AvaliacaoProrrogacao>();
            AvaliacaoTema = new HashSet<AvaliacaoTema>();
            AvalPessoaResultado = new HashSet<AvalPessoaResultado>();
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

        public DateTime DtCadastro { get; set; }

        public DateTime? DtAplicacao { get; set; }

        public int? Duracao { get; set; }

        public bool FlagLiberada { get; set; }

        public bool FlagArquivo { get; set; }

        public virtual AvalAcademica AvalAcademica { get; set; }

        public virtual AvalAcadReposicao AvalAcadReposicao { get; set; }

        public virtual AvalAuto AvalAuto { get; set; }

        public virtual AvalAvi AvalAvi { get; set; }

        public virtual AvalCertificacao AvalCertificacao { get; set; }

        public virtual TipoAvaliacao TipoAvaliacao { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AvaliacaoProrrogacao> AvaliacaoProrrogacao { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AvaliacaoTema> AvaliacaoTema { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AvalPessoaResultado> AvalPessoaResultado { get; set; }
    }
}