using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace SIAC.Web.Hubs
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
            }
            if (!String.IsNullOrEmpty(aval))
            {
                var mapping = avaliacoes.SelecionarAcademica(aval);
                
                var matr = mapping.SelecionarMatriculaPorAluno(connId);

                if (!String.IsNullOrEmpty(matr))
                {
                    mapping.InserirEvento(matr, "red power", "Desconectou");
                    if (!String.IsNullOrEmpty(mapping.SelecionarConnectionIdProfessor()))
                    {
                        Clients.Client(mapping.SelecionarConnectionIdProfessor()).desconectarAluno(matr);
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
                // ListarChat()
                Clients.Client(mapping.SelecionarConnectionIdProfessor()).atualizarProgresso(alnMatricula, mapping.ListarQuestaoRespondidasPorAluno(alnMatricula).Count);
                Clients.Client(mapping.SelecionarConnectionIdProfessor()).conectarAluno(alnMatricula);
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
                // ListarChat()
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
            mapping.AlterarAlunoFlagFinalizou(usrMatricula);
            mapping.InserirEvento(usrMatricula, "red sign out", "Finalizou");
            Clients.Client(mapping.SelecionarConnectionIdProfessor()).alunoFinalizou(usrMatricula);
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

    public class Evento
    {
        public string Icone { get; set; }
        public string Descricao { get; set; }
        public DateTime Data { get; set; }
    }

    public class Mensagem
    {
        public string Texto { get; set; }
        public bool FlagAutor { get; set; }
    }

    public class Aluno
    {
        public string ConnectionId { get; set; }
        public bool FlagFinalizou { get; set; }
        public List<Evento> Feed { get; set; }
        public Dictionary<int, bool> Questoes { get; set; }
        public List<Mensagem> Chat { get; set; }
    }

    public class Professor
    {
        public string ConnectionId { get; set; }
    }

    public class Academica
    {
        // _professor: key = Matricula, value = ConnectionId
        // _alunos: key = Matricula, value = { ConnectionId, FlagFinalizou, Feed, Questoes: key = CodQuestao, value = FlagRespondido, Chat = [{ Texto, FlagAutor }] }
        // _questaoMapa: key = CodQuestao, value = Indice em Avaliacao.Questao
        private KeyValuePair<string, Professor> _professor = new KeyValuePair<string, Professor>();
        private Dictionary<string, Aluno> _alunos = new Dictionary<string, Aluno>();
        private Dictionary<int, string> _questaoMapa = new Dictionary<int, string>();

        public List<int> ListarQuestoes()
        {
            return _questaoMapa.Keys.ToList();
        }

        public void MapearQuestao(List<int> lstCodQuestao)
        {
            if (_questaoMapa.Count == 0)
            {
                for (int i = 0, length = lstCodQuestao.Count; i < length; i++)
                {
                    _questaoMapa.Add(lstCodQuestao[i], i.GetIndiceQuestao());
                }
            }
        }

        public string SelecionarIndiceQuestao(int codQuestao)
        {
            if (_questaoMapa.Count > 0)
            {
                return _questaoMapa[codQuestao];
            }
            return null;
        } 

        public void InserirProfessor(string matricula, string connectionId)
        {
            _professor = new KeyValuePair<string, Professor>(matricula, new Professor { ConnectionId = connectionId });
        }

        public void InserirAluno(string matricula, string connectionId)
        {
            List<Evento> lstEvento = new List<Evento>();
            List<Mensagem> lstMensagem = new List<Mensagem>();            
            if (_alunos.ContainsKey(matricula))
            {
                lstEvento = _alunos[matricula].Feed;
                _alunos.Remove(matricula);
            }
            _alunos.Add(matricula, new Aluno { ConnectionId = connectionId, FlagFinalizou = false, Feed = lstEvento, Chat = lstMensagem,  Questoes = new Dictionary<int, bool>() });
            for (int i = 0, length = _questaoMapa.Count; i < length; i++)
            {
                _alunos[matricula].Questoes.Add(_questaoMapa.Keys.ElementAt(i), false);
            }                        
        }

        public void InserirEvento(string matricula, string icone, string descricao)
        {
            if (_alunos.ContainsKey(matricula))
            {
                _alunos[matricula].Feed.Add(new Evento() { Icone = icone, Descricao = descricao, Data = DateTime.Now });
            }
        }

        public void InserirMensagem(string matricula, string mensagem, bool flagAutor)
        {
            if (_alunos.ContainsKey(matricula))
            {
                _alunos[matricula].Chat.Add(new Mensagem() { Texto = mensagem, FlagAutor = flagAutor });
            }
        }

        public List<Evento> ListarFeed(string matricula)
        {
            if (_alunos.ContainsKey(matricula))
            {
                return _alunos[matricula].Feed;
            }
            return null;
        }

        public List<Mensagem> ListarChat(string matricula)
        {
            if (_alunos.ContainsKey(matricula))
            {
                return _alunos[matricula].Chat;
            }
            return null;
        }

        public string SelecionarConnectionIdProfessor()
        {
            if (_professor.Key != null)
            {
                return _professor.Value.ConnectionId;
            }
            return null;
        }

        public string SelecionarConnectionIdPorAluno(string matricula)
        {
            if (_alunos.ContainsKey(matricula))
            {
                return _alunos[matricula].ConnectionId;
            }
            return null;
        }

        public string SelecionarMatriculaPorAluno(string connectionId)
        {
            foreach (var key in _alunos.Keys)
            {
                if (_alunos[key].ConnectionId == connectionId)
                {
                    return key;
                }
            }
            return null;
        }

        public List<string> ListarMatriculaAlunos()
        {            
            return _alunos.Keys.ToList();
        }

        public List<string> ListarConnectionIdAlunos()
        {
            return _alunos.Values.Select(a=>a.ConnectionId).ToList();
        }

        public void AlterarAlunoQuestao(string matricula, int codQuestao, bool flag)
        {
            if (_alunos.ContainsKey(matricula))
            {
                _alunos[matricula].Questoes[codQuestao] = flag;
            }
        }

        public List<int> ListarQuestaoRespondidasPorAluno(string matricula)
        {
            if (_alunos.ContainsKey(matricula))
            {
                return _alunos[matricula].Questoes.Where(q => q.Value == true).Select(q=>q.Key).ToList();
            }
            return null;
        }

        public void AlterarAlunoFlagFinalizou(string matricula)
        {
            if (_alunos.ContainsKey(matricula))
            {
                _alunos[matricula].FlagFinalizou = true;
            }
        }

        public bool ContemAluno(string matricula)
        {
            return _alunos.ContainsKey(matricula);
        }

        public void RemoverAluno(string matricula)
        {
            if (_alunos.ContainsKey(matricula))
            {
                _alunos.Remove(matricula);
            }            
        }
    }
}