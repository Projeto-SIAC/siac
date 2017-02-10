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
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

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