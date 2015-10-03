using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Web.Models
{
    public partial class QuestaoTema
    {
        private static dbSIACEntities contexto = DataContextSIAC.GetInstance();

        private static int ParamTempoInatividade = Parametro.Obter().TempoInatividade;

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
                        if (PrazoValido(item))
                        {
                            ret.Add(item);
                        }
                    }
                }
            }

            return ret;
        }

        public static bool PrazoValido(QuestaoTema questao)
        {
            Questao q = Questao.ListarPorCodigo(questao.CodQuestao);
            if (q.DtUltimoUso.HasValue)
            {
                DateTime prazo = questao.Questao.DtUltimoUso.Value.AddDays(ParamTempoInatividade);
                if (DateTime.Now >= prazo)
                    return true;
                else
                    return false;
            }
            else
            {
                return true;
            }
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