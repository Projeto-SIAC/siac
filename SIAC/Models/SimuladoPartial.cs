using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SIAC.Models
{
    public partial class Simulado
    {
        [NotMapped]
        public string Codigo => $"SIMUL{Ano}{NumIdentificador.ToString("d5")}";

        [NotMapped]
        public bool FlagNenhumInscritoAposPrazo => this.SimCandidato.Count == 0 && this.DtTerminoInscricao < DateTime.Now;

        [NotMapped]
        public bool FlagTemVaga => this.QteVagas > this.SimCandidato.Count;

        [NotMapped]
        public bool FlagInscricaoAberta => !this.FlagInscricaoEncerrado && this.DtInicioInscricao <= DateTime.Now && this.DtTerminoInscricao > DateTime.Now;

        [NotMapped]
        public bool FlagInscricaoNaoLiberada => this.FlagInscricaoEncerrado && this.DtInicioInscricao <= DateTime.Now && this.DtTerminoInscricao > DateTime.Now;

        [NotMapped]
        public bool FlagAguardaPrazoInscricao => this.DtInicioInscricao > DateTime.Now;

        [NotMapped]
        public bool FlagAguardaDefinicaoSalas => this.SimSala.Sum(s => s.Sala.Capacidade) < this.QteVagas;

        [NotMapped]
        public bool FlagVagasEsgotadas => this.QteVagas == this.SimCandidato.Count;

        [NotMapped]
        public bool FlagSalaMapeada => this.SimCandidato.FirstOrDefault(c => !c.CodSala.HasValue) == null;

        [NotMapped]
        public List<SimProva> Provas
        {
            get
            {
                List<SimProva> provas = new List<SimProva>();
                foreach (var dia in this.SimDiaRealizacao)
                {
                    provas.AddRange(dia.SimProva);
                }
                return provas;
            }
        }

        [NotMapped]
        public List<SimCandidato> Classificacao =>
            this.SimCandidato.OrderByDescending(c => c.EscorePadronizadoFinal).ToList();

        [NotMapped]
        public SimDiaRealizacao PrimeiroDiaRealizacao =>
            this.SimDiaRealizacao.OrderBy(d => d.DtRealizacao).FirstOrDefault();

        [NotMapped]
        public SimDiaRealizacao UltimoDiaRealizacao =>
            this.SimDiaRealizacao.OrderBy(d => d.DtRealizacao).LastOrDefault();

        public List<Questao> TodasQuestoesPorDisciplina(int codDisciplina, int eviteCodDia = 0, int eviteCodProva = 0)
        {
            var questoes = new List<Questao>();
            foreach (var dia in this.SimDiaRealizacao)
            {
                var provas = dia.SimProva.Where(p => p.CodDisciplina == codDisciplina && !(p.CodProva == eviteCodProva && p.CodDiaRealizacao == eviteCodDia));
                foreach (var prova in provas)
                {
                    questoes.AddRange(prova.SimProvaQuestao.Select(q => q.Questao));
                }
            }
            return questoes;
        }

        public bool CandidatoInscrito(int codCandidato) =>
       this.SimCandidato.FirstOrDefault(sc => sc.CodCandidato == codCandidato) != null;

        public int ObterNumInscricao()
        {
            if (!Sistema.ProxInscricao.ContainsKey(this.Codigo))
            {
                Sistema.ProxInscricao[this.Codigo] = this.SimCandidato.Count > 0 ? this.SimCandidato.Max(c => c.NumInscricao) + 1 : 1;
            }
            return Sistema.ProxInscricao[this.Codigo]++;
        }

        public SimSala ObterSalaDisponivel()
        {
            foreach (var sala in this.SimSala)
            {
                if (sala.Sala.Capacidade > sala.SimCandidato.Count)
                {
                    return sala;
                }
            }
            return this.SimSala.FirstOrDefault();
        }

        [NotMapped]
        public bool CadastroCompleto =>
            !this.FlagSimuladoEncerrado
            && !this.FlagProvaEncerrada
            && this.DtInicioInscricao.HasValue
            && this.SimDiaRealizacao.Count > 0
            && this.SimDiaRealizacao.First().SimProva.Count > 0
            && this.SimSala.Count > 0;

        private static Contexto contexto => Repositorio.GetInstance();

        public static void Inserir(Simulado simulado)
        {
            contexto.Simulado.Add(simulado);
            contexto.SaveChanges();
        }

        public static int ObterNumIdentificador() =>
            contexto.Simulado.Where(s => s.Ano == DateTime.Now.Year).Count() + 1;

        public static Simulado ListarPorCodigo(string codigo)
        {
            int numIdentificador = int.Parse(codigo.Substring(codigo.Length - 5));
            codigo = codigo.Remove(codigo.Length - 5);
            int ano = int.Parse(codigo.Substring(codigo.Length - 4));

            return contexto.Simulado.FirstOrDefault(s => s.Ano == ano && s.NumIdentificador == numIdentificador);
        }

        public static List<Simulado> ListarNaoEncerradoOrdenadamente() =>
            contexto.Simulado.Where(s => !s.FlagSimuladoEncerrado).OrderByDescending(s => s.DtCadastro).ToList();

        public static List<Simulado> ListarEncerradoOrdenadamente() =>
            contexto.Simulado.Where(s => s.FlagSimuladoEncerrado).OrderByDescending(s => s.DtCadastro).ToList();

        public static List<Simulado> ListarPorInscricoesAbertas()
        {
            return contexto.Simulado
                .Where(simulado =>
                  !simulado.FlagInscricaoEncerrado
                && simulado.DtInicioInscricao.Value <= DateTime.Today
                && simulado.DtTerminoInscricao.Value >= DateTime.Today
                ).OrderByDescending(s => s.DtInicioInscricao).ToList();
        }

        public static List<Simulado> ListarParaInscricoes()
        {
            return ListarPorInscricoesAbertas().Where(s => s.CadastroCompleto).ToList();
        }

        public static List<int> ObterQuestoesCodigos(int codDisciplina, int quantidadeQuestoes, int codTipo = TipoQuestao.OBJETIVA, List<int> eviteCodQuestao = null)
        {
            List<int> codigos = new List<int>();

            if (quantidadeQuestoes > 0)
            {
                List<int> ids =
                    (from qt in contexto.QuestaoTema
                     where qt.CodDisciplina == codDisciplina
                     && qt.Questao.CodTipoQuestao == codTipo
                     //&& QuestaoTema.PrazoValido(qt)
                     select qt.CodQuestao).Distinct().ToList();

                if (eviteCodQuestao != null)
                {
                    ids = ids.Except(eviteCodQuestao).ToList();
                }

                if (ids.Count > quantidadeQuestoes)
                {
                    for (int i = 0; i < quantidadeQuestoes; i++)
                    {
                        int random = Sistema.Random.Next(0, ids.Count);

                        int codQuestao = ids[random];

                        codigos.Add(codQuestao);
                        ids.Remove(codQuestao);
                    }
                }
                else
                {
                    codigos.AddRange(ids);
                }
            }
            return codigos;
        }
    }
}