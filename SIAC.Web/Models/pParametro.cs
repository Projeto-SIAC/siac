using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            temp.TermoResponsabilidade = parametro.TermoResponsabilidade.Trim();

            contexto.SaveChanges();
        }

        public async static Task<Parametro> ObterAsync()
        {
            return await contexto.Parametro.FindAsync(1);
        }
    }
}