using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Web
{
    public static class DateTimeExtensao
    {
        public static string ToBrazilianString(this DateTime dateTime)
        {
            return dateTime.ToString("dddd, dd 'de' MMMM 'de' yyyy 'às' HH'h'mm", new System.Globalization.CultureInfo("pt-BR"));
        }

        public static bool IsFuture(this DateTime dateTime)
        {
            return dateTime > DateTime.Now ? true : false;
        }

        public static string ToElapsedTimeString(this DateTime dt)
        {
            string s = String.Empty;
            double segundos = (DateTime.Now - dt).TotalSeconds;
            if (segundos < 1)
            {
                s = "Agora a pouco";
            }
            else if (segundos < 60)
            {
                double q = Math.Round(segundos);
                s = q > 1 ? q + " segundos atrás" : q + " segundo atrás";
            }
            else if (segundos < 3600)
            {
                double q = Math.Round(segundos / 60);
                s = q > 1 ? q + " minutos atrás" : q + " minuto atrás";
            }
            else if (segundos < 86400)
            {
                double q = Math.Round((segundos / 60) / 60);
                s = q > 1 ? q + " horas atrás" : q + " hora atrás";
            }
            else if (segundos < (86400 * 15))
            {
                double q = Math.Round(((segundos / 60) / 60) / 24);
                s = q > 1 ? q + " dias atrás" : q + " dia atrás";
            }
            else
            {
                s = dt.ToBrazilianString();
            }
            return s;
        }
    }
}