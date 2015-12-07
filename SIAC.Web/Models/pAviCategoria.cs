using System.Collections.Generic;
using System.Linq;

namespace SIAC.Models
{
    public partial class AviCategoria
    {
        private static dbSIACEntities contexto => Repositorio.GetInstance();

        public static List<AviCategoria> ListarOrdenadamente() => contexto.AviCategoria.OrderBy(c => c.Descricao).ToList();

        public static void Inserir(AviCategoria categoria)
        {
            contexto.AviCategoria.Add(categoria);
            contexto.SaveChanges();
        }
    }
}