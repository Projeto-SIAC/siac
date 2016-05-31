namespace SIAC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AviPublico")]
    public partial class AviPublico
    {
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

        [Key]
        [Column(Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CodOrdem { get; set; }

        [Key]
        [Column(Order = 5)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CodAviTipoPublico { get; set; }

        public virtual AvalAvi AvalAvi { get; set; }

        public virtual AviTipoPublico AviTipoPublico { get; set; }

        public virtual Campus Campus { get; set; }

        public virtual Curso Curso { get; set; }

        public virtual Diretoria Diretoria { get; set; }

        public virtual Instituicao Instituicao { get; set; }

        public virtual PessoaFisica PessoaFisica { get; set; }

        public virtual ProReitoria ProReitoria { get; set; }

        public virtual Reitoria Reitoria { get; set; }

        public virtual Turma Turma { get; set; }
    }
}
