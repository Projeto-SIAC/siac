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
            Simulado.ListarParaInscricoes().FirstOrDefault(sim => sim.Codigo.ToLower() == codigo.ToLower());

        // GET: simulado/inscricao
        public ActionResult Index() => View(new InscricaoIndexViewModel()
        {
            Simulados = Simulado.ListarParaInscricoes()
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
                        string numeroMascara = new HashidsNet.Hashids(s.Codigo, 6)
                            .Encode(s.Ano, s.NumIdentificador, Sessao.Candidato.CodCandidato);

                        var candidato = new SimCandidato()
                        {
                            NumInscricao = s.ObterNumInscricao(),
                            Candidato = Sessao.Candidato,
                            DtInscricao = DateTime.Now,
                            NumeroMascara = numeroMascara
                        };

                        s.SimCandidato.Add(candidato);

                        foreach (var prova in s.Provas)
                        {
                            prova.SimCandidatoProva.Add(new SimCandidatoProva()
                            {
                                SimCandidato = candidato,
                                SimProva = prova
                            });
                        }

                        Repositorio.Commit();

                        string url = Request.Url.ToString();
                        string simuladoUrl = url.Remove(url.IndexOf("/", url.IndexOf("//") + 2)) + Url.Action("Inscricoes", "Candidato", new { codigo = s.Codigo });

                        EnviarEmail.Inscricao(Sessao.Candidato.Email, Sessao.Candidato.Nome, simuladoUrl, s.Titulo);

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

        // POST: simulado/inscricao/confirmado/simul201600123
        [CandidatoFilter]
        [HttpPost]
        public ActionResult Cancelar(string codigo, string simuladoCancelar)
        {
            if (!StringExt.IsNullOrWhiteSpace(codigo, simuladoCancelar))
            {
                Simulado s = Simulado.ListarPorCodigo(codigo);
                if (s != null && s.CandidatoInscrito(Sessao.Candidato.CodCandidato))
                {
                    if (codigo.ToLower() == simuladoCancelar.ToLower())
                    {
                        SimCandidato simCandidato = s.SimCandidato.First(sc => sc.CodCandidato == Sessao.Candidato.CodCandidato);
                        Repositorio.GetInstance().SimCandidatoProva.RemoveRange(simCandidato.SimCandidatoProva);
                        s.SimCandidato.Remove(simCandidato);
                        Repositorio.Commit();
                    }
                }
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Inscricoes", "Candidato", new { codigo });
            }
        }
    }
}