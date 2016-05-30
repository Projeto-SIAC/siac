namespace SIAC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UsuarioOpiniao")]
    public partial class UsuarioOpiniao
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string Matricula { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CodOrdem { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string Opiniao { get; set; }

        public DateTime? DtEnvio { get; set; }

        public virtual Usuario Usuario { get; set; }
    }
}
