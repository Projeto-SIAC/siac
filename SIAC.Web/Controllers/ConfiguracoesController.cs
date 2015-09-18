using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIAC.Web.Models;
using SIAC.Web.Helpers;

namespace SIAC.Web.Controllers
{
    public class ConfiguracoesController : Controller
    {

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if(Session["Autenticado"] == null)
            {
                if (TempData["UrlReferrer"] != null)
                {
                    filterContext.Result = Redirect(TempData["UrlReferrer"].ToString());
                }
                else filterContext.Result = RedirectToAction("Index", "Dashboard");
            }
            else if(!(bool)Session["Autenticado"])
            {
                if (TempData["UrlReferrer"] != null)
                {
                    filterContext.Result = Redirect(TempData["UrlReferrer"].ToString());
                }
                else filterContext.Result = RedirectToAction("Index", "Dashboard");
            }
            else if((int)Session["UsuarioCategoriaCodigo"] != 3)
            {
                if (TempData["UrlReferrer"] != null)
                {
                    filterContext.Result = Redirect(TempData["UrlReferrer"].ToString());
                }
                else filterContext.Result = RedirectToAction("Index", "Dashboard");
            }

            base.OnActionExecuting(filterContext);
        }

        // GET: /Configuracoes
        public ActionResult Index()
        {
            Parametro model = Parametro.Obter();

            //ViewBag.Disciplinas = Disciplina.ListarOrdenadamente(); AINDA NÃO SEI PORQUE NÃO FUNCIONOU

            return View(model);
        }

        //POST: /Configuracoes
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Index(FormCollection formCollection)
        {
            if (formCollection.HasKeys())
            {
                Parametro temp = Parametro.Obter();
                temp.TempoInatividade = int.Parse(formCollection["txtTempoInatividade"]);
                temp.NumeracaoQuestao = int.Parse(formCollection["ddlNumeracaoQuestao"]);
                temp.NumeracaoAlternativa = int.Parse(formCollection["ddlNumeracaoAlternativa"]);
                temp.QteSemestres = int.Parse(formCollection["txtQteSemestre"]);
                temp.TermoResponsabilidade = formCollection["txtTermoResponsabilidade"];

                Parametro.Atualizar(temp);
            }

            return null;
        }

        //POST: /Configuracoes/CadastrarProfessor
        [HttpPost]
        public ActionResult CadastrarProfessor(FormCollection formCollection)
        {
            if(formCollection.HasKeys())
            {

                string ProfessorNome = formCollection["txtProfessorNome"];
                string ProfessorMatricula = formCollection["txtProfessorMatricula"];

                int codPessoa = Pessoa.Inserir(new Pessoa() { TipoPessoa = "F" });

                PessoaFisica pf = new PessoaFisica();
                pf.CodPessoa = codPessoa;
                pf.Nome = ProfessorNome;
                pf.Categoria.Add(Categoria.ListarPorCodigo(2));

                int codPessoaFisica = PessoaFisica.Inserir(pf);

                Usuario usuario = new Usuario();
                usuario.Matricula = ProfessorMatricula;
                usuario.CodPessoaFisica = codPessoaFisica;
                usuario.CodCategoria = 2;
                usuario.Senha = Criptografia.RetornarHash("senha");

                int codUsuario = Usuario.Inserir(usuario);

                Professor professor = new Professor();
                professor.MatrProfessor = ProfessorMatricula;

                string[] disciplinas = formCollection["ddlProfessorDisciplinas"].Split(',');
                foreach (string item in disciplinas)
                {
                    professor.Disciplina.Add(Disciplina.ListarPorCodigo(int.Parse(item)));
                }

                Professor.Inserir(professor);

            }


            return RedirectToAction("Index");
        }
    }
}