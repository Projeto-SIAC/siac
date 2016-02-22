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
            Sistema.Notificacoes[Sessao.UsuarioMatricula].Add(new Dictionary<string, string> {
                { "Mensagem", mensagem },
                { "Estilo", estilo },
                { "Tempo", tempo.ToString() }
            });
        }
    }
}