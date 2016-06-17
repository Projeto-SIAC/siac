using SIAC.Helpers;
using SIAC.Models;
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