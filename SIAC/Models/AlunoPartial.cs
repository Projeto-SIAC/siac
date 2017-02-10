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
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SIAC.Models
{
    public partial class Aluno
    {
        [NotMapped]
        public Turma Turma => this.TurmaDiscAluno.OrderBy(t => t.AnoLetivo).Last()?.Turma;

        [NotMapped]
        public List<Disciplina> Disciplinas => this.TurmaDiscAluno.Where(t => t.AnoLetivo == DateTime.Today.Year).Select(t => t.Disciplina).Distinct().ToList();

        [NotMapped]
        public List<Professor> Professores => this.TurmaDiscAluno.Where(t => t.AnoLetivo == DateTime.Today.Year).Join(
                contexto.TurmaDiscProfHorario,
                tda => new { tda.CodDisciplina, tda.AnoLetivo, tda.SemestreLetivo, tda.Turma },
                tdph => new { tdph.CodDisciplina, tdph.AnoLetivo, tdph.SemestreLetivo, tdph.Turma },
                (tda, tdph) =>
                    tdph.Professor
            ).Distinct().ToList();

        private static Contexto contexto => Repositorio.GetInstance();

        public static void Inserir(Aluno aluno)
        {
            contexto.Aluno.Add(aluno);
            contexto.SaveChanges();
        }

        public static List<Aluno> ListarOrdenadamente() => contexto.Aluno.OrderBy(a => a.Usuario.PessoaFisica.Nome).ToList();

        public static Aluno ListarPorCodigo(int codAluno) => contexto.Aluno.Find(codAluno);

        public static Aluno ListarPorMatricula(string strMatricula) => contexto.Aluno.FirstOrDefault(a => a.MatrAluno == strMatricula);
    }
}