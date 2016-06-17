namespace SIAC.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Alternativa")]
    public partial class Alternativa
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CodQuestao { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CodOrdem { get; set; }

        [Required]
        [StringLength(250)]
        public string Enunciado { get; set; }

        [StringLength(250)]
        public string Comentario { get; set; }

        public bool FlagGabarito { get; set; }

        public virtual Questao Questao { get; set; }
    }
}