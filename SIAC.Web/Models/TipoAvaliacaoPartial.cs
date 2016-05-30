using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SIAC.Models
{
    public partial class TipoAvaliacao
    {
        public const int AUTOAVALIACAO = 1;
        public const int ACADEMICA = 2;
        public const int CERTIFICACAO = 3;
        public const int AVI = 4;
        public const int REPOSICAO = 5;

        [NotMapped]
        public string DescricaoCurta => CodTipoAvaliacao == 4 ? Descricao : Descricao.Split(' ').Last();

        private static dbSIACEntities contexto => Repositorio.GetInstance();

        public static TipoAvaliacao ListarPorCodigo(int codTipoAvaliacao) => contexto.TipoAvaliacao.Find(codTipoAvaliacao);

        public static TipoAvaliacao ListarPorSigla(string sigla) => contexto.TipoAvaliacao.FirstOrDefault(ta => ta.Sigla == sigla);
    }
}