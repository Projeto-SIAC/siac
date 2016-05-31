using System.Linq;

namespace SIAC.Models
{
    public partial class Categoria
    {
        public const int ESTUDANTE = 1;
        public const int PROFESSOR = 2;
        public const int COLABORADOR = 3;
        public const int VISITANTE = 4;

        private static Contexto contexto => Repositorio.GetInstance();

        public static Categoria ListarPorCodigo(int codCategoria) => contexto.Categoria.FirstOrDefault(c => c.CodCategoria == codCategoria);
    }
}