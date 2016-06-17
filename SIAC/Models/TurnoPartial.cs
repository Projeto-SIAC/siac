﻿using System.Collections.Generic;
using System.Linq;

namespace SIAC.Models
{
    public partial class Turno
    {
        private static Contexto contexto => Repositorio.GetInstance();

        public static List<Turno> ListarOrdenadamente() => contexto.Turno.ToList();
    }
}