using SIAC.Models;
using SIAC.Helpers;
using System.Web.Mvc;

namespace SIAC.Controllers
{
    [Filters.AutenticacaoFilter]
    public class HistoricoController : Controller
    {
        // GET: historico
        public ActionResult Index()
        {
            Usuario usuario = Usuario.ListarPorMatricula(Sessao.UsuarioMatricula);
            return View(usuario);
        }

        // GET: historico/avaliacao
        public ActionResult Avaliacao() => RedirectToAction("Index");
    }
}