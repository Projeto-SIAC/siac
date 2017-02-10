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
    public partial class Turno
    {
        public const string MATUTINO = "M";
        public const string VESPERTINO = "V";
        public const string NOTURNO = "N";


        private static Contexto contexto => Repositorio.GetInstance();

        public static List<Turno> ListarOrdenadamente() => contexto.Turno.ToList();

        public static string ObterCodTurnoPorData(DateTime data)
        {
            int hora = data.Hour;
            
            if (hora < 12)
            {
                return Turno.MATUTINO;
            }
            else if (hora < 18)
            {
                return Turno.VESPERTINO;
            }
            else
            {
                return Turno.NOTURNO;
            }
        }
    }
}