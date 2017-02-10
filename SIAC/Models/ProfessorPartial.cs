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
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SIAC.Models
{
    public partial class Professor
    {
        [NotMapped]
        public Instituicao Instituicao => this.TurmaDiscProfHorario.OrderBy(t => t.AnoLetivo).LastOrDefault()?.Turma.Curso.Diretoria.Campus.Instituicao;

        [NotMapped]
        public Campus Campus => this.TurmaDiscProfHorario.OrderBy(t => t.AnoLetivo).LastOrDefault()?.Turma.Curso.Diretoria.Campus;

        public bool Leciona(int codDisciplina) => this.Disciplina.FirstOrDefault(d => d.CodDisciplina == codDisciplina) != null;

        private static Contexto contexto => Repositorio.GetInstance();

        public static Professor ListarPorMatricula(string matricula) => contexto.Professor.FirstOrDefault(p => p.MatrProfessor == matricula);

        public static Professor ListarPorCodigo(int codProfessor) => contexto.Professor.Find(codProfessor);

        public static void Inserir(Professor professor)
        {
            contexto.Professor.Add(professor);
            contexto.SaveChanges();
        }

        public static bool ProfessorLeciona(int codProfessor, int codDisciplina) =>
            (bool)contexto.Professor.Find(codProfessor)?.Leciona(codDisciplina);

        public static List<Disciplina> ObterDisciplinas(int codProfessor) => contexto.Professor.FirstOrDefault(p => p.CodProfessor == codProfessor)?.Disciplina.OrderBy(d => d.Descricao).ToList();

        public static List<Disciplina> ObterDisciplinas(string matrProfessor) => contexto.Professor.FirstOrDefault(p => p.MatrProfessor == matrProfessor)?.Disciplina.OrderBy(d => d.Descricao).ToList();

        public static List<Professor> ListarOrdenadamente() => contexto.Professor.OrderBy(p => p.Usuario.PessoaFisica.Nome).ToList();

        public override string ToString()
        {
            return this.Usuario.PessoaFisica.Nome;
        }
    }
}