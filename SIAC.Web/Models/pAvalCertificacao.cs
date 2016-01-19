using System;
using System.Collections.Generic;
using System.Linq;

namespace SIAC.Models
{
    public partial class AvalCertificacao
    {
        public List<PessoaFisica> PessoasRealizaram
        {
            get
            {
                List<PessoaFisica> result = new List<PessoaFisica>();
                foreach (PessoaFisica a in this.PessoaFisica)
                {
                    var lstRespostas = this.Avaliacao.PessoaResposta.Where(p => p.CodPessoaFisica == a.CodPessoa);
                    if (lstRespostas.Count() > 0)
                        result.Add(a);
                }
                return result;
            }
        }

        private static dbSIACEntities contexto => Repositorio.GetInstance();

        public static void Inserir(AvalCertificacao avalCertificacao)
        {
            contexto.AvalCertificacao.Add(avalCertificacao);
            contexto.SaveChanges();
        }

        public static List<AvalCertificacao> ListarCorrecaoPendentePorProfessor(int codProfessor) =>
            contexto.AvalQuesPessoaResposta
                .Where(a => a.AvalTemaQuestao.AvaliacaoTema.Avaliacao.AvalCertificacao.CodProfessor == codProfessor && !a.RespNota.HasValue)
                .OrderBy(a => a.AvalTemaQuestao.AvaliacaoTema.Avaliacao.DtAplicacao)
                .Select(a => a.AvalTemaQuestao.AvaliacaoTema.Avaliacao.AvalCertificacao)
                .Distinct()
                .ToList();

        public static AvalCertificacao ListarPorCodigoAvaliacao(string codigo)
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

                AvalCertificacao avalCert = contexto.AvalCertificacao.FirstOrDefault(acad => acad.Ano == ano && acad.Semestre == semestre && acad.NumIdentificador == numIdentificador && acad.CodTipoAvaliacao == codTipoAvaliacao);

                return avalCert;
            }
            return null;
        }

        public static List<AvalCertificacao> ListarPorPessoa(int codPessoaFisica) =>
            contexto.AvalCertificacao.Where(ac => ac.PessoaFisica.FirstOrDefault(p => p.CodPessoa == codPessoaFisica) != null)
                                         .OrderByDescending(ac => ac.Avaliacao.DtCadastro)
                                         .ToList();

        public static List<AvalCertificacao> ListarPorProfessor(int codProfessor) =>
            contexto.AvalCertificacao.Where(ac => ac.CodProfessor == codProfessor)
                                         .OrderByDescending(ac => ac.Avaliacao.DtCadastro)
                                         .ToList();            

        public static List<AvalCertificacao> ListarAgendadaPorUsuario(Usuario usuario)
        {
            var codPessoaFisica = usuario.CodPessoaFisica;
            switch (usuario.CodCategoria)
            {
                case 1:
                    return contexto.AvalCertificacao
                        .Where(a => a.PessoaFisica.FirstOrDefault(p => p.CodPessoa == codPessoaFisica) != null
                            && a.Avaliacao.DtAplicacao.HasValue
                            && a.Avaliacao.AvalPessoaResultado.Count == 0
                            && !a.Avaliacao.FlagArquivo)
                        .OrderByDescending(ac => ac.Avaliacao.DtCadastro)
                        .ToList();
                case 2:
                    var codProfessor = usuario.Professor.First().CodProfessor;
                    return contexto.AvalCertificacao
                        .Where(a => a.CodProfessor == codProfessor
                            && a.Avaliacao.DtAplicacao.HasValue
                            && a.Avaliacao.AvalPessoaResultado.Count == 0
                            && !a.Avaliacao.FlagArquivo)
                        .OrderBy(a => a.Avaliacao.DtAplicacao)
                        .ToList();
                case 3:
                    var codColaborador = usuario.Colaborador.First().CodColaborador;
                    return contexto.AvalCertificacao
                        .Where(a =>
                            a.Avaliacao.DtAplicacao.HasValue
                            && a.Avaliacao.AvalPessoaResultado.Count == 0
                            && !a.Avaliacao.FlagArquivo &&
                            (
                                (a.PessoaFisica.FirstOrDefault(p => p.CodPessoa == codPessoaFisica) != null) 
                                || (a.Professor.TurmaDiscProfHorario.Where(t => t.Turma.Curso.CodColabCoordenador == codColaborador
                                        || t.Turma.Curso.Diretoria.CodColaboradorDiretor == codColaborador
                                        || t.Turma.Curso.Diretoria.Campus.CodColaboradorDiretor == codColaborador
                                        || t.Turma.Curso.Diretoria.Campus.Instituicao.Reitoria.Where(r => r.CodColaboradorReitor == codColaborador).Count() > 0).Count() > 0
                                    )
                            )
                        )
                        .OrderBy(a => a.Avaliacao.DtAplicacao)
                        .ToList();
                default:
                    return new List<AvalCertificacao>();
            }
        }

        public static List<AvalCertificacao> ListarAgendadaPorUsuario(Usuario usuario, DateTime inicio, DateTime termino)
        {
            var codPessoaFisica = usuario.CodPessoaFisica;
            switch (usuario.CodCategoria)
            {
                case 1:
                    return contexto.AvalCertificacao
                        .Where(a => a.PessoaFisica.FirstOrDefault(p => p.CodPessoa == codPessoaFisica) != null
                            && a.Avaliacao.DtAplicacao >= inicio && a.Avaliacao.DtAplicacao <= termino
                            && a.Avaliacao.AvalPessoaResultado.Count == 0
                            && !a.Avaliacao.FlagArquivo)
                        .OrderByDescending(ac => ac.Avaliacao.DtCadastro)
                        .ToList();
                case 2:
                    var codProfessor = usuario.Professor.First().CodProfessor;
                    return contexto.AvalCertificacao
                        .Where(a => a.CodProfessor == codProfessor
                            && a.Avaliacao.DtAplicacao >= inicio && a.Avaliacao.DtAplicacao <= termino
                            && a.Avaliacao.AvalPessoaResultado.Count == 0
                            && !a.Avaliacao.FlagArquivo)
                        .OrderBy(a => a.Avaliacao.DtAplicacao)
                        .ToList();
                case 3:
                    var codColaborador = usuario.Colaborador.First().CodColaborador;
                    return contexto.AvalCertificacao
                        .Where(a =>
                            a.Avaliacao.DtAplicacao >= inicio && a.Avaliacao.DtAplicacao <= termino
                            && a.Avaliacao.AvalPessoaResultado.Count == 0
                            && !a.Avaliacao.FlagArquivo &&
                            (
                                (a.PessoaFisica.FirstOrDefault(p => p.CodPessoa == codPessoaFisica) != null)
                                || (a.Professor.TurmaDiscProfHorario.Where(t => t.Turma.Curso.CodColabCoordenador == codColaborador
                                        || t.Turma.Curso.Diretoria.CodColaboradorDiretor == codColaborador
                                        || t.Turma.Curso.Diretoria.Campus.CodColaboradorDiretor == codColaborador
                                        || t.Turma.Curso.Diretoria.Campus.Instituicao.Reitoria.Where(r => r.CodColaboradorReitor == codColaborador).Count() > 0).Count() > 0
                                    )
                            )
                        )
                        .OrderBy(a => a.Avaliacao.DtAplicacao)
                        .ToList();
                default:
                    return new List<AvalCertificacao>();
            }
        }

        public static bool CorrigirQuestaoAluno(string codAvaliacao, string matrAluno, int codQuestao, double notaObtida, string profObservacao)
        {
            if (!String.IsNullOrEmpty(codAvaliacao) && !String.IsNullOrEmpty(matrAluno) && codQuestao != 0)
            {
                AvalCertificacao cert = ListarPorCodigoAvaliacao(codAvaliacao);

                int codPessoaFisica = int.Parse(matrAluno);

                AvalQuesPessoaResposta resposta = cert.Avaliacao.PessoaResposta.FirstOrDefault(pr => pr.CodQuestao == codQuestao && pr.CodPessoaFisica == codPessoaFisica);

                resposta.RespNota = notaObtida;
                resposta.ProfObservacao = profObservacao;

                cert.Avaliacao.AvalPessoaResultado
                    .Single(r => r.CodPessoaFisica == codPessoaFisica)
                    .Nota = cert.Avaliacao.PessoaResposta
                                .Where(pr => pr.CodPessoaFisica == codPessoaFisica)
                                .Average(pr => pr.RespNota);

                contexto.SaveChanges();

                return true;
            }

            return false;
        }
    }
}