using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Web
{
    public static class StringExtensao
    {
        public static string ToShortString(this string str, int length)
        {
            string text = string.Empty;

            if (str.Length > length) { 
                text = str.Substring(0, length);
                string afterText = str.Substring(length);

                afterText = afterText.Remove(afterText.IndexOf(' '));

                text += afterText;

                text += "...";
            }
            else
            {
                text = str;
            }

            return text;
        }
    }
}