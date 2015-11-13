using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Hubs
{
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
        public bool FlagConectado { get; set; }
        public bool FlagFinalizou { get; set; }
        public List<Evento> Feed { get; set; }
        public Dictionary<int, bool> Questoes { get; set; }
        public List<Mensagem> Chat { get; set; }
    }

    public class Professor
    {
        public string ConnectionId { get; set; }
        public bool FlagConectado { get; set; }
    }

    public class Academica
    {
        // _professor: key = Matricula, value = { ConnectionId, FlagConectado }
        // _alunos: key = Matricula, value = { ConnectionId, FlagConectado, FlagFinalizou, Feed, Questoes: key = CodQuestao, value = FlagRespondido, Chat = [{ Texto, FlagAutor }] }
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
            _professor = new KeyValuePair<string, Professor>(matricula, new Professor { ConnectionId = connectionId, FlagConectado = true });
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
            _alunos.Add(matricula, new Aluno { ConnectionId = connectionId, FlagConectado = true, FlagFinalizou = false, Feed = lstEvento, Chat = lstMensagem, Questoes = new Dictionary<int, bool>() });
            for (int i = 0, length = _questaoMapa.Count; i < length; i++)
            {
                _alunos[matricula].Questoes.Add(_questaoMapa.Keys.ElementAt(i), false);
            }
        }

        public void InserirEvento(string matricula, string icone, string descricao)
        {
            if (_alunos.ContainsKey(matricula) && !SeAlunoFinalizou(matricula))
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

        public bool SeAlunoFinalizou(string matricula)
        {
            if (_alunos.ContainsKey(matricula))
            {
                return _alunos[matricula].FlagFinalizou;
            }
            return false;
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
            return _alunos.Values.Select(a => a.ConnectionId).ToList();
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
                return _alunos[matricula].Questoes.Where(q => q.Value == true).Select(q => q.Key).ToList();
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

        public void DesconectarPorConnectionId(string connectionId)
        {
            foreach (var key in _alunos.Keys)
            {
                if (_alunos[key].ConnectionId == connectionId)
                {
                    _alunos[key].FlagConectado = false;
                    return;
                }
            }
            if (_professor.Value.ConnectionId == connectionId)
            {
                _professor.Value.FlagConectado = false;
            }
        }

        public bool SeTodosDesconectados()
        {
            if (_professor.Key != null && _professor.Value.FlagConectado == true)
            {
                return false;
            }
            foreach (var key in _alunos.Keys)
            {
                if (_alunos[key].FlagConectado == true)
                {
                    return false;
                }
            }
            return true;
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