namespace SIAC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UsuarioAcessoPagina")]
    public partial class UsuarioAcessoPagina
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string Matricula { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CodOrdem { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int NumIdentificador { get; set; }

        [Required]
        [StringLength(200)]
        public string Pagina { get; set; }

        public DateTime DtAbertura { get; set; }

        [StringLength(200)]
        public string PaginaReferencia { get; set; }

        [Column(TypeName = "text")]
        public string Dados { get; set; }

        public virtual UsuarioAcesso UsuarioAcesso { get; set; }
    }
}
