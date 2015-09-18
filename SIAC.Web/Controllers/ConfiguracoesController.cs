using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIAC.Web.Models;

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

        // GET: /Configuracoes
        public ActionResult Index()
        {
            Parametro model = Parametro.Obter();
            return View(model);
        }

        //POST: /Configuracoes
        [HttpPost]
        public ActionResult Index(FormCollection formCollection)
        {
            if(formCollection.HasKeys())
            {
                Parametro temp = Parametro.Obter();
                temp.TempoInatividade = int.Parse(formCollection["txtTempoInatividade"]);
                temp.NumeracaoQuestao = int.Parse(formCollection["ddlNumeracaoQuestao"]);
                temp.NumeracaoAlternativa = int.Parse(formCollection["ddlNumeracaoAlternativa"]);
                temp.QteSemestres = int.Parse(formCollection["txtQtdSemestre"]);

                Parametro.Atualizar(temp);

                ViewBag.Atualizado = true;

                return RedirectToAction("Index", "Configuracoes");
            }

            return View();
        }
    }
}