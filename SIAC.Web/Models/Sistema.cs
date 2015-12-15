using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Models
{
    public class Sistema
    {
        public const int CodOcupacaoCoordenadorAvi = 5;
        public const int CodOcupacaoAdministradorSIAC = 6;

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
            {
                CookieUsuario[cookie.Value] = matricula;
            }
        }

        public static void RemoverCookie(string matricula)
        {
            var cookie = string.Empty;
            foreach (var key in CookieUsuario.Keys)
            {
                if (CookieUsuario[key] == matricula)
                {
                    cookie = key;
                    break;
                }
            }
            if (!String.IsNullOrEmpty(cookie))
            {
                CookieUsuario.Remove(cookie);
            }
        }
    }
}