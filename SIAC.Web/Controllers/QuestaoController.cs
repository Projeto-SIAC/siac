using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIAC.Web.Controllers
{
    public class QuestaoController : Controller
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
            else if ((int)Session["UsuarioCategoriaCodigo"] != 2)
            {
                filterContext.Result = RedirectToAction("Entrar", "Acesso");
            }
            base.OnActionExecuting(filterContext);
        }

        // GET: Questao
        public ActionResult Index()
        {
            return View();
        }
        
        // GET: Questao/Cadastrar
        public ActionResult Cadastrar()
        {
            var dc = Models.DataContextSIAC.GetInstance();
            ViewBag.Disciplinas = dc.Disciplina.OrderBy(d=>d.Descricao).ToList(); // futuramente: retornar apenas disciplinas do professor
            ViewBag.Tipos = dc.TipoQuestao.OrderBy(d => d.Descricao).ToList();
            ViewBag.Dificuldades = dc.Dificuldade.ToList();
            return View();
        }

        // POST: Questao/Confirmar
        public ActionResult Confirmar(FormCollection formCollection)
        {
            ViewBag.FormCollection = formCollection;
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
                    var dc = Models.DataContextSIAC.GetInstance();
                    var temas = dc.Tema.Where(t => t.CodDisciplina == cod).ToList();
                    var result = from t in temas select new { CodTema = t.CodTema, Descricao = t.Descricao };
                    return Json(result.ToList(), JsonRequestBehavior.AllowGet);
                }
            }
            return Json(null);
        }
    }
}