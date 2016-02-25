using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace SIAC.Hubs
{
    public class CertificacaoHub : Hub
    {
        #region Override

        public override Task OnDisconnected(bool stopCalled)
        {
            var connId = Context.ConnectionId;

            var acads = avaliacoes.ListarCertificacoes();

            var aval = String.Empty;

            foreach (var key in acads.Keys)
            {
                if (acads[key].ListarConnectionIdAvaliados().Contains(connId))
                {
                    aval = key;
                    break;
                }
                else if (acads[key].SelecionarConnectionIdProfessor() == connId)
                {
                    aval = key;
                    break;
                }
            }
            if (!String.IsNullOrEmpty(aval))
            {
                var mapping = avaliacoes.SelecionarAcademica(aval);
                mapping.DesconectarPorConnectionId(connId);
                var matr = mapping.SelecionarMatriculaPorAvaliado(connId);

                if (!String.IsNullOrEmpty(matr))
                {
                    if (Models.Sistema.AvaliacaoUsuario.ContainsKey(aval.ToLower()))
                    {
                        Models.Sistema.AvaliacaoUsuario[aval.ToLower()].Remove(matr.ToLower());
                    }
                }

                if (mapping.SeTodosDesconectados())
                {
                    avaliacoes.RemoverAcademica(aval);
                    if (Models.Sistema.AvaliacaoUsuario.ContainsKey(aval.ToLower()))
                    {
                        Models.Sistema.AvaliacaoUsuario.Remove(aval.ToLower());
                    }
                }
                else
                {
                    if (!String.IsNullOrEmpty(matr))
                    {
                        if (!mapping.SeAvaliadoFinalizou(matr))
                        {
                            mapping.InserirEvento(matr, "red power", "Desconectou");

                            if (!String.IsNullOrEmpty(mapping.SelecionarConnectionIdProfessor()))
                            {
                                Clients.Client(mapping.SelecionarConnectionIdProfessor()).desconectarAvaliado(matr);
                            }
                        }
                    }
                }
            }
            return base.OnDisconnected(stopCalled);
        }

        #endregion

        private readonly static CertificacaoMapping avaliacoes = new CertificacaoMapping();

        public void Realizar(string codAvaliacao)
        {
            Groups.Add(Context.ConnectionId, codAvaliacao);
        }

        public void Liberar(string codAvaliacao, bool flag)
        {
            if (flag)
            {
                Clients.Group(codAvaliacao).liberar(codAvaliacao);
            }
            else
            {
                Clients.Group(codAvaliacao).bloquear(codAvaliacao);
            }
        }

        public void ProfessorConectou(string codAvaliacao, string usrMatricula)
        {
            avaliacoes.InserirCertificacao(codAvaliacao);
            var mapping = avaliacoes.SelecionarAcademica(codAvaliacao);
            mapping.InserirProfessor(usrMatricula, Context.ConnectionId);

            foreach (var alnMatricula in mapping.ListarMatriculaAvaliados())
            {
                foreach (var codQuestao in mapping.ListarQuestaoRespondidasPorAvaliado(alnMatricula))
                {
                    Clients.Client(mapping.SelecionarConnectionIdProfessor()).respondeuQuestao(alnMatricula, codQuestao, true);
                }
                Clients.Client(mapping.SelecionarConnectionIdProfessor()).listarChat(alnMatricula, mapping.ListarChat(alnMatricula));

                Clients.Client(mapping.SelecionarConnectionIdProfessor()).atualizarProgresso(alnMatricula, mapping.ListarQuestaoRespondidasPorAvaliado(alnMatricula).Count);
                Clients.Client(mapping.SelecionarConnectionIdProfessor()).conectarAvaliado(alnMatricula);
                if (mapping.SeAvaliadoFinalizou(alnMatricula))
                {
                    Clients.Client(mapping.SelecionarConnectionIdProfessor()).avaliadoFinalizou(alnMatricula);
                }
            }
        }

        public void AvaliadoConectou(string codAvaliacao, string usrMatricula)
        {
            avaliacoes.InserirCertificacao(codAvaliacao);
            if (!Models.Sistema.AvaliacaoUsuario.ContainsKey(codAvaliacao.ToLower()))
            {
                Models.Sistema.AvaliacaoUsuario.Add(codAvaliacao.ToLower(), new List<string>());
            }
            Models.Sistema.AvaliacaoUsuario[codAvaliacao.ToLower()].Add(usrMatricula.ToLower());

            var mapping = avaliacoes.SelecionarAcademica(codAvaliacao);

            if (mapping.ListarMatriculaAvaliados().Contains(usrMatricula))
            {
                mapping.InserirEvento(usrMatricula, "sign in", "Reconectou");
                mapping.InserirAvaliado(usrMatricula, Context.ConnectionId);
            }
            else
            {
                mapping.InserirAvaliado(usrMatricula, Context.ConnectionId);
                mapping.InserirEvento(usrMatricula, "green sign in", "Conectou");
            }


            if (!String.IsNullOrEmpty(mapping.SelecionarConnectionIdProfessor()))
            {
                foreach (var codQuestao in mapping.ListarQuestoes())
                {
                    Clients.Client(mapping.SelecionarConnectionIdProfessor()).respondeuQuestao(usrMatricula, codQuestao, false);
                }
                Clients.Client(mapping.SelecionarConnectionIdProfessor()).atualizarProgresso(usrMatricula, mapping.ListarQuestaoRespondidasPorAvaliado(usrMatricula).Count);
                Clients.Client(mapping.SelecionarConnectionIdProfessor()).conectarAvaliado(usrMatricula);
            }
        }

        public void RequererAval(string codAvaliacao, string usrMatricula)
        {
            Clients.Client(avaliacoes.SelecionarAcademica(codAvaliacao).SelecionarConnectionIdPorAvaliado(usrMatricula)).enviarAval(codAvaliacao);
        }

        public void AvalEnviada(string codAvaliacao, string alnMatricula)
        {
            avaliacoes.SelecionarAcademica(codAvaliacao).InserirEvento(alnMatricula, "send outline", "Enviou printscreen");

            Clients.Client(avaliacoes.SelecionarAcademica(codAvaliacao).SelecionarConnectionIdProfessor()).receberAval(alnMatricula);
        }

        public void ResponderQuestao(string codAvaliacao, string usrMatricula, int questao, bool flag)
        {
            var mapping = avaliacoes.SelecionarAcademica(codAvaliacao);

            if (flag)
            {
                if (mapping.ListarQuestaoRespondidasPorAvaliado(usrMatricula).Contains(questao))
                {
                    mapping.InserirEvento(usrMatricula, "refresh", "Mudou a resposta da questão " + mapping.SelecionarIndiceQuestao(questao));
                }
                else
                {
                    mapping.InserirEvento(usrMatricula, "write", "Respondeu a questão " + mapping.SelecionarIndiceQuestao(questao));
                }
            }
            else
            {
                mapping.InserirEvento(usrMatricula, "erase", "Retirou resposta da questão " + mapping.SelecionarIndiceQuestao(questao));
            }

            mapping.AlterarAvaliadoQuestao(usrMatricula, questao, flag);

            if (!string.IsNullOrEmpty(mapping.SelecionarConnectionIdProfessor()))
            {
                Clients.Client(mapping.SelecionarConnectionIdProfessor()).atualizarProgresso(usrMatricula, mapping.ListarQuestaoRespondidasPorAvaliado(usrMatricula).Count);
                Clients.Client(mapping.SelecionarConnectionIdProfessor()).respondeuQuestao(usrMatricula, questao, flag);
            }
        }

        public void Alertar(string codAvaliacao, string mensagem, string alnMatricula)
        {
            if (!String.IsNullOrWhiteSpace(mensagem))
            {
                if (String.IsNullOrEmpty(alnMatricula))
                {
                    foreach (var matr in avaliacoes.SelecionarAcademica(codAvaliacao).ListarMatriculaAvaliados())
                    {
                        avaliacoes.SelecionarAcademica(codAvaliacao).InserirEvento(matr, "announcement", "Recebeu alerta geral");
                    }
                    Clients.Clients(avaliacoes.SelecionarAcademica(codAvaliacao).ListarConnectionIdAvaliados()).alertar(mensagem);
                }
                else
                {
                    avaliacoes.SelecionarAcademica(codAvaliacao).InserirEvento(alnMatricula, "announcement", "Recebeu alerta específico");
                    Clients.Client(avaliacoes.SelecionarAcademica(codAvaliacao).SelecionarConnectionIdPorAvaliado(alnMatricula)).alertar(mensagem);
                }
            }
        }

        public void Feed(string codAvaliacao, string alnMatricula)
        {
            if (avaliacoes.SelecionarAcademica(codAvaliacao).ListarMatriculaAvaliados().Contains(alnMatricula))
            {
                var lstEvento = avaliacoes.SelecionarAcademica(codAvaliacao).ListarFeed(alnMatricula).Select(e => new { Icone = e.Icone, Descricao = e.Descricao, DataCompleta = e.Data.ToBrazilianString(), Data = e.Data.ToElapsedTimeString() });
                if (!String.IsNullOrEmpty(avaliacoes.SelecionarAcademica(codAvaliacao).SelecionarConnectionIdProfessor()))
                {
                    Clients.Client(avaliacoes.SelecionarAcademica(codAvaliacao).SelecionarConnectionIdProfessor()).atualizarFeed(alnMatricula, lstEvento);
                }
            }
        }

        public void FocoAvaliacao(string codAvaliacao, string alnMatricula, bool flag)
        {
            if (flag)
            {
                avaliacoes.SelecionarAcademica(codAvaliacao).InserirEvento(alnMatricula, "warning sign", "Estabeleceu o foco na avaliação");
            }
            else
            {
                avaliacoes.SelecionarAcademica(codAvaliacao).InserirEvento(alnMatricula, "red warning sign", "Perdeu o foco na avaliação");
            }
            Feed(codAvaliacao, alnMatricula);
        }

        public void ChatProfessorEnvia(string codAvaliacao, string usrMatricula, string mensagem)
        {
            avaliacoes.SelecionarAcademica(codAvaliacao).InserirMensagem(usrMatricula, mensagem, false);
            Clients.Client(avaliacoes.SelecionarAcademica(codAvaliacao).SelecionarConnectionIdPorAvaliado(usrMatricula)).chatAvaliadoRecebe(mensagem);
        }

        public void ChatAvaliadoEnvia(string codAvaliacao, string usrMatricula, string mensagem)
        {
            avaliacoes.SelecionarAcademica(codAvaliacao).InserirMensagem(usrMatricula, mensagem, true);
            Clients.Client(avaliacoes.SelecionarAcademica(codAvaliacao).SelecionarConnectionIdProfessor()).chatProfessorRecebe(usrMatricula, mensagem);
        }

        public void AvaliadoVerificando(string codAvaliacao, string usrMatricula)
        {
            var mapping = avaliacoes.SelecionarAcademica(codAvaliacao);
            mapping.InserirEvento(usrMatricula, "write square", "Verificando respostas");
        }

        public void AvaliadoFinalizou(string codAvaliacao, string usrMatricula)
        {
            var mapping = avaliacoes.SelecionarAcademica(codAvaliacao);
            mapping.InserirEvento(usrMatricula, "red sign out", "Finalizou");
            mapping.AlterarAvaliadoFlagFinalizou(usrMatricula);
            if (!String.IsNullOrEmpty(mapping.SelecionarConnectionIdProfessor()))
            {
                Clients.Client(mapping.SelecionarConnectionIdProfessor()).avaliadoFinalizou(usrMatricula);
            }
        }

        public void Prorrogar(string codAvaliacao, int duracao, string observacao)
        {
            if (duracao > 0)
            {
                var mapping = avaliacoes.SelecionarAcademica(codAvaliacao);
                using (var e = new Models.dbSIACEntities())
                {
                    int numIdentificador = 0;
                    int semestre = 0;
                    int ano = 0;

                    if (codAvaliacao.Length == 13)
                    {
                        string codigo = codAvaliacao;
                        int.TryParse(codigo.Substring(codigo.Length - 4), out numIdentificador);
                        codigo = codigo.Remove(codigo.Length - 4);
                        int.TryParse(codigo.Substring(codigo.Length - 1), out semestre);
                        codigo = codigo.Remove(codigo.Length - 1);
                        int.TryParse(codigo.Substring(codigo.Length - 4), out ano);
                        codigo = codigo.Remove(codigo.Length - 4);
                        int codTipoAvaliacao = e.TipoAvaliacao.FirstOrDefault(t => t.Sigla == codigo).CodTipoAvaliacao;

                        Models.Avaliacao aval = e.Avaliacao.FirstOrDefault(acad => acad.Ano == ano && acad.Semestre == semestre && acad.NumIdentificador == numIdentificador && acad.CodTipoAvaliacao == codTipoAvaliacao);

                        var prorrogacao = new Models.AvaliacaoProrrogacao();
                        prorrogacao.DtProrrogacao = DateTime.Now;
                        prorrogacao.Observacao = String.IsNullOrWhiteSpace(observacao) ? null : observacao;
                        prorrogacao.DuracaoAnterior = aval.Duracao.Value;
                        prorrogacao.DuracaoNova = prorrogacao.DuracaoAnterior + duracao;
                        aval.Duracao = prorrogacao.DuracaoNova;
                        aval.AvaliacaoProrrogacao.Add(prorrogacao);

                        e.SaveChanges();

                        Clients.Clients(mapping.ListarConnectionIdAvaliados()).prorrogar(duracao);
                    }
                }
            }
        }
    }

    public class CertificacaoMapping
    {
        private readonly Dictionary<string, Avaliacao> certificacoes = new Dictionary<string, Avaliacao>();

        public Dictionary<string, Avaliacao> ListarCertificacoes()
        {
            return certificacoes;
        }

        public void InserirCertificacao(string codAvaliacao)
        {
            lock (certificacoes)
            {
                if (!certificacoes.ContainsKey(codAvaliacao))
                {
                    certificacoes.Add(codAvaliacao, new Avaliacao());
                    using (var e = new Models.dbSIACEntities())
                    {
                        int numIdentificador = 0;
                        int semestre = 0;
                        int ano = 0;

                        if (codAvaliacao.Length == 13)
                        {
                            string codigo = codAvaliacao;
                            int.TryParse(codigo.Substring(codigo.Length - 4), out numIdentificador);
                            codigo = codigo.Remove(codigo.Length - 4);
                            int.TryParse(codigo.Substring(codigo.Length - 1), out semestre);
                            codigo = codigo.Remove(codigo.Length - 1);
                            int.TryParse(codigo.Substring(codigo.Length - 4), out ano);
                            codigo = codigo.Remove(codigo.Length - 4);
                            int codTipoAvaliacao = e.TipoAvaliacao.FirstOrDefault(t => t.Sigla == codigo).CodTipoAvaliacao;

                            Models.AvalCertificacao avalCertificacao = e.AvalCertificacao.FirstOrDefault(acad => acad.Ano == ano && acad.Semestre == semestre && acad.NumIdentificador == numIdentificador && acad.CodTipoAvaliacao == codTipoAvaliacao);

                            certificacoes[codAvaliacao].MapearQuestao(avalCertificacao.Avaliacao.Questao.Select(q => q.CodQuestao).ToList());
                        }
                    }
                }
            }
        }

        public void RemoverAcademica(string codAvaliacao)
        {
            lock (certificacoes)
            {
                if (certificacoes.ContainsKey(codAvaliacao))
                {
                    certificacoes.Remove(codAvaliacao);
                }
            }
        }

        public Avaliacao SelecionarAcademica(string codAvaliacao)
        {
            if (certificacoes.ContainsKey(codAvaliacao))
            {
                return certificacoes[codAvaliacao];
            }
            return null;
        }
    }
}