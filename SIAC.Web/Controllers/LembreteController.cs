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
        public ActionResult Index() => null;

        [HttpPost]
        public ActionResult Dashboard()
        {
            string matricula = Sessao.UsuarioMatricula;
            var Contadores = new Dictionary<string, int>();
            Contadores.Add("autoavaliacao", AvalAuto.ListarNaoRealizadaPorPessoa(Sistema.UsuarioAtivo[matricula].Usuario.CodPessoaFisica).Count);

            if (Sessao.UsuarioCategoriaCodigo == 2)
            {
                int codProfessor = Professor.ListarPorMatricula(matricula).CodProfessor;
                var lst = AvalAcademica.ListarCorrecaoPendentePorProfessor(codProfessor).Select(a => a.Avaliacao);
                lst = lst.Union(AvalCertificacao.ListarCorrecaoPendentePorProfessor(codProfessor).Select(a => a.Avaliacao));
                lst = lst.Union(AvalAcadReposicao.ListarCorrecaoPendentePorProfessor(codProfessor).Select(a => a.Avaliacao));
                Contadores.Add("correcao", lst.Count());
            }

            return Json(Contadores);
        }
    }
}