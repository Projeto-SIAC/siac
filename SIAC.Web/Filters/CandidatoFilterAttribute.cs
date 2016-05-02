using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIAC.Filters
{
    public class CandidatoFilterAttribute : ActionFilterAttribute
    {
        private ActionResult Redirecionar(ActionExecutingContext filterContext, string url = null)
        {
            if (filterContext.HttpContext.Request.HttpMethod == "GET")
            {
                if (string.IsNullOrEmpty(url))
                {
                    return new RedirectResult("~/simulado/candidato/acessar?continuar=" + filterContext.HttpContext.Request.Path);
                }
                else
                {
                    return new RedirectResult(url);
                }
            }
            else
            {
                return new JsonResult();
            }
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var autenticado = Helpers.Sessao.Candidato != null;

            if (!autenticado)
            {
                filterContext.Result = Redirecionar(filterContext);
            }

            base.OnActionExecuting(filterContext);
        }

    }
}