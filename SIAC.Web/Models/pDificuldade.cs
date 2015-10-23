using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Models
{
    public partial class Dificuldade
    {
        private static dbSIACEntities contexto { get { return Repositorio.GetInstance(); } }

        public static List<Dificuldade> ListarOrdenadamente()
        {
            return contexto.Dificuldade.OrderBy(d => d.CodDificuldade).ToList();
        }

        public static Dificuldade ListarPorCodigo(int CodDificuldade)
        {
            return contexto.Dificuldade.SingleOrDefault(d => d.CodDificuldade == CodDificuldade);
        }
    }
}