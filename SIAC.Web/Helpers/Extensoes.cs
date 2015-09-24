using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SIAC.Web.Models;

namespace SIAC.Web
{
    public static class Extensoes
    {
        // String
        public static string RemoveSpaces(this string aText)
        {
            aText = aText.Replace("\t", " ");
            aText = aText.Replace("\n", " ");
            aText = aText.Replace("\r", " ");
            string result = aText.Trim();
            while (result.IndexOf("  ") > 0)
            {
                result = result.Replace("  ", " ");
            }
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

                text += afterText;

            }
            else
            {
                text = str;
            }

            return text;
        }


        // Int
        public static string GetIndiceAlternativa(this int i)
        {
            i++;
            int tipo = DataContextSIAC.GetInstance().Parametro.First().NumeracaoAlternativa;

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
            int tipo = DataContextSIAC.GetInstance().Parametro.First().NumeracaoQuestao;

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
            const string letters = " abcdefghijklmnopqrstuvwxyz";

            string value = "";

            if (number >= letters.Length)
                value += letters[number / letters.Length - 1];

            value += letters[number % letters.Length];

            return value;
        }

        public static string paraCaixaAlta(int number)
        {
            return paraCaixaBaixa(number).ToUpper();
        }


        // DateTime
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


        // List<AvaliacaoTema>
        public static int QteQuestoesPorTipo(this List<AvaliacaoTema> lstAvaliacaoTema, int codDisciplina, int codTipoQuestao)
        {
            int qteQuestoes = 0;

            foreach (var avaliacaoTema in lstAvaliacaoTema.Where(a=>a.Tema.CodDisciplina == codDisciplina))
            {
                var lstAvalTemaQuestao = avaliacaoTema.AvalTemaQuestao.ToList();
                var lstAvalTemaQuestaoFiltrada = lstAvalTemaQuestao.Where(a => a.QuestaoTema.Questao.CodTipoQuestao == codTipoQuestao).ToList();
                qteQuestoes += lstAvalTemaQuestaoFiltrada.Count;
            }

            return qteQuestoes;
        }

        public static string MaxDificuldade(this List<AvaliacaoTema> lstAvaliacaoTema, int codDisciplina)
        {
            Dificuldade dificuldade = new Dificuldade();

            foreach (var avaliacaoTema in lstAvaliacaoTema.Where(a => a.Tema.CodDisciplina == codDisciplina))
            {
                var lstDificuldade = avaliacaoTema.AvalTemaQuestao.Select(a=>a.QuestaoTema.Questao.Dificuldade).ToList();
                if (lstDificuldade.Count > 0 && lstDificuldade.Max(a=>a.CodDificuldade) > dificuldade.CodDificuldade)
                {
                    dificuldade = lstDificuldade.First(a=>a.CodDificuldade == lstDificuldade.Max(d => d.CodDificuldade));
                }
            }

            return dificuldade.Descricao;
        }


        // Avaliacao
        public static string CodAvaliacao(this Avaliacao avaliacao)
        {
            string codAvalicao = String.Empty;

            codAvalicao += avaliacao.TipoAvaliacao.Sigla;
            codAvalicao += avaliacao.Ano;
            codAvalicao += avaliacao.Semestre;
            codAvalicao += avaliacao.NumIdentificador.ToString("0000");

            return codAvalicao;
        }

        public static int QteQuestoes(this Avaliacao avaliacao)
        {
            int qte = 0;

            foreach (var avalTema in avaliacao.AvaliacaoTema)
            {
                qte += avalTema.AvalTemaQuestao.Count;
            }

            return qte;
        }

        public static List<Questao> EmbaralharQuestao(this Avaliacao avaliacao)
        {
            List<Questao> lstQuestao = new List<Models.Questao>();
            List<Questao> lstQuestaoEmbalharada = new List<Questao>();
            Random r = new Random();

            foreach (var avalTema in avaliacao.AvaliacaoTema)
            {
                lstQuestao.AddRange(avalTema.AvalTemaQuestao.Select(a => a.QuestaoTema.Questao).ToList());
            }

            while (lstQuestao.Count > 0)
            {
                int i = r.Next(lstQuestao.Count);
                Questao qst = lstQuestao.ElementAt(i);
                lstQuestaoEmbalharada.Add(qst);
                lstQuestao.Remove(qst);
            }

            return lstQuestaoEmbalharada;
        }

        // Questao
        public static bool TemUmaCorreta(this Questao questao)
        {
            bool unica = false;

            foreach (var alt in questao.Alternativa)
            {
                if (alt.FlagGabarito.HasValue && alt.FlagGabarito.Value)
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
    }
}