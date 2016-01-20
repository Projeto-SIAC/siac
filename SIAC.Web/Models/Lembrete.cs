using SIAC.Helpers;
using System.Collections.Generic;

namespace SIAC.Models
{
    public class Lembrete
    {
        public const string NORMAL = "label";
        public const string POSITIVO = "green";
        public const string NEGATIVO = "red";
        public const string INFO = "blue";

        public static void AdicionarNotificacao(string mensagem, string estilo = NORMAL, int tempo = 5)
        {
            List<Dictionary<string, string>> notificacoes = (List<Dictionary<string, string>>)Sessao.Retornar("Notificacoes");

            if (notificacoes == null)
                notificacoes = new List<Dictionary<string, string>>();

            notificacoes.Add(new Dictionary<string, string> {
                { "Mensagem", mensagem },
                { "Estilo", estilo },
                { "Tempo", tempo.ToString() }
            });

            Sessao.Inserir("Notificacoes", notificacoes);
        }
    }
}