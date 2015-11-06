using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Models
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

        public List<Tema> Temas
        {
            get
            {
                List<Tema> temas = (from t in AvaliacaoTema
                                    select t.Tema).Distinct().ToList();
                return temas;
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

        public List<QuestaoTema> QuestaoTema
        {
            get
            {
                List<QuestaoTema> QuestoesTema = new List<QuestaoTema>();
                foreach (AvaliacaoTema item in this.AvaliacaoTema)
                {
                    foreach (AvalTemaQuestao qt in item.AvalTemaQuestao)
                    {
                        QuestoesTema.Add(qt.QuestaoTema);
                    }
                }
                return QuestoesTema;
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

        public int CodDificuldade
        {
            get
            {
                return Questao.Max(q => q.CodDificuldade);
            }
        }

        public int TipoQuestoes
        {
            get
            {
                int qteObj = Questao.Count(q => q.CodTipoQuestao == 1);
                int qteDis = Questao.Count(q => q.CodTipoQuestao == 2);

                if (qteObj > 0 && qteDis > 0) return 3;
                else if (qteObj > 0) return 1;
                else if (qteDis > 0) return 2;
                else return -1;
            }
        }

        public bool FlagPendente
        {
            get
            {
                return this.AvalPessoaResultado.Count > 0 || this.FlagArquivo ? false : true;
            }
        }

        public bool FlagRealizada
        {
            get
            {
                return this.AvalPessoaResultado.Count > 0;
            }
        }

        public bool FlagAgendada
        {
            get
            {
                return this.DtAplicacao.HasValue && this.FlagPendente;
            }
        }

        public bool FlagAgora
        {
            get
            {
                var total = (DateTime.Now - DtAplicacao.Value).TotalMinutes;
                return total < (Duracao / 2) && total > 0;
            }
        }

        public bool FlagVencida
        {
            get
            {
                return (DateTime.Now - DtAplicacao.Value).TotalMinutes > (Duracao / 2);
            }
        }

        private static dbSIACEntities contexto { get { return Repositorio.GetInstance(); } }

        public static int ObterNumIdentificador(int codTipoAvaliacao)
        {
            if (Sistema.NumIdentificador.Count == 0)
            {
                for (int i = 1; i <= contexto.TipoAvaliacao.Max(t=>t.CodTipoAvaliacao); i++)
                {
                    int ano = DateTime.Now.Year;
                    int semestre = (DateTime.Now.Month > 6) ? 2 : 1;
                    Avaliacao avalTemp = contexto.Avaliacao
                        .Where(a => a.Ano == ano 
                            && a.Semestre == semestre 
                            && a.CodTipoAvaliacao == i)
                        .OrderByDescending(a => a.NumIdentificador)
                        .FirstOrDefault();
                    if (avalTemp != null)
                    {
                        Sistema.NumIdentificador.Add(i, avalTemp.NumIdentificador + 1);
                    } 
                    else
                    {
                        Sistema.NumIdentificador.Add(i, 1);
                    }
                }
            }
            return Sistema.NumIdentificador[codTipoAvaliacao]++;
        }

        public void TrocarQuestao(int codQuestaoAntiga,QuestaoTema novoQuestaoTema)
        {
            AvalTemaQuestao aqtAntiga = (from atq in contexto.AvalTemaQuestao
                                    where atq.Ano == Ano
                                    && atq.Semestre == Semestre
                                    && atq.CodTipoAvaliacao == CodTipoAvaliacao
                                    && atq.NumIdentificador == NumIdentificador
                                    && atq.CodQuestao == codQuestaoAntiga select atq).FirstOrDefault();

            contexto.AvalTemaQuestao.Remove(aqtAntiga);

            AvalTemaQuestao aqtNovo = new AvalTemaQuestao();
            aqtNovo.QuestaoTema = novoQuestaoTema;
            aqtNovo.Ano = Ano;
            aqtNovo.Semestre = Semestre;
            aqtNovo.CodTipoAvaliacao = CodTipoAvaliacao;
            aqtNovo.NumIdentificador = NumIdentificador;

            contexto.AvalTemaQuestao.Add(aqtNovo);

            //contexto.SaveChanges();
        }

        public static Avaliacao ListarPorCodigoAvaliacao(string codigo)
        {
            int numIdentificador = int.Parse(codigo.Substring(codigo.Length - 4));
            codigo = codigo.Remove(codigo.Length - 4);
            int semestre = int.Parse(codigo.Substring(codigo.Length - 1));
            codigo = codigo.Remove(codigo.Length - 1);
            int ano = int.Parse(codigo.Substring(codigo.Length - 4));
            codigo = codigo.Remove(codigo.Length - 4);

            int codTipoAvaliacao = TipoAvaliacao.ListarPorSigla(codigo).CodTipoAvaliacao;

            Avaliacao aval = contexto.Avaliacao.FirstOrDefault(a => a.Ano == ano && a.Semestre == semestre && a.NumIdentificador == numIdentificador && a.CodTipoAvaliacao == codTipoAvaliacao);

            return aval;
        }

        public static bool AlternarFlagArquivo(string codigo)
        {
            Avaliacao aval = ListarPorCodigoAvaliacao(codigo);
            if (aval.AvalPessoaResultado.Count == 0)
            {
                aval.FlagArquivo = !aval.FlagArquivo;
                contexto.SaveChanges();
            }
            return aval.FlagArquivo;
        }
    }
}