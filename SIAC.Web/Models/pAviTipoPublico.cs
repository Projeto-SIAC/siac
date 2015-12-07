using System.Collections.Generic;
using System.Linq;

namespace SIAC.Models
{
    public partial class AviTipoPublico
    {
        private static dbSIACEntities contexto => Repositorio.GetInstance();

        public static List<AviTipoPublico> ListarOrdenadamente() => contexto.AviTipoPublico.OrderBy(p => p.CodAviTipoPublico).ToList();
    }
}