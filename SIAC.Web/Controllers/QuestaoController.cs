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
            var dc = Models.DataContextSIAC.GetInstance();
            Models.Questao questao = new Models.Questao();

            questao.Professor = dc.Professor.Single(p => p.MatrProfessor == (string)Session["UsuarioMatricula"]);
            questao.CodProfessor = questao.Professor.CodProfessor;

            // Gerais
            questao.CodDificuldade = int.Parse(formCollection["ddlDificuldade"]);
            questao.Dificuldade = dc.Dificuldade.Single(d => d.CodDificuldade == questao.CodDificuldade);
            questao.CodTipoQuestao = int.Parse(formCollection["ddlTipo"]);
            questao.TipoQuestao = dc.TipoQuestao.Single(t => t.CodTipoQuestao == questao.CodTipoQuestao);
            var codDisciplina = int.Parse(formCollection["ddlDisciplina"]);
            var codTemas = formCollection["ddlTema"].Split(',');
            foreach (var strCod in codTemas)
            {
                var codTema = int.Parse(strCod);
                questao.QuestaoTemas.Add(new Models.QuestaoTema {
                    CodDisciplina = codDisciplina,
                    CodTema = codTema,
                    Tema = dc.Tema.Single(t=>t.CodTema == codTema && t.CodDisciplina == codDisciplina)                    
                });
            }


            // Detalhes
            questao.Enunciado = formCollection["txtEnunciado"];
            questao.Objetivo = !String.IsNullOrEmpty(formCollection["txtObjetivo"]) ? formCollection["txtObjetivo"] : null;

            // Discursiva
            if (questao.CodTipoQuestao == 2)
            {
                questao.ChaveDeResposta = formCollection["txtChaveDeResposta"];
                questao.Comentario = !String.IsNullOrEmpty(formCollection["txtComentario"]) ? formCollection["txtComentario"] : null;
            }

            // Objetiva
            if (questao.CodTipoQuestao == 1)
            {
                var qteAlternativas = int.Parse(formCollection["txtQtdAlternativas"]);
                for (int i = 0; i < qteAlternativas; i++)
                {
                    questao.Alternativas.Add(new Models.Alternativa {
                        CodOrdem = i,
                        Enunciado = formCollection["txtAlternativaEnunciado" + (i + 1)],
                        Comentario = !String.IsNullOrEmpty(formCollection["txtAlternativaComentario" + (i + 1)]) ? formCollection["txtAlternativaComentario" + (i + 1)] : null,
                        FlagGabarito = !String.IsNullOrEmpty(formCollection["chkAlternativaCorreta" + (i + 1)]) ? true : false
                    });
                }
            }

            TempData["Questao"] = questao;

            return View(questao);
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