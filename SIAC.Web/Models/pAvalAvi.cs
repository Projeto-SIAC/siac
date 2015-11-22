using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Models
{
    public partial class AvalAvi
    {
        private static dbSIACEntities contexto { get { return Repositorio.GetInstance(); } }

        public static void Inserir(AvalAvi avi)
        {
            contexto.AvalAvi.Add(avi);
            contexto.SaveChanges();
        }
    }
}