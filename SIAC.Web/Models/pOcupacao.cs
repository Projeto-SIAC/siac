using System.Collections.Generic;
using System.Linq;

namespace SIAC.Models
{
    public partial class Ocupacao
    {
        private static dbSIACEntities contexto => Repositorio.GetInstance();

        public static List<Ocupacao> Listar() => contexto.Ocupacao.ToList();

        public static Ocupacao ListarPorCodigo(int codOcupacao) => contexto.Ocupacao.FirstOrDefault(a => a.CodOcupacao == codOcupacao);
    }
}