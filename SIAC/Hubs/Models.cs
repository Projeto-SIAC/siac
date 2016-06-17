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

    public class Avaliado
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

    public class Avaliacao
    {
        private KeyValuePair<string, Professor> _professor = new KeyValuePair<string, Professor>();
        private Dictionary<string, Avaliado> _avaliados = new Dictionary<string, Avaliado>();
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

        public void InserirAvaliado(string matricula, string connectionId)
        {
            List<Evento> lstEvento = new List<Evento>();
            List<Mensagem> lstMensagem = new List<Mensagem>();
            if (_avaliados.ContainsKey(matricula))
            {
                lstEvento = _avaliados[matricula].Feed;
                _avaliados.Remove(matricula);
            }
            _avaliados.Add(matricula, new Avaliado { ConnectionId = connectionId, FlagConectado = true, FlagFinalizou = false, Feed = lstEvento, Chat = lstMensagem, Questoes = new Dictionary<int, bool>() });
            for (int i = 0, length = _questaoMapa.Count; i < length; i++)
            {
                _avaliados[matricula].Questoes.Add(_questaoMapa.Keys.ElementAt(i), false);
            }
        }

        public void InserirEvento(string matricula, string icone, string descricao)
        {
            if (_avaliados.ContainsKey(matricula) && !SeAvaliadoFinalizou(matricula))
            {
                _avaliados[matricula].Feed.Add(new Evento() { Icone = icone, Descricao = descricao, Data = DateTime.Now });
            }
        }

        public void InserirMensagem(string matricula, string mensagem, bool flagAutor)
        {
            if (_avaliados.ContainsKey(matricula))
            {
                _avaliados[matricula].Chat.Add(new Mensagem() { Texto = mensagem, FlagAutor = flagAutor });
            }
        }

        public List<Evento> ListarFeed(string matricula)
        {
            if (_avaliados.ContainsKey(matricula))
            {
                return _avaliados[matricula].Feed;
            }
            return null;
        }

        public List<Mensagem> ListarChat(string matricula)
        {
            if (_avaliados.ContainsKey(matricula))
            {
                return _avaliados[matricula].Chat;
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

        public string SelecionarConnectionIdPorAvaliado(string matricula)
        {
            if (_avaliados.ContainsKey(matricula))
            {
                return _avaliados[matricula].ConnectionId;
            }
            return null;
        }

        public bool SeAvaliadoFinalizou(string matricula)
        {
            if (_avaliados.ContainsKey(matricula))
            {
                return _avaliados[matricula].FlagFinalizou;
            }
            return false;
        }

        public string SelecionarMatriculaPorAvaliado(string connectionId)
        {
            foreach (var key in _avaliados.Keys)
            {
                if (_avaliados[key].ConnectionId == connectionId)
                {
                    return key;
                }
            }
            return null;
        }

        public List<string> ListarMatriculaAvaliados()
        {
            return _avaliados.Keys.ToList();
        }

        public List<string> ListarConnectionIdAvaliados()
        {
            return _avaliados.Values.Select(a => a.ConnectionId).ToList();
        }

        public void AlterarAvaliadoQuestao(string matricula, int codQuestao, bool flag)
        {
            if (_avaliados.ContainsKey(matricula))
            {
                _avaliados[matricula].Questoes[codQuestao] = flag;
            }
        }

        public List<int> ListarQuestaoRespondidasPorAvaliado(string matricula)
        {
            if (_avaliados.ContainsKey(matricula))
            {
                return _avaliados[matricula].Questoes.Where(q => q.Value == true).Select(q => q.Key).ToList();
            }
            return null;
        }

        public void AlterarAvaliadoFlagFinalizou(string matricula)
        {
            if (_avaliados.ContainsKey(matricula))
            {
                _avaliados[matricula].FlagFinalizou = true;
            }
        }

        public void DesconectarPorConnectionId(string connectionId)
        {
            foreach (var key in _avaliados.Keys)
            {
                if (_avaliados[key].ConnectionId == connectionId)
                {
                    _avaliados[key].FlagConectado = false;
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
            foreach (var key in _avaliados.Keys)
            {
                if (_avaliados[key].FlagConectado == true)
                {
                    return false;
                }
            }
            return true;
        }

        public bool ContemAvaliado(string matricula)
        {
            return _avaliados.ContainsKey(matricula);
        }

        public void RemoverAvaliado(string matricula)
        {
            if (_avaliados.ContainsKey(matricula))
            {
                _avaliados.Remove(matricula);
            }
        }
    }
}