using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Web.Models
{
    public partial class AvalAcademica
    {
        private static dbSIACEntities contexto = DataContextSIAC.GetInstance();
        
        public static void Inserir(AvalAcademica avalAcademica)
        {
            contexto.AvalAcademica.Add(avalAcademica);
            contexto.SaveChanges();
        }
    }
}