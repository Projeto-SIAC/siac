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

    [Table("AvalAcademica")]
    public partial class AvalAcademica
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Ano { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Semestre { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CodTipoAvaliacao { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int NumIdentificador { get; set; }

        [StringLength(1)]
        public string CodTurno { get; set; }

        public int? NumTurma { get; set; }

        public int? Periodo { get; set; }

        public int? CodCurso { get; set; }

        public int? CodSala { get; set; }

        public int CodProfessor { get; set; }

        public int CodDisciplina { get; set; }

        public double? Valor { get; set; }

        public virtual Disciplina Disciplina { get; set; }

        public virtual Professor Professor { get; set; }

        public virtual Sala Sala { get; set; }

        public virtual Avaliacao Avaliacao { get; set; }

        public virtual Turma Turma { get; set; }
    }
}