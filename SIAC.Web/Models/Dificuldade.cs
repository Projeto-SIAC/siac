using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Web.Models
{
    public partial class Dificuldade
    {
        private static DataClassesSIACDataContext contexto = DataContextSIAC.GetInstance();

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