using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIAC.Controllers
{
    [Filters.AutenticacaoFilter(Categorias = new[] { 1, 2, 3 })]
    public class PerfilController : Controller
    {
        // GET: Perfil
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Acesso");
        }
    }
}