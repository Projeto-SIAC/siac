using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Helpers
{
    public class Sessao
    {
        private static HttpContext context
        {
            get { return HttpContext.Current; }
        }

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

        public static string UsuarioMatricula
        {
            get
            {
                if (context!=null)
                {
                    return (string)HttpContext.Current.Session["UsuarioMatricula"];
                }
                return String.Empty;
            }
        }

        public static string UsuarioNome
        {
            get
            {
                if (context != null)
                {
                    return (string)context.Session["UsuarioNome"];
                }
                return String.Empty;
            }
        }

        public static string UsuarioCategoria
        {
            get
            {
                if (context != null)
                {
                    return (string)HttpContext.Current.Session["UsuarioCategoria"];
                }
                return String.Empty;
            }
        }

        public static int UsuarioCategoriaCodigo
        {
            get
            {                
                if (context != null)
                {
                    return (int)context.Session["UsuarioCategoriaCodigo"];
                }
                return 0;
            }
        }

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

        public static void Inserir(string chave, object valor)
        {
            if (context != null)
            {
                context.Session[chave] = valor;
            }
        }

        public static object Retornar(string chave)
        {
            if (context != null)
            {
                return context.Session[chave];
            }
            return null;
        }

        public static void Limpar()
        {
            if (context != null)
            {
                context.Session.Clear();
            }
        }
    }
}