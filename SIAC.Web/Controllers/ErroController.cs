using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIAC.Web.Controllers
{
    public class ErroController : Controller
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
            base.OnActionExecuting(filterContext);
        }

        // GET: Erro
        public ActionResult Index(int code = 0)
        {
            switch (code)
            {
                case 403:
                    ViewBag.Codigo = code;
                    ViewBag.Title = "Acesso proibido";
                    ViewBag.Mensagem = "A página solicitada é proibida";
                    break;
                case 404:
                    ViewBag.Codigo = code;
                    ViewBag.Title = "Não encontrado";
                    ViewBag.Mensagem = "A página solicitada não foi encontrada";
                    break;
                default:
                    ViewBag.Codigo = "desconhecido";
                    ViewBag.Title = "Tente novamente";
                    ViewBag.Mensagem = "Ocorreu um erro inesperado";
                    break;
            }
            return View();
        }
    }
}