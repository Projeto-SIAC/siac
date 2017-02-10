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
    public partial class Horario
    {
        private static Contexto contexto => Repositorio.GetInstance();

        public static List<Horario> ListarOrdenadamente() => contexto.Horario.OrderBy(h => h.CodTurno).OrderBy(h => h.CodGrupo).ToList();

        public static void Inserir(Horario horario)
        {
            contexto.Horario.Add(horario);
            contexto.SaveChanges();
        }

        public static void Inserir(List<Horario> horarios)
        {
            contexto.Horario.AddRange(horarios);
            contexto.SaveChanges();
        }
    }
}