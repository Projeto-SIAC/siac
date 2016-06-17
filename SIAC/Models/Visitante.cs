namespace SIAC.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Visitante")]
    public partial class Visitante
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CodVisitante { get; set; }

        [Required]
        [StringLength(20)]
        public string MatrVisitante { get; set; }

        public DateTime? DtValidade { get; set; }

        public virtual Usuario Usuario { get; set; }
    }
}