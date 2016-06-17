using System.Collections.Generic;
using System.Linq;

namespace SIAC.Models
{
    public partial class AviTipoPublico
    {
        public const int INSTITUICAO = 1;
        public const int REITORIA = 2;
        public const int PRO_REITORIA = 3;
        public const int CAMPUS = 4;
        public const int DIRETORIA = 5;
        public const int CURSO = 6;
        public const int TURMA = 7;
        public const int PESSOA = 8;

        private static Contexto contexto => Repositorio.GetInstance();

        public static List<AviTipoPublico> ListarOrdenadamente() => contexto.AviTipoPublico.OrderBy(p => p.CodAviTipoPublico).ToList();
    }
}