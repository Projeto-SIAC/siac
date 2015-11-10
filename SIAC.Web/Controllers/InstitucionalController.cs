using SIAC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIAC.Controllers
{
    [Filters.AutenticacaoFilter(Categorias = new[] { 1, 2, 3 })]
    public class InstitucionalController : Controller
    {
        // GET: Institucional/
        public ActionResult Index()
        {
            Usuario usuario = Usuario.ListarPorMatricula(Helpers.Sessao.UsuarioMatricula);
            return View(usuario);
        }

        // GET: Institucional/Avaliacao/Gerar
        public ActionResult Gerar()
        {
            return View();
        }
    }
}