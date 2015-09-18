using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Web.Models
{
    public partial class Parametro
    {
        private static dbSIACEntities contexto = DataContextSIAC.GetInstance();

        public static Parametro Obter()
        {
            return contexto.Parametro.FirstOrDefault();
        }

        public static void Atualizar(Parametro parametro)
        {
            Parametro temp = Parametro.Obter();

            temp.TempoInatividade = parametro.TempoInatividade;
            temp.NumeracaoQuestao = parametro.NumeracaoQuestao;
            temp.NumeracaoAlternativa = parametro.NumeracaoAlternativa;
            temp.QteSemestres = parametro.QteSemestres;

            contexto.SaveChanges();
        }
    }
}