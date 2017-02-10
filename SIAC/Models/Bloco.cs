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
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Bloco")]
    public partial class Bloco
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Bloco()
        {
            Sala = new HashSet<Sala>();
        }

        [Key]
        public int CodBloco { get; set; }

        public int? CodInstituicao { get; set; }

        public int? CodCampus { get; set; }

        [StringLength(100)]
        public string Descricao { get; set; }

        [StringLength(15)]
        public string Sigla { get; set; }

        [StringLength(255)]
        public string RefLocal { get; set; }

        [StringLength(140)]
        public string Observacao { get; set; }

        public virtual Campus Campus { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sala> Sala { get; set; }
    }
}