using System.Collections.Generic;
using System.Linq;

namespace SIAC.Models
{
    public partial class TipoQuestao
    {
        private static dbSIACEntities contexto => Repositorio.GetInstance();

        public static List<TipoQuestao> ListarOrdenadamente() => contexto.TipoQuestao.OrderBy(tq => tq.Descricao).ToList();

        public static TipoQuestao ListarPorCodigo(int codTipoQuestao) => contexto.TipoQuestao.Find(codTipoQuestao);
    }
}