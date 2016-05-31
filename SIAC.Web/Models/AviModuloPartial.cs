using System.Collections.Generic;
using System.Linq;

namespace SIAC.Models
{
    public partial class AviModulo
    {
        private static Contexto contexto => Repositorio.GetInstance();

        public static List<AviModulo> ListarOrdenadamente() => contexto.AviModulo.OrderBy(m => m.Descricao).ToList();

        public static void Inserir(AviModulo modulo)
        {
            contexto.AviModulo.Add(modulo);
            contexto.SaveChanges();
        }
    }
}