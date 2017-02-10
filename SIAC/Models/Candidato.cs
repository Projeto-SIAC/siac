/*
This file is part of SIAC.

Copyright (C) 2016 Felipe Mateus Freire Pontes <felipemfpontes@gmail.com>
Copyright (C) 2016 Francisco Bento da Silva Júnior <francisco.bento.jr@hotmail.com>

SIAC is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details. 
*/
namespace SIAC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

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
        [StringLength(60)]
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

        [StringLength(64)]
        public string AlterarSenha { get; set; }

        public virtual Municipio Municipio { get; set; }

        public virtual Usuario Usuario { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SimCandidato> SimCandidato { get; set; }
    }
}