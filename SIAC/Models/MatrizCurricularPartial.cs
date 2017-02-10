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
    public partial class MatrizCurricular
    {
        private static Contexto contexto => Repositorio.GetInstance();

        public static List<MatrizCurricular> ListarOrdenadamente() => contexto.MatrizCurricular.OrderBy(m => m.Curso.Descricao).ToList();

        public static void Inserir(MatrizCurricular matrizCurricular)
        {
            contexto.MatrizCurricular.Add(matrizCurricular);
            contexto.SaveChanges();
        }

        public static int ObterCodMatriz(int codCurso)
        {
            int codMatriz = 1;
            int qteMatriz = contexto.MatrizCurricular.Where(m => m.CodCurso == codCurso).Count();
            if (qteMatriz != 0)
            {
                codMatriz = qteMatriz + 1;
            }
            return codMatriz;
        }
    }
}