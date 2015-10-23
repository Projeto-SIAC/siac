using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Models
{
    public partial class Turno
    {
        private static dbSIACEntities contexto { get { return Repositorio.GetInstance(); } }

        public static List<Turno> ListarOrdenadamente()
        {
            return contexto.Turno.ToList();
        }
    }
}