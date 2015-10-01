using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIAC.Web.Models;

namespace SIAC.Web.Controllers
{
    public class AvaliacaoAcademicaController : Controller
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
            {
                filterContext.Result = RedirectToAction("Entrar", "Acesso");
            }
            else if ((int)Session["UsuarioCategoriaCodigo"] != 2)
            {
                if (TempData["UrlReferrer"] != null)
                {
                    filterContext.Result = Redirect(TempData["UrlReferrer"].ToString());
                }
                else filterContext.Result = RedirectToAction("Index", "Dashboard");
            }
            base.OnActionExecuting(filterContext);
        }

        // GET: AvaliacaoAcademica
        public ActionResult Index()
        {
            return View();
        }

        //GET: AvaliacaoAcademica/Agendar
        public ActionResult Agendar()
        {
            ViewBag.Disciplinas = Disciplina.ListarPorProfessor(Session["UsuarioMatricula"].ToString());
            ViewBag.Dificuldades = Dificuldade.ListarOrdenadamente();

            return View();
        }
    }
}