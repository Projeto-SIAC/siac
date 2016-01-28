using System;
using System.Collections.Generic;
using System.Linq;

namespace SIAC.Models
{
    public partial class Avaliacao
    {
        public string CodAvaliacao => $"{TipoAvaliacao.Sigla.ToUpper()}{Ano}{Semestre}{NumIdentificador.ToString("0000")}";

        public DateTime? DtTermino => this.DtAplicacao.HasValue && this.Duracao.HasValue ? this.DtAplicacao.Value.AddMinutes(this.Duracao.Value) : new Nullable<DateTime>();

        public Professor Professor
        {
            get
            {
                switch (this.CodTipoAvaliacao)
                {
                    case TipoAvaliacao.ACADEMICA:
                        return this.AvalAcademica.Professor;
                    case TipoAvaliacao.CERTIFICACAO:
                        return this.AvalCertificacao.Professor;
                    case TipoAvaliacao.REPOSICAO:
                        return this.AvalAcadReposicao.Professor;
                    default:
                        return null;
                }
            }
        }

        public List<Tema> Temas => this.AvaliacaoTema.Select(at => at.Tema).Distinct().ToList();
        
        public List<Questao> Questao
        {
            get
            {
                List<Questao> lstQuestao = new List<Questao>();
                foreach (AvaliacaoTema avaliacaoTema in AvaliacaoTema)
                    foreach (AvalTemaQuestao avalTemaQuestao in avaliacaoTema.AvalTemaQuestao)
                        lstQuestao.Add(avalTemaQuestao.QuestaoTema.Questao);
                return lstQuestao;
            }
        }

        public List<QuestaoTema> QuestaoTema
        {
            get
            {
                List<QuestaoTema> QuestoesTema = new List<QuestaoTema>();
                foreach (AvaliacaoTema item in this.AvaliacaoTema)
                    foreach (AvalTemaQuestao qt in item.AvalTemaQuestao)
                        QuestoesTema.Add(qt.QuestaoTema);
                return QuestoesTema;
            }
        }

        public List<AvalQuesPessoaResposta> PessoaResposta
        {
            get
            {
                List<AvalQuesPessoaResposta> lstPessoaResposta = new List<AvalQuesPessoaResposta>();
                foreach (AvaliacaoTema avaliacaoTema in AvaliacaoTema)
                    foreach (AvalTemaQuestao avalTemaQuestao in avaliacaoTema.AvalTemaQuestao)
                        lstPessoaResposta.AddRange(avalTemaQuestao.AvalQuesPessoaResposta);
                return lstPessoaResposta;
            }
        }

        public int CodDificuldade => Questao.Max(q => q.CodDificuldade);

        public int TipoQuestoes
        {
            get
            {
                int tipo = 0;

                foreach (Questao questao in this.Questao)
                    if (questao.CodTipoQuestao == TipoQuestao.OBJETIVA)
                    {
                        tipo += 1;
                        break;
                    }

                foreach (Questao questao in this.Questao)
                    if (questao.CodTipoQuestao == TipoQuestao.DISCURSIVA)
                    {
                        tipo += 2;
                        break;
                    }

                return tipo > 0 ? tipo : -1;
            }
        }

        public bool FlagPendente => this.AvalPessoaResultado.Count > 0 || this.FlagArquivo ? false : true;

        public bool FlagRealizada => this.AvalPessoaResultado.Count > 0;

        public bool FlagAgendada => this.DtAplicacao.HasValue && this.FlagPendente;

        public bool FlagAgora
        {
            get
            {
                var total = (DateTime.Now - DtAplicacao.Value).TotalMinutes;
                return total < (Duracao / 2) && total > 0;
            }
        }

        public bool FlagVencida => (DateTime.Now - DtAplicacao.Value).TotalMinutes > (Duracao / 2);

        public bool FlagCorrecaoPendente => this.FlagRealizada && this.PessoaResposta.Where(pr => !pr.RespNota.HasValue).Count() > 0;

        private static dbSIACEntities contexto => Repositorio.GetInstance();

        public static int ObterNumIdentificador(int codTipoAvaliacao)
        {
            if (Sistema.NumIdentificador.Count == 0)
            {
                for (int i = 1; i <= contexto.TipoAvaliacao.Max(t => t.CodTipoAvaliacao); i++)
                {
                    int ano = DateTime.Now.Year;
                    int semestre = DateTime.Now.SemestreAtual();
                    Avaliacao avalTemp = contexto.Avaliacao
                        .Where(a => a.Ano == ano
                            && a.Semestre == semestre
                            && a.CodTipoAvaliacao == i)
                        .OrderByDescending(a => a.NumIdentificador)
                        .FirstOrDefault();
                    if (avalTemp != null)
                        Sistema.NumIdentificador.Add(i, avalTemp.NumIdentificador + 1);
                    else
                        Sistema.NumIdentificador.Add(i, 1);
                }
            }
            return Sistema.NumIdentificador[codTipoAvaliacao]++;
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

            Avaliacao aval = contexto.Avaliacao
                .FirstOrDefault(a => a.Ano == ano
                    && a.Semestre == semestre
                    && a.NumIdentificador == numIdentificador
                    && a.CodTipoAvaliacao == codTipoAvaliacao);

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

        public static bool AlternarFlagLiberada(string codAvaliacao)
        {
            Avaliacao aval = ListarPorCodigoAvaliacao(codAvaliacao);
            aval.FlagLiberada = !aval.FlagLiberada;
            contexto.SaveChanges();
            return aval.FlagLiberada;
        }

        public static void AtualizarQuestoes(string codigo, int[] questoes)
        {
            Avaliacao aval = ListarPorCodigoAvaliacao(codigo);

            List<QuestaoTema> questoesTema = new List<QuestaoTema>();

            foreach (int questao in questoes)
            {
                List<QuestaoTema> qtTemp = contexto.QuestaoTema.Where(qt => qt.CodQuestao == questao).ToList();
                questoesTema.AddRange(qtTemp);
            }

            if (questoesTema.Count > 0)
            {
                //Deletar questões antigas
                List<AvalTemaQuestao> questoesAntigas = contexto.AvalTemaQuestao
                    .Where(atq => atq.Ano == aval.Ano
                        && atq.Semestre == aval.Semestre
                        && atq.CodTipoAvaliacao == aval.CodTipoAvaliacao
                        && atq.NumIdentificador == aval.NumIdentificador)
                    .ToList();
                contexto.AvalTemaQuestao.RemoveRange(questoesAntigas);

                List<AvalTemaQuestao> questoesAdicionadas = new List<AvalTemaQuestao>();

                foreach (Tema tema in aval.Temas)
                {
                    List<QuestaoTema> questaoTema = questoesTema.Where(qt => qt.Tema == tema).ToList();
                    AvaliacaoTema avalTema = aval.AvaliacaoTema.FirstOrDefault(at => at.Tema == tema);

                    if (questaoTema.Count > 0)
                    {
                        foreach (QuestaoTema qt in questaoTema)
                        {
                            AvalTemaQuestao proximaQuestao = new AvalTemaQuestao
                            {
                                AvaliacaoTema = avalTema,
                                QuestaoTema = qt
                            };

                            //Verificar se a questão já não foi adicionada
                            if (questoesAdicionadas
                                .Where(atq => atq.Ano == aval.Ano
                                    && atq.Semestre == aval.Semestre
                                    && atq.CodTipoAvaliacao == aval.CodTipoAvaliacao
                                    && atq.NumIdentificador == aval.NumIdentificador
                                    && atq.CodQuestao == qt.CodQuestao).ToList().Count == 0)
                            {
                                contexto.AvalTemaQuestao.Add(proximaQuestao);
                                questoesAdicionadas.Add(proximaQuestao);
                            }
                        }
                    }
                }

                contexto.SaveChanges();
            }
        }
    }
}