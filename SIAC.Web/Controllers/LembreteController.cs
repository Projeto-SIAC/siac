using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using SIAC.Models;
using SIAC.Helpers;

namespace SIAC.Controllers
{
    [Filters.AutenticacaoFilter]
    public class LembreteController : Controller
    {
        // GET: Lembrete
        public ActionResult Index() => RedirectToAction("Index", "Acesso");

        [HttpPost]
        [OutputCache(Duration = 120)]
        public ActionResult Dashboard()
        {
            string matricula = Sessao.UsuarioMatricula;
            var Atalho = new Dictionary<string, int>();
            Atalho.Add("autoavaliacao", AvalAuto.ListarNaoRealizadaPorPessoa(Sistema.UsuarioAtivo[matricula].Usuario.CodPessoaFisica).Count);

            if (Sessao.UsuarioCategoriaCodigo == 2)
            {
                int codProfessor = Professor.ListarPorMatricula(matricula).CodProfessor;
                var lst = AvalAcademica.ListarCorrecaoPendentePorProfessor(codProfessor).Select(a => a.Avaliacao);
                lst = lst.Union(AvalCertificacao.ListarCorrecaoPendentePorProfessor(codProfessor).Select(a => a.Avaliacao));
                lst = lst.Union(AvalAcadReposicao.ListarCorrecaoPendentePorProfessor(codProfessor).Select(a => a.Avaliacao));
                Atalho.Add("correcao", lst.Count());
            }

            return Json(Atalho);
        }

        [HttpPost]
        [OutputCache(Duration = 120)]
        public ActionResult Institucional()
        {
            string matricula = Sessao.UsuarioMatricula;
            var Atalho = new Dictionary<string, int>();
            Atalho.Add("andamento", AvalAvi.ListarPorUsuario(Sessao.UsuarioMatricula).Count);

            return Json(Atalho);
        }

        [HttpPost]
        [OutputCache(Duration = 600)]
        public ActionResult Menu()
        {
            string matricula = Sessao.UsuarioMatricula;
            var Menu = new Dictionary<string, int>();
            Menu.Add("avi", AvalAvi.ListarPorUsuario(Sessao.UsuarioMatricula).Count);

            return Json(Menu);
        }
    }
}