using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SIAC.Web.Models
{
    public partial class Parametro
    {
        private static dbSIACEntities contexto { get { return DataContextSIAC.GetInstance(); } }

        private static Parametro parametro;

        private Parametro() { }

        public static Parametro Obter()
        {
            if (parametro == null)
            {
                parametro = contexto.Parametro.FirstOrDefault();
            }
            return parametro;
        }

        public static void Atualizar(Parametro parametro)
        {
            Parametro temp = Parametro.Obter();

            temp.TempoInatividade = parametro.TempoInatividade;
            temp.NumeracaoQuestao = parametro.NumeracaoQuestao;
            temp.NumeracaoAlternativa = parametro.NumeracaoAlternativa;
            temp.QteSemestres = parametro.QteSemestres;
            temp.TermoResponsabilidade = parametro.TermoResponsabilidade.Trim();
            temp.NotaUso = parametro.NotaUso.Trim();

            contexto.SaveChanges();
        }

        public async static Task<Parametro> ObterAsync()
        {
            return await contexto.Parametro.FindAsync(1);
        }
    }
}