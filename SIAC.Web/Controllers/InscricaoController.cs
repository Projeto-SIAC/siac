using SIAC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIAC.Controllers
{
    public class InscricaoController : Controller
    {
        // GET: simulado/inscricao
        public ActionResult Index()
        {
            return View(new InscricaoIndexViewModel());
        }
    }
}