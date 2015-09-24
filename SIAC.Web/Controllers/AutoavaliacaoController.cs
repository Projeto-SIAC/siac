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
            var lstAutos = (from avalAuto in DataContextSIAC.GetInstance().AvalAuto
                               where avalAuto.CodPessoaFisica == codPessoaFisica
                               orderby avalAuto.Avaliacao.DtCadastro descending
                               select avalAuto).ToList();
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

        // POST: Autoavaliacao/Realizar
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

            //AvalAuto.Inserir(auto);
            ViewBag.QuestoesDaAvaliacao = VBQuestoes;
            ViewBag.QteQuestoes = VBQuestoes.Count;

            return View(auto);
        }

        // GET: Autoavaliacao/Realizar/AUTO201520001
        public ActionResult Realizar(string codigo)
        {
            if (!String.IsNullOrEmpty(codigo))
            {
                int numIdentificador = int.Parse(codigo.Substring(codigo.Length-4));
                codigo = codigo.Remove(codigo.Length - 4);
                int semestre = int.Parse(codigo.Substring(codigo.Length - 1));
                codigo = codigo.Remove(codigo.Length - 1);
                int ano = int.Parse(codigo.Substring(codigo.Length - 4));
                codigo = codigo.Remove(codigo.Length - 4);
                int codTipoAvaliacao = (from tipo in DataContextSIAC.GetInstance().TipoAvaliacao
                                          where tipo.Sigla == codigo
                                          select tipo.CodTipoAvaliacao).First();
                AvalAuto auto = (from avalAuto in DataContextSIAC.GetInstance().AvalAuto
                                where avalAuto.Ano == ano 
                                && avalAuto.Semestre == semestre
                                && avalAuto.NumIdentificador == numIdentificador
                                && avalAuto.CodTipoAvaliacao == codTipoAvaliacao
                                select avalAuto).SingleOrDefault();
                return View(auto);
            }
            int codPessoaFisica = Usuario.ObterPessoaFisica(Session["UsuarioMatricula"].ToString());
            ViewBag.Geradas = (from avalAuto in DataContextSIAC.GetInstance().AvalAuto
                              where avalAuto.CodPessoaFisica == codPessoaFisica
                              && avalAuto.Avaliacao.AvalPessoaResultado.Count == 0
                              select avalAuto).ToList();
            return View("Novo");
        }
    }
}