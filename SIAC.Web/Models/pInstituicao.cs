using System.Collections.Generic;
using System.Linq;

namespace SIAC.Models
{
    public partial class Instituicao
    {
        private static dbSIACEntities contexto => Repositorio.GetInstance();

        public static List<Instituicao> ListarOrdenadamente() => contexto.Instituicao.OrderBy(ins => ins.Sigla).ToList();

        public static Instituicao ListarPorCodigo(int codInstituicao) => contexto.Instituicao.FirstOrDefault(ins => ins.CodInstituicao == codInstituicao);
    }
}