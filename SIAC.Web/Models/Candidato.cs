namespace SIAC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Candidato")]
    public partial class Candidato
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Candidato()
        {
            SimCandidato = new HashSet<SimCandidato>();
        }

        [Key]
        public int CodCandidato { get; set; }

        [Required]
        [StringLength(255)]
        public string Nome { get; set; }

        [Required]
        [StringLength(11)]
        public string Cpf { get; set; }

        public int? RgNumero { get; set; }

        [StringLength(20)]
        public string RgOrgao { get; set; }

        [Column(TypeName = "date")]
        public DateTime? RgDtExpedicao { get; set; }

        [Required]
        [StringLength(200)]
        public string Email { get; set; }

        [Required]
        [StringLength(64)]
        public string Senha { get; set; }

        public DateTime DtCadastro { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DtNascimento { get; set; }

        [StringLength(1)]
        public string Sexo { get; set; }

        [StringLength(20)]
        public string Matricula { get; set; }

        [StringLength(20)]
        public string TelefoneFixo { get; set; }

        [StringLength(20)]
        public string TelefoneCelular { get; set; }

        public int? CodPais { get; set; }

        public int? CodEstado { get; set; }

        public int? CodMunicipio { get; set; }

        public bool? FlagAdventista { get; set; }

        public bool? FlagNecessidadeEspecial { get; set; }

        [StringLength(255)]
        public string DescricaoNecessidadeEspecial { get; set; }

        public virtual Municipio Municipio { get; set; }

        public virtual Usuario Usuario { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SimCandidato> SimCandidato { get; set; }
    }
}
