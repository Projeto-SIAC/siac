using System;
using System.Web;

namespace SIAC.Helpers
{
    public class Sessao
    {
        private static HttpContext context => HttpContext.Current;

        public static bool RealizandoAvaliacao
        {
            get
            {
                if (context != null)
                {
                    return context.Session["RealizandoAvaliacao"] != null ? (bool)HttpContext.Current.Session["RealizandoAvaliacao"] : false;
                }
                return false;
            }
        }

        public static string UsuarioMatricula => (string)context?.Session["UsuarioMatricula"] ?? String.Empty;

        public static string UsuarioNome => (string)context?.Session["UsuarioNome"] ?? String.Empty;

        public static string UsuarioCategoria => (string)context?.Session["UsuarioCategoria"] ?? String.Empty;

        public static int UsuarioCategoriaCodigo => (int)context?.Session["UsuarioCategoriaCodigo"];

        public static string UsuarioAvaliacao
        {
            get
            {
                if (context != null)
                {
                    return (string)context.Session["UsuarioAvaliacao"];
                }
                return String.Empty;
            }
        }

        public static void Inserir(string chave, object valor) => context.Session[chave] = valor;

        public static object Retornar(string chave) => context?.Session[chave];

        public static void Limpar() => context?.Session.Clear();
    }
}