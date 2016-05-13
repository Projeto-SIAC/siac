using SIAC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC
{
    public static class Extensoes
    {
        #region byte[]
        public static string GetString(this byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }
        public static string GetImageSource(this byte[] bytes)
        {
            string src = "data:image/png;base64,";
            src += Convert.ToBase64String(bytes, 0, bytes.Length);
            return src;
        }
        #endregion

        // String
        #region String
        public static byte[] GetBytes(this string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static string RemoveSpaces(this string aText)
        {
            aText = aText.Replace("\t", " ");
            aText = aText.Replace("\n", " ");
            aText = aText.Replace("\r", " ");
            string result = aText.Trim();
            while (result.IndexOf("  ") > -1)
                result = result.Replace("  ", " ");
            return result;
        }

        public static string ToShortString(this string str, int length)
        {
            string text = string.Empty;
            if (str.Length > length)
            {
                text = str.Substring(0, length);
                string afterText = str.Substring(length);

                if (afterText.IndexOf(' ') > -1)
                {
                    afterText = afterText.Remove(afterText.IndexOf(' '));
                    afterText += "...";
                }
                else
                {
                    afterText = "...";
                }
                text += afterText;
            }
            else
            {
                text = str;
            }

            return text;
        }

        public static string ToHtml(this string str, params string[] tags)
        {
            string html = String.Empty;
            string[] lines = str.Split('\n', '\r');
            foreach (string line in lines)
            {
                if (!String.IsNullOrWhiteSpace(line))
                {
                    tags = tags.Reverse().ToArray();
                    string text = line;
                    foreach (string tag in tags)
                        text = $"<{tag}>{text}</{tag}>";
                    html += text;
                }
            }
            return html;
        }

        public static string ReplaceChars(this string str, string oldChars, string newChars)
        {
            string newStr = str;
            for (int i = 0, length = oldChars.Length; i < length; i++)
                newStr = newStr.Replace(oldChars[i], newChars[i]);
            return newStr;
        }

        public static string Sumarizada(this string descricao, params int[] indices)
        {
            string indice = "";
            string caracterSeparador = ".";

            foreach (int i in indices)
            {
                indice += i + caracterSeparador;
            }

            return indice + " " + descricao;
        }

        public static string Between(this string str, int indexA, int indexB)
        {
            string r = string.Empty;
            for (int i = indexA; i < indexB; i++)
            {
                r += str[i];
            }
            return r;
        }

        public static bool IsNumber(this string str)
        {
            foreach (var c in str)
            {
                if (!char.IsDigit(c) && c != '.' && c != ',' && c != '-')
                {
                    return false;
                }
            }
            return true;
        }

        #endregion

        // Int
        #region Int
        public static string GetIndiceAlternativa(this int i)
        {
            i++;
            int tipo = Parametro.Obter().NumeracaoAlternativa;
            switch (tipo)
            {
                case 1:
                    return i.ToString();
                case 2:
                    return paraRomano(i);
                case 3:
                    return paraCaixaBaixa(i);
                case 4:
                    return paraCaixaAlta(i);
                default:
                    return paraCaixaBaixa(i);
            }
        }

        public static string GetIndiceQuestao(this int i)
        {
            i++;
            int tipo = Parametro.Obter().NumeracaoQuestao;

            switch (tipo)
            {
                case 1:
                    return i.ToString();
                case 2:
                    return paraRomano(i);
                case 3:
                    return paraCaixaBaixa(i);
                case 4:
                    return paraCaixaAlta(i);
                default:
                    return paraCaixaBaixa(i);
            }
        }

        public static string paraRomano(int number)
        {
            if ((number < 0) || (number > 3999)) return number.ToString();
            if (number < 1) return string.Empty;
            if (number >= 1000) return "M" + paraRomano(number - 1000);
            if (number >= 900) return "CM" + paraRomano(number - 900);
            if (number >= 500) return "D" + paraRomano(number - 500);
            if (number >= 400) return "CD" + paraRomano(number - 400);
            if (number >= 100) return "C" + paraRomano(number - 100);
            if (number >= 90) return "XC" + paraRomano(number - 90);
            if (number >= 50) return "L" + paraRomano(number - 50);
            if (number >= 40) return "XL" + paraRomano(number - 40);
            if (number >= 10) return "X" + paraRomano(number - 10);
            if (number >= 9) return "IX" + paraRomano(number - 9);
            if (number >= 5) return "V" + paraRomano(number - 5);
            if (number >= 4) return "IV" + paraRomano(number - 4);
            if (number >= 1) return "I" + paraRomano(number - 1);

            return number.ToString();
        }

        public static string paraCaixaBaixa(int number)
        {
            number--;
            const string letters = "abcdefghijklmnopqrstuvwxyz";

            string value = "";

            if (number >= letters.Length)
                value += letters[number / letters.Length - 1];

            value += letters[number % letters.Length];

            return value;
        }

        public static string paraCaixaAlta(int number) => paraCaixaBaixa(number).ToUpper();

        public static bool ContainsOne(this int[] i, int[] j)
        {
            foreach (int k in j)
                if (i.Contains(k))
                    return true;
            return false;
        }

        public static string ParaRepresentarDuracao(this int minutos)
        {
            return TimeSpan.FromMinutes(Convert.ToDouble(minutos)).ToString(@"hh'h'mm'min'");
        }
        #endregion

        // DateTime
        #region DateTime
        public static string ToBrazilianString(this DateTime dateTime)
        {
            return dateTime.ToString("dddd, dd 'de' MMMM 'de' yyyy 'às' HH'h'mm", new System.Globalization.CultureInfo("pt-BR"));
        }

        public static bool IsFuture(this DateTime dateTime)
        {
            return dateTime > DateTime.Now;
        }

        public static string ToElapsedTimeString(this DateTime dt)
        {
            string s = String.Empty;
            double segundos = (DateTime.Now - dt).TotalSeconds;
            if (segundos < 1)
            {
                s = "pouco";
            }
            else if (segundos < 60)
            {
                double q = Math.Round(segundos);
                s = q > 1 ? q + " segundos" : q + " segundo";
            }
            else if (segundos < 3600)
            {
                double q = Math.Round(segundos / 60);
                s = q > 1 ? q + " minutos" : q + " minuto";
            }
            else if (segundos < 86400)
            {
                double q = Math.Round((segundos / 60) / 60);
                s = q > 1 ? q + " horas" : q + " hora";
            }
            else if (segundos < (86400 * 15))
            {
                double q = Math.Round(((segundos / 60) / 60) / 24);
                s = q > 1 ? q + " dias" : q + " dia";
            }
            else
            {
                s = dt.ToShortDateString();
            }
            return s;
        }

        public static string ToLeftTimeString(this DateTime dt)
        {
            string s = String.Empty;
            double segundos = (dt - DateTime.Now).TotalSeconds;
            if (segundos < 1)
            {
                s = "Agora";
            }
            else if (segundos < 60)
            {
                double q = Math.Round(segundos);
                s = q > 1 ? q + " segundos" : q + " segundo";
            }
            else if (segundos < 3600)
            {
                double q = Math.Round(segundos / 60);
                s = q > 1 ? q + " minutos" : q + " minuto";
            }
            else if (segundos < 86400)
            {
                double q = Math.Round((segundos / 60) / 60);
                s = q > 1 ? q + " horas" : q + " hora";
            }
            else
            {
                double q = Math.Round(((segundos / 60) / 60) / 24);
                s = q > 1 ? q + " dias" : q + " dia";
            }
            return s;
        }

        public static string ToJSString(this DateTime dt)
        {
            return dt.ToString("yyyy','MM','dd','HH','mm");
        }

        public static string ToHtmlDateTimeString(this DateTime dt)
        {
            return dt.ToString("yyyy'-'MM'-'dd' 'HH'-'mm");
        }

        public static int SemestreAtual(this DateTime dt) => dt.Month > 6 ? 2 : 1;

        public static string DiaRealizacaoRepresentacao(this DateTime dt)
        {
            return dt.ToString("dd/MM/yyyy' às 'HH'h'mm");
        }
        #endregion

        // List<AvaliacaoTema>
        #region List<AvaliacaoTema>
        public static int QteQuestoesPorTipo(this List<AvaliacaoTema> lstAvaliacaoTema, int codDisciplina, int codTipoQuestao)
        {
            int qteQuestoes = 0;

            foreach (AvaliacaoTema avaliacaoTema in lstAvaliacaoTema.Where(a => a.Tema.CodDisciplina == codDisciplina))
            {
                List<AvalTemaQuestao> lstAvalTemaQuestao = avaliacaoTema.AvalTemaQuestao.ToList();
                List<AvalTemaQuestao> lstAvalTemaQuestaoFiltrada = lstAvalTemaQuestao.Where(a => a.QuestaoTema.Questao.CodTipoQuestao == codTipoQuestao).ToList();
                qteQuestoes += lstAvalTemaQuestaoFiltrada.Count;
            }

            return qteQuestoes;
        }

        public static int QteQuestoesPorTipo(this List<AvaliacaoTema> lstAvaliacaoTema, int codTipoQuestao)
        {
            int qteQuestoes = 0;

            foreach (AvaliacaoTema avaliacaoTema in lstAvaliacaoTema)
            {
                List<AvalTemaQuestao> lstAvalTemaQuestao = avaliacaoTema.AvalTemaQuestao.ToList();
                List<AvalTemaQuestao> lstAvalTemaQuestaoFiltrada = lstAvalTemaQuestao.Where(a => a.QuestaoTema.Questao.CodTipoQuestao == codTipoQuestao).ToList();
                qteQuestoes += lstAvalTemaQuestaoFiltrada.Count;
            }

            return qteQuestoes;
        }

        public static string MaxDificuldade(this List<AvaliacaoTema> lstAvaliacaoTema, int codDisciplina)
        {
            Dificuldade dificuldade = new Dificuldade();

            foreach (AvaliacaoTema avaliacaoTema in lstAvaliacaoTema.Where(a => a.Tema.CodDisciplina == codDisciplina))
            {
                List<Dificuldade> lstDificuldade = avaliacaoTema.AvalTemaQuestao.Select(a => a.QuestaoTema.Questao.Dificuldade).ToList();
                if (lstDificuldade.Count > 0 && lstDificuldade.Max(a => a.CodDificuldade) > dificuldade.CodDificuldade)
                    dificuldade = lstDificuldade.First(a => a.CodDificuldade == lstDificuldade.Max(d => d.CodDificuldade));
            }

            return dificuldade.Descricao;
        }
        #endregion

        // Avaliacao
        #region Avaliacao
        public static int QteQuestoes(this Avaliacao avaliacao)
        {
            int qte = 0;
            foreach (AvaliacaoTema avalTema in avaliacao.AvaliacaoTema)
                qte += avalTema.AvalTemaQuestao.Count;
            return qte;
        }

        public static List<Questao> EmbaralharQuestao(this Avaliacao avaliacao)
        {
            List<Questao> lstQuestao = new List<Questao>();
            List<Questao> lstQuestaoEmbalharada = new List<Questao>();
            Random r = new Random();
            foreach (var avalTema in avaliacao.AvaliacaoTema)
                lstQuestao.AddRange(avalTema.AvalTemaQuestao.Select(a => a.QuestaoTema.Questao).ToList());

            while (lstQuestao.Count > 0)
            {
                int i = r.Next(lstQuestao.Count);
                Questao qst = lstQuestao.ElementAt(i);
                lstQuestaoEmbalharada.Add(qst);
                lstQuestao.Remove(qst);
            }

            return lstQuestaoEmbalharada.OrderBy(q => q.CodTipoQuestao).ToList();
        }
        #endregion

        // Questao
        #region Questao
        public static bool TemUmaCorreta(this Questao questao)
        {
            bool unica = false;

            foreach (var alt in questao.Alternativa)
            {
                if (alt.FlagGabarito)
                {
                    if (unica == true)
                    {
                        unica = false;
                        break;
                    }
                    unica = true;
                }
            }

            return unica;
        }

        public static List<Alternativa> EmbaralharAlternativa(this Questao questao)
        {
            List<Alternativa> lstAlternativa = questao.Alternativa.ToList();
            List<Alternativa> lstAlternativaEmbaralhada = new List<Alternativa>();
            Random r = new Random();

            while (lstAlternativaEmbaralhada.Count != questao.Alternativa.Count)
            {
                int i = r.Next(lstAlternativa.Count);
                Alternativa alt = lstAlternativa.ElementAt(i);
                lstAlternativaEmbaralhada.Add(alt);
                lstAlternativa.Remove(alt);
            }

            return lstAlternativaEmbaralhada;
        }

        public static string ToJsonChart(this Questao questao, List<AvalQuesPessoaResposta> lstResposta)
        {
            Random random = new Random();
            string json = string.Empty;
            json += "[";

            for (int i = 0, length = questao.Alternativa.Count; i < length; i++)
            {
                string rgba = Helpers.CorDinamica.Rgba(random);

                json += "{";

                json += $"\"value\":\"{lstResposta.Where(r => r.RespAlternativa == i).Count()}\"";

                json += ",";

                json += $"\"label\":\"Alternativa {i.GetIndiceAlternativa()}\"";

                json += ",";

                json += $"\"color\":\"{rgba}\"";

                json += ",";

                json += $"\"highlight\":\"{rgba.Replace("1)", "0.8)")}\"";

                json += "}";

                if (i != length - 1)
                {
                    json += ",";
                }
            }

            json += "]";

            return json;
        }

        public static string ToJsonChart(this AviQuestao questao, List<AviQuestaoPessoaResposta> respostas = null)
        {
            respostas = questao.Respostas;
            Random random = new Random();
            string json = string.Empty;
            json += "[";

            for (int i = 1, length = questao.AviQuestaoAlternativa.Count; i <= length; i++)
            {
                string rgba = Helpers.CorDinamica.Rgba(random);

                json += "{";

                json += $"\"value\":\"{respostas.Where(r => r.RespAlternativa == i).Count()}\"";

                json += ",";

                json += $"\"label\":\"Alternativa {(i-1).GetIndiceAlternativa()}\"";

                json += ",";

                json += $"\"color\":\"{rgba}\"";

                json += ",";

                json += $"\"highlight\":\"{rgba.Replace("1)", "0.8)")}\"";

                json += "}";

                if (i != length)
                {
                    json += ",";
                }
            }

            json += "]";

            return json;
        }


        #endregion

        //Double
        #region Double
        public static string ToValueHtml(this double value) => value.ToString().Replace(',', '.');
        #endregion

        public static string RecuperarIp(this HttpContext contexto)
        {
            string ip = contexto.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (!string.IsNullOrEmpty(ip))
            {
                string[] enderecos = ip.Split(',');
                if (enderecos.Length > 0)
                    return enderecos[0];
            }
            return contexto.Request.ServerVariables["REMOTE_ADDR"];
        }
    }
}