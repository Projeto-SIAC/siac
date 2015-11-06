using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIAC.Models;

namespace SIAC.Controllers
{
    [Filters.CategoriaFilter(Categorias = new[] { 1, 2, 3 })]
    public class CertificacaoController : Controller
    {
        // GET: Certificacao
        public ActionResult Index()
        {
            return View();
        }

        // GET: Certificacao/Gerar
        [Filters.CategoriaFilter(Categorias = new[] { 2/*, 3*/ })]
        public ActionResult Gerar()
        {
            ViewBag.Disciplinas = /*Helpers.Sessao.UsuarioCategoriaCodigo == 2 ? */Disciplina.ListarPorProfessor(Helpers.Sessao.UsuarioMatricula)/*: Disciplina.ListarOrdenadamente()*/;
            ViewBag.Dificuldades = Dificuldade.ListarOrdenadamente();
            ViewBag.Termo = Parametro.Obter().NotaUso;
            return View();
        }

        // POST: Certificacao/Confirmar
        [HttpPost]
        [Filters.CategoriaFilter(Categorias = new[] { 2/*, 3*/ })]
        public ActionResult Confirmar(FormCollection formCollection)
        {
            AvalCertificacao cert = new AvalCertificacao();
            if (formCollection.HasKeys())
            {
                DateTime hoje = DateTime.Now;

                /* Chave */
                cert.Avaliacao = new Avaliacao();
                cert.Avaliacao.TipoAvaliacao = TipoAvaliacao.ListarPorCodigo(3);
                cert.Avaliacao.Ano = hoje.Year;
                cert.Avaliacao.Semestre = hoje.Month > 6 ? 2 : 1;
                cert.Avaliacao.NumIdentificador = Avaliacao.ObterNumIdentificador(3);
                cert.Avaliacao.DtCadastro = hoje;

                /* Professor */
                string strMatr = Helpers.Sessao.UsuarioMatricula;
                cert.Professor = Professor.ListarPorMatricula(strMatr);

                /* Dados */
                int codDisciplina = int.Parse(formCollection["ddlDisciplina"]);

                cert.CodDisciplina = codDisciplina;

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
                List<QuestaoTema> lstQuestoes = new List<QuestaoTema>();

                if (qteObjetiva > 0)
                {
                    lstQuestoes.AddRange(Questao.ListarPorDisciplina(codDisciplina, arrTemaCods, codDificuldade, 1, qteObjetiva));
                }
                if (qteDiscursiva > 0)
                {
                    lstQuestoes.AddRange(Questao.ListarPorDisciplina(codDisciplina, arrTemaCods, codDificuldade, 2, qteDiscursiva));
                }

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
                    cert.Avaliacao.AvaliacaoTema.Add(avalTema);
                }

                ViewBag.QteQuestoes = lstQuestoes.Count;
                ViewBag.QuestoesDaAvaliacao = lstQuestoes;

                AvalCertificacao.Inserir(cert);
            }

            return View(cert);
        }

        // GET: Certificacao/Configurar/CERT201520001
        public ActionResult Configurar(string codigo)
        {
            if (!String.IsNullOrEmpty(codigo))
            {
                AvalCertificacao cert = AvalCertificacao.ListarPorCodigoAvaliacao(codigo);
                if (cert != null && !cert.Avaliacao.FlagRealizada)
                {
                    Professor prof = Professor.ListarPorMatricula(Helpers.Sessao.UsuarioMatricula);
                    if (prof != null && prof.CodProfessor == cert.Professor.CodProfessor)
                    {
                        ViewBag.Dificuldades = Dificuldade.ListarOrdenadamente();
                        ViewBag.TiposQuestao = TipoQuestao.ListarOrdenadamente();
                        return View(cert);
                    }
                }
            }
            return RedirectToAction("Index");
        }

        // POST: Certificacao/CarregarQuestoes/CERT201520001/{temas}/{dificuldade}/{tipo}
        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CarregarQuestoes(string codigo, int[] temas, int dificuldade, int tipo)
        {
            if (!String.IsNullOrEmpty(codigo) && temas.Count() > 0 && dificuldade > 0 && tipo > 0 )
            {
                AvalCertificacao cert = AvalCertificacao.ListarPorCodigoAvaliacao(codigo);
                if (cert != null)
                {
                    List<Questao> questoes = Questao.ListarQuestoesFiltradas(cert.CodDisciplina, temas, dificuldade, tipo);
                    if (questoes != null)
                    {
                        return PartialView("_ListaQuestaoCard", questoes);
                    }
                }
            }
            return null;
        }
    }
}