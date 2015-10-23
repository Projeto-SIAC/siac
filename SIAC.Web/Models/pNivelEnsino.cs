using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Models
{
    public partial class NivelEnsino
    {
        private static dbSIACEntities contexto { get { return Repositorio.GetInstance(); } }

        public static List<NivelEnsino> ListarOrdenadamente()
        {
            return contexto.NivelEnsino.OrderBy(ne => ne.Descricao).ToList();
        }
    }
}