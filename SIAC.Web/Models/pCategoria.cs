using System.Linq;

namespace SIAC.Models
{
    public partial class Categoria
    {
        private static dbSIACEntities contexto => Repositorio.GetInstance();

        public static Categoria ListarPorCodigo(int codCategoria) => contexto.Categoria.FirstOrDefault(c => c.CodCategoria == codCategoria);
    }
}