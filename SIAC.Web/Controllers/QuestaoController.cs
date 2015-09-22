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
            else if (!(bool)Session["Autenticado"])
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
            if (Request.Url.ToString().ToLower().Contains("dashboard"))
            {
                return Redirect("~/Historico/Questao");
            }
            //List<Questao> model = Questao.ListarPorProfessor(Usuario.SMatricula);

            return View();
        }

        // GET: Questao/Minhas
        public ActionResult Minhas()
        {
            var lstQuestoes = Questao.ListarPorProfessor(Session["UsuarioMatricula"].ToString());
            var result = from q in lstQuestoes
                         select new
                         {
                             CodQuestao = q.CodQuestao,
                             Enunciado = q.Enunciado.ToShortString(140),
                             DtCadastro = q.DtCadastro.ToString(),
                             Disciplina = q.QuestaoTema.First().Tema.Disciplina.Descricao,
                             Temas = q.QuestaoTema.Select(qt => qt.Tema.Descricao).ToList(),
                             TipoQuestao = q.TipoQuestao.Descricao,
                             Dificuldade = q.Dificuldade.Descricao,
                         };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // GET: Questao/Cadastrar
        public ActionResult Cadastrar()
        {
            //var dc = DataContextSIAC.GetInstance();
            ViewBag.Termo = Parametro.Obter().TermoResponsabilidade;
            ViewBag.Disciplinas = Professor.ObterDisciplinas(Session["UsuarioMatricula"].ToString());
            ViewBag.Tipos = TipoQuestao.ListarOrdenadamente();
            ViewBag.Dificuldades = Dificuldade.ListarOrdenadamente();
            ViewBag.TiposAnexo = TipoAnexo.ListarOrdenadamente();
            return View();
        }

        // POST: Questao/Confirmar
        [HttpPost]
        public ActionResult Confirmar(FormCollection formCollection)
        {
            //var dc = DataContextSIAC.GetInstance();
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
                questao.QuestaoTema.Add(new QuestaoTema
                {
                    CodDisciplina = codDisciplina,
                    CodTema = codTema,
                    Tema = Tema.ListarPorCodigo(codDisciplina, codTema)
                });
            }

            // Detalhes
            questao.Enunciado = formCollection["txtEnunciado"].Trim();
            questao.Objetivo = !String.IsNullOrEmpty(formCollection["txtObjetivo"]) ? formCollection["txtObjetivo"].RemoveSpaces() : null;

            // Discursiva
            if (questao.CodTipoQuestao == 2)
            {
                questao.ChaveDeResposta = formCollection["txtChaveDeResposta"].Trim();
                questao.Comentario = !String.IsNullOrEmpty(formCollection["txtComentario"]) ? formCollection["txtComentario"].RemoveSpaces() : null;
            }

            // Objetiva
            if (questao.CodTipoQuestao == 1)
            {
                var qteAlternativas = int.Parse(formCollection["txtQtdAlternativas"]);
                for (int i = 0; i < qteAlternativas; i++)
                {
                    questao.Alternativa.Add(new Alternativa
                    {
                        CodOrdem = i,
                        Enunciado = formCollection["txtAlternativaEnunciado" + (i + 1)].RemoveSpaces(),
                        Comentario = !String.IsNullOrEmpty(formCollection["txtAlternativaComentario" + (i + 1)]) ? formCollection["txtAlternativaComentario" + (i + 1)].RemoveSpaces() : null,
                        FlagGabarito = !String.IsNullOrEmpty(formCollection["chkAlternativaCorreta" + (i + 1)]) ? true : false
                    });
                }
            }

            // Anexos
            if (!String.IsNullOrEmpty(formCollection["chkAnexos"]) && !String.IsNullOrEmpty(formCollection["txtQtdAnexos"]))
            {
                var qteAnexos = int.Parse(formCollection["txtQtdAnexos"]);
                for (int i = 0; i < qteAnexos; i++)
                {
                    var tipoAnexo = int.Parse(formCollection["txtAnexoTipo" + (i + 1)]);
                    switch (tipoAnexo)
                    {
                        case 1:
                            questao.QuestaoAnexo.Add(new QuestaoAnexo
                            {
                                CodOrdem = i,
                                CodTipoAnexo = tipoAnexo,
                                Legenda = formCollection["txtAnexoLegenda" + (i + 1)].RemoveSpaces(),
                                Fonte = !String.IsNullOrEmpty(formCollection["txtAnexoFonte" + (i + 1)]) ? formCollection["txtAnexoFonte" + (i + 1)].RemoveSpaces() : null,
                                Anexo = new System.IO.BinaryReader(Request.Files[i].InputStream).ReadBytes(Request.Files[i].ContentLength)
                            });
                            break;
                        default:
                            break;
                    }
                }
            }

            //TempData["Questao"] = questao;

            Questao.Inserir(questao);

            return RedirectToAction("Detalhe", new { codigo = questao.CodQuestao });
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
    }
}