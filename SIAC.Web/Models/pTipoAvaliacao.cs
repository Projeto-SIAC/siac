using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Web.Models
{
    public partial class TipoAvaliacao
    {
        private static dbSIACEntities contexto { get { return DataContextSIAC.GetInstance(); } }

        public static TipoAvaliacao ListarPorCodigo(int codTipoAvaliacao)
        {
            return contexto.TipoAvaliacao.FirstOrDefault(ta => ta.CodTipoAvaliacao == codTipoAvaliacao);
        }

        public static TipoAvaliacao ListarPorSigla(string sigla)
        {
            return contexto.TipoAvaliacao.FirstOrDefault(ta => ta.Sigla == sigla);
        }
    }
}