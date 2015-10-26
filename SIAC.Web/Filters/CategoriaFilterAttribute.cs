using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIAC.Filters
{
    public class CategoriaFilterAttribute : ActionFilterAttribute
    {
        public int[] Categorias { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!Helpers.Sessao.Autenticado)
            {
                filterContext.Result = new RedirectResult("~/Acesso");
            }
            else if (Categorias != null && !Categorias.Contains(Helpers.Sessao.UsuarioCategoriaCodigo))
            {
                filterContext.Result = new RedirectResult("~/Acesso");
            }
            base.OnActionExecuting(filterContext);
        }
    }
}