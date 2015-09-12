using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Web.Helpers
{
    public class TimeLog
    {
        private static DateTime tempo;

        public static void Iniciar()
        {
            tempo = DateTime.Now;
        }

        public static double Parar()
        {
            return (DateTime.Now - tempo).TotalSeconds;
        }
    }
}