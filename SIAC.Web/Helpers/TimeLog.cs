using System;
using System.Diagnostics;

namespace SIAC.Helpers
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
            Debug.WriteLine($"TimeLog: {total.ToString()} segundos para realizar {teste} [{tempo.ToString()}]");
        }
    }
}