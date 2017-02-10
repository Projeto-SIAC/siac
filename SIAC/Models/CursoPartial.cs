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
    public partial class Curso
    {
        private static Contexto contexto => Repositorio.GetInstance();

        public static List<Curso> ListarOrdenadamente() => contexto.Curso.OrderBy(c => c.Descricao).ToList();

        public static Curso ListarPorCodigo(int codCurso) => contexto.Curso.Find(codCurso);

        public static void Inserir(Curso curso)
        {
            curso.CodCurso = ObterCodCurso();
            contexto.Curso.Add(curso);
            contexto.SaveChanges();
        }

        public static int ObterCodCurso()
        {
            int codCurso = contexto.Curso.Max(c => c.CodCurso);
            return codCurso + 1;
        }
    }
}