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
        // GET: principal
        [OutputCache(CacheProfile = "PorUsuario")]
        public ActionResult Index()
        {
            Lembrete.AdicionarNotificacao("Este é sua tela principal.", Lembrete.INFO);
            Usuario usuario = Sistema.UsuarioAtivo[Sessao.UsuarioMatricula].Usuario;
            return View(usuario);
        }

        // GET: principal/avaliacao
        public ActionResult Avaliacao() => RedirectToAction("Index");

        // GET: principal/pendente
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.PROFESSOR })]
        public ActionResult Pendente()
        {
            int codProfessor = Professor.ListarPorMatricula(Sessao.UsuarioMatricula).CodProfessor;

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