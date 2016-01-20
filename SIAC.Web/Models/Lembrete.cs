using SIAC.Helpers;
using System.Collections.Generic;

namespace SIAC.Models
{
    public class Lembrete
    {
        public const string Normal = "label";
        public const string Positivo = "green";
        public const string Negativo = "red";
        public const string Info = "blue";

        public static void AdicionarNotificacao(string mensagem, string estilo = Normal, int tempo = 5)
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