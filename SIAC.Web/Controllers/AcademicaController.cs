using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIAC.Web.Models;

namespace SIAC.Web.Controllers
{
    public class AcademicaController : Controller
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
            else if ((int)Session["UsuarioCategoriaCodigo"] > 2)
            {
                if (TempData["UrlReferrer"] != null)
                {
                    filterContext.Result = Redirect(TempData["UrlReferrer"].ToString());
                }
                else filterContext.Result = RedirectToAction("Index", "Dashboard");
            }
            base.OnActionExecuting(filterContext);
        }

        //GET: Historico/Academica/Minhas <- Ajax 
        public ActionResult Minhas()
        {
            if ((int)Session["UsuarioCategoriaCodigo"] != 2)
            {
                if (TempData["UrlReferrer"] != null)
                {
                    return Redirect(TempData["UrlReferrer"].ToString());
                }
                else return RedirectToAction("Index", "Dashboard");
            }

            int codProfessor = Professor.ListarPorMatricula(Session["UsuarioMatricula"].ToString()).CodProfessor;

            var result = from a in AvalAcademica.ListarPorProfessor(codProfessor)
                         select new
                         {
                             CodAvaliacao = a.Avaliacao.CodAvaliacao,
                             DtCadastro = a.Avaliacao.DtCadastro.ToBrazilianString(),
                             DtCadastroTempo = a.Avaliacao.DtCadastro.ToElapsedTimeString(),
                             Turma = a.NumTurma != null ? a.Turma.Nome : null,
                             QteQuestoes = a.Avaliacao.QteQuestoes(),
                             Disciplinas = a.Avaliacao.AvaliacaoTema.Select(at => at.Tema.Disciplina.Descricao).Distinct().ToList(),
                             FlagLiberada = a.Avaliacao.FlagLiberada
                         };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // GET: Historico/Avaliacao/Academica
        public ActionResult Index()
        {
            if (Request.Url.ToString().ToLower().Contains("dashboard"))
            {
                return Redirect("~/Historico/Academica");
            }
            return View();
        }

        //GET: Dashboard/Avaliacao/Academica/Gerar
        public ActionResult Gerar()
        {
            if ((int)Session["UsuarioCategoriaCodigo"] != 2)
            {
                if (TempData["UrlReferrer"] != null)
                {
                    return Redirect(TempData["UrlReferrer"].ToString());
                }
                else return RedirectToAction("Index", "Dashboard");
            }
            ViewBag.Disciplinas = Disciplina.ListarPorProfessor(Session["UsuarioMatricula"].ToString());
            ViewBag.Dificuldades = Dificuldade.ListarOrdenadamente();
            ViewBag.Termo = Parametro.Obter().NotaUso;

            return View();
        }

        //POST: Dashboard/Avaliacao/Academica/Confirmar
        [HttpPost]
        public ActionResult Confirmar(FormCollection formCollection)
        {
            if ((int)Session["UsuarioCategoriaCodigo"] != 2)
            {
                if (TempData["UrlReferrer"] != null)
                {
                    return Redirect(TempData["UrlReferrer"].ToString());
                }
                else return RedirectToAction("Index", "Dashboard");
            }
            AvalAcademica acad = new AvalAcademica();
            Helpers.TimeLog.Iniciar("AvalAcad");
            if (formCollection.HasKeys())
            {
                DateTime hoje = DateTime.Now;

                /* Chave */
                acad.Avaliacao = new Avaliacao();
                acad.Avaliacao.TipoAvaliacao = TipoAvaliacao.ListarPorCodigo(2);
                acad.Avaliacao.Ano = hoje.Year;
                acad.Avaliacao.Semestre = hoje.Month > 6 ? 2 : 1;
                acad.Avaliacao.NumIdentificador = Avaliacao.ObterNumIdentificador(2);
                acad.Avaliacao.DtCadastro = hoje;

                /* Professor */
                string strMatr = Session["UsuarioMatricula"].ToString();
                acad.Professor = Professor.ListarPorMatricula(strMatr);

                /* Dados */
                int codDisciplina = int.Parse(formCollection["ddlDisciplina"]);

                acad.CodDisciplina = codDisciplina;

                /* Dificuldade */
                int codDificuldade = int.Parse(formCollection["ddlDificuldade"]);

                /* Quantidade */
                int qteObjetiva = 0;
                int qteDiscursiva = 0;
                if (formCollection["ddlTipo"] == "3")
                {
                    int.TryParse(formCollection["txtQteObjetiva"], out qteObjetiva);
                    int.TryParse(formCollection["txtQteDiscursiva"], out qteDiscursiva);
                }
                else if (formCollection["ddlTipo"] == "2")
                {
                    int.TryParse(formCollection["txtQteDiscursiva"], out qteDiscursiva);
                }
                else if (formCollection["ddlTipo"] == "1")
                {
                    int.TryParse(formCollection["txtQteObjetiva"], out qteObjetiva);
                }

                /* Temas */
                string[] arrTemaCods = formCollection["ddlTemas"].Split(',');

                /* Questões */
                List<QuestaoTema> lstQuestoes = Questao.ListarPorDisciplina(codDisciplina, arrTemaCods, codDificuldade, qteObjetiva, qteDiscursiva);

                foreach (var strTemaCod in arrTemaCods)
                {
                    AvaliacaoTema avalTema = new AvaliacaoTema();
                    avalTema.Tema = Tema.ListarPorCodigo(codDisciplina, int.Parse(strTemaCod));
                    foreach (var queTma in lstQuestoes.Where(q => q.CodTema == int.Parse(strTemaCod)))
                    {
                        AvalTemaQuestao avalTemaQuestao = new AvalTemaQuestao();
                        avalTemaQuestao.QuestaoTema = queTma;
                        avalTema.AvalTemaQuestao.Add(avalTemaQuestao);
                    }
                    acad.Avaliacao.AvaliacaoTema.Add(avalTema);
                }

                ViewBag.QteQuestoes = lstQuestoes.Count;
                ViewBag.QuestoesDaAvaliacao = lstQuestoes;

                AvalAcademica.Inserir(acad);
                Helpers.TimeLog.Parar();
            }

            return View(acad);
        }

        //GET: Dashboard/Avaliacao/Academica/Agendar/ACAD2015100002
        [HttpGet]
        public ActionResult Agendar(string codigo)
        {
            if ((int)Session["UsuarioCategoriaCodigo"] != 2)
            {
                if (TempData["UrlReferrer"] != null)
                {
                    return Redirect(TempData["UrlReferrer"].ToString());
                }
                else return RedirectToAction("Index", "Dashboard");
            }
            AvalAcademica acad = AvalAcademica.ListarPorCodigoAvaliacao(codigo);

            string strMatr = Session["UsuarioMatricula"].ToString();
            Professor prof = Professor.ListarPorMatricula(strMatr);

            ViewBag.lstTurma = prof.TurmaDiscProfHorario.Select(d => d.Turma).Distinct().OrderBy(t => t.Curso.Descricao).ToList();
            ViewBag.lstSala = Sala.ListarOrdenadamente();

            return View(acad);
        }

        //POST: Dashboard/Avaliacao/Academica/Agendar/ACAD2015100002
        [HttpPost]
        public ActionResult Agendar(string codigo, FormCollection form)
        {
            if ((int)Session["UsuarioCategoriaCodigo"] != 2)
            {
                if (TempData["UrlReferrer"] != null)
                {
                    return Redirect(TempData["UrlReferrer"].ToString());
                }
                else return RedirectToAction("Index", "Dashboard");
            }
            string strCodTurma = form["ddlTurma"];
            string strCodSala = form["ddlSala"];
            string strData = form["txtData"];
            string strHoraInicio = form["txtHoraInicio"];
            string strHoraTermino = form["txtHoraTermino"];
            if (Helpers.StringExt.IsNullOrWhiteSpace(strCodTurma, strCodSala, strData, strHoraInicio, strHoraTermino))
            {
                AvalAcademica acad = AvalAcademica.ListarPorCodigoAvaliacao(codigo);

                string strMatr = Session["UsuarioMatricula"].ToString();
                Professor prof = Professor.ListarPorMatricula(strMatr);

                if (acad.CodProfessor == prof.CodProfessor)
                {
                    // Turma
                    acad.Turma = Turma.ListarPorCodigo(strCodTurma);

                    // Sala
                    int codSala;
                    int.TryParse(strCodSala, out codSala);
                    acad.Sala = Sala.ListarPorCodigo(codSala);

                    // Data de Aplicacao
                    acad.Avaliacao.DtAplicacao = DateTime.Parse(strData+" "+strHoraInicio);

                    // Duracao
                    acad.Avaliacao.Duracao = Convert.ToInt32((DateTime.Parse(strData + " " + strHoraTermino) - acad.Avaliacao.DtAplicacao.Value).TotalMinutes);

                    /* 
                     * DataContextSIAC.GetInstance().SaveChanges();
                     * OU
                     * AvalAcademica.Agendar(acad);
                     */
                }
            }

            return RedirectToAction("Index");
        }
    }
}