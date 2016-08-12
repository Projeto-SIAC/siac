namespace SIAC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Questao")]
    public partial class Questao
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Questao()
        {
            Alternativa = new HashSet<Alternativa>();
            QuestaoAnexo = new HashSet<QuestaoAnexo>();
            QuestaoTema = new HashSet<QuestaoTema>();
            SimProvaQuestao = new HashSet<SimProvaQuestao>();
        }

        [Key]
        public int CodQuestao { get; set; }

        public int CodProfessor { get; set; }

        public int CodDificuldade { get; set; }

        public int CodTipoQuestao { get; set; }

        [Required]
        public string Enunciado { get; set; }

        [StringLength(250)]
        public string Objetivo { get; set; }

        [StringLength(250)]
        public string Comentario { get; set; }

        public string ChaveDeResposta { get; set; }

        public DateTime DtCadastro { get; set; }

        public DateTime? DtUltimoUso { get; set; }

        public bool FlagArquivo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Alternativa> Alternativa { get; set; }

        public virtual Dificuldade Dificuldade { get; set; }

        public virtual Professor Professor { get; set; }

        public virtual TipoQuestao TipoQuestao { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QuestaoAnexo> QuestaoAnexo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QuestaoTema> QuestaoTema { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SimProvaQuestao> SimProvaQuestao { get; set; }
    }
}