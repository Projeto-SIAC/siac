using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Web.Models
{ 
    public class DataContextSIAC
    {
        private static DataClassesSIACDataContext contexto;
        
        private DataContextSIAC() {}

        public static DataClassesSIACDataContext GetInstance()
        {
            if (contexto == null)
            {
                contexto = new DataClassesSIACDataContext();
            }   
            return contexto;
        }

        public static int Iniciar()
        {
            contexto = GetInstance();
            var l = contexto.Usuarios.ToList();
            if (l != null)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}