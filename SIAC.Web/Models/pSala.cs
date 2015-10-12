using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Web.Models
{
    public partial class Sala
    {
        private static dbSIACEntities contexto { get { return Repositorio.GetInstance(); } }

        public static List<Sala> ListarOrdenadamente()
        {
            return contexto.Sala.OrderBy(s => s.Descricao).ToList();
        }
        
        public static Sala ListarPorCodigo(int codSala)
        {
            return contexto.Sala.SingleOrDefault(s => s.CodSala == codSala);
        }

        public static void Inserir(Sala sala)
        {
            contexto.Sala.Add(sala);
            contexto.SaveChanges();
        }
    }
}