using System;
using System.Collections.Generic;
using System.Linq;
using SIAC.Helpers;

namespace SIAC.Models
{
    public partial class AvalAcademica
    {
        public List<Aluno> Alunos => this.Turma?.TurmaDiscAluno.Select(t => t.Aluno).ToList();

        public List<Aluno> AlunosRealizaram
        {
            get
            {
                List<Aluno> retorno = new List<Aluno>();
                foreach (Aluno aluno in this.Alunos)
                {
                    IEnumerable<AvalQuesPessoaResposta> lstRespostas = this.Avaliacao.PessoaResposta.Where(p => p.CodPessoaFisica == aluno.Usuario.CodPessoaFisica);
                    if (lstRespostas.Count() > 0)
                        retorno.Add(aluno);
                }
                return retorno;
            }
        }

        public List<Aluno> AlunoAusente
        {
            get
            {
                List<Aluno> retorno = new List<Aluno>();
                foreach (Aluno aluno in this.Alunos)
                {
                    IEnumerable<AvalQuesPessoaResposta> lstRespostas = this.Avaliacao.PessoaResposta.Where(p => p.CodPessoaFisica == aluno.Usuario.CodPessoaFisica);
                    if (lstRespostas.Count() == 0)
                        retorno.Add(aluno);
                }
                return retorno;
            }
        }

        public List<Aluno> AlunoSemJustificacao
        {
            get
            {
                List<Aluno> retorno = new List<Aluno>();

                foreach (Aluno aluno in this.AlunoAusente)
                {
                    if (this.Avaliacao.AvalPessoaResultado.FirstOrDefault(j => j.CodPessoaFisica == aluno.Usuario.CodPessoaFisica) == null)
                    {
                        retorno.Add(aluno);
                    }
                }

                return retorno;
            }
        }

        public List<Justificacao> Justificacoes
        {
            get
            {
                List<Justificacao> retorno = new List<Justificacao>();
                foreach (AvalPessoaResultado avalPessoaResultado in this.Avaliacao.AvalPessoaResultado)
                {
                    retorno.AddRange(avalPessoaResultado.Justificacao.Where(j => j.AvalAcadReposicao.Count == 0));
                }
                return retorno;
            }
        }

        public List<AvalAcadReposicao> Reposicoes
        {
            get
            {
                List<AvalAcadReposicao> retorno = new List<AvalAcadReposicao>();

                foreach (AvalPessoaResultado avalPessoaResultado in this.Avaliacao.AvalPessoaResultado)
                {
                    foreach (Justificacao justificacao in avalPessoaResultado.Justificacao)
                    {
                        retorno.AddRange(justificacao.AvalAcadReposicao.ToList());
                    }
                }

                return retorno.Distinct().ToList();
            }
        }

        private static dbSIACEntities contexto => Repositorio.GetInstance();

        public static void Inserir(AvalAcademica avalAcademica)
        {
            contexto.AvalAcademica.Add(avalAcademica);
            
            Questao.AtualizarDtUltimoUso(avalAcademica.Avaliacao.Questao);

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

        public static List<AvalAcademica> ListarPorProfessor(int codProfessor) =>
            contexto.AvalAcademica.Where(ac => ac.CodProfessor == codProfessor)
                .OrderByDescending(ac => ac.Avaliacao.DtCadastro)
                .ToList();

        public static List<AvalAcademica> ListarCorrecaoPendentePorProfessor(int codProfessor) =>
            contexto.AvalQuesPessoaResposta
                .Where(a => a.AvalTemaQuestao.AvaliacaoTema.Avaliacao.AvalAcademica.CodProfessor == codProfessor && !a.RespNota.HasValue)
                .OrderBy(a => a.AvalTemaQuestao.AvaliacaoTema.Avaliacao.DtAplicacao)
                .Select(a => a.AvalTemaQuestao.AvaliacaoTema.Avaliacao.AvalAcademica)
                .Distinct()
                .ToList();

        public static List<AvalAcademica> ListarPorAluno(int codAluno) =>
            contexto.AvalAcademica
                .Where(a => a.Turma.TurmaDiscAluno.FirstOrDefault(t => t.CodAluno == codAluno) != null)
                .OrderByDescending(a => a.Avaliacao.DtCadastro)
                .ToList();

        public static List<AvalAcademica> ListarAgendadaPorUsuario(Usuario usuario)
        {
            switch (usuario.CodCategoria)
            {
                case Categoria.ESTUDANTE:
                    int codAluno = usuario.Aluno.First().CodAluno;
                    return contexto.AvalAcademica
                        .Where(a => a.Turma.TurmaDiscAluno.FirstOrDefault(t => t.CodAluno == codAluno) != null
                            && a.Avaliacao.DtAplicacao.HasValue
                            && a.Avaliacao.AvalPessoaResultado.Count == 0
                            && !a.Avaliacao.FlagArquivo)
                        .OrderBy(a => a.Avaliacao.DtAplicacao)
                        .ToList();
                case Categoria.PROFESSOR:
                    int codProfessor = usuario.Professor.First().CodProfessor;
                    return contexto.AvalAcademica
                        .Where(a => a.CodProfessor == codProfessor
                            && a.Avaliacao.DtAplicacao.HasValue
                            && a.Avaliacao.AvalPessoaResultado.Count == 0
                            && !a.Avaliacao.FlagArquivo)
                        .OrderBy(a => a.Avaliacao.DtAplicacao)
                        .ToList();
                case Categoria.COLABORADOR:
                    int codColaborador = usuario.Colaborador.First().CodColaborador;
                    return contexto.AvalAcademica
                        .Where(a =>
                            a.Avaliacao.DtAplicacao.HasValue
                            && a.Avaliacao.AvalPessoaResultado.Count == 0
                            && !a.Avaliacao.FlagArquivo
                            &&
                            (
                                a.Turma.Curso.CodColabCoordenador == codColaborador
                                || a.Turma.Curso.Diretoria.CodColaboradorDiretor == codColaborador
                                || a.Turma.Curso.Diretoria.Campus.CodColaboradorDiretor == codColaborador
                                || a.Turma.Curso.Diretoria.Campus.Instituicao.Reitoria.Where(r => r.CodColaboradorReitor == codColaborador).Count() > 0
                            ))
                        .OrderBy(a => a.Avaliacao.DtAplicacao)
                        .ToList();
                default:
                    return new List<AvalAcademica>();
            }
        }

        public static List<AvalAcademica> ListarAgendadaPorUsuario(Usuario usuario, DateTime inicio, DateTime termino)
        {
            switch (usuario.CodCategoria)
            {
                case Categoria.ESTUDANTE:
                    int codAluno = usuario.Aluno.LastOrDefault()?.CodAluno ?? 0;
                    return contexto.AvalAcademica
                        .Where(a =>
                            a.Avaliacao.DtAplicacao >= inicio && a.Avaliacao.DtAplicacao <= termino
                            && a.Turma.TurmaDiscAluno.FirstOrDefault(t => t.CodAluno == codAluno) != null
                            && a.Avaliacao.AvalPessoaResultado.Count == 0
                            && !a.Avaliacao.FlagArquivo)
                        .OrderBy(a => a.Avaliacao.DtAplicacao)
                        .ToList();
                case Categoria.PROFESSOR:
                    int codProfessor = usuario.Professor.LastOrDefault()?.CodProfessor ?? 0;
                    return contexto.AvalAcademica
                        .Where(a => a.CodProfessor == codProfessor
                            && a.Avaliacao.DtAplicacao >= inicio && a.Avaliacao.DtAplicacao <= termino
                            && a.Avaliacao.AvalPessoaResultado.Count == 0
                            && !a.Avaliacao.FlagArquivo)
                        .OrderBy(a => a.Avaliacao.DtAplicacao)
                        .ToList();
                case Categoria.COLABORADOR:
                    int codColaborador = usuario.Colaborador.LastOrDefault()?.CodColaborador ?? 0;
                    return contexto.AvalAcademica
                        .Where(a =>
                            a.Avaliacao.DtAplicacao >= inicio && a.Avaliacao.DtAplicacao <= termino
                            && a.Avaliacao.AvalPessoaResultado.Count == 0
                            && !a.Avaliacao.FlagArquivo
                            &&
                            (
                                a.Turma.Curso.CodColabCoordenador == codColaborador
                                || a.Turma.Curso.Diretoria.CodColaboradorDiretor == codColaborador
                                || a.Turma.Curso.Diretoria.Campus.CodColaboradorDiretor == codColaborador
                                || a.Turma.Curso.Diretoria.Campus.Instituicao.Reitoria.Where(r => r.CodColaboradorReitor == codColaborador).Count() > 0
                            ))
                        .OrderBy(a => a.Avaliacao.DtAplicacao)
                        .ToList();
                default:
                    return new List<AvalAcademica>();
            }
        }

        public static AvalAcademica ListarPorCodigoAvaliacao(string codigo) => Avaliacao.ListarPorCodigoAvaliacao(codigo)?.AvalAcademica; 

        public static void Persistir() => contexto.SaveChanges();

        public static bool CorrigirQuestaoAluno(string codAvaliacao, string matrAluno, int codQuestao, double notaObtida, string profObservacao)
        {
            if (!StringExt.IsNullOrWhiteSpace(codAvaliacao,matrAluno) && codQuestao != 0)
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

        public static void RecalcularResultados(string codigo = "")
        {
            if (String.IsNullOrEmpty(codigo))
            {
                foreach (AvalAcademica acad in contexto.AvalAcademica.ToList())
                {
                    foreach (AvalPessoaResultado avalPessoaResultado in acad.Avaliacao.AvalPessoaResultado)
                    {
                        foreach (AvalQuesPessoaResposta pessoaResposta in acad.Avaliacao.PessoaResposta.Where(r => r.CodPessoaFisica == avalPessoaResultado.CodPessoaFisica).ToList())
                        {
                            if (pessoaResposta.AvalTemaQuestao.QuestaoTema.Questao.CodTipoQuestao == TipoQuestao.OBJETIVA)
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
            }
            else
            {
                AvalAcademica acad = AvalAcademica.ListarPorCodigoAvaliacao(codigo);
                foreach (AvalPessoaResultado avalPessoaResultado in acad.Avaliacao.AvalPessoaResultado)
                {
                    foreach (AvalQuesPessoaResposta pessoaResposta in acad.Avaliacao.PessoaResposta.Where(r => r.CodPessoaFisica == avalPessoaResultado.CodPessoaFisica).ToList())
                    {
                        if (pessoaResposta.AvalTemaQuestao.QuestaoTema.Questao.CodTipoQuestao == TipoQuestao.OBJETIVA)
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