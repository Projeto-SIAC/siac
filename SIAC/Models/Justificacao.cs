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

    [Table("Justificacao")]
    public partial class Justificacao
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Justificacao()
        {
            AvalAcadReposicao = new HashSet<AvalAcadReposicao>();
        }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CodProfessor { get; set; }

        [Key]
        [Column(Order = 1)]
        public int CodJustificacao { get; set; }

        public int CodPessoaFisica { get; set; }

        public int Ano { get; set; }

        public int Semestre { get; set; }

        public int CodTipoAvaliacao { get; set; }

        public int NumIdentificador { get; set; }

        public DateTime DtCadastro { get; set; }

        public DateTime? DtConfirmacao { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string Descricao { get; set; }

        public virtual AvalPessoaResultado AvalPessoaResultado { get; set; }

        public virtual Professor Professor { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AvalAcadReposicao> AvalAcadReposicao { get; set; }
    }
}