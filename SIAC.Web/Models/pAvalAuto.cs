using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Web.Models
{
    public partial class AvalAuto
    {
        private static dbSIACEntities contexto = DataContextSIAC.GetInstance();

        public static void Inserir(AvalAuto AvalAuto)
        {
            contexto.AvalAuto.Add(AvalAuto);
            contexto.SaveChanges();
        }
    }
}