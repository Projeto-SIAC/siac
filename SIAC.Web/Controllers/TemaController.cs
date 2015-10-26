using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIAC.Models;

namespace SIAC.Controllers
{
    [CategoriaFilter]
    public class TemaController : Controller
    {
        // GET: Tema
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Acesso");
        }

        [AcceptVerbs(HttpVerbs.Post)]
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

        [AcceptVerbs(HttpVerbs.Post)]
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