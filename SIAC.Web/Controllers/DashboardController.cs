using SIAC.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIAC.Web.Controllers
{
    public class DashboardController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!Helpers.Sessao.Autenticado)
            {
                filterContext.Result = RedirectToAction("Entrar", "Acesso");
            }
            base.OnActionExecuting(filterContext);
        }

        // GET: Dashboard
        public ActionResult Index()
        {
            Usuario usuario = Usuario.ListarPorMatricula(Helpers.Sessao.UsuarioMatricula);
            return View(usuario);            
        }
       
    }
}