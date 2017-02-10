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
using System.Linq;

namespace SIAC.Models
{
    public partial class Disciplina
    {
        private static Contexto contexto => Repositorio.GetInstance();

        public static List<Disciplina> ListarOrdenadamente() => contexto.Disciplina.OrderBy(d => d.Descricao).ToList();

        public static Disciplina ListarPorCodigo(int codDisciplina) => contexto.Disciplina.Find(codDisciplina);

        public static int Inserir(Disciplina disciplina)
        {
            contexto.Disciplina.Add(disciplina);
            contexto.SaveChanges();
            return disciplina.CodDisciplina;
        }

        public static List<Disciplina> ListarTemQuestoes() =>
            contexto.QuestaoTema.Select(qt => qt.Tema.Disciplina)
            .Distinct()
            .OrderBy(d => d.Descricao)
            .ToList();

        public static List<Disciplina> ListarPorProfessor(string matrProfessor) => contexto.Professor.FirstOrDefault(p => p.MatrProfessor == matrProfessor).Disciplina.ToList();
    }
}