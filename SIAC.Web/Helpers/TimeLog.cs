using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Web.Helpers
{
    public class TimeLog
    {
        private static DateTime tempo;

        private static string teste;

        public static void Iniciar(string strTeste = "")
        {
            teste = strTeste;
            tempo = DateTime.Now;
        }

        public static void Parar()
        {
            var total = (DateTime.Now - tempo).TotalSeconds;
            Debug.WriteLine("TimeLog: {0} segundos para realizar {1} [{2}]", total.ToString(), teste, tempo.ToString());
        }
    }
}