using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Web.Models
{
    public partial class QuestaoTema
    {
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
    }
}