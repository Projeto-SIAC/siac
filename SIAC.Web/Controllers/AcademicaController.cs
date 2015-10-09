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
            if (!Helpers.Sessao.Autenticado)
            {
                filterContext.Result = RedirectToAction("Entrar", "Acesso");
            }
            else if (Helpers.Sessao.UsuarioCategoriaCodigo > 2)
            {
                if (TempData["UrlReferrer"] != null)
                {
                    filterContext.Result = Redirect(TempData["UrlReferrer"].ToString());
                }
                else filterContext.Result = RedirectToAction("Index", "Dashboard");
            }
            base.OnActionExecuting(filterContext);
        }

        //GET: Historico/Avaliacao/Academica/Minhas <- Ajax 
        public ActionResult Minhas()
        {
            if (Helpers.Sessao.UsuarioCategoriaCodigo != 2)
            {
                if (TempData["UrlReferrer"] != null)
                {
                    return Redirect(TempData["UrlReferrer"].ToString());
                }
                else return RedirectToAction("Index", "Dashboard");
            }

            int codProfessor = Professor.ListarPorMatricula(Helpers.Sessao.UsuarioMatricula).CodProfessor;
            List<AvalAcademica> avaliacoes = AvalAcademica.ListarPorProfessor(codProfessor);

            var result = from a in avaliacoes
                         select new
                         {
                             CodAvaliacao = a.Avaliacao.CodAvaliacao,
                             DtCadastro = a.Avaliacao.DtCadastro.ToBrazilianString(),
                             DtCadastroTempo = a.Avaliacao.DtCadastro.ToElapsedTimeString(),
                             Turma = a.NumTurma.HasValue ? a.Turma.CodTurma : null,
                             Curso = a.NumTurma.HasValue ? a.Turma.Curso.Descricao : "Curso",
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
            if (Helpers.Sessao.UsuarioCategoriaCodigo != 2)
            {
                if (TempData["UrlReferrer"] != null)
                {
                    return Redirect(TempData["UrlReferrer"].ToString());
                }
                else return RedirectToAction("Index", "Dashboard");
            }
            ViewBag.Disciplinas = Disciplina.ListarPorProfessor(Helpers.Sessao.UsuarioMatricula);
            ViewBag.Dificuldades = Dificuldade.ListarOrdenadamente();
            ViewBag.Termo = Parametro.Obter().NotaUso;

            return View();
        }

        //POST: Dashboard/Avaliacao/Academica/Confirmar
        [HttpPost]
        public ActionResult Confirmar(FormCollection formCollection)
        {
            if (Helpers.Sessao.UsuarioCategoriaCodigo != 2)
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
                string strMatr = Helpers.Sessao.UsuarioMatricula;
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
            if (Helpers.Sessao.UsuarioCategoriaCodigo != 2)
            {
                if (TempData["UrlReferrer"] != null)
                {
                    return Redirect(TempData["UrlReferrer"].ToString());
                }
                else return RedirectToAction("Index", "Dashboard");
            }
            AvalAcademica acad = AvalAcademica.ListarPorCodigoAvaliacao(codigo);

            string strMatr = Helpers.Sessao.UsuarioMatricula;
            Professor prof = Professor.ListarPorMatricula(strMatr);

            ViewBag.lstTurma = prof.TurmaDiscProfHorario.Select(d => d.Turma).Distinct().OrderBy(t => t.Curso.Descricao).ToList();
            ViewBag.lstSala = Sala.ListarOrdenadamente();

            return View(acad);
        }

        //POST: Dashboard/Avaliacao/Academica/Agendar/ACAD2015100002
        [HttpPost]
        public ActionResult Agendar(string codigo, FormCollection form)
        {
            if (Helpers.Sessao.UsuarioCategoriaCodigo != 2)
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

                string strMatr = Helpers.Sessao.UsuarioMatricula;
                Professor prof = Professor.ListarPorMatricula(strMatr);

                if (acad.CodProfessor == prof.CodProfessor)
                {
                    // Turma
                    Turma turma = Turma.ListarPorCodigo(strCodTurma);
                    if (turma != null)
                    {
                        acad.Turma = turma;
                    }

                    // Sala
                    int codSala;
                    int.TryParse(strCodSala, out codSala);
                    Sala sala = Sala.ListarPorCodigo(codSala);
                    if (sala!=null)
                    {
                        acad.Sala = sala;
                    }

                    // Data de Aplicacao
                    DateTime dtAplicacao = DateTime.Parse(strData + " " + strHoraInicio);
                    DateTime dtAplicacaoTermino = DateTime.Parse(strData + " " + strHoraTermino);

                    if (dtAplicacao.IsFuture() && dtAplicacaoTermino.IsFuture() && dtAplicacaoTermino > dtAplicacao)
                    {
                        acad.Avaliacao.DtAplicacao = dtAplicacao;
                        acad.Avaliacao.Duracao = Convert.ToInt32((dtAplicacaoTermino - acad.Avaliacao.DtAplicacao.Value).TotalMinutes);
                    }

                    DataContextSIAC.GetInstance().SaveChanges();
                    // OU
                    // AvalAcademica.Agendar(acad);
                    
                }
            }

            return RedirectToAction("Index");
        }

        // GET: Avaliacao/Academica/Detalhe/ACAD201520001
        public ActionResult Detalhe(string codigo)
        {
            if (!String.IsNullOrEmpty(codigo))
            {
                AvalAcademica acad = AvalAcademica.ListarPorCodigoAvaliacao(codigo);
                if (acad != null)
                {
                    return View(acad);
                }
            }

            return RedirectToAction("Index");
        }

        // GET: Avaliacao/Academica/Configurar/ACAD201520001
        [HttpGet]
        public ActionResult Configurar(string codigo)
        {
            TempData["listaQuestoesAntigas"] = new List<AvalTemaQuestao>();
            TempData["listaQuestoesNovas"] = new List<AvalTemaQuestao>();
            TempData["listaQuestoesIndices"] = new List<int>();
            TempData["listaQuestoesPossiveisObj"] = new List<QuestaoTema>();
            TempData["listaQuestoesPossiveisDisc"] = new List<QuestaoTema>();

            if (!String.IsNullOrEmpty(codigo))
            {
                AvalAcademica acad = AvalAcademica.ListarPorCodigoAvaliacao(codigo);
                if (acad != null)
                {
                    string strMatr = Helpers.Sessao.UsuarioMatricula;
                    Professor prof = Professor.ListarPorMatricula(strMatr);
                    if (prof.CodProfessor == acad.CodProfessor)
                    {
                        return View(acad);
                    }
                }
            }
            return RedirectToAction("Index");
        }

        // GET: Avaliacao/Academica/Imprimir/ACAD201520001
        public ActionResult Imprimir(string codigo)
        {
            if (!String.IsNullOrEmpty(codigo))
            {
                AvalAcademica acad = AvalAcademica.ListarPorCodigoAvaliacao(codigo);
                if (acad != null)
                {
                    string strMatr = Helpers.Sessao.UsuarioMatricula;
                    Professor prof = Professor.ListarPorMatricula(strMatr);
                    if (prof.CodProfessor == acad.CodProfessor)
                    {
                        return View(acad);
                    }
                }
            }
            return RedirectToAction("Index");
        }

        // GET: Avaliacao/Academica/Agendada
        [HttpGet]
        public ActionResult Agendada()
        {
            if (Helpers.Sessao.UsuarioCategoriaCodigo == 2)
            {
                string strMatr = Helpers.Sessao.UsuarioMatricula;
                int codProfessor = Professor.ListarPorMatricula(strMatr).CodProfessor;
                return View(AvalAcademica.ListarAgendadaPorProfessor(codProfessor));
            }
            return RedirectToAction("Index");
        }

        // POST: Avaliacao/Academica/Agendada/ACAD201520001
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Agendada(string codigo)
        {
            if (String.IsNullOrEmpty(codigo))
            {
                return RedirectToAction("Index");
            }
            if (Helpers.Sessao.UsuarioCategoriaCodigo == 2)
            {
                string strMatr = Helpers.Sessao.UsuarioMatricula;
                int codProfessor = Professor.ListarPorMatricula(strMatr).CodProfessor;
                AvalAcademica avalAcademica = AvalAcademica.ListarPorCodigoAvaliacao(codigo);
                if (codProfessor == avalAcademica.CodProfessor)
                {
                    return PartialView("_Agendada", avalAcademica);
                }
            }
            return RedirectToAction("Index");
        }

        //POST: Avaliacao/Academica/Trocar/ACAD201520001
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Trocar(string codigoAvaliacao, int tipo, int indice, int codQuestao)
        {
            List<AvalTemaQuestao> antigas = (List<AvalTemaQuestao>)TempData["listaQuestoesAntigas"];
            List<AvalTemaQuestao> novas = (List<AvalTemaQuestao>)TempData["listaQuestoesNovas"];
            List<QuestaoTema> questoesTrocaObj = (List<QuestaoTema>)TempData["listaQuestoesPossiveisObj"];
            List<QuestaoTema> questoesTrocaDisc = (List<QuestaoTema>)TempData["listaQuestoesPossiveisDisc"];
            List<int> indices = (List<int>)TempData["listaQuestoesIndices"];

            TempData.Keep();
            Random r = new Random();

            if (!String.IsNullOrEmpty(codigoAvaliacao))
            {
                AvalAcademica acad = AvalAcademica.ListarPorCodigoAvaliacao(codigoAvaliacao);
                if (acad != null)
                {
                    List<QuestaoTema> AvalQuestTema = acad.Avaliacao.QuestaoTema;

                    QuestaoTema questao = null;

                    if (tipo == 1)
                    {
                        if (questoesTrocaObj.Count <= 0)
                        {
                            TempData["listaQuestoesPossiveisObj"] = Questao.ObterNovasQuestoes(AvalQuestTema, tipo);
                            questoesTrocaObj = (List<QuestaoTema>)TempData["listaQuestoesPossiveisObj"];
                        }

                        int random = r.Next(0, questoesTrocaObj.Count);
                        questao = questoesTrocaObj.ElementAtOrDefault(random);
                    }
                    else if(tipo ==2)
                    {
                        if (questoesTrocaDisc.Count <= 0)
                        {
                            TempData["listaQuestoesPossiveisDisc"] = Questao.ObterNovasQuestoes(AvalQuestTema, tipo);
                            questoesTrocaDisc = (List<QuestaoTema>)TempData["listaQuestoesPossiveisDisc"];
                        }

                        int random = r.Next(0, questoesTrocaDisc.Count);
                        questao = questoesTrocaDisc.ElementAtOrDefault(random);
                    }

                    if (questao != null)
                    {
                        if(!indices.Contains(indice))
                        {
                            AvalTemaQuestao aqtAntiga = (from atq in DataContextSIAC.GetInstance().AvalTemaQuestao
                                                         where atq.Ano == acad.Ano
                                                         && atq.Semestre == acad.Semestre
                                                         && atq.CodTipoAvaliacao == acad.CodTipoAvaliacao
                                                         && atq.NumIdentificador == acad.NumIdentificador
                                                         && atq.CodQuestao == codQuestao
                                                         select atq).FirstOrDefault();
                            antigas.Add(aqtAntiga);
                            indices.Add(indice);
                        }

                        int index = indices.IndexOf(indice);

                        AvalTemaQuestao atqNova = new AvalTemaQuestao();
                        atqNova.Ano = acad.Avaliacao.Ano;
                        atqNova.Semestre = acad.Avaliacao.Semestre;
                        atqNova.CodTipoAvaliacao = acad.Avaliacao.CodTipoAvaliacao;
                        atqNova.NumIdentificador = acad.Avaliacao.NumIdentificador;
                        atqNova.QuestaoTema = questao;

                        if (novas.Count > index)
                        {
                            novas.RemoveAt(index);
                        }

                        novas.Insert(index, atqNova);

                        ViewData["Index"] = indice;
                        return PartialView("_Questao", questao.Questao);
                    }
                }
            }

            return Json(String.Empty);
        }

        //POST: Avaliacao/Academica/Salvar/ACAD201520001
        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        public ActionResult Salvar(string codigoAval)
        {
            List<AvalTemaQuestao> antigas = (List<AvalTemaQuestao>)TempData["listaQuestoesAntigas"];
            List<AvalTemaQuestao> novas = (List<AvalTemaQuestao>)TempData["listaQuestoesNovas"];

            if(antigas.Count != 0 && novas.Count != 0)
            {
                //AvalAcademica acad = AvalAcademica.ListarPorCodigoAvaliacao(codigoAval);
                var contexto = DataContextSIAC.GetInstance();
                for (int i = 0; i < antigas.Count && i < novas.Count; i++)
                {
                    contexto.AvalTemaQuestao.Remove(antigas.ElementAtOrDefault(i));
                    contexto.AvalTemaQuestao.Add(novas.ElementAtOrDefault(i));
                }
                contexto.SaveChanges();
            }
            TempData.Clear();


            return RedirectToAction("Detalhe",new { codigo = codigoAval });
        }
    }
}