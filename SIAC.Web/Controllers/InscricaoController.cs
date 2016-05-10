using SIAC.Filters;
using SIAC.Helpers;
using SIAC.Models;
using SIAC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIAC.Controllers
{
    public class InscricaoController : Controller
    {
        private Simulado ListarSimuladoAbertoPorCodigo(string codigo) => 
            Simulado.ListarPorInscricoesAbertas().FirstOrDefault(sim => sim.Codigo.ToLower() == codigo.ToLower());

        // GET: simulado/inscricao
        public ActionResult Index() => View(new InscricaoIndexViewModel()
        {
            Simulados = Simulado.ListarPorInscricoesAbertas()
        });

        // POST: simulado/inscricao/detalhe
        [HttpPost]
        public ActionResult Detalhe(string codigo)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                Simulado s = Simulado.ListarPorCodigo(codigo);
                if (s != null)
                {
                    return PartialView("_SimuladoDetalhe", s);
                }
            }
            return Json(string.Empty);
        }

        // GET: simulado/inscricao/confirmar/simul201600122
        [CandidatoFilter]
        public ActionResult Confirmar(string codigo)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                Simulado s = ListarSimuladoAbertoPorCodigo(codigo);
                if (s != null && s.FlagTemVaga && !s.CandidatoInscrito(Sessao.Candidato.CodCandidato))
                {
                    if (Sessao.Candidato.PerfilCompleto)
                    {
                        return View(s);
                    }
                    else
                    {
                        return RedirectToAction("Perfil", "Candidato");
                    }
                }
            }
            return RedirectToAction("Index");
        }

        // POST: simulado/inscricao/confirmado/simul201600123
        [CandidatoFilter]
        [HttpPost]
        public ActionResult Confirmado(string codigo)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                Simulado s = ListarSimuladoAbertoPorCodigo(codigo);
                if (s != null && s.FlagTemVaga && !s.CandidatoInscrito(Sessao.Candidato.CodCandidato))
                {
                    if (Sessao.Candidato.PerfilCompleto)
                    {
                        s.SimCandidato.Add(new SimCandidato()
                        {
                            NumInscricao = s.ObterNumInscricao(),
                            Candidato = Sessao.Candidato,
                            DtInscricao = DateTime.Now
                        });
                        Repositorio.Commit();
                        return RedirectToAction("Inscricoes", "Candidato", new { codigo = s.Codigo });
                    }
                    else
                    {
                        return RedirectToAction("Perfil", "Candidato");
                    }
                }
            }
            return RedirectToAction("Index");
        }
    }
}