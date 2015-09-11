using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SIAC.Web.Models;

namespace SIAC.Web.Helpers
{ 
    public class DataContextSIAC
    {
        private static DataClassesSIACDataContext contexto;

        public static DataClassesSIACDataContext GetInstance()
        {
            if (contexto == null)
            {
                contexto = new DataClassesSIACDataContext();
            }   
            return contexto;
        }
    }
}