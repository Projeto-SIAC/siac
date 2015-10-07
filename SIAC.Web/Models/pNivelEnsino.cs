using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Web.Models
{
    public partial class NivelEnsino
    {
        private static dbSIACEntities contexto { get { return DataContextSIAC.GetInstance(); } }

        public static List<NivelEnsino> ListarOrdenadamente()
        {
            return contexto.NivelEnsino.OrderBy(ne => ne.Descricao).ToList();
        }
    }
}