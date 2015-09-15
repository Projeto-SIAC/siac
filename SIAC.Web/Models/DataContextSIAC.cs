using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Web.Models
{ 
    public class DataContextSIAC
    {
        private static dbSIACEntities contexto;
        
        private DataContextSIAC() {}

        public static dbSIACEntities GetInstance()
        {
            if (contexto == null)
            {
                contexto = new dbSIACEntities();
            }   
            return contexto;
        }
    }
}