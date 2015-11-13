using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Models
{
    public partial class AviModulo
    {
        private static dbSIACEntities contexto { get { return Repositorio.GetInstance(); } }

        public static List<AviModulo> ListarOrdenadamente()
        {
            return contexto.AviModulo.OrderBy(m => m.Descricao).ToList();
        }

        public static void Inserir(AviModulo modulo)
        {
            contexto.AviModulo.Add(modulo);
            contexto.SaveChanges();
        }
    }
}