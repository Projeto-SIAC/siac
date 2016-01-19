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
        // GET: Acesso
        public ActionResult Index()
        {
            Parametro.ObterAsync();
            if (Sistema.Autenticado(Sessao.UsuarioMatricula))
            {
                return RedirectToAction("Index", "Principal");
            }
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

        // POST: Acesso/Entrar
        [HttpPost]
        public ActionResult Index(FormCollection formCollection)
        {
            if (Sistema.Autenticado(Sessao.UsuarioMatricula))
            {
                return RedirectToAction("Index", "Principal");
            }

            bool valido = false;

            if (formCollection.HasKeys())
            {
                if (!StringExt.IsNullOrWhiteSpace(formCollection["txtMatricula"], formCollection["txtSenha"]))
                {
                    string matricula = formCollection["txtMatricula"].ToString();
                    string senha = formCollection["txtSenha"].ToString();

                    Usuario usuario = Usuario.Autenticar(matricula, senha);

                    if (usuario != null)
                    {
                        valido = true;
                        Sessao.Inserir("UsuarioMatricula", usuario.Matricula);
                        Sessao.Inserir("UsuarioNome", usuario.PessoaFisica.Nome);
                        Sessao.Inserir("UsuarioCategoriaCodigo", usuario.CodCategoria);
                        Sessao.Inserir("UsuarioCategoria", usuario.Categoria.Descricao);

                        //Verificando se a senha do usuário é diferente do padrão de VISITANTE
                        if (usuario.CodCategoria == 4)
                        {
                            string senhaVisitante = usuario.PessoaFisica.PrimeiroNome + "@" + usuario.PessoaFisica.Cpf.Substring(0, 3);

                            if (senha == senhaVisitante)
                            {
                                Sessao.Inserir("UsuarioSenhaPadrao", true);
                            }
                        }

                        Usuario.RegistrarAcesso(usuario.Matricula);
                        Sistema.RegistrarCookie(usuario.Matricula);
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
                return RedirectToAction("Index", "Principal");
            }
            else
            {
                AcessoIndexViewModel model = new AcessoIndexViewModel();
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
            Sistema.UsuarioAtivo.Remove(Sessao.UsuarioMatricula);
            Sistema.RemoverCookie(Sessao.UsuarioMatricula);
            Sessao.Limpar();
            return Redirect("~/");
        }

        // GET: Acesso/Visitante
        public ActionResult Visitante()
        {
            if (!String.IsNullOrEmpty(Sessao.UsuarioMatricula))
            {
                if (Sessao.UsuarioCategoriaCodigo == 4)
                {
                    if (Sessao.UsuarioSenhaPadrao)
                    {
                        Usuario usuario = Usuario.ListarPorMatricula(Sessao.UsuarioMatricula);
                        if (usuario != null)
                        {
                            return View(usuario);
                        }
                    }
                }
            }
            return RedirectToAction("Index");
        }

        // POST: Acesso/Visitante
        [HttpPost]
        public ActionResult Visitante(FormCollection formCollection)
        {
            if (!String.IsNullOrEmpty(Sessao.UsuarioMatricula))
            {
                if (Sessao.UsuarioCategoriaCodigo == 4)
                {
                    Usuario usuario = Usuario.ListarPorMatricula(Sessao.UsuarioMatricula);
                    if (usuario != null)
                    {
                        string novaSenha = formCollection["txtNovaSenha"];
                        string confirmaNovaSenha = formCollection["txtConfirmaNovaSenha"];

                        if(novaSenha != usuario.PessoaFisica.PrimeiroNome + "@" + usuario.PessoaFisica.Cpf.Substring(0, 3))
                        {
                            if (novaSenha == confirmaNovaSenha)
                            {
                                usuario.AtualizarSenha(novaSenha);
                                Sessao.Inserir("UsuarioSenhaPadrao", false);
                            }
                        }
                    }
                }
            }
            return RedirectToAction("Index");
        }
    }
}