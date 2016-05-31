namespace SIAC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("QuestaoAnexo")]
    public partial class QuestaoAnexo
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CodQuestao { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CodOrdem { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CodTipoAnexo { get; set; }

        [Column(TypeName = "image")]
        [Required]
        public byte[] Anexo { get; set; }

        [StringLength(250)]
        public string Legenda { get; set; }

        [StringLength(250)]
        public string Fonte { get; set; }

        public virtual Questao Questao { get; set; }

        public virtual TipoAnexo TipoAnexo { get; set; }
    }
}
