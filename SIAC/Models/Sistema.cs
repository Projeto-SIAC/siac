using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Models
{
    public class Sistema
    {
        private static Random rnd = new Random();
        public static Random Random => rnd;

        public static Dictionary<string, string> CookieUsuario = new Dictionary<string, string>();
        public static Dictionary<string, UsuarioAcesso> UsuarioAtivo = new Dictionary<string, UsuarioAcesso>();
        public static Dictionary<int, int> NumIdentificador = new Dictionary<int, int>();
        public static List<string> AlertarMudanca = new List<string>();
        public static Dictionary<string, string> TempDataUrlImage = new Dictionary<string, string>();
        public static int? ProxCodVisitante = null;

        public static Dictionary<string, List<string>> AvaliacaoUsuario = new Dictionary<string, List<string>>();

        public static bool Autenticado(string matricula) => !String.IsNullOrEmpty(matricula) && UsuarioAtivo.Keys.Contains(matricula) && UsuarioAtivo[matricula].IpAcesso == HttpContext.Current.RecuperarIp();

        public static void RegistrarCookie(string matricula)
        {
            var cookie = HttpContext.Current.Request.Cookies["SIAC_Session"];
            if (cookie != null)
                CookieUsuario[cookie.Value] = matricula;
        }

        public static void RemoverCookie(string matricula)
        {
            string cookie = string.Empty;
            foreach (var chave in CookieUsuario.Keys)
            {
                if (CookieUsuario[chave] == matricula)
                {
                    cookie = chave;
                    break;
                }
            }
            if (!String.IsNullOrEmpty(cookie))
                CookieUsuario.Remove(cookie);
        }

        public static string GerarSenhaPadrao(Usuario usuario) => $"{usuario.PessoaFisica.PrimeiroNome.ToLower()}@{usuario.PessoaFisica.Cpf?.Substring(0, 3)}";

        // Notificações
        public static Dictionary<string, List<Dictionary<string, string>>> Notificacoes = new Dictionary<string, List<Dictionary<string, string>>>();

        // Simulados
        public static Dictionary<string, int> ProxInscricao = new Dictionary<string, int>();
    }
}