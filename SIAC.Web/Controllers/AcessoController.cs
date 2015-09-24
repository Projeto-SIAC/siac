using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIAC.Web.Models;
using System.Threading.Tasks;

namespace SIAC.Web.Controllers
{
    public class AcessoController : Controller
    {
        // GET: Acesso
        public ActionResult Index()
        {
            Parametro.ObterAsync();
            if (Session["Autenticado"] != null && (bool)Session["Autenticado"])
            {
                return RedirectToAction("Index", "Dashboard");
            }
            return View();
        }

        // GET: Acesso/Entrar
        [HttpGet]
        public ActionResult Entrar()
        {
            if (Session["Autenticado"] != null && (bool)Session["Autenticado"])
            {
                return RedirectToAction("Index", "Dashboard");
            }
            ViewBag.Acao = "$('.modal').modal('show')";
            return View("Index");
        }

        // POST: Acesso/Entrar
        [HttpPost]
        public ActionResult Entrar(FormCollection formCollection)
        {
            if (Session["Autenticado"] != null && (bool)Session["Autenticado"])
            {
                return RedirectToAction("Index", "Dashboard");
            }

            bool valido = false;

            if (formCollection.HasKeys())
            {
                if (!String.IsNullOrWhiteSpace(formCollection["txtMatricula"]) && !String.IsNullOrWhiteSpace(formCollection["txtSenha"]))
                {
                    string matricula = formCollection["txtMatricula"].ToString();
                    string senha = formCollection["txtSenha"].ToString();

                    ViewBag.TextBoxMatricula = matricula;

                    Usuario usuario = Usuario.Autenticar(matricula, senha);

                    if (usuario != null)
                    {
                        valido = true;
                        Session["Autenticado"] = true;
                        Session["UsuarioMatricula"] = usuario.Matricula;
                        Session["UsuarioNome"] = usuario.PessoaFisica.Nome;
                        Session["UsuarioCategoriaCodigo"] = usuario.CodCategoria;
                        Session["UsuarioCategoria"] = usuario.Categoria.Descricao;
                    }
                }
            }

            if (valido)
            {
                if (TempData["UrlReferrer"] != null)
                {
                    return Redirect(TempData["UrlReferrer"].ToString());
                }
                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                ViewBag.Acao = "$('.modal').modal('show')";
                ViewBag.Erro = "error";
                return View("Index");
            }
        }

        // GET: Acesso/Sair
        public ActionResult Sair()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }
    }
}