using SIAC.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIAC.Models
{
    public partial class AvalAcadReposicao
    {
        public AvalAcademica Academica => this.Justificacao.FirstOrDefault()?.AvalPessoaResultado.Avaliacao.AvalAcademica;
        public Disciplina Disciplina => this.Justificacao.FirstOrDefault()?.AvalPessoaResultado.Avaliacao.AvalAcademica.Disciplina;
        public Professor Professor => this.Justificacao.FirstOrDefault()?.Professor;

        public List<Aluno> Alunos
        {
            get
            {
                IEnumerable<Aluno> alunos = this.Justificacao.FirstOrDefault()?.AvalPessoaResultado.Avaliacao.AvalAcademica.Alunos;
                List<Aluno> retorno = new List<Aluno>();
                foreach (Aluno a in alunos)
                {
                    if (this.Justificacao.FirstOrDefault(j => j.CodPessoaFisica == a.Usuario.CodPessoaFisica) != null)
                    {
                        retorno.Add(a);
                    }
                }
                return retorno;
            }
        }

        public List<Aluno> AlunosRealizaram
        {
            get
            {
                List<Aluno> retorno = new List<Aluno>();
                foreach (Aluno a in this.Alunos)
                {
                    IEnumerable<AvalQuesPessoaResposta> lstRespostas = this.Avaliacao.PessoaResposta.Where(p => p.CodPessoaFisica == a.Usuario.CodPessoaFisica);
                    if (lstRespostas.Count() > 0)
                        retorno.Add(a);
                }
                return retorno;
            }
        }

        private static dbSIACEntities contexto => Repositorio.GetInstance();

        public static AvalAcadReposicao ListarPorCodigoAvaliacao(string codigo) => Avaliacao.ListarPorCodigoAvaliacao(codigo)?.AvalAcadReposicao;

        public static List<AvalAcadReposicao> ListarPorProfessor(int codProfessor) =>
            contexto.AvalAcadReposicao.Where(ac => ac.Justificacao.FirstOrDefault().Professor.CodProfessor == codProfessor)
                .OrderByDescending(ac => ac.Avaliacao.DtCadastro)
                .ToList();

        public static List<AvalAcadReposicao> ListarPorAluno(int codAluno)
        {
            int codPessoaFisica = Aluno.ListarPorCodigo(codAluno).Usuario.CodPessoaFisica;

            return contexto.AvalAcadReposicao
                .Where(a => a.Justificacao.FirstOrDefault(j => j.CodPessoaFisica == codPessoaFisica) != null)
                .OrderByDescending(a => a.Avaliacao.DtCadastro)
                .ToList();
        }

        public static List<AvalAcadReposicao> ListarAgendadaPorUsuario(Usuario usuario)
        {
            switch (usuario.CodCategoria)
            {
                case Categoria.ESTUDANTE:
                    return contexto.AvalAcadReposicao
                        .Where(a => a.Justificacao.FirstOrDefault(j => j.CodPessoaFisica == usuario.CodPessoaFisica) != null
                            && a.Avaliacao.DtAplicacao.HasValue
                            && a.Avaliacao.AvalPessoaResultado.Count == 0
                            && !a.Avaliacao.FlagArquivo)
                        .OrderBy(a => a.Avaliacao.DtAplicacao)
                        .ToList();
                case Categoria.PROFESSOR:
                    int codProfessor = usuario.Professor.First().CodProfessor;
                    return contexto.AvalAcadReposicao
                        .Where(a => a.Justificacao.Count > 0 && a.Justificacao.FirstOrDefault().CodProfessor == codProfessor
                            && a.Avaliacao.DtAplicacao.HasValue
                            && a.Avaliacao.AvalPessoaResultado.Count == 0
                            && !a.Avaliacao.FlagArquivo)
                        .OrderBy(a => a.Avaliacao.DtAplicacao)
                        .ToList();
                case Categoria.COLABORADOR:
                    int codColaborador = usuario.Colaborador.First().CodColaborador;
                    return contexto.AvalAcadReposicao
                        .Where(a =>
                            a.Justificacao.Count > 0
                            && a.Avaliacao.DtAplicacao.HasValue
                            && a.Avaliacao.AvalPessoaResultado.Count == 0
                            && !a.Avaliacao.FlagArquivo &&
                            (
                                a.Justificacao.FirstOrDefault().AvalPessoaResultado.Avaliacao.AvalAcademica.Turma.Curso.CodColabCoordenador == codColaborador
                                || a.Justificacao.FirstOrDefault().AvalPessoaResultado.Avaliacao.AvalAcademica.Turma.Curso.Diretoria.CodColaboradorDiretor == codColaborador
                                || a.Justificacao.FirstOrDefault().AvalPessoaResultado.Avaliacao.AvalAcademica.Turma.Curso.Diretoria.Campus.CodColaboradorDiretor == codColaborador
                                || a.Justificacao.FirstOrDefault().AvalPessoaResultado.Avaliacao.AvalAcademica.Turma.Curso.Diretoria.Campus.Instituicao.Reitoria.Where(r => r.CodColaboradorReitor == codColaborador).Count() > 0
                            ))
                        .OrderBy(a => a.Avaliacao.DtAplicacao)
                        .ToList();
                default:
                    return new List<AvalAcadReposicao>();
            }
        }

        public static List<AvalAcadReposicao> ListarAgendadaPorUsuario(Usuario usuario, DateTime inicio, DateTime termino)
        {
            switch (usuario.CodCategoria)
            {
                case Categoria.ESTUDANTE:
                    return contexto.AvalAcadReposicao
                        .Where(a => a.Avaliacao.DtAplicacao >= inicio && a.Avaliacao.DtAplicacao <= termino
                            && a.Justificacao.FirstOrDefault(j => j.CodPessoaFisica == usuario.CodPessoaFisica) != null
                            && a.Avaliacao.AvalPessoaResultado.Count == 0
                            && !a.Avaliacao.FlagArquivo)
                        .OrderBy(a => a.Avaliacao.DtAplicacao)
                        .ToList();
                case Categoria.PROFESSOR:
                    int codProfessor = usuario.Professor.LastOrDefault()?.CodProfessor ?? 0;
                    return contexto.AvalAcadReposicao
                        .Where(a => a.Avaliacao.DtAplicacao >= inicio && a.Avaliacao.DtAplicacao <= termino
                            && a.Justificacao.Count > 0 && a.Justificacao.FirstOrDefault().CodProfessor == codProfessor
                            && a.Avaliacao.AvalPessoaResultado.Count == 0
                            && !a.Avaliacao.FlagArquivo)
                        .OrderBy(a => a.Avaliacao.DtAplicacao)
                        .ToList();
                case Categoria.COLABORADOR:
                    int codColaborador = usuario.Colaborador.LastOrDefault()?.CodColaborador ?? 0;
                    return contexto.AvalAcadReposicao
                        .Where(a => a.Avaliacao.DtAplicacao >= inicio && a.Avaliacao.DtAplicacao <= termino
                            && a.Justificacao.Count > 0
                            && a.Avaliacao.AvalPessoaResultado.Count == 0
                            && !a.Avaliacao.FlagArquivo &&
                            (
                                a.Justificacao.FirstOrDefault().AvalPessoaResultado.Avaliacao.AvalAcademica.Turma.Curso.CodColabCoordenador == codColaborador
                                || a.Justificacao.FirstOrDefault().AvalPessoaResultado.Avaliacao.AvalAcademica.Turma.Curso.Diretoria.CodColaboradorDiretor == codColaborador
                                || a.Justificacao.FirstOrDefault().AvalPessoaResultado.Avaliacao.AvalAcademica.Turma.Curso.Diretoria.Campus.CodColaboradorDiretor == codColaborador
                                || a.Justificacao.FirstOrDefault().AvalPessoaResultado.Avaliacao.AvalAcademica.Turma.Curso.Diretoria.Campus.Instituicao.Reitoria.Where(r => r.CodColaboradorReitor == codColaborador).Count() > 0
                            ))
                        .OrderBy(a => a.Avaliacao.DtAplicacao)
                        .ToList();
                default:
                    return new List<AvalAcadReposicao>();
            }
        }

        public static bool CorrigirQuestaoAluno(string codAvaliacao, string matrAluno, int codQuestao, double notaObtida, string profObservacao)
        {
            if (!StringExt.IsNullOrWhiteSpace(codAvaliacao,matrAluno) && codQuestao != 0)
            {
                AvalAcadReposicao aval = ListarPorCodigoAvaliacao(codAvaliacao);
                Aluno aluno = Aluno.ListarPorMatricula(matrAluno);
                int codPessoaFisica = aluno.Usuario.PessoaFisica.CodPessoa;

                AvalQuesPessoaResposta resposta = aval.Avaliacao.PessoaResposta.FirstOrDefault(pr => pr.CodQuestao == codQuestao && pr.CodPessoaFisica == codPessoaFisica);

                resposta.RespNota = notaObtida;
                resposta.ProfObservacao = profObservacao;
                
                aval.Avaliacao.AvalPessoaResultado
                    .Single(r => r.CodPessoaFisica == codPessoaFisica)
                    .Nota = aval.Avaliacao.PessoaResposta
                                .Where(pr => pr.CodPessoaFisica == codPessoaFisica)
                                .Average(pr => pr.RespNota);

                contexto.SaveChanges();

                return true;
            }

            return false;
        }

        public static List<AvalAcadReposicao> ListarCorrecaoPendentePorProfessor(int codProfessor) =>
            contexto.AvalQuesPessoaResposta
                .Where(a => a.AvalTemaQuestao.AvaliacaoTema.Avaliacao.AvalAcadReposicao != null && !a.RespNota.HasValue && a.AvalTemaQuestao.AvaliacaoTema.Avaliacao.AvalAcadReposicao.Justificacao.Count > 0 && a.AvalTemaQuestao.AvaliacaoTema.Avaliacao.AvalAcadReposicao.Justificacao.FirstOrDefault().CodProfessor == codProfessor)
                .OrderBy(a => a.AvalTemaQuestao.AvaliacaoTema.Avaliacao.DtAplicacao)
                .Select(a => a.AvalTemaQuestao.AvaliacaoTema.Avaliacao.AvalAcadReposicao)
                .Distinct()
                .ToList();
    }
}