using SIAC.Helpers;
using SIAC.Models;
using SIAC.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIAC.Controllers
{
    [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.COLABORADOR })]
    public class SimuladoController : Controller
    {
        //public ActionResult Index() => View();

        private dbSIACEntities contexto => Repositorio.GetInstance();

        public ActionResult Novo() => View();

        [HttpPost]
        public ActionResult Novo(FormCollection form)
        {
            if (!StringExt.IsNullOrWhiteSpace(form["txtTitulo"]))
            {
                Simulado sim = new Simulado();
                DateTime hoje = DateTime.Now;
                /* Chave */
                sim.Ano = hoje.Year;
                sim.NumIdentificador = Simulado.ObterNumIdentificador();
                sim.DtCadastro = hoje;

                /* Simulado */
                sim.Titulo = form["txtTitulo"];
                sim.Descricao = form["txtDescricao"];
                sim.FlagInscricaoEncerrado = true;

                /* Colaborador */
                sim.Colaborador = Colaborador.ListarPorMatricula(Sessao.UsuarioMatricula);

                Simulado.Inserir(sim);
                Lembrete.AdicionarNotificacao($"Simulado cadastrado com sucesso.", Lembrete.POSITIVO);
                return RedirectToAction("Provas", new { codigo = sim.Codigo });
            }
            return RedirectToAction("Novo");
        }

        public ActionResult Provas(string codigo)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                Simulado sim = Simulado.ListarPorCodigo(codigo);

                if (sim != null && sim.Colaborador.MatrColaborador == Sessao.UsuarioMatricula)
                {
                    return View(sim);
                }
            }

            return RedirectToAction("", "Gerencia");
        }

        public ActionResult Datas(string codigo)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                Simulado sim = Simulado.ListarPorCodigo(codigo);

                if (sim != null && sim.Colaborador.MatrColaborador == Sessao.UsuarioMatricula)
                {
                    return View(sim);
                }
            }

            return RedirectToAction("", "Gerencia");
        }

        [HttpPost]
        public ActionResult Datas(string codigo, FormCollection form)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                Simulado sim = Simulado.ListarPorCodigo(codigo);

                if (sim != null && sim.Colaborador.MatrColaborador == Sessao.UsuarioMatricula)
                {
                    string inicioInscricao = form["txtInicioInscricao"];
                    string terminoInscricao = form["txtTerminoInscricao"];
                    string qteVagas = form["txtQteVagas"];
                    if (!StringExt.IsNullOrWhiteSpace(inicioInscricao, terminoInscricao, qteVagas))
                    {
                        /* Simulado */
                        sim.FlagInscricaoEncerrado = false;
                        sim.QteVagas = int.Parse(qteVagas);
                        sim.DtInicioInscricao = DateTime.Parse(inicioInscricao, new CultureInfo("pt-BR"));
                        sim.DtTerminoInscricao = DateTime.Parse(terminoInscricao, new CultureInfo("pt-BR"));

                        Repositorio.GetInstance().SaveChanges();

                        return RedirectToAction("Salas", new { codigo = sim.Codigo });
                    }
                }
            }

            return RedirectToAction("", "Gerencia");
        }

        public ActionResult Salas(string codigo)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                Simulado sim = Simulado.ListarPorCodigo(codigo);

                if (sim != null && sim.Colaborador.MatrColaborador == Sessao.UsuarioMatricula)
                {
                    SimuladoSalasViewModel model = new SimuladoSalasViewModel();
                    model.Simulado = sim;
                    model.Campi = Campus.ListarOrdenadamente();
                    model.Blocos = Bloco.ListarOrdenadamente();
                    model.Salas = Sala.ListarOrdenadamente();

                    return View(model);
                }
            }

            return RedirectToAction("", "Gerencia");
        }

        [HttpPost]
        public ActionResult Salas(string codigo, FormCollection form)
        {
            string ddlCampus = form["ddlCampus"];
            string ddlBloco = form["ddlBloco"];
            string ddlSala = form["ddlSala"];

            if (!StringExt.IsNullOrWhiteSpace(codigo, ddlCampus, ddlBloco, ddlSala))
            {
                Simulado sim = Simulado.ListarPorCodigo(codigo);

                if (sim != null && sim.Colaborador.MatrColaborador == Sessao.UsuarioMatricula)
                {
                    int codSala = int.Parse(ddlSala);

                    SimSala simSala = contexto.SimSala.FirstOrDefault(s => s.Ano == sim.Ano
                                                                      && s.NumIdentificador == sim.NumIdentificador
                                                                      && s.CodSala == codSala);

                    if (simSala == null)
                    {
                        sim.SimSala.Add(new SimSala()
                        {
                            Sala = Sala.ListarPorCodigo(int.Parse(ddlSala))
                        });

                        //simSala = new SimSala();
                        //simSala.Simulado = sim;
                        //simSala.Sala = Sala.ListarPorCodigo(int.Parse(ddlSala));

                        //contexto.SimSala.Add(simSala);
                        Repositorio.Commit();
                    }

                    return RedirectToAction("Salas", new { codigo = codigo });
                }
            }

            return RedirectToAction("Salas", new { codigo = codigo });
        }

        [HttpPost]
        public ActionResult RemoverSala(string codigo, int codSala)
        {
            if (!StringExt.IsNullOrWhiteSpace(codigo, codSala.ToString()))
            {
                Simulado sim = Simulado.ListarPorCodigo(codigo);

                if (sim != null && sim.Colaborador.MatrColaborador == Sessao.UsuarioMatricula)
                {
                    SimSala simSala = contexto.SimSala
                        .FirstOrDefault(s => s.Ano == sim.Ano && s.NumIdentificador == sim.NumIdentificador && s.CodSala == codSala);

                    //contexto.SimSala.Remove(simSala);

                    sim.SimSala.Remove(simSala);

                    //contexto.SaveChanges();
                    Repositorio.Commit();

                    return RedirectToAction("Salas", new { codigo = codigo });
                }
            }

            return RedirectToAction("Salas", new { codigo = codigo });
        }
    }
}