using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIAC.Web.Controllers
{
    public class AcessoController : Controller
    {
        // GET: Acesso
        public ActionResult Index()
        {
            return View();
        }

        // GET: Acesso/Login
        [HttpGet]
        public ActionResult Login()
        {
            ViewBag.Acao = "show";
            return View("Index");
        }

        // POST: Acesso/Login
        [HttpPost]
        public ActionResult Login(FormCollection formCollection)
        {
            bool valido = false;

            if (formCollection.HasKeys())
            {
                if (!String.IsNullOrWhiteSpace(formCollection["TextBoxMatricula"]) && formCollection["TextBoxMatricula"].ToString() == "postero")
                {
                    if (!String.IsNullOrWhiteSpace(formCollection["TextBoxSenha"]) && formCollection["TextBoxSenha"].ToString() == "2699")
                    {
                        valido = true;
                    }
                }
            }

            if (valido)
                return RedirectToAction("Index", "Dashboard");
            else
                return RedirectToAction("Index");
        }
    }
}