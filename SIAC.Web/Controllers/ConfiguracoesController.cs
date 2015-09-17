using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIAC.Web.Controllers
{
    public class ConfiguracoesController : Controller
    {

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if(Session["Autenticado"] == null)
            {
                if (TempData["UrlReferrer"] != null)
                {
                    filterContext.Result = Redirect(TempData["UrlReferrer"].ToString());
                }
                else filterContext.Result = RedirectToAction("Index", "Dashboard");
            }
            else if(!(bool)Session["Autenticado"])
            {
                if (TempData["UrlReferrer"] != null)
                {
                    filterContext.Result = Redirect(TempData["UrlReferrer"].ToString());
                }
                else filterContext.Result = RedirectToAction("Index", "Dashboard");
            }
            else if((int)Session["UsuarioCategoriaCodigo"] != 3)
            {
                if (TempData["UrlReferrer"] != null)
                {
                    filterContext.Result = Redirect(TempData["UrlReferrer"].ToString());
                }
                else filterContext.Result = RedirectToAction("Index", "Dashboard");
            }

            base.OnActionExecuting(filterContext);
        }

        // GET: PainelAdministrativo
        public ActionResult Index()
        {
            return View();
        }
    }
}