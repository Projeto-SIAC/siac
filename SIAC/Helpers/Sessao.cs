using SIAC.Models;
using System;
using System.Web;

namespace SIAC.Helpers
{
    public class Sessao
    {
        private static HttpContextBase context => HttpContextManager.Current;

        public static bool RealizandoAvaliacao
        {
            get
            {
                foreach (var avaliacao in Sistema.AvaliacaoUsuario.Keys)
                    if (Sistema.AvaliacaoUsuario[avaliacao].Contains(UsuarioMatricula))
                        return true;
                return false;
            }
        }

        public static bool Apresentacao => Retornar("Apresentacao") != null ? (bool)Retornar("Apresentacao") : false;

        public static bool AjudaEstado => Retornar("AjudaEstado") != null ? (bool)Retornar("AjudaEstado") : false;

        public static string UsuarioMatricula => (string)Retornar("UsuarioMatricula") ?? String.Empty;

        public static string UsuarioNome => (string)Retornar("UsuarioNome") ?? String.Empty;

        public static string UsuarioCategoria => (string)Retornar("UsuarioCategoria") ?? String.Empty;

        public static int UsuarioCategoriaCodigo => (int)Retornar("UsuarioCategoriaCodigo");

        public static bool UsuarioSenhaPadrao => Retornar("UsuarioSenhaPadrao") != null ? (bool)Retornar("UsuarioSenhaPadrao") : false;

        public static Candidato Candidato => (Candidato)Retornar("SimuladoCandidato") ?? null;

        public static void Inserir(string chave, object valor)
        {
            if (context?.Session != null) context.Session[chave] = valor;
        }

        public static object Retornar(string chave)
        {
            if (context?.Session != null)
            {
                return context?.Session[chave];
            }
            return null;
        }

        public static void Remover(string chave) => context?.Session?.Remove(chave);

        public static void Limpar() => context?.Session.Clear();
    }
}