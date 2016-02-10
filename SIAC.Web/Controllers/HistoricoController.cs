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
            Lembrete.AdicionarNotificacao("Este é seu histórico de atividades.", Lembrete.INFO);
            Usuario usuario = Usuario.ListarPorMatricula(Sessao.UsuarioMatricula);
            return View(usuario);
        }

        // GET: historico/avaliacao
        public ActionResult Avaliacao() => RedirectToAction("Index");
    }
}