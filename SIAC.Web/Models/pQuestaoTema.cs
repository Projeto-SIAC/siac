using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Web.Models
{
    public partial class QuestaoTema
    {
        private static dbSIACEntities contexto = DataContextSIAC.GetInstance();

        public static List<QuestaoTema> LimparRepeticao(List<QuestaoTema> retorno, List<QuestaoTema> lst1, List<QuestaoTema> lst2)
        {
            List<int> codigos1 = (from qt in lst1 select qt.CodQuestao).ToList();
            List<int> codigos2 = (from qt in lst2 select qt.CodQuestao).ToList();
            List<QuestaoTema> ret = new List<QuestaoTema>();


            foreach (QuestaoTema item in retorno)
            {
                if (!codigos1.Contains(item.CodQuestao))
                {
                    if (!codigos2.Contains(item.CodQuestao))
                    {
                        ret.Add(item);
                    }
                }
            }

            return ret;
        }

        public static List<QuestaoTema> LimparPorData(List<QuestaoTema> questoes)
        {
            List<QuestaoTema> ret = new List<QuestaoTema>();

            foreach (QuestaoTema item in questoes)
            {
                if(item.Questao.DtUltimoUso != null)
                {
                    DateTime prazo = DateTime.Parse(item.Questao.DtUltimoUso.ToString()).AddDays(Parametro.Obter().TempoInatividade);
                    if(DateTime.Now >= prazo)
                    {
                        ret.Add(item);
                    }
                }
                else
                {
                    ret.Add(item);
                }
            }
            return ret;
        }

        public static void AtualizarDtUltimoUso(List<QuestaoTema> questoes)
        {
            foreach (QuestaoTema item in questoes)
            {
                Questao q = item.Questao;
                q.DtUltimoUso = DateTime.Now;
            }
            contexto.SaveChanges();
        }
    }
}