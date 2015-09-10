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
        public ActionResult Login()
        {
            ViewBag.Acao = "show";
            return View("Index");
        }
    }
}