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

    [Table("PessoaLocalTrabalho")]
    public partial class PessoaLocalTrabalho
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CodPessoa { get; set; }

        public int? CodInstituicao { get; set; }

        public int? CodReitoria { get; set; }

        public int? CodProReitoria { get; set; }

        public int? CodCampus { get; set; }

        public int? CodDiretoria { get; set; }

        public virtual Campus Campus { get; set; }

        public virtual Diretoria Diretoria { get; set; }

        public virtual Instituicao Instituicao { get; set; }

        public virtual PessoaFisica PessoaFisica { get; set; }

        public virtual Reitoria Reitoria { get; set; }

        public virtual ProReitoria ProReitoria { get; set; }
    }
}