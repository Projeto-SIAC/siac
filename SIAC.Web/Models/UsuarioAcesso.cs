namespace SIAC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("UsuarioAcesso")]
    public partial class UsuarioAcesso
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UsuarioAcesso()
        {
            UsuarioAcessoPagina = new HashSet<UsuarioAcessoPagina>();
        }

        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string Matricula { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CodOrdem { get; set; }

        public DateTime DtAcesso { get; set; }

        [StringLength(50)]
        public string IpAcesso { get; set; }

        public virtual Usuario Usuario { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UsuarioAcessoPagina> UsuarioAcessoPagina { get; set; }
    }
}