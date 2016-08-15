using System;
using System.Collections.Generic;
using System.Linq;

namespace SIAC.Models
{
    public partial class Turno
    {
        public const string MATUTINO = "M";
        public const string VESPERTINO = "V";
        public const string NOTURNO = "N";


        private static Contexto contexto => Repositorio.GetInstance();

        public static List<Turno> ListarOrdenadamente() => contexto.Turno.ToList();

        public static string ObterCodTurnoPorData(DateTime data)
        {
            int hora = data.Hour;
            
            if (hora < 12)
            {
                return Turno.MATUTINO;
            }
            else if (hora < 18)
            {
                return Turno.VESPERTINO;
            }
            else
            {
                return Turno.NOTURNO;
            }
        }
    }
}