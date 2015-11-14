using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Models
{
    public partial class AviCategoria
    {
        private static dbSIACEntities contexto { get { return Repositorio.GetInstance(); } }

        public static List<AviCategoria> ListarOrdenadamente()
        {
            return contexto.AviCategoria.OrderBy(c => c.Descricao).ToList();
        }

        public static void Inserir(AviCategoria categoria)
        {
            contexto.AviCategoria.Add(categoria);
            contexto.SaveChanges();
        }
    }
}