namespace SIAC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Disciplina")]
    public partial class Disciplina
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Disciplina()
        {
            AvalAcademica = new HashSet<AvalAcademica>();
            AvalCertificacao = new HashSet<AvalCertificacao>();
            MatrizCurricularDisciplina = new HashSet<MatrizCurricularDisciplina>();
            SimProva = new HashSet<SimProva>();
            Tema = new HashSet<Tema>();
            TurmaDiscProfHorario = new HashSet<TurmaDiscProfHorario>();
            TurmaDiscAluno = new HashSet<TurmaDiscAluno>();
            Professor = new HashSet<Professor>();
        }

        [Key]
        public int CodDisciplina { get; set; }

        [Required]
        [StringLength(50)]
        public string Descricao { get; set; }

        [StringLength(10)]
        public string Sigla { get; set; }

        public bool? FlagEletivaOptativa { get; set; }

        public bool? FlagFlexivel { get; set; }

        public int? CodDiscIFRN { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AvalAcademica> AvalAcademica { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AvalCertificacao> AvalCertificacao { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MatrizCurricularDisciplina> MatrizCurricularDisciplina { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SimProva> SimProva { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tema> Tema { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TurmaDiscProfHorario> TurmaDiscProfHorario { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TurmaDiscAluno> TurmaDiscAluno { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Professor> Professor { get; set; }
    }
}
