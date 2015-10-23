using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Models
{
    public partial class TipoAnexo
    {
        private static dbSIACEntities contexto { get { return Repositorio.GetInstance(); } }

        public static List<TipoAnexo> ListarOrdenadamente()
        {
            return contexto.TipoAnexo.OrderBy(ta => ta.Descricao).ToList();
        }
    }
}