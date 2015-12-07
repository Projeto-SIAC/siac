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

        public static Dictionary<string, UsuarioAcesso> UsuarioAtivo = new Dictionary<string, UsuarioAcesso>();
        public static Dictionary<int, int> NumIdentificador = new Dictionary<int, int>();
        public static List<string> AlertarMudanca = new List<string>();
        public static Dictionary<string, string> TempDataUrlImage = new Dictionary<string, string>();
        public static int? ProxCodVisitante = null;

        public static bool Autenticado(string matricula) => !String.IsNullOrEmpty(matricula) && UsuarioAtivo.Keys.Contains(matricula) && UsuarioAtivo[matricula].IpAcesso == HttpContext.Current.RecuperarIp();
    }
}