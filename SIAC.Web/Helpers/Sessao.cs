using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Web.Helpers
{
    public class Sessao
    {
        public static bool Autenticado
        {
            get
            {
                return HttpContext.Current.Session["Autenticado"] != null ? (bool)HttpContext.Current.Session["Autenticado"] : false;
            }
        }

        public static string UsuarioMatricula
        {
            get
            {
                return (string)HttpContext.Current.Session["UsuarioMatricula"];
            }
        }

        public static string UsuarioNome
        {
            get
            {
                return (string)HttpContext.Current.Session["UsuarioNome"];
            }
        }

        public static string UsuarioCategoria
        {
            get
            {
                return (string)HttpContext.Current.Session["UsuarioCategoria"];
            }
        }

        public static int UsuarioCategoriaCodigo
        {
            get
            {
                return (int)HttpContext.Current.Session["UsuarioCategoriaCodigo"];
            }
        }

        public static void Inserir(string chave, object valor)
        {
            HttpContext.Current.Session[chave] = valor;
        }

        public static object Retornar(string chave)
        {
            return HttpContext.Current.Session[chave];
        }

        public static void Limpar()
        {
            HttpContext.Current.Session.Clear();
        }
    }
}