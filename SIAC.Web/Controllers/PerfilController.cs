using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using SIAC.Models;
using SIAC.Helpers;

namespace SIAC.Controllers
{
    [Filters.AutenticacaoFilter(Categorias = new[] { 1, 2, 3 })]
    public class PerfilController : Controller
    {
        // GET: Perfil
        public ActionResult Index()
        {
            return View(Sistema.UsuarioAtivo[Sessao.UsuarioMatricula].Usuario);
        }
        public ActionResult Estatisticas()
        {
            return PartialView("_Estatisticas", Sistema.UsuarioAtivo[Sessao.UsuarioMatricula].Usuario);
        }

    }
}