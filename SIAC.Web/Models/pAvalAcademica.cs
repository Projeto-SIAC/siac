using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Models
{
    public partial class AvalAcademica
    {
        public List<Aluno> Alunos
        {
            get
            {                
                return this.Turma.TurmaDiscAluno.Select(t => t.Aluno).ToList();
            }
        }

        public List<Aluno> AlunosRealizaram
        {
            get
            {
                List<Aluno> result = new List<Aluno>();
                foreach (Aluno a in this.Alunos)
                {
                    var lstRespostas = this.Avaliacao.PessoaResposta.Where(p => p.CodPessoaFisica == a.Usuario.CodPessoaFisica);
                    if (lstRespostas.Count() > 0)
                        result.Add(a);
                }
                return result;
            }
        }

        private static dbSIACEntities contexto { get { return Repositorio.GetInstance(); } }
        
        public static void Inserir(AvalAcademica avalAcademica)
        {
            contexto.AvalAcademica.Add(avalAcademica);
            contexto.SaveChanges();
        }

        public static void Agendar(AvalAcademica avalAcad)
        {
            AvalAcademica temp = contexto.AvalAcademica.FirstOrDefault(acad => acad.Ano == avalAcad.Ano && acad.Semestre == avalAcad.Semestre && acad.NumIdentificador == avalAcad.NumIdentificador && acad.CodTipoAvaliacao == avalAcad.CodTipoAvaliacao);

            temp.Turma = avalAcad.Turma;
            temp.Sala = avalAcad.Sala;
            temp.Avaliacao.DtAplicacao = avalAcad.Avaliacao.DtAplicacao;
            temp.Avaliacao.Duracao = avalAcad.Avaliacao.Duracao;

            contexto.SaveChanges();
        }

        public static List<AvalAcademica> ListarAgendadaPorColaborador(int codColaborador)
        {
            return contexto.AvalAcademica
                .Where(a => 
                    (
                        a.Turma.Curso.CodColabCoordenador == codColaborador 
                        || a.Turma.Curso.Diretoria.CodColaboradorDiretor == codColaborador 
                        || a.Turma.Curso.Diretoria.Campus.CodColaboradorDiretor == codColaborador 
                        || a.Turma.Curso.Diretoria.Campus.Instituicao.Reitoria.Where(r=>r.CodColaboradorReitor == codColaborador).Count() > 0
                    )
                    && a.Avaliacao.DtAplicacao.HasValue
                    && a.Avaliacao.AvalPessoaResultado.Count == 0
                    && !a.Avaliacao.FlagArquivo)
                .OrderBy(a => a.Avaliacao.DtAplicacao)
                .ToList();
        }

        public static List<AvalAcademica> ListarPorProfessor(int codProfessor)
        {
            return contexto.AvalAcademica.Where(ac => ac.CodProfessor == codProfessor)
                                         .OrderByDescending(ac => ac.Avaliacao.DtCadastro)
                                         .ToList();
        }

        public static List<AvalAcademica> ListarAgendadaPorProfessor(int codProfessor)
        {
            return contexto.AvalAcademica
                .Where(a => a.CodProfessor == codProfessor
                    && a.Avaliacao.DtAplicacao.HasValue
                    && a.Avaliacao.AvalPessoaResultado.Count == 0
                    && !a.Avaliacao.FlagArquivo)
                .OrderBy(a => a.Avaliacao.DtAplicacao)
                .ToList();
        }

        public static List<AvalAcademica> ListarCorrecaoPendentePorProfessor(int codProfessor)
        {
            return contexto.AvalQuesPessoaResposta
                .Where(a => a.AvalTemaQuestao.AvaliacaoTema.Avaliacao.AvalAcademica.CodProfessor == codProfessor && !a.RespNota.HasValue)
                .OrderBy(a => a.AvalTemaQuestao.AvaliacaoTema.Avaliacao.DtAplicacao)
                .Select(a=>a.AvalTemaQuestao.AvaliacaoTema.Avaliacao.AvalAcademica)
                .Distinct()
                .ToList();
        }

        public static List<AvalAcademica> ListarPorAluno(int codAluno)
        {
            return contexto.AvalAcademica
                .Where(a => a.Turma.TurmaDiscAluno.Where(t => t.CodAluno == codAluno).Count() > 0)
                .OrderByDescending(a => a.Avaliacao.DtCadastro)
                .ToList();
        }

        public static List<AvalAcademica> ListarAgendadaPorAluno(int codAluno)
        {          
            return contexto.AvalAcademica
                .Where(a => a.Turma.TurmaDiscAluno.Where(t => t.CodAluno == codAluno).Count() > 0 
                    && a.Avaliacao.DtAplicacao.HasValue 
                    && a.Avaliacao.AvalPessoaResultado.Count == 0
                    && !a.Avaliacao.FlagArquivo)
                .OrderBy(a => a.Avaliacao.DtAplicacao)
                .ToList();
        }

        public static AvalAcademica ListarPorCodigoAvaliacao(string codigo)
        {
            int numIdentificador = 0;
            int semestre = 0;
            int ano = 0;

            if (codigo.Length == 13)
            {

                int.TryParse(codigo.Substring(codigo.Length - 4), out numIdentificador);
                codigo = codigo.Remove(codigo.Length - 4);
                int.TryParse(codigo.Substring(codigo.Length - 1), out semestre);
                codigo = codigo.Remove(codigo.Length - 1);
                int.TryParse(codigo.Substring(codigo.Length - 4), out ano);
                codigo = codigo.Remove(codigo.Length - 4);

                int codTipoAvaliacao = TipoAvaliacao.ListarPorSigla(codigo).CodTipoAvaliacao;

                AvalAcademica avalAcademica = contexto.AvalAcademica.FirstOrDefault(acad => acad.Ano == ano && acad.Semestre == semestre && acad.NumIdentificador == numIdentificador && acad.CodTipoAvaliacao == codTipoAvaliacao);

                return avalAcademica;
            }
            return null;
        }

        public static bool AlternarLiberar(string codAvaliacao)
        {
            AvalAcademica avalAcad = ListarPorCodigoAvaliacao(codAvaliacao);
            avalAcad.Avaliacao.FlagLiberada = !avalAcad.Avaliacao.FlagLiberada;
            contexto.SaveChanges();
            return avalAcad.Avaliacao.FlagLiberada;
        }

        public static void Persistir()
        {
            contexto.SaveChanges();
        }

        public static bool CorrigirQuestaoAluno(string codAvaliacao, string matrAluno, int codQuestao,double notaObtida,string profObservacao)
        {
            if (!String.IsNullOrEmpty(codAvaliacao) && !String.IsNullOrEmpty(matrAluno) && codQuestao != 0)
            {
                AvalAcademica acad = AvalAcademica.ListarPorCodigoAvaliacao(codAvaliacao);
                Aluno aluno = Aluno.ListarPorMatricula(matrAluno);
                int codPessoaFisica = aluno.Usuario.PessoaFisica.CodPessoa;

                AvalQuesPessoaResposta resposta = acad.Avaliacao.PessoaResposta.FirstOrDefault(pr => pr.CodQuestao == codQuestao && pr.CodPessoaFisica == codPessoaFisica);

                resposta.RespNota = notaObtida;
                resposta.ProfObservacao = profObservacao;

                acad.Avaliacao.AvalPessoaResultado
                    .Single(r => r.CodPessoaFisica == codPessoaFisica)
                    .Nota = acad.Avaliacao.PessoaResposta
                                .Where(pr => pr.CodPessoaFisica == codPessoaFisica)
                                .Average(pr => pr.RespNota);

                contexto.SaveChanges();

                return true;
            }

            return false;
        }

        public static void RecalcularResultados()
        {
            foreach (var acad in contexto.AvalAcademica.ToList())
            {
                foreach (var avalPessoaResultado in acad.Avaliacao.AvalPessoaResultado)
                {
                    foreach (var pessoaResposta in acad.Avaliacao.PessoaResposta.Where(r=>r.CodPessoaFisica == avalPessoaResultado.CodPessoaFisica).ToList())
                    {
                        if (pessoaResposta.AvalTemaQuestao.QuestaoTema.Questao.CodTipoQuestao == 1)
                        {
                            if (pessoaResposta.RespAlternativa == pessoaResposta.AvalTemaQuestao.QuestaoTema.Questao.Alternativa.Single(a => a.FlagGabarito.HasValue && a.FlagGabarito.Value).CodOrdem)
                            {
                                pessoaResposta.RespNota = 10;
                            }
                            else
                            {
                                pessoaResposta.RespNota = 0;
                            }
                        }
                    }
                    avalPessoaResultado.Nota = acad.Avaliacao.PessoaResposta
                                .Where(pr => pr.CodPessoaFisica == avalPessoaResultado.CodPessoaFisica)
                                .Average(pr => pr.RespNota);
                }
            }
            contexto.SaveChanges();
        }
    }
}