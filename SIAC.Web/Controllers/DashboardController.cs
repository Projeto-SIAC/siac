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
            TempData["UrlReferrer"] = Request.Url.ToString();
            if (Session["Autenticado"] == null)
            {
                filterContext.Result = RedirectToAction("Entrar", "Acesso");
            }
            else if (String.IsNullOrEmpty(Session["Autenticado"].ToString()))
            {
                filterContext.Result = RedirectToAction("Entrar", "Acesso");
            }
            else if (!(bool)Session["Autenticado"])
            //if (!Usuario.SAutenticado)
            {
                filterContext.Result = RedirectToAction("Entrar", "Acesso");
            }
            base.OnActionExecuting(filterContext);
        }

        // GET: Dashboard
        public ActionResult Index()
        {
            Usuario usuario = Usuario.ListarPorMatricula((string)Session["UsuarioMatricula"]);
            return View(usuario);            
        }
       
    }
}