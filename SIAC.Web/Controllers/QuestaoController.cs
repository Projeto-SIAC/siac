using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIAC.Web.Models;

namespace SIAC.Web.Controllers
{
    public class QuestaoController : Controller
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
            else if(!(bool)Session["Autenticado"])
            //if (!Usuario.SAutenticado)
            {
                filterContext.Result = RedirectToAction("Entrar", "Acesso");
            }
            //else if (Usuario.SCategoriaCodigo != 2)
            else if ((int)Session["UsuarioCategoriaCodigo"] != 2)
            {
                filterContext.Result = RedirectToAction("Entrar", "Acesso");
            }
            base.OnActionExecuting(filterContext);
        }

        // GET: Questao
        public ActionResult Index()
        {
            List<Questao> model =  Questao.ListarPorProfessor(Session["UsuarioMatricula"].ToString());
            //List<Questao> model = Questao.ListarPorProfessor(Usuario.SMatricula);

            return View(model);
        }
        
        // GET: Questao/Cadastrar
        public ActionResult Cadastrar()
        {
            var dc = DataContextSIAC.GetInstance();
            ViewBag.Disciplinas = Disciplina.ListarOrdenadamente(); // futuramente: retornar apenas disciplinas do professor
            ViewBag.Tipos = TipoQuestao.ListarOrdenadamente();
            ViewBag.Dificuldades = Dificuldade.ListarOrdenadamente();
            ViewBag.TiposAnexo = TipoAnexo.ListarOrdenadamente();
            return View();
        }

        // POST: Questao/Confirmar
        [HttpPost]
        public ActionResult Confirmar(FormCollection formCollection)
        {
            var dc = DataContextSIAC.GetInstance();
            Questao questao = new Questao();

            questao.Professor = Professor.ListarPorMatricula(Session["UsuarioMatricula"].ToString());
            //questao.Professor = Professor.ListarPorMatricula(Usuario.SMatricula);

            questao.CodProfessor = questao.Professor.CodProfessor;

            // Gerais
            questao.CodDificuldade = int.Parse(formCollection["ddlDificuldade"]);
            questao.Dificuldade = Dificuldade.ListarPorCodigo(questao.CodDificuldade);
            questao.CodTipoQuestao = int.Parse(formCollection["ddlTipo"]);
            questao.TipoQuestao = TipoQuestao.ListarPorCodigo(questao.CodTipoQuestao);

            var codDisciplina = int.Parse(formCollection["ddlDisciplina"]);
            var codTemas = formCollection["ddlTema"].Split(',');
            foreach (var strCod in codTemas)
            {
                var codTema = int.Parse(strCod);
                questao.QuestaoTema.Add(new QuestaoTema {
                    CodDisciplina = codDisciplina,
                    CodTema = codTema,
                    Tema = Tema.ListarPorCodigo(codDisciplina, codTema)
                });
            }
            
            // Detalhes
            questao.Enunciado = formCollection["txtEnunciado"].RemoveSpaces();
            questao.Objetivo = !String.IsNullOrEmpty(formCollection["txtObjetivo"]) ? formCollection["txtObjetivo"].RemoveSpaces() : null;

            // Discursiva
            if (questao.CodTipoQuestao == 2)
            {
                questao.ChaveDeResposta = formCollection["txtChaveDeResposta"].RemoveSpaces();
                questao.Comentario = !String.IsNullOrEmpty(formCollection["txtComentario"]) ? formCollection["txtComentario"].RemoveSpaces() : null;
            }

            // Objetiva
            if (questao.CodTipoQuestao == 1)
            {
                var qteAlternativas = int.Parse(formCollection["txtQtdAlternativas"]);
                for (int i = 0; i < qteAlternativas; i++)
                {
                    questao.Alternativa.Add(new Alternativa {
                        CodOrdem = i,
                        Enunciado = formCollection["txtAlternativaEnunciado" + (i + 1)].RemoveSpaces(),
                        Comentario = !String.IsNullOrEmpty(formCollection["txtAlternativaComentario" + (i + 1)]) ? formCollection["txtAlternativaComentario" + (i + 1)].RemoveSpaces() : null,
                        FlagGabarito = !String.IsNullOrEmpty(formCollection["chkAlternativaCorreta" + (i + 1)]) ? true : false
                    });
                }
            }

            TempData["Questao"] = questao;

            return View(questao);
        }

        //GET:Dashboard/Questão/Confirmar
        [HttpGet]
        public ActionResult Confirmar()
        {
            if (TempData.Keys.Count > 0)
            {
                if (TempData["Questao"] != null)
                {
                    Questao temp = TempData["Questao"] as Questao;

                    Questao.Inserir(temp);

                    TempData.Clear();

                    return RedirectToAction("Index");
                }
            }

            return RedirectToAction("Index");
        }

        //GET: Dashboard/Questao/Detalhe/4
        public ActionResult Detalhe(string codigo)
        {
            int codQuestao = 0;
            int.TryParse(codigo, out codQuestao);
            Questao model = null;
            if (codQuestao > 0)
            {
                model = Questao.PesquisarPorCodigo(codQuestao);
            }
            if (model != null)
            {
                return View(model);
            }            
            return RedirectToAction("Index");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult RecuperarTemasPorCodDisciplina(string codDisciplina)
        {
            if (!String.IsNullOrEmpty(codDisciplina))
            {
                int cod = 0;
                if (int.TryParse(codDisciplina, out cod))
                {
                    var dc = DataContextSIAC.GetInstance();
                    var temas = Tema.ListarPorDisciplina(cod);
                    var result = from t in temas select new { CodTema = t.CodTema, Descricao = t.Descricao };
                    return Json(result.ToList(), JsonRequestBehavior.AllowGet);
                }
            }
            return Json(null);
        }
    }
}