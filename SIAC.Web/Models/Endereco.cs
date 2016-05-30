namespace SIAC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Endereco")]
    public partial class Endereco
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CodPessoa { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CodOrdem { get; set; }

        public int CodMunicipio { get; set; }

        public int CodBairro { get; set; }

        public int CodPais { get; set; }

        public int CodEstado { get; set; }

        [Required]
        [StringLength(100)]
        public string Logradouro { get; set; }

        [Required]
        [StringLength(10)]
        public string Numero { get; set; }

        [StringLength(140)]
        public string Complemento { get; set; }

        public int? Cep { get; set; }

        public virtual Bairro Bairro { get; set; }

        public virtual Pessoa Pessoa { get; set; }
    }
}
