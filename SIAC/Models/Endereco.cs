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
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

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