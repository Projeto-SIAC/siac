using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Models
{
    public partial class AviIndicador
    {
        private static dbSIACEntities contexto { get { return Repositorio.GetInstance(); } }

        public static List<AviIndicador> ListarOrdenadamente()
        {
            return contexto.AviIndicador.OrderBy(i => i.Descricao).ToList();
        }
    }
}