using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace SIAC.Helpers
{
    public class Formate
    {
        public static string ParaCPF(string valor)
        {
            if (valor.Length == 11)
            {
                return $"{valor.Between(0, 3)}.{valor.Between(3, 6)}.{valor.Between(6, 9)}-{valor.Between(9, 11)}";
            }
            return string.Empty;
        }

        public static string DeCPF(string cpf)
        {
            Regex rgx = new Regex(@"\D");
            return rgx.Replace(cpf, "");
        }
    }
}