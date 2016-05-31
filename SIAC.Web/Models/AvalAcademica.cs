namespace SIAC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AvalAcademica")]
    public partial class AvalAcademica
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

        [Required]
        [StringLength(1)]
        public string CodTurno { get; set; }

        public int NumTurma { get; set; }

        public int Periodo { get; set; }

        public int CodCurso { get; set; }

        public int CodSala { get; set; }

        public int CodProfessor { get; set; }

        public int CodDisciplina { get; set; }

        public double? Valor { get; set; }

        public virtual Disciplina Disciplina { get; set; }

        public virtual Professor Professor { get; set; }

        public virtual Sala Sala { get; set; }

        public virtual Avaliacao Avaliacao { get; set; }

        public virtual Turma Turma { get; set; }
    }
}
