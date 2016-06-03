namespace SIAC.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PessoaLocalTrabalho")]
    public partial class PessoaLocalTrabalho
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CodPessoa { get; set; }

        public int? CodInstituicao { get; set; }

        public int? CodReitoria { get; set; }

        public int? CodProReitoria { get; set; }

        public int? CodCampus { get; set; }

        public int? CodDiretoria { get; set; }

        public virtual Campus Campus { get; set; }

        public virtual Diretoria Diretoria { get; set; }

        public virtual Instituicao Instituicao { get; set; }

        public virtual PessoaFisica PessoaFisica { get; set; }

        public virtual Reitoria Reitoria { get; set; }

        public virtual ProReitoria ProReitoria { get; set; }
    }
}