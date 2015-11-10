using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Models
{
    public partial class Reitoria
    {
        public string CodComposto => $"{CodInstituicao}.{CodReitoria}";

        private static dbSIACEntities contexto { get { return Repositorio.GetInstance(); } }

        public static List<Reitoria> ListarOrdenadamente()
        {
            return contexto.Reitoria.OrderBy(c => c.Sigla).ToList();
        }
    }
}