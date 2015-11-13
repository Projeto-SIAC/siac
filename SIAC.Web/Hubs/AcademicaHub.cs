using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace SIAC.Hubs
{
    public class AcademicaHub : Hub
    {
        #region Override

        public override Task OnDisconnected(bool stopCalled)
        {
            var connId = Context.ConnectionId;

            var acads = avaliacoes.ListarAcademicas();

            var aval = String.Empty;

            foreach (var key in acads.Keys)
            {
                if (acads[key].ListarConnectionIdAlunos().Contains(connId))
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

                if (mapping.SeTodosDesconectados())
                {
                    avaliacoes.RemoverAcademica(aval);
                }
                else
                {
                    var matr = mapping.SelecionarMatriculaPorAluno(connId);

                    if (!String.IsNullOrEmpty(matr))
                    {
                        if (!mapping.SeAlunoFinalizou(matr))
                        {
                            mapping.InserirEvento(matr, "red power", "Desconectou");

                            if (!String.IsNullOrEmpty(mapping.SelecionarConnectionIdProfessor()))
                            {
                                Clients.Client(mapping.SelecionarConnectionIdProfessor()).desconectarAluno(matr);
                            }
                        }
                    }
                }
            }
            return base.OnDisconnected(stopCalled);
        }

        #endregion

        private readonly static AcademicaMapping avaliacoes = new AcademicaMapping();

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
            avaliacoes.InserirAcademica(codAvaliacao);
            var mapping = avaliacoes.SelecionarAcademica(codAvaliacao);
            mapping.InserirProfessor(usrMatricula, Context.ConnectionId);
            
            foreach (var alnMatricula in mapping.ListarMatriculaAlunos())
            {
                foreach (var codQuestao in mapping.ListarQuestaoRespondidasPorAluno(alnMatricula))
                {
                    Clients.Client(mapping.SelecionarConnectionIdProfessor()).respondeuQuestao(alnMatricula, codQuestao, true);
                }
                Clients.Client(mapping.SelecionarConnectionIdProfessor()).listarChat(alnMatricula, mapping.ListarChat(alnMatricula));
                
                Clients.Client(mapping.SelecionarConnectionIdProfessor()).atualizarProgresso(alnMatricula, mapping.ListarQuestaoRespondidasPorAluno(alnMatricula).Count);
                Clients.Client(mapping.SelecionarConnectionIdProfessor()).conectarAluno(alnMatricula);
                if (mapping.SeAlunoFinalizou(alnMatricula))
                {
                    Clients.Client(mapping.SelecionarConnectionIdProfessor()).alunoFinalizou(alnMatricula);
                }
            }
        }

        public void AlunoConectou(string codAvaliacao, string usrMatricula)
        {
            avaliacoes.InserirAcademica(codAvaliacao);

            var mapping = avaliacoes.SelecionarAcademica(codAvaliacao);

            if (mapping.ListarMatriculaAlunos().Contains(usrMatricula))
            {
                mapping.InserirEvento(usrMatricula, "sign in", "Reconectou");
                mapping.InserirAluno(usrMatricula, Context.ConnectionId);
            }
            else
            {
                mapping.InserirAluno(usrMatricula, Context.ConnectionId);
                mapping.InserirEvento(usrMatricula, "green sign in", "Conectou");
            }


            if (!String.IsNullOrEmpty(mapping.SelecionarConnectionIdProfessor()))
            {
                foreach (var codQuestao in mapping.ListarQuestoes())
                {
                    Clients.Client(mapping.SelecionarConnectionIdProfessor()).respondeuQuestao(usrMatricula, codQuestao, false);
                }
                Clients.Client(mapping.SelecionarConnectionIdProfessor()).atualizarProgresso(usrMatricula, mapping.ListarQuestaoRespondidasPorAluno(usrMatricula).Count);
                Clients.Client(mapping.SelecionarConnectionIdProfessor()).conectarAluno(usrMatricula);
            }
        }

        public void RequererAval(string codAvaliacao,string usrMatricula)
        {
            Clients.Client(avaliacoes.SelecionarAcademica(codAvaliacao).SelecionarConnectionIdPorAluno(usrMatricula)).enviarAval(codAvaliacao);
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
                if (mapping.ListarQuestaoRespondidasPorAluno(usrMatricula).Contains(questao))
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

            mapping.AlterarAlunoQuestao(usrMatricula, questao, flag);

            if (!string.IsNullOrEmpty(mapping.SelecionarConnectionIdProfessor()))
            {
                Clients.Client(mapping.SelecionarConnectionIdProfessor()).atualizarProgresso(usrMatricula, mapping.ListarQuestaoRespondidasPorAluno(usrMatricula).Count);
                Clients.Client(mapping.SelecionarConnectionIdProfessor()).respondeuQuestao(usrMatricula, questao, flag);
            }
        }

        public void Alertar(string codAvaliacao, string mensagem, string alnMatricula)
        {
            if (!String.IsNullOrWhiteSpace(mensagem))
            {
                if (String.IsNullOrEmpty(alnMatricula))
                {
                    foreach (var matr in avaliacoes.SelecionarAcademica(codAvaliacao).ListarMatriculaAlunos())
                    {
                        avaliacoes.SelecionarAcademica(codAvaliacao).InserirEvento(matr, "announcement", "Recebeu alerta geral");
                    }
                    Clients.Clients(avaliacoes.SelecionarAcademica(codAvaliacao).ListarConnectionIdAlunos()).alertar(mensagem);
                }
                else
                {
                    avaliacoes.SelecionarAcademica(codAvaliacao).InserirEvento(alnMatricula, "announcement", "Recebeu alerta específico");
                    Clients.Client(avaliacoes.SelecionarAcademica(codAvaliacao).SelecionarConnectionIdPorAluno(alnMatricula)).alertar(mensagem);
                }
            }
        }

        public void Feed(string codAvaliacao, string alnMatricula)
        {
            if (avaliacoes.SelecionarAcademica(codAvaliacao).ListarMatriculaAlunos().Contains(alnMatricula))
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
            Clients.Client(avaliacoes.SelecionarAcademica(codAvaliacao).SelecionarConnectionIdPorAluno(usrMatricula)).chatAlunoRecebe(mensagem);
        }

        public void ChatAlunoEnvia(string codAvaliacao, string usrMatricula, string mensagem)
        {
            avaliacoes.SelecionarAcademica(codAvaliacao).InserirMensagem(usrMatricula, mensagem, true);
            Clients.Client(avaliacoes.SelecionarAcademica(codAvaliacao).SelecionarConnectionIdProfessor()).chatProfessorRecebe(usrMatricula, mensagem);
        }

        public void AlunoVerificando(string codAvaliacao, string usrMatricula)
        {
            var mapping = avaliacoes.SelecionarAcademica(codAvaliacao);
            mapping.InserirEvento(usrMatricula, "write square", "Verificando respostas");
        }

        public void AlunoFinalizou(string codAvaliacao, string usrMatricula)
        {
            var mapping = avaliacoes.SelecionarAcademica(codAvaliacao);
            mapping.InserirEvento(usrMatricula, "red sign out", "Finalizou");
            mapping.AlterarAlunoFlagFinalizou(usrMatricula);
            if (!String.IsNullOrEmpty(mapping.SelecionarConnectionIdProfessor()))
            {
                Clients.Client(mapping.SelecionarConnectionIdProfessor()).alunoFinalizou(usrMatricula);
            }
        }
    }

    public class AcademicaMapping
    {
        private readonly Dictionary<string, Academica> academicas = new Dictionary<string, Academica>();

        public Dictionary<string, Academica> ListarAcademicas()
        {
            return academicas;
        }

        public void InserirAcademica(string codAvaliacao)
        {
            lock (academicas)
            {
                if (!academicas.ContainsKey(codAvaliacao))
                {
                    academicas.Add(codAvaliacao, new Academica());
                    using (var e = new Models.dbSIACEntities()) {
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
                            int codTipoAvaliacao = e.TipoAvaliacao.SingleOrDefault(t=>t.Sigla == codigo).CodTipoAvaliacao;

                            Models.AvalAcademica avalAcademica = e.AvalAcademica.SingleOrDefault(acad => acad.Ano == ano && acad.Semestre == semestre && acad.NumIdentificador == numIdentificador && acad.CodTipoAvaliacao == codTipoAvaliacao);
                            
                            academicas[codAvaliacao].MapearQuestao(avalAcademica.Avaliacao.Questao.Select(q => q.CodQuestao).ToList());
                        }
                    }
                }
            }
        }

        public void RemoverAcademica(string codAvaliacao)
        {
            lock(academicas)
            {
                if (academicas.ContainsKey(codAvaliacao))
                {
                    academicas.Remove(codAvaliacao);
                }
            }
        }

        public Academica SelecionarAcademica(string codAvaliacao)
        {
            if (academicas.ContainsKey(codAvaliacao))
            {
                return academicas[codAvaliacao];
            }
            return null;
        }
    }    
}