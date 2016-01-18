using System;
using System.Web;

namespace SIAC.Helpers
{
    public class Sessao
    {
        private static HttpContext context => HttpContext.Current;

        //public static bool RealizandoAvaliacao => context?.Session["RealizandoAvaliacao"] != null ? (bool)context.Session["RealizandoAvaliacao"] : false;

        public static bool RealizandoAvaliacao
        {
            get
            {
                foreach (var avaliacao in Models.Sistema.AvaliacaoUsuario.Keys)
                {
                    if (Models.Sistema.AvaliacaoUsuario[avaliacao].Contains(UsuarioMatricula))
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public static string UsuarioMatricula => (string)context?.Session["UsuarioMatricula"] ?? String.Empty;

        public static string UsuarioNome => (string)context?.Session["UsuarioNome"] ?? String.Empty;

        public static string UsuarioCategoria => (string)context?.Session["UsuarioCategoria"] ?? String.Empty;

        public static int UsuarioCategoriaCodigo => (int)context?.Session["UsuarioCategoriaCodigo"];

        public static bool UsuarioSenhaPadrao => context?.Session["UsuarioSenhaPadrao"] != null ? (bool)context?.Session["UsuarioSenhaPadrao"] : false;

        public static void Inserir(string chave, object valor) { if (context != null) context.Session[chave] = valor; } 

        public static object Retornar(string chave) => context?.Session[chave];

        public static void Limpar() => context?.Session.Clear();
    }
}