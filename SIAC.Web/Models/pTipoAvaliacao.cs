using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Web.Models
{
    public partial class TipoAvaliacao
    {
        private static dbSIACEntities contexto = DataContextSIAC.GetInstance();

        public static TipoAvaliacao ListarPorCodigo(int codTipoAvaliacao)
        {
            return contexto.TipoAvaliacao.FirstOrDefault(ta => ta.CodTipoAvaliacao == codTipoAvaliacao);
        }
    }
}