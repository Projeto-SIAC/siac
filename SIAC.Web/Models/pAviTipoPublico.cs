using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Models
{
    public partial class AviTipoPublico
    {
        private static dbSIACEntities contexto { get { return Repositorio.GetInstance(); } }

        public static List<AviTipoPublico> ListarOrdenadamente()
        {
            return contexto.AviTipoPublico.OrderBy(p => p.CodAviTipoPublico).ToList();
        }
    }
}