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
            if (Session["Autenticado"] == null)
            {
                filterContext.Result = RedirectToAction("Entrar", "Acesso");
            }
            else if (String.IsNullOrEmpty(Session["Autenticado"].ToString()))
            {
                filterContext.Result = RedirectToAction("Entrar", "Acesso");
            }
            else if (!(bool)Session["Autenticado"])
            {
                filterContext.Result = RedirectToAction("Entrar", "Acesso");
            }
            base.OnActionExecuting(filterContext);
        }

        // GET: Dashboard
        public ActionResult Index()
        {
            return View();            
        }
    }
}