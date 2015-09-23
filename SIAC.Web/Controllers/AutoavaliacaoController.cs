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
            auto.Avaliacao.CodTipoAvaliacao = 1;
            auto.Avaliacao.Ano = hoje.Year;
            auto.Avaliacao.Semestre = hoje.Month > 6 ? 2 : 1;
            auto.Avaliacao.NumIdentificador = Avaliacao.ObterNumIdentificador(1);

            /* Pessoa */
            var strMatr = Session["UsuarioMatricula"].ToString();
            auto.CodPessoaFisica = Usuario.ObterPessoaFisica(strMatr);

            var disciplinas = formCollection["ddlDisciplinas"].Split(',');
            /* Dados */
            List<int> dificuldades = new List<int>();
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
                //var temasCod = new List<int>();
                string[] temasCod = formCollection["ddlTemas" + strDisc].Split(',');
                //foreach (var strTema in formCollection["ddlTemas" + strDisc].Split(','))
                //{
                //temasCod.Add(int.Parse(strTema));
                //}

                /* Questões */
                Helpers.TimeLog.Iniciar("Lista de Questões");
                List<QuestaoTema> lstQuestoes = Questao.ListarPorDisciplina(int.Parse(strDisc), temasCod, codDificuldade, qteObjetiva, qteDiscursiva);
                Helpers.TimeLog.Parar();
                /* return QuestaoTema
                foreach (var temaCod in temasCod)
                {
                    auto.Avaliacao.AvaliacaoTema.Add(new AvaliacaoTema
                    {
                        Tema = Tema.ListarPorCodigo(int.Parse(strDisc), temaCod),
                    });

                    foreach (var questaoTema in lstQuestoes.Select(q=>q.QuestaoTema.Where(qt=>qt.CodTema == temaCod)))
                    {
                        auto.Avaliacao.AvaliacaoTema.Add()
                    }
                }*/
                ViewBag.QuestoesDaAvaliacao = lstQuestoes;
                ViewBag.QteQuestoes = lstQuestoes.Count;

            }
            /*auto.Dificuldade = Dificuldade.ListarPorCodigo(dificuldades.Max());
            auto.Avaliacao.DtCadastro = hoje;

            int coddisciplina =int.Parse(disciplinas.ElementAt(0));
            int dificuldade = int.Parse(formCollection["ddlDificuldade"+coddisciplina]);
            int qteObj  = int.Parse(formCollection["txtQteObjetiva" + coddisciplina]);
            int qteDis  = int.Parse(formCollection["txtQteDiscursiva" + coddisciplina]);
            List <int> temasi = new List<int>();
            foreach (var item in formCollection["ddlTemas" + coddisciplina].Split(','))
            {
                temasi.Add(int.Parse(item));
            }
            Helpers.TimeLog.Iniciar("Lista de Questões");
            List<QuestaoTema> questoes = Questao.ListarPorDisciplina(coddisciplina, temasi, dificuldade, qteObj, qteDis);
            Helpers.TimeLog.Parar();
            */
            /* TERÁ QUE MELHORAR A PERFORMANCE */
            
            //ESSA PARTE EU NÃO SEI O QUE CÊ VAI FAZER DEPOIS
            //ViewBag.QuestoesDaAvaliacao = questoes;

            return View(auto);
        }

        // GET: Autoavaliacao/Realizar/AUTO201520001
        public ActionResult Realizar(string codigo)
        {
            AvalAuto auto = new AvalAuto();
            return View(auto);
        }
    }
}