using System.Linq;

namespace SIAC.Models
{
    public partial class TipoAvaliacao
    {
        private static dbSIACEntities contexto => Repositorio.GetInstance();

        public static TipoAvaliacao ListarPorCodigo(int codTipoAvaliacao) => contexto.TipoAvaliacao.FirstOrDefault(ta => ta.CodTipoAvaliacao == codTipoAvaliacao);

        public static TipoAvaliacao ListarPorSigla(string sigla) => contexto.TipoAvaliacao.FirstOrDefault(ta => ta.Sigla == sigla);
    }
}