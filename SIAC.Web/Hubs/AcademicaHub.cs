using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace SIAC.Web.Hubs
{
    public class AcademicaHub : Hub
    {
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
            avaliacoes.AddAcademica(codAvaliacao);
            avaliacoes.GetAcademica(codAvaliacao).AddProfessor(usrMatricula, Context.ConnectionId);
            foreach (var alnMatricula in avaliacoes.GetAcademica(codAvaliacao).GetAtivoMatriculaAluno())
            {
                Clients.Client(avaliacoes.GetAcademica(codAvaliacao).GetConnectionIdProfessor()).conectarAluno(alnMatricula);
            }
            //Groups.Add(Context.ConnectionId, "PRF" + codAvaliacao);
        }

        public void AlunoConectou(string codAvaliacao,string usrMatricula)
        {
            avaliacoes.AddAcademica(codAvaliacao);

            avaliacoes.GetAcademica(codAvaliacao).AddAluno(usrMatricula, Context.ConnectionId);

            avaliacoes.GetAcademica(codAvaliacao).AddEvento(usrMatricula, "green sign in", "Conectou");

            if (!String.IsNullOrEmpty(avaliacoes.GetAcademica(codAvaliacao).GetConnectionIdProfessor()))
            {
                Clients.Client(avaliacoes.GetAcademica(codAvaliacao).GetConnectionIdProfessor()).conectarAluno(usrMatricula);
            }
        }

        public void RequererAval(string codAvaliacao,string usrMatricula)
        {
            //Clients.Group("AVA" + codAvaliacao + "ALN" + usrMatricula).enviarAval(codAvaliacao);
            Clients.Client(avaliacoes.GetAcademica(codAvaliacao).GetConnectionIdAluno(usrMatricula)).enviarAval(codAvaliacao);
        }

        public void AvalEnviada(string codAvaliacao, string alnMatricula)
        {
            avaliacoes.GetAcademica(codAvaliacao).AddEvento(alnMatricula, "send outline", "Enviou printscreen");

            Clients.Client(avaliacoes.GetAcademica(codAvaliacao).GetConnectionIdProfessor()).receberAval(alnMatricula);
        }

        public void AtualizarAlunoProgresso(string codAvaliacao, string usrMatricula, int value)
        {
            if (!String.IsNullOrEmpty(avaliacoes.GetAcademica(codAvaliacao).GetConnectionIdProfessor()))
            {
                Clients.Client(avaliacoes.GetAcademica(codAvaliacao).GetConnectionIdProfessor()).atualizarProgresso(usrMatricula, value);
            }
        }

        public void ResponderQuestao(string codAvaliacao, string usrMatricula, int questao, bool flag)
        {
            if (!String.IsNullOrEmpty(avaliacoes.GetAcademica(codAvaliacao).GetConnectionIdProfessor()))
            {
                if (flag)
                {
                    avaliacoes.GetAcademica(codAvaliacao).AddEvento(usrMatricula, "write", "Respondeu questão");
                }
                else
                {
                    avaliacoes.GetAcademica(codAvaliacao).AddEvento(usrMatricula, "erase", "Retirou resposta questão");
                }
                Clients.Client(avaliacoes.GetAcademica(codAvaliacao).GetConnectionIdProfessor()).respondeuQuestao(usrMatricula, questao, flag);
            }
        }

        public void Alertar(string codAvaliacao, string mensagem, string alnMatricula)
        {
            if (!String.IsNullOrWhiteSpace(mensagem))
            {
                if (String.IsNullOrEmpty(alnMatricula))
                {
                    foreach (var matr in avaliacoes.GetAcademica(codAvaliacao).GetAtivoMatriculaAluno())
                    {
                        avaliacoes.GetAcademica(codAvaliacao).AddEvento(matr, "announcement", "Recebeu alerta geral");
                    }
                    Clients.Clients(avaliacoes.GetAcademica(codAvaliacao).GetAtivoConnectionIdAluno()).alertar(mensagem);
                }
                else
                {
                    avaliacoes.GetAcademica(codAvaliacao).AddEvento(alnMatricula, "announcement", "Recebeu alerta específico");
                    Clients.Client(avaliacoes.GetAcademica(codAvaliacao).GetConnectionIdAluno(alnMatricula)).alertar(mensagem);
                }
            }
        }

        public void Feed(string codAvaliacao, string alnMatricula)
        {
            if (avaliacoes.GetAcademica(codAvaliacao).GetAtivoMatriculaAluno().Contains(alnMatricula))
            {
                var lstEvento = avaliacoes.GetAcademica(codAvaliacao).GetFeed(alnMatricula).Select(e => new { Icone = e.Icone, Descricao = e.Descricao, DataCompleta = e.Data.ToBrazilianString(), Data = e.Data.ToElapsedTimeString() });
                Clients.Client(avaliacoes.GetAcademica(codAvaliacao).GetConnectionIdProfessor()).atualizarFeed(alnMatricula, lstEvento);
            }
        }

        public void FocoAvaliacao(string codAvaliacao, string alnMatricula, bool flag)
        {
            if (flag)
            {
                avaliacoes.GetAcademica(codAvaliacao).AddEvento(alnMatricula, "warning sign", "Estabeleceu o foco na avaliação");
            }
            else
            {
                avaliacoes.GetAcademica(codAvaliacao).AddEvento(alnMatricula, "red warning sign", "Perdeu o foco na avaliação");
            }
            Feed(codAvaliacao, alnMatricula);
        }
    }

    public class AcademicaMapping
    {
        private readonly Dictionary<string, Academica> academicas = new Dictionary<string, Academica>();

        public void AddAcademica(string codAvaliacao)
        {
            lock (academicas)
            {
                if (!academicas.ContainsKey(codAvaliacao))
                {
                    academicas.Add(codAvaliacao, new Academica());
                }
            }
        }

        public Academica GetAcademica(string codAvaliacao)
        {
            if (academicas.ContainsKey(codAvaliacao))
            {
                return academicas[codAvaliacao];
            }
            return null;
        }
    }

    public class Evento
    {
        public string Icone { get; set; }
        public string Descricao { get; set; }
        public DateTime Data { get; set; }
    }

    public class Academica
    {
        private readonly Dictionary<string, string> alunos = new Dictionary<string, string>();
        private readonly string[,] professor = new string[1,2];
        private readonly Dictionary<string, List<Evento>> feed = new Dictionary<string, List<Evento>>();

        public void AddProfessor(string matricula, string connectionId)
        {
            lock (professor)
            {
                professor[0, 0] = matricula;
                professor[0, 1] = connectionId;
            }
        }

        public void AddAluno(string matricula, string connectionId)
        {
            lock(alunos)
            {
                if (alunos.ContainsKey(matricula))
                {
                    alunos.Remove(matricula);
                }
                alunos.Add(matricula, connectionId);
                if (feed.ContainsKey(matricula))
                {
                    feed[matricula].Add(new Evento() { Icone = "green sign in", Descricao = "Reconectou", Data = DateTime.Now });
                }
                else { 
                    feed.Add(matricula, new List<Evento>());
                }
            }
        }

        public void AddEvento(string matricula, string icone, string descricao)
        {
            lock(feed)
            {
                if (alunos.ContainsKey(matricula))
                {
                    if (!feed.ContainsKey(matricula))
                    {
                        feed.Add(matricula, new List<Evento>());
                    }
                    feed[matricula].Add(new Evento() { Icone = icone, Descricao = descricao, Data = DateTime.Now });
                }
            }
        }

        public List<Evento> GetFeed(string matricula)
        {
            if (feed.ContainsKey(matricula))
            {
                return feed[matricula];
            }
            return null;
        }

        public string GetConnectionIdProfessor()
        {
            if (!String.IsNullOrEmpty(professor[0,1]))
            {
                return professor[0, 1];
            }
            return String.Empty;
        }

        public string GetConnectionIdAluno(string matricula)
        {
            if (alunos.ContainsKey(matricula))
            {
                return alunos[matricula];
            }
            return String.Empty;
        }

        public List<string> GetAtivoMatriculaAluno()
        {            
            return alunos.Keys.ToList();
        }

        public List<string> GetAtivoConnectionIdAluno()
        {
            return alunos.Values.ToList();
        }

        public void RemoveAluno(string matricula)
        {
            lock(alunos)
            {
                if (alunos.ContainsKey(matricula))
                {
                    alunos.Remove(matricula);
                }
            }
        }
    }
}