/*
This file is part of SIAC.

Copyright (C) 2016 Felipe Mateus Freire Pontes <felipemfpontes@gmail.com>
Copyright (C) 2016 Francisco Bento da Silva Júnior <francisco.bento.jr@hotmail.com>

SIAC is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details. 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace SIAC.Helpers
{
    public class LeroLero : IDisposable
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
            try
            {
                string html = wc.DownloadString("http://www.lerolero.com/");
                p = TextBetween(html, "id=\"frase_aqui\">", "</blockquote>");
                p = p.Trim();
            }
            catch (Exception ex)
            {
                p = ex.Message;
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

        public void Dispose()
        {
            this.wc.Dispose();
        }
    }
}