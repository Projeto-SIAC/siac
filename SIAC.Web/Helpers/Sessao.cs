using System;
using System.Web;
using SIAC.Models;

namespace SIAC.Helpers
{
    public class Sessao
    {
        private static HttpContext context => HttpContext.Current;

        public static bool RealizandoAvaliacao
        {
            get
            {
                foreach (string avaliacao in Sistema.AvaliacaoUsuario.Keys)
                    if (Sistema.AvaliacaoUsuario[avaliacao].Contains(UsuarioMatricula))
                        return true;
                return false;
            }
        }

        public static bool Apresentacao => context?.Session["Apresentacao"] != null ? (bool)context?.Session["Apresentacao"] : false;

        public static bool AjudaEstado => context?.Session["AjudaEstado"] != null ? (bool)context?.Session["AjudaEstado"] : false;

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