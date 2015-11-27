using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using SIAC.Models;
using SIAC.ViewModels;
using SIAC.Helpers;

namespace SIAC.Controllers
{
    public class VisitanteController : Controller
    {
        private List<Visitante> Visitantes
        {
            get
            {
                return new List<Visitante>();
            }
        }

        public ActionResult Listar()
        {
            return Json(null);
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Cadastrar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Cadastrar(FormCollection form)
        {
            return View();
        }
    }
}