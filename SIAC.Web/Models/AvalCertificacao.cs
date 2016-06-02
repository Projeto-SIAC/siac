namespace SIAC.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("AvalCertificacao")]
    public partial class AvalCertificacao
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AvalCertificacao()
        {
            PessoaFisica = new HashSet<PessoaFisica>();
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

        public int CodSala { get; set; }

        public int CodProfessor { get; set; }

        public int CodDisciplina { get; set; }

        public double? Valor { get; set; }

        public virtual Disciplina Disciplina { get; set; }

        public virtual Professor Professor { get; set; }

        public virtual Sala Sala { get; set; }

        public virtual Avaliacao Avaliacao { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PessoaFisica> PessoaFisica { get; set; }
    }
}