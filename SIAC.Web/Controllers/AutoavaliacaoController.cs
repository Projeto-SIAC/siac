using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIAC.Web.Models;

namespace SIAC.Web.Controllers
{
    public class AutoavaliacaoController : Controller
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

        // GET: Autoavaliacao
        public ActionResult Index()
        {
            if (Request.Url.ToString().ToLower().Contains("dashboard"))
            {
                return Redirect("~/Historico/Autoavaliacao");
            }
            return View();
        }

        // GET: Autoavaliacao/Minhas
        public ActionResult Minhas()
        {
            int codPessoaFisica = Usuario.ObterPessoaFisica(Session["UsuarioMatricula"].ToString());
            List<AvalAuto> lstAutos = AvalAuto.ListarPorPessoa(codPessoaFisica);

            var result = from a in lstAutos
                         select new
                         {
                             CodAvaliacao = a.Avaliacao.CodAvaliacao(),
                             DtCadastro = a.Avaliacao.DtCadastro.ToBrazilianString(),
                             DtCadastroTempo = a.Avaliacao.DtCadastro.ToElapsedTimeString(),
                             Dificuldade = a.Dificuldade.Descricao,
                             QteQuestoes = a.Avaliacao.QteQuestoes(),
                             Disciplinas = a.Avaliacao.AvaliacaoTema.Select(at => at.Tema.Disciplina.Descricao).Distinct().ToList(),
                             FlagPendente = a.Avaliacao.AvalPessoaResultado.Count > 0 ? false : true
                         };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // GET: Autoavaliacao/Gerar
        public ActionResult Gerar()
        {
            ViewBag.Disciplinas = Disciplina.ListarTemQuestoes();
            ViewBag.Dificuldades = Dificuldade.ListarOrdenadamente();
            return View();
        }

        // POST: Autoavaliacao/Confirmar
        [HttpPost]
        public ActionResult Confirmar(FormCollection formCollection)
        {
            ViewBag.Form = formCollection;

            AvalAuto auto = new AvalAuto();

            var hoje = DateTime.Now;

            /* Chave */
            auto.Avaliacao = new Avaliacao();
            auto.Avaliacao.TipoAvaliacao = TipoAvaliacao.ListarPorCodigo(1);
            //auto.Avaliacao.CodTipoAvaliacao = 1;
            auto.Avaliacao.Ano = hoje.Year;
            auto.Avaliacao.Semestre = hoje.Month > 6 ? 2 : 1;
            auto.Avaliacao.NumIdentificador = Avaliacao.ObterNumIdentificador(1);

            /* Pessoa */
            var strMatr = Session["UsuarioMatricula"].ToString();
            auto.CodPessoaFisica = Usuario.ObterPessoaFisica(strMatr);

            var disciplinas = formCollection["ddlDisciplinas"].Split(',');
            /* Dados */
            List<int> dificuldades = new List<int>();
            List<QuestaoTema> VBQuestoes = new List<QuestaoTema>();
            foreach (var strDisc in disciplinas)
            {
                /* Dificuldade */
                int codDificuldade = int.Parse(formCollection["ddlDificuldade" + strDisc]);
                dificuldades.Add(codDificuldade);

                /* Quantidade */
                int qteObjetiva = 0;
                int qteDiscursiva = 0;
                if (formCollection["ddlTipo"] == "3")
                {
                    int.TryParse(formCollection["txtQteObjetiva" + strDisc], out qteObjetiva);
                    int.TryParse(formCollection["txtQteDiscursiva" + strDisc], out qteDiscursiva);
                }
                else if (formCollection["ddlTipo"] == "2")
                {
                    int.TryParse(formCollection["txtQteDiscursiva" + strDisc], out qteDiscursiva);
                }
                else if (formCollection["ddlTipo"] == "1")
                {
                    int.TryParse(formCollection["txtQteObjetiva" + strDisc], out qteObjetiva);
                }

                /* Temas */
                string[] arrTemaCods = formCollection["ddlTemas" + strDisc].Split(',');

                /* Questões */
                Helpers.TimeLog.Iniciar("Lista de Questões");
                List<QuestaoTema> lstQuestoes = Questao.ListarPorDisciplina(int.Parse(strDisc), arrTemaCods, codDificuldade, qteObjetiva, qteDiscursiva);
                Helpers.TimeLog.Parar();
                foreach (var strTemaCod in arrTemaCods)
                {
                    AvaliacaoTema avalTema = new AvaliacaoTema();
                    avalTema.Tema = Tema.ListarPorCodigo(int.Parse(strDisc), int.Parse(strTemaCod));
                    foreach (var queTma in lstQuestoes.Where(q=>q.CodTema == int.Parse(strTemaCod)))
                    {
                        AvalTemaQuestao avalTemaQuestao = new AvalTemaQuestao();
                        avalTemaQuestao.QuestaoTema = queTma;
                        avalTema.AvalTemaQuestao.Add(avalTemaQuestao);
                    }                   
                    auto.Avaliacao.AvaliacaoTema.Add(avalTema);                    
                }
                VBQuestoes.AddRange(lstQuestoes);
            }

            auto.Avaliacao.DtCadastro = hoje;
            auto.CodDificuldade = dificuldades.Max();

            AvalAuto.Inserir(auto);

            return View(auto);
        }


        // GET: Autoavaliacao/Detalhe/AUTO201520001
        public ActionResult Detalhe(string codigo)
        {
            if (!String.IsNullOrEmpty(codigo))
            {
                AvalAuto auto = AvalAuto.ListarPorCodigoAvaliacao(codigo);
                int codPessoaFisica = Usuario.ObterPessoaFisica(Session["UsuarioMatricula"].ToString());
                if (auto.CodPessoaFisica == codPessoaFisica)
                {
                    double qteObjetiva = 0;
                    Dictionary<string, double> qteObjetivaDisciplina = new Dictionary<string, double>();
                    Dictionary<string, double> qteObjetivaAcertoDisciplina = new Dictionary<string, double>();

                    foreach (var avaliacaoTema in auto.Avaliacao.AvaliacaoTema)
                    {
                        if (!qteObjetivaDisciplina.ContainsKey(avaliacaoTema.Tema.Disciplina.Descricao))
                        {
                            qteObjetivaDisciplina.Add(avaliacaoTema.Tema.Disciplina.Descricao, 0);
                        }
                        if (!qteObjetivaAcertoDisciplina.ContainsKey(avaliacaoTema.Tema.Disciplina.Descricao))
                        {
                            qteObjetivaAcertoDisciplina.Add(avaliacaoTema.Tema.Disciplina.Descricao, 0);
                        }
                        foreach (var avalTemaQuestao in avaliacaoTema.AvalTemaQuestao)
                        {
                            AvalQuesPessoaResposta avalQuesPessoaResposta = avalTemaQuestao.AvalQuesPessoaResposta.First();
                            if (avalTemaQuestao.QuestaoTema.Questao.CodTipoQuestao == 1)
                            {
                                qteObjetivaDisciplina[avaliacaoTema.Tema.Disciplina.Descricao]++;
                                qteObjetiva++;
                                if (avalTemaQuestao.QuestaoTema.Questao.Alternativa.First(q => q.FlagGabarito.HasValue && q.FlagGabarito.Value).CodOrdem == avalQuesPessoaResposta.RespAlternativa)
                                {
                                    qteObjetivaAcertoDisciplina[avaliacaoTema.Tema.Disciplina.Descricao]++;
                                }
                            }
                        }
                    }

                    ViewBag.Porcentagem = (auto.Avaliacao.AvalPessoaResultado.First().QteAcertoObj / qteObjetiva) * 100;
                    ViewBag.Desempenho = new Dictionary<string, double>();
                    foreach (var key in qteObjetivaDisciplina.Keys)
                    {
                        if (qteObjetivaDisciplina[key] > 0)
                        {
                            ViewBag.Desempenho.Add(key, (qteObjetivaAcertoDisciplina[key] / qteObjetivaDisciplina[key]) * 100);
                        }
                    }

                    return View(auto);
                }
            }
            return RedirectToAction("Index");
        }

        // GET: Autoavaliacao/Realizar/AUTO201520001
        [HttpGet]
        public ActionResult Realizar(string codigo)
        {
            if (!String.IsNullOrEmpty(codigo))
            {
                AvalAuto auto = AvalAuto.ListarPorCodigoAvaliacao(codigo);
                if (auto.CodPessoaFisica == Usuario.ObterPessoaFisica(Session["UsuarioMatricula"].ToString()))
                {
                    return View(auto);
                }
            }
            int codPessoaFisica = Usuario.ObterPessoaFisica(Session["UsuarioMatricula"].ToString());
            ViewBag.Geradas = AvalAuto.ListarNaoRealizadaPorPessoa(codPessoaFisica);
            return View("Novo");
        }

        // POST: Autoavaliacao/Resultado/AUTO201520001
        [HttpPost]
        public ActionResult Resultado(string codigo, FormCollection form)
        {
            int codPessoaFisica = Usuario.ObterPessoaFisica(Session["UsuarioMatricula"].ToString());
            if (!String.IsNullOrEmpty(codigo))
            {
                AvalAuto auto = AvalAuto.ListarPorCodigoAvaliacao(codigo);

                AvalPessoaResultado avalPessoaResultado = new AvalPessoaResultado();
                avalPessoaResultado.CodPessoaFisica = codPessoaFisica;
                avalPessoaResultado.HoraTermino = DateTime.Now;
                avalPessoaResultado.QteAcertoObj = 0;

                double qteObjetiva = 0;
                Dictionary<string, double> qteObjetivaDisciplina = new Dictionary<string, double>();
                Dictionary<string, double> qteObjetivaAcertoDisciplina = new Dictionary<string, double>();

                foreach (var avaliacaoTema in auto.Avaliacao.AvaliacaoTema)
                {
                    if (!qteObjetivaDisciplina.ContainsKey(avaliacaoTema.Tema.Disciplina.Descricao))
                    {
                        qteObjetivaDisciplina.Add(avaliacaoTema.Tema.Disciplina.Descricao, 0);
                    }
                    if (!qteObjetivaAcertoDisciplina.ContainsKey(avaliacaoTema.Tema.Disciplina.Descricao))
                    {
                        qteObjetivaAcertoDisciplina.Add(avaliacaoTema.Tema.Disciplina.Descricao, 0);
                    }
                    foreach (var avalTemaQuestao in avaliacaoTema.AvalTemaQuestao)
                    {
                        AvalQuesPessoaResposta avalQuesPessoaResposta = new AvalQuesPessoaResposta();
                        avalQuesPessoaResposta.CodPessoaFisica = codPessoaFisica;
                        if (avalTemaQuestao.QuestaoTema.Questao.CodTipoQuestao == 1)
                        {
                            qteObjetivaDisciplina[avaliacaoTema.Tema.Disciplina.Descricao]++;
                            qteObjetiva++;
                            avalQuesPessoaResposta.RespAlternativa = int.Parse(form["rdoResposta" + avalTemaQuestao.QuestaoTema.Questao.CodQuestao]);
                            if (avalTemaQuestao.QuestaoTema.Questao.Alternativa.First(q=> q.FlagGabarito.HasValue && q.FlagGabarito.Value).CodOrdem == avalQuesPessoaResposta.RespAlternativa)
                            {
                                avalPessoaResultado.QteAcertoObj++;
                                qteObjetivaAcertoDisciplina[avaliacaoTema.Tema.Disciplina.Descricao]++;
                            }
                        }
                        else
                        {
                            avalQuesPessoaResposta.RespDiscursiva = form["txtResposta" + avalTemaQuestao.QuestaoTema.Questao.CodQuestao].Trim();
                        }
                        avalQuesPessoaResposta.RespComentario = !String.IsNullOrEmpty(form["txtComentario" + avalTemaQuestao.QuestaoTema.Questao.CodQuestao]) ? form["txtComentario" + avalTemaQuestao.QuestaoTema.Questao.CodQuestao].Trim() : null;
                        avalTemaQuestao.AvalQuesPessoaResposta.Add(avalQuesPessoaResposta);
                    }

                }

                auto.Avaliacao.AvalPessoaResultado.Add(avalPessoaResultado);

                DataContextSIAC.GetInstance().SaveChanges();

                ViewBag.Porcentagem = (avalPessoaResultado.QteAcertoObj / qteObjetiva) * 100;
                ViewBag.Desempenho = new Dictionary<string, double>();
                foreach (var key in qteObjetivaDisciplina.Keys)
                {
                    if (qteObjetivaDisciplina[key]>0)
                    {
                        ViewBag.Desempenho.Add(key, (qteObjetivaAcertoDisciplina[key] / qteObjetivaDisciplina[key]) * 100);
                    }
                }                

                return View(auto);
            }            
            ViewBag.Geradas = AvalAuto.ListarNaoRealizadaPorPessoa(codPessoaFisica);
            return View("Novo");
        }
    }
}