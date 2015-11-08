using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIAC.Filters
{
    public class AutenticacaoFilterAttribute : ActionFilterAttribute
    {
        public int[] Categorias { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!Helpers.Sessao.Autenticado)
            {
                filterContext.Result = new RedirectResult("~/?continuar="+filterContext.HttpContext.Request.Path);
            }
            else if (Categorias != null && !Categorias.Contains(Helpers.Sessao.UsuarioCategoriaCodigo))
            {
                filterContext.Result = new RedirectResult("~/?continuar="+filterContext.HttpContext.Request.Path);
            }
            else if (Helpers.Sessao.RealizandoAvaliacao)
            {
                string[] paths = filterContext.HttpContext.Request.Path.ToLower().Split('/');
                if (paths.Length > 0)
                {
                    string codigo = paths[paths.Length - 1];
                    if ((!paths.Contains("desistir") || !paths.Contains("resultado") || !paths.Contains("realizar")) && codigo != Helpers.Sessao.UsuarioAvaliacao) 
                    {
                        filterContext.Result = new RedirectResult("~/dashboard/avaliacao/academica/realizar/" + Helpers.Sessao.UsuarioAvaliacao);
                    }
                }
                else
                {
                    filterContext.Result = new RedirectResult("~/dashboard/avaliacao/academica/realizar/" + Helpers.Sessao.UsuarioAvaliacao);
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}