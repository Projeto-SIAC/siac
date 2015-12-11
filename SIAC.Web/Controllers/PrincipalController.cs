using SIAC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIAC.Controllers
{
    [Filters.AutenticacaoFilter]
    public class PrincipalController : Controller
    {
        // GET: Principal
        public ActionResult Index()
        {
            Lembrete.AdicionarNotificacao("Este é sua tela principal.", Lembrete.Info);
            Usuario usuario = Usuario.ListarPorMatricula(Helpers.Sessao.UsuarioMatricula);
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
            string strMatr = Helpers.Sessao.UsuarioMatricula;
            int codProfessor = Professor.ListarPorMatricula(strMatr).CodProfessor;
            var lst = AvalAcademica.ListarCorrecaoPendentePorProfessor(codProfessor).Select(a=>a.Avaliacao);
            lst = lst.Union(AvalCertificacao.ListarCorrecaoPendentePorProfessor(codProfessor).Select(a => a.Avaliacao));
            lst = lst.Union(AvalAcadReposicao.ListarCorrecaoPendentePorProfessor(codProfessor).Select(a => a.Avaliacao));

            return View(lst.OrderBy(a=>a.DtAplicacao).ToList());
        }
    }
}