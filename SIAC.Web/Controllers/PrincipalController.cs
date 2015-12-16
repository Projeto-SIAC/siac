using SIAC.Helpers;
using SIAC.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SIAC.Controllers
{
    [Filters.AutenticacaoFilter]
    public class PrincipalController : Controller
    {
        // GET: Principal
        [OutputCache(CacheProfile = "PorUsuario")]
        public ActionResult Index()
        {
            Lembrete.AdicionarNotificacao("Este é sua tela principal.", Lembrete.Info);
            Usuario usuario = Sistema.UsuarioAtivo[Sessao.UsuarioMatricula].Usuario;
            return View(usuario);
        }

        // GET: Principal/Avaliacao
        public ActionResult Avaliacao()
        {
            return RedirectToAction("Index");
        }

        // GET: Principal/Pendente
        [Filters.AutenticacaoFilter(Categorias = new[] { 2 })]
        public ActionResult Pendente()
        {
            string matricula = Sessao.UsuarioMatricula;
            int codProfessor = Professor.ListarPorMatricula(matricula).CodProfessor;

            IEnumerable<Avaliacao> avaliacoes = AvalAcademica.ListarCorrecaoPendentePorProfessor(codProfessor)
                .Select(a => a.Avaliacao);
            avaliacoes = avaliacoes.Union(AvalCertificacao.ListarCorrecaoPendentePorProfessor(codProfessor)
                .Select(a => a.Avaliacao));
            avaliacoes = avaliacoes.Union(AvalAcadReposicao.ListarCorrecaoPendentePorProfessor(codProfessor)
                .Select(a => a.Avaliacao));

            return View(avaliacoes.OrderBy(a => a.DtAplicacao).ToList());
        }
    }
}