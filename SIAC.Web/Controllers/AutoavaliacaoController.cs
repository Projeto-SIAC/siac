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
            auto.Avaliacao.TipoAvaliacao = DataContextSIAC.GetInstance().TipoAvaliacao.First();
            auto.Avaliacao.CodTipoAvaliacao = 1;
            auto.Avaliacao.Ano = hoje.Year;
            auto.Avaliacao.Semestre = hoje.Month > 6 ? 2 : 1;
            //var list = DataContextSIAC.GetInstance().Avaliacao
            //  .Where(a => (a.CodTipoAvaliacao == auto.Avaliacao.CodTipoAvaliacao) && (a.Ano == auto.Avaliacao.Ano) && (a.Semestre == auto.Avaliacao.Semestre))
            //  .ToList();
            //auto.Avaliacao.NumIdentificador = list.Count > 0 ? list.Max(a => a.NumIdentificador) + 1 : 1;
            auto.Avaliacao.NumIdentificador = Avaliacao.ObterNumIdentificador(1);

            /* Pessoa */
            var strMatr = Session["UsuarioMatricula"].ToString();
            //auto.PessoaFisica = DataContextSIAC.GetInstance().Usuario.SingleOrDefault(u => u.Matricula == strMatr).PessoaFisica;
            auto.CodPessoaFisica = Usuario.ObterPessoaFisica(strMatr);

            var disciplinas = formCollection["ddlDisciplinas"].Split(',');
            /* Dados */
            List<int> dificuldades = new List<int>();
            foreach (var strDisc in disciplinas)
            {
                dificuldades.Add(int.Parse(formCollection["ddlDificuldade"+strDisc]));
                var temas = formCollection["ddlTemas" + strDisc].Split(',');
                foreach (var strTema in temas)
                {
                    auto.Avaliacao.AvaliacaoTema.Add(new AvaliacaoTema {
                        Tema = Tema.ListarPorCodigo(int.Parse(strDisc), int.Parse(strTema))
                    });
                }
            }
            auto.Dificuldade = Dificuldade.ListarPorCodigo(dificuldades.Max());
            auto.Avaliacao.DtCadastro = hoje;

            /* Codigo para selecionar as questões */

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
            List<Questao> questoes = Questao.ListarPorDisciplina(coddisciplina, temasi, dificuldade, qteObj, qteDis);
            Helpers.TimeLog.Parar();

            /* TERÁ QUE MELHORAR A PERFORMANCE */

            //ESSA PARTE EU NÃO SEI O QUE CÊ VAI FAZER DEPOIS
            ViewBag.QuestoesDaAvaliacao = questoes;

            return View(auto);
        }

        // GET: Autoavaliacao/Realizar
        public ActionResult Realizar()
        {
            return View();
        }
    }
}