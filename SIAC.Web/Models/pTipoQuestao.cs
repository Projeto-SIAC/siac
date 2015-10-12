using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Web.Models
{
    public partial class TipoQuestao
    {
        private static dbSIACEntities contexto { get { return Repositorio.GetInstance(); } }

        public static List<TipoQuestao> ListarOrdenadamente()
        {
            return contexto.TipoQuestao.OrderBy(tq => tq.Descricao).ToList();
        }

        public static TipoQuestao ListarPorCodigo(int CodTipoQuestao)
        {
            return contexto.TipoQuestao.SingleOrDefault(tq => tq.CodTipoQuestao == CodTipoQuestao);
        }
    }
}