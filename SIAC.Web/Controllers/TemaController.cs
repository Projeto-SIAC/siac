using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIAC.Web.Models;

namespace SIAC.Web.Controllers
{
    public class TemaController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            TempData["UrlReferrer"] = Request.Url.ToString();
            if (!Helpers.Sessao.Autenticado)
            {
                filterContext.Result = RedirectToAction("Entrar", "Acesso");
            }
            base.OnActionExecuting(filterContext);
        }

        // GET: Tema
        public ActionResult Index()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult RecuperarTemasPorCodDisciplina(string codDisciplina)
        {
            if (!String.IsNullOrEmpty(codDisciplina))
            {
                int cod = 0;
                if (int.TryParse(codDisciplina, out cod))
                {                    
                    var temas = Tema.ListarPorDisciplina(cod);
                    var result = from t in temas select new { CodTema = t.CodTema, Descricao = t.Descricao };
                    return Json(result.ToList(), JsonRequestBehavior.AllowGet);
                }
            }
            return Json(null);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult RecuperarTemasPorCodDisciplinaTemQuestao(string codDisciplina)
        {
            if (!String.IsNullOrEmpty(codDisciplina))
            {
                int cod = 0;
                if (int.TryParse(codDisciplina, out cod))
                {
                    var temas = Tema.ListarPorDisciplinaTemQuestao(cod);
                    var result = from t in temas select new { CodTema = t.CodTema, Descricao = t.Descricao };
                    return Json(result.ToList(), JsonRequestBehavior.AllowGet);
                }
            }
            return Json(null);
        }
    }
}