using System.Collections.Generic;
using System.Linq;

namespace SIAC.Models
{
    public partial class Ocupacao
    {
        public const int REITOR = 1;
        public const int PRO_REITOR = 2;
        public const int DIRETOR_GERAL = 3;
        public const int DIRETOR = 4;
        public const int COORDENADOR = 7;

        public const int COORDENADOR_AVI = 5;
        public const int ADMINISTRADOR_SIAC = 6;

        private static dbSIACEntities contexto => Repositorio.GetInstance();

        public static List<Ocupacao> Listar() => contexto.Ocupacao.ToList();

        public static Ocupacao ListarPorCodigo(int codOcupacao) => contexto.Ocupacao.Find(codOcupacao);
    }
}