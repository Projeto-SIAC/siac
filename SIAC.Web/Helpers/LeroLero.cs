using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace SIAC.Web.Helpers
{
    public class LeroLero
    {
        private WebClient wc;

        public LeroLero()
        {
            wc = new WebClient();
            wc.Encoding = System.Text.Encoding.UTF8;
        }
        
        public string Paragrafo()
        {
            string p = String.Empty;
            try {
                        string html = wc.DownloadString("http://www.lerolero.com/");
                        p = TextBetween(html, "id=\"frase_aqui\">", "</blockquote>");
                        p = p.Trim();
            } 
            catch (Exception ex) {
                        p=ex.Message;             
            }
            return p;
        }


        // StringUtils

        private static string RemainingText;

        public static string TextBetween(string wholeText, string beforeText, string afterText)
        {
            int foundPos;
            string workText;
            string result;

            RemainingText = wholeText;
            result = String.Empty;

            foundPos = wholeText.IndexOf(beforeText);
            if (foundPos < 0)
            {
                return result;
            }

            workText = wholeText.Substring(foundPos + beforeText.Length);

            foundPos = workText.IndexOf(afterText);
            if (foundPos < 0)
            {
                return result;
            }

            result = workText.Substring(0, foundPos);

            RemainingText = workText.Substring(foundPos + afterText.Length);

            return result;
        }
    }
}