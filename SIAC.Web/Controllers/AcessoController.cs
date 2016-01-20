using SIAC.Helpers;
using SIAC.Models;
using SIAC.ViewModels;
using System;
using System.Linq;
using System.Web.Mvc;

namespace SIAC.Controllers
{
    public class AcessoController : Controller
    {
        // GET: /
        public ActionResult Index()
        {
            Parametro.ObterAsync();
            if (Sistema.Autenticado(Sessao.UsuarioMatricula))
                return RedirectToAction("Index", "Principal");
            return View(new AcessoIndexViewModel());
        }

        /*
        GET: Acesso/Entrar
        [HttpGet]
        public ActionResult Entrar()
        {
            if (Helpers.Sessao.Autenticado)
            {
                return RedirectToAction("Index", "Principal");
            }
            ViewBag.Acao = "$('.modal').modal('show')";
            return View("Index");
        }
        */

        // POST: /
        [HttpPost]
        public ActionResult Index(FormCollection formCollection)
        {
            if (Sistema.Autenticado(Sessao.UsuarioMatricula))
                return RedirectToAction("Index", "Principal");

            bool validado = false;

            if (formCollection.HasKeys())
            {
                if (!StringExt.IsNullOrWhiteSpace(formCollection["txtMatricula"], formCollection["txtSenha"]))
                {
                    string matricula = formCollection["txtMatricula"].ToString();
                    string senha = formCollection["txtSenha"].ToString();

                    Usuario usuario = Usuario.Autenticar(matricula, senha);

                    if (usuario != null)
                    {
                        validado = true;
                        Sessao.Inserir("UsuarioMatricula", usuario.Matricula);
                        Sessao.Inserir("UsuarioNome", usuario.PessoaFisica.Nome);
                        Sessao.Inserir("UsuarioCategoriaCodigo", usuario.CodCategoria);
                        Sessao.Inserir("UsuarioCategoria", usuario.Categoria.Descricao);

                        //Verificando se a senha do usuário é padrão de visitante
                        if (usuario.CodCategoria == 4 && senha == Sistema.GerarSenhaPadrao(usuario))
                            Sessao.Inserir("UsuarioSenhaPadrao", true);

                        Usuario.RegistrarAcesso(usuario.Matricula);
                        Sistema.RegistrarCookie(usuario.Matricula);
                    }
                }
            }

            if (validado)
            {
                Lembrete.AdicionarNotificacao("Seu usuário foi autenticado com sucesso.", Lembrete.Positivo);
                if (Request.QueryString["continuar"] != null)
                    return Redirect(Request.QueryString["continuar"].ToString());
                return RedirectToAction("Index", "Principal");
            }
            else
            {
                AcessoIndexViewModel model = new AcessoIndexViewModel();
                model.Matricula = formCollection.HasKeys() ? formCollection["txtMatricula"] : "";
                //ViewBag.Acao = "$('.modal').modal('show');";
                model.Erro = true;
                if (!String.IsNullOrWhiteSpace(model.Matricula) && Sistema.UsuarioAtivo.Keys.Contains(model.Matricula))
                {
                    model.Mensagens = new string[] { "Seu usuário já está conectado." };
                }
                return View(model);
            }
        }

        // GET: acesso/conectado
        public ActionResult Conectado() => null;

        // GET: acesso/sair
        public ActionResult Sair()
        {
            Sistema.UsuarioAtivo.Remove(Sessao.UsuarioMatricula);
            Sistema.RemoverCookie(Sessao.UsuarioMatricula);
            Sessao.Limpar();
            return Redirect("~/");
        }

        // GET: acesso/visitante
        public ActionResult Visitante()
        {
            if (!String.IsNullOrWhiteSpace(Sessao.UsuarioMatricula) && Sessao.UsuarioCategoriaCodigo == 4 && Sessao.UsuarioSenhaPadrao)
            {
                Usuario usuario = Usuario.ListarPorMatricula(Sessao.UsuarioMatricula);
                if (usuario != null)
                    return View(usuario);
            }
            return RedirectToAction("Index");
        }

        // POST: acesso/visitante
        [HttpPost]
        public ActionResult Visitante(FormCollection formCollection)
        {
            if (formCollection.HasKeys() && !StringExt.IsNullOrWhiteSpace(formCollection["txtNovaSenha"], formCollection["txtConfirmarNovaSenha"]))
            {
                if (!String.IsNullOrWhiteSpace(Sessao.UsuarioMatricula) && Sessao.UsuarioCategoriaCodigo == 4)
                {
                    Usuario usuario = Usuario.ListarPorMatricula(Sessao.UsuarioMatricula);
                    if (usuario != null)
                    {
                        string novaSenha = formCollection["txtNovaSenha"];
                        string confirmarNovaSenha = formCollection["txtConfirmarNovaSenha"];

                        if (novaSenha != Sistema.GerarSenhaPadrao(usuario) && novaSenha == confirmarNovaSenha)
                        {
                            usuario.AtualizarSenha(novaSenha);
                            Sessao.Inserir("UsuarioSenhaPadrao", false);
                        }
                    }
                }
            }
            return RedirectToAction("Index");
        }
    }
}