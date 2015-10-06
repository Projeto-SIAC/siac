using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Web.Models
{
    public partial class Avaliacao
    {
        public string CodAvaliacao
        {
            get
            {
                return String.Format("{0}{1}{2}{3}", TipoAvaliacao.Sigla.ToUpper(), Ano, Semestre, NumIdentificador.ToString("0000"));
            }
        }

        public List<Questao> Questao
        {
            get
            {
                List<Questao> lstQuestao = new List<Questao>();
                foreach (var avaliacaoTema in AvaliacaoTema)
                {
                    foreach (var avalTemaQuestao in avaliacaoTema.AvalTemaQuestao)
                    {
                        lstQuestao.Add(avalTemaQuestao.QuestaoTema.Questao);
                    }
                };
                return lstQuestao;
            }
        }

        public List<AvalQuesPessoaResposta> PessoaResposta
        {
            get
            {
                List<AvalQuesPessoaResposta> lstPessoaResposta = new List<AvalQuesPessoaResposta>();
                foreach (var avaliacaoTema in AvaliacaoTema)
                {
                    foreach (var avalTemaQuestao in avaliacaoTema.AvalTemaQuestao)
                    {
                        lstPessoaResposta.AddRange(avalTemaQuestao.AvalQuesPessoaResposta);
                    }
                };
                return lstPessoaResposta;
            }
        }

        private static dbSIACEntities contexto = DataContextSIAC.GetInstance();

        public static int ObterNumIdentificador(int codTipoAvaliacao)
        {
            int ano = DateTime.Now.Year;
            int semestre = (DateTime.Now.Month > 6) ? 2 : 1;

            Avaliacao avalTemp = contexto.Avaliacao.Where(a => a.Ano == ano && a.Semestre == semestre && a.CodTipoAvaliacao == codTipoAvaliacao).OrderByDescending(a => a.NumIdentificador).FirstOrDefault();

            return (avalTemp != null) ? avalTemp.NumIdentificador + 1 : 1;
        }
    }
}