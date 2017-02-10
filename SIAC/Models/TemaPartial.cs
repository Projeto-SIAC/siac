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
    public partial class Tema
    {
        private static Contexto contexto => Repositorio.GetInstance();

        public static Tema ListarPorCodigo(int CodDisciplina, int CodTema) => contexto.Tema.SingleOrDefault(t => t.CodDisciplina == CodDisciplina && t.CodTema == CodTema);

        public static List<Tema> ListarPorDisciplina(int CodDisciplina) => contexto.Tema.Where(t => t.CodDisciplina == CodDisciplina).OrderBy(t => t.Descricao).ToList();

        public static int Inserir(Tema tema)
        {
            List<Tema> temas = contexto.Disciplina.Find(tema.CodDisciplina).Tema.ToList();
            int id = temas.Count > 0 ? temas.Max(t => t.CodTema) + 1 : 1;

            tema.CodTema = id;

            contexto.Tema.Add(tema);
            contexto.SaveChanges();
            return tema.CodTema;
        }

        public static List<Tema> ListarPorDisciplinaTemQuestao(int codDisciplina) => contexto.QuestaoTema.Where(qt => qt.CodDisciplina == codDisciplina).Select(qt => qt.Tema).Distinct().OrderBy(t => t.Descricao).ToList();

        public static List<Tema> ListarOrdenadamenteComDisciplina() => contexto.Tema.OrderBy(t => t.Disciplina.Descricao).OrderBy(t => t.Descricao).ToList();
    }
}