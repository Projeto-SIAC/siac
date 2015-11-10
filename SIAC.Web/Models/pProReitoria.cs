using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Models
{
    public partial class ProReitoria
    {
        public string CodComposto => $"{CodInstituicao}.{CodProReitoria}";

        private static dbSIACEntities contexto { get { return Repositorio.GetInstance(); } }

        public static List<ProReitoria> ListarOrdenadamente()
        {
            return contexto.ProReitoria.OrderBy(c => c.Sigla).ToList();
        }
    }
}