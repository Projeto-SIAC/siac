using SIAC.Models;
using System.Web.Mvc;

namespace SIAC.Controllers
{
    [Filters.AutenticacaoFilter]
    public class HistoricoController : Controller
    {
        // GET: Historico
        [OutputCache(CacheProfile = "PorUsuario")]
        public ActionResult Index()
        {
            Lembrete.AdicionarNotificacao("Este é seu histórico de atividades.", Lembrete.INFO);
            Usuario usuario = Usuario.ListarPorMatricula(Helpers.Sessao.UsuarioMatricula);
            return View(usuario);
        }

        // GET: Historico/Avaliacao
        public ActionResult Avaliacao()
        {
            return RedirectToAction("Index");
        }
    }
}