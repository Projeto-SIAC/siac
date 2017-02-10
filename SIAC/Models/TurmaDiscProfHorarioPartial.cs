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
using System.Linq;

namespace SIAC.Models
{
    public partial class TurmaDiscProfHorario
    {
        private static Contexto contexto => Repositorio.GetInstance();

        public static List<TurmaDiscProfHorario> ListarPorUsuario(Usuario usuario)
        {
            int ano = DateTime.Now.Year;
            int semestre = DateTime.Now.SemestreAtual();

            List<TurmaDiscProfHorario> retorno = new List<TurmaDiscProfHorario>();

            switch (usuario.CodCategoria)
            {
                case Categoria.ALUNO:
                    int codAluno = usuario.Aluno.Last().CodAluno;
                    retorno = contexto.TurmaDiscProfHorario
                        .Where(h => h.Turma.TurmaDiscAluno.FirstOrDefault(t => t.CodAluno == codAluno) != null
                            && h.AnoLetivo == ano
                            && h.SemestreLetivo == semestre)
                        .ToList();
                    break;

                case Categoria.PROFESSOR:
                    int codProfessor = usuario.Professor.Last().CodProfessor;
                    retorno = contexto.TurmaDiscProfHorario
                        .Where(h => h.CodProfessor == codProfessor
                            && h.AnoLetivo == ano
                            && h.SemestreLetivo == semestre)
                        .ToList();
                    break;

                default:
                    break;
            }
            return retorno;
        }
    }
}