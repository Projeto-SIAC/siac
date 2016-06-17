namespace SIAC.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Email")]
    public partial class Email
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CodPessoa { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CodOrdem { get; set; }

        public int CodTipoContato { get; set; }

        [Column("Email")]
        [Required]
        [StringLength(100)]
        public string Email1 { get; set; }

        public virtual Pessoa Pessoa { get; set; }

        public virtual TipoContato TipoContato { get; set; }
    }
}