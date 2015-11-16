using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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

        private static dbSIACEntities contexto { get { return Repositorio.GetInstance(); } }

        public static void Inserir(AvalCertificacao avalCertificacao)
        {
            contexto.AvalCertificacao.Add(avalCertificacao);
            contexto.SaveChanges();
        }

        public static bool AlternarLiberar(string codAvaliacao)
        {
            AvalCertificacao avalAcad = ListarPorCodigoAvaliacao(codAvaliacao);
            avalAcad.Avaliacao.FlagLiberada = !avalAcad.Avaliacao.FlagLiberada;
            contexto.SaveChanges();
            return avalAcad.Avaliacao.FlagLiberada;
        }

        public static List<AvalCertificacao> ListarCorrecaoPendentePorProfessor(int codProfessor)
        {
            return contexto.AvalQuesPessoaResposta
                .Where(a => a.AvalTemaQuestao.AvaliacaoTema.Avaliacao.AvalCertificacao.CodProfessor == codProfessor && !a.RespNota.HasValue)
                .OrderBy(a => a.AvalTemaQuestao.AvaliacaoTema.Avaliacao.DtAplicacao)
                .Select(a => a.AvalTemaQuestao.AvaliacaoTema.Avaliacao.AvalCertificacao)
                .Distinct()
                .ToList();
        }

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

        public static List<AvalCertificacao> ListarPorPessoa(int codPessoaFisica)
        {
            return contexto.AvalCertificacao.Where(ac => ac.PessoaFisica.FirstOrDefault(p => p.CodPessoa == codPessoaFisica) != null)
                                         .OrderByDescending(ac => ac.Avaliacao.DtCadastro)
                                         .ToList();
        }

        public static List<AvalCertificacao> ListarPorProfessor(int codProfessor)
        {
            return contexto.AvalCertificacao.Where(ac => ac.CodProfessor == codProfessor)
                                         .OrderByDescending(ac => ac.Avaliacao.DtCadastro)
                                         .ToList();
        }

        public static List<AvalCertificacao> ListarAgendada()
        {
            return contexto.AvalCertificacao
                .Where(a => a.Avaliacao.DtAplicacao.HasValue
                    && a.Avaliacao.AvalPessoaResultado.Count == 0
                    && !a.Avaliacao.FlagArquivo)
                .OrderBy(a => a.Avaliacao.DtAplicacao)
                .ToList();
        }

        public static List<AvalCertificacao> ListarAgendadaPorProfessor(int codProfessor)
        {
            return ListarAgendada()
                .Where(a => a.CodProfessor == codProfessor)
                .OrderBy(a => a.Avaliacao.DtAplicacao)
                .ToList();
        }

        public static List<AvalCertificacao> ListarAgendadaPorPessoa(int codPessoaFisica)
        {
            return ListarAgendada()
                .Where(ac => ac.PessoaFisica.FirstOrDefault(p => p.CodPessoa == codPessoaFisica) != null)
                .OrderByDescending(ac => ac.Avaliacao.DtCadastro)
                .ToList();
        }

        public static List<AvalCertificacao> ListarAgendadaPorColaborador(int codColaborador)
        {
            return ListarAgendada()
                .Where(a =>
                        a.Professor.TurmaDiscProfHorario.Where(t => t.Turma.Curso.CodColabCoordenador == codColaborador
                        || t.Turma.Curso.Diretoria.CodColaboradorDiretor == codColaborador
                        || t.Turma.Curso.Diretoria.Campus.CodColaboradorDiretor == codColaborador
                        || t.Turma.Curso.Diretoria.Campus.Instituicao.Reitoria.Where(r => r.CodColaboradorReitor == codColaborador).Count() > 0).Count() > 0)
                .OrderBy(a => a.Avaliacao.DtAplicacao)
                .ToList();
        }

        public static List<AvalCertificacao> ListarAgendadaPorUsuario(Usuario usuario)
        {
            switch (usuario.CodCategoria)
            {
                case 1:
                    return ListarAgendadaPorPessoa(usuario.CodPessoaFisica);
                case 2:
                    return Models.AvalCertificacao.ListarAgendadaPorProfessor(usuario.Professor.First().CodProfessor);
                case 3:
                    return Models.AvalCertificacao.ListarAgendadaPorPessoa(usuario.CodPessoaFisica)
                        .Union(Models.AvalCertificacao.ListarAgendadaPorColaborador(usuario.Colaborador.First().CodColaborador))
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