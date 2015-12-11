using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIAC.Models;
using System.Threading.Tasks;

namespace SIAC.Controllers
{
    public class AcessoController : Controller
    {
        // GET: Acesso
        public ActionResult Index()
        {
            Parametro.ObterAsync();
            if (Models.Sistema.Autenticado(Helpers.Sessao.UsuarioMatricula))
            {
                return RedirectToAction("Index", "Dashboard");
            }
            return View(new ViewModels.AcessoIndexViewModel());
        }

        /*
        GET: Acesso/Entrar
        [HttpGet]
        public ActionResult Entrar()
        {
            if (Helpers.Sessao.Autenticado)
            {
                return RedirectToAction("Index", "Dashboard");
            }
            ViewBag.Acao = "$('.modal').modal('show')";
            return View("Index");
        }
        */

        // POST: Acesso/Entrar
        [HttpPost]
        public ActionResult Index(FormCollection formCollection)
        {
            if (Models.Sistema.Autenticado(Helpers.Sessao.UsuarioMatricula))
            {
                return RedirectToAction("Index", "Dashboard");
            }

            bool valido = false;

            if (formCollection.HasKeys())
            {
                if (!Helpers.StringExt.IsNullOrWhiteSpace(formCollection["txtMatricula"], formCollection["txtSenha"]))
                {
                    string matricula = formCollection["txtMatricula"].ToString();
                    string senha = formCollection["txtSenha"].ToString();

                    ViewBag.TextBoxMatricula = matricula;

                    Usuario usuario = Usuario.Autenticar(matricula, senha);

                    if (usuario != null)
                    {
                        valido = true;
                        Helpers.Sessao.Inserir("UsuarioMatricula", usuario.Matricula);
                        Helpers.Sessao.Inserir("UsuarioNome", usuario.PessoaFisica.Nome);
                        Helpers.Sessao.Inserir("UsuarioCategoriaCodigo", usuario.CodCategoria);
                        Helpers.Sessao.Inserir("UsuarioCategoria", usuario.Categoria.Descricao);
                        Usuario.RegistrarAcesso(usuario.Matricula);
                    }
                }
            }

            if (valido)
            {
                Lembrete.AdicionarNotificacao("Seu usuário foi autenticado com sucesso.", Lembrete.Positivo);
                if (Request.QueryString["continuar"] != null)
                {
                    return Redirect(Request.QueryString["continuar"].ToString());
                }
                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                var model = new ViewModels.AcessoIndexViewModel();
                model.Matricula = formCollection.HasKeys() ? formCollection["txtMatricula"] : "";
                //ViewBag.Acao = "$('.modal').modal('show');";
                model.Erro = true;
                if (!String.IsNullOrEmpty(model.Matricula) && Sistema.UsuarioAtivo.Keys.Contains(model.Matricula))
                {
                    model.Mensagens = new string[] { "Seu usuário já está conectado." };
                }
                return View(model);
            }
        }

        // GET: Acesso/Conectado
        public ActionResult Conectado()
        {
            return null;
        }

        // GET: Acesso/Sair
        public ActionResult Sair()
        {
            Sistema.UsuarioAtivo.Remove(Helpers.Sessao.UsuarioMatricula);
            Helpers.Sessao.Limpar();
            return Redirect("~/");
        }
    }
}