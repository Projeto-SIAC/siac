using SIAC.Models;
using SIAC.Helpers;
using System.Web.Mvc;

namespace SIAC.Controllers
{
    [Filters.AutenticacaoFilter]
    public class HistoricoController : Controller
    {
        // GET: historico
        public ActionResult Index() => View();

        // GET: historico/avaliacao
        public ActionResult Avaliacao() => RedirectToAction("Index");
    }
}