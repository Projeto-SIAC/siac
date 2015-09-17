using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Web.Models
{
    public partial class TipoAnexo
    {
        private static dbSIACEntities contexto = DataContextSIAC.GetInstance();

        public static List<TipoAnexo> ListarOrdenadamente()
        {
            return contexto.TipoAnexo.OrderBy(ta => ta.Descricao).ToList();
        }
    }
}