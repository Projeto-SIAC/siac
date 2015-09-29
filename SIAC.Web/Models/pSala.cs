using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Web.Models
{
    public partial class Sala
    {
        private static dbSIACEntities contexto = DataContextSIAC.GetInstance();

        public static List<Sala> ListarOrdenadamente()
        {
            return contexto.Sala.OrderBy(s => s.Descricao).ToList();
        }

        public static void Inserir(Sala sala)
        {
            contexto.Sala.Add(sala);
            contexto.SaveChanges();
        }
    }
}