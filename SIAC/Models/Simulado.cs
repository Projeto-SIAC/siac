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

    [Table("Simulado")]
    public partial class Simulado
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Simulado()
        {
            SimCandidato = new HashSet<SimCandidato>();
            SimDiaRealizacao = new HashSet<SimDiaRealizacao>();
            SimSala = new HashSet<SimSala>();
        }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Ano { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int NumIdentificador { get; set; }

        [Required]
        [StringLength(250)]
        public string Titulo { get; set; }

        [Column(TypeName = "text")]
        public string Descricao { get; set; }

        public int CodColaborador { get; set; }

        public DateTime? DtInicioInscricao { get; set; }

        public DateTime? DtTerminoInscricao { get; set; }

        public DateTime DtCadastro { get; set; }

        public int? QteVagas { get; set; }

        public bool FlagSimuladoEncerrado { get; set; }

        public bool FlagInscricaoEncerrado { get; set; }

        public bool FlagProvaEncerrada { get; set; }

        public virtual Colaborador Colaborador { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SimCandidato> SimCandidato { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SimDiaRealizacao> SimDiaRealizacao { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SimSala> SimSala { get; set; }
    }
}