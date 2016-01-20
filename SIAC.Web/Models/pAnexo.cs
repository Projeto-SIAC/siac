using System.Collections.Generic;
using System.Linq;

namespace SIAC.Models
{
    public partial class TipoAnexo
    {
        public const int IMAGEM = 1;
        public const int CODIGO = 2;

        private static dbSIACEntities contexto => Repositorio.GetInstance();

        public static List<TipoAnexo> ListarOrdenadamente() => contexto.TipoAnexo.OrderBy(ta => ta.Descricao).ToList();
    }
}