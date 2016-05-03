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
                    return View(new SimuladoProvaViewModel()
                    {
                        Simulado = sim,
                        Disciplinas = Disciplina.ListarOrdenadamente()
                    });
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
                        sim.DtTerminoInscricao = DateTime.Parse($"{terminoInscricao} 23:59:59", new CultureInfo("pt-BR"));

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

                    sim.SimSala.Remove(simSala);
                    Repositorio.Commit();

                    return RedirectToAction("Salas", new { codigo = codigo });
                }
            }

            return RedirectToAction("Salas", new { codigo = codigo });
        }

        [HttpPost]
        public ActionResult NovoDia(string codigo, FormCollection form)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                Simulado sim = Simulado.ListarPorCodigo(codigo);

                if (sim != null && sim.Colaborador.MatrColaborador == Sessao.UsuarioMatricula)
                {
                    string strDataRealizacao = form["txtDataRealizacao"];
                    string strHorarioInicio = form["txtHorarioInicio"];
                    string strHorarioTermino = form["txtHorarioTermino"];

                    if (!StringExt.IsNullOrWhiteSpace(strDataRealizacao, strHorarioInicio, strHorarioTermino))
                    {
                        CultureInfo cultureBr = new CultureInfo("pt-BR");
                        /* Simulado */
                        DateTime dataRealizacao = DateTime.Parse($"{strDataRealizacao} {strHorarioInicio}", cultureBr);
                        TimeSpan inicio = TimeSpan.Parse(strHorarioInicio, cultureBr);
                        TimeSpan termino = TimeSpan.Parse(strHorarioTermino, cultureBr);

                        SimDiaRealizacao diaRealizacao = sim.SimDiaRealizacao.FirstOrDefault(s => s.DtRealizacao.Date == dataRealizacao.Date);

                        if (diaRealizacao != null && inicio >= diaRealizacao.DtRealizacao.TimeOfDay && inicio <= diaRealizacao.DtRealizacao.AddMinutes(diaRealizacao.Duracao).TimeOfDay)
                        {
                            Lembrete.AdicionarNotificacao($"Já existe um data marcada com a realização nesse periodo {dataRealizacao.ToShortDateString()}: {inicio} - {termino} ", Lembrete.NEGATIVO, 10);
                        }
                        else
                        {
                            int codDiaRealizacao = sim.SimDiaRealizacao.Count > 0 ? sim.SimDiaRealizacao.Max(s => s.CodDiaRealizacao) + 1 : 1;

                            diaRealizacao = new SimDiaRealizacao();
                            diaRealizacao.CodDiaRealizacao = codDiaRealizacao;
                            diaRealizacao.DtRealizacao = dataRealizacao;
                            diaRealizacao.CodTurno = "V";
                            diaRealizacao.Duracao = int.Parse((termino - dataRealizacao.TimeOfDay).TotalMinutes.ToString());

                            sim.SimDiaRealizacao.Add(diaRealizacao);
                            Repositorio.GetInstance().SaveChanges();
                        }
                    }
                }
            }

            return RedirectToAction("Provas", new { codigo = codigo });
        }

        [HttpPost]
        public ActionResult RemoverDia(string codigo, int codDia)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                Simulado sim = Simulado.ListarPorCodigo(codigo);

                if (sim != null && sim.Colaborador.MatrColaborador == Sessao.UsuarioMatricula)
                {
                    SimDiaRealizacao diaRealizacao = sim.SimDiaRealizacao.FirstOrDefault(s => s.CodDiaRealizacao == codDia);

                    sim.SimDiaRealizacao.Remove(diaRealizacao);
                    Repositorio.GetInstance().SaveChanges();
                }
            }

            return RedirectToAction("Provas", new { codigo = codigo });
        }

        [HttpPost]
        public ActionResult CarregarProvas(string codigo, int codDia)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                Simulado sim = Simulado.ListarPorCodigo(codigo);

                if (sim != null && sim.Colaborador.MatrColaborador == Sessao.UsuarioMatricula)
                {
                    SimDiaRealizacao diaRealizacao = sim.SimDiaRealizacao.FirstOrDefault(s => s.CodDiaRealizacao == codDia);

                    return PartialView("_DiaProvas", diaRealizacao);
                }
            }

            return RedirectToAction("Provas", new { codigo = codigo });
        }

        [HttpPost]
        public ActionResult CarregarProva(string codigo, int codDia, int codProva)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                Simulado sim = Simulado.ListarPorCodigo(codigo);

                if (sim != null && sim.Colaborador.MatrColaborador == Sessao.UsuarioMatricula)
                {
                    SimDiaRealizacao diaRealizacao = sim.SimDiaRealizacao.FirstOrDefault(s => s.CodDiaRealizacao == codDia);

                    if (diaRealizacao != null)
                    {
                        SimProva prova = diaRealizacao.SimProva.FirstOrDefault(p => p.CodProva == codProva);

                        if (prova == null) prova = new SimProva();

                        var model = new SimuladoProvaViewModel()
                        {
                            Simulado = sim,
                            Prova = prova,
                            Disciplinas = Disciplina.ListarOrdenadamente()
                        };

                        return PartialView("_SimuladoProvaCarregar", model);
                    }
                }
            }
            return RedirectToAction("Provas", new { codigo = codigo });
        }

        [HttpPost]
        public ActionResult NovaProva(string codigo, int codDia, FormCollection form)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                Simulado sim = Simulado.ListarPorCodigo(codigo);

                if (sim != null && sim.Colaborador.MatrColaborador == Sessao.UsuarioMatricula)
                {
                    string ddlDisciplina = form["ddlDisciplina"];
                    string txtQteQuestoes = form["txtQteQuestoes"];
                    string txtTitulo = form["txtTitulo"];
                    string txtDescricao = form["txtDescricao"];

                    if (!StringExt.IsNullOrWhiteSpace(ddlDisciplina, txtQteQuestoes, txtTitulo))
                    {
                        SimDiaRealizacao diaRealizacao = sim.SimDiaRealizacao.FirstOrDefault(s => s.CodDiaRealizacao == codDia);

                        SimProva prova = new SimProva();

                        prova.CodProva = diaRealizacao.SimProva.Count > 0 ? diaRealizacao.SimProva.Max(p => p.CodProva) + 1 : 1;
                        prova.Titulo = txtTitulo;
                        prova.Descricao = String.IsNullOrWhiteSpace(txtDescricao) ? String.Empty : txtDescricao;
                        prova.QteQuestoes = int.Parse(txtQteQuestoes);
                        prova.CodDisciplina = int.Parse(ddlDisciplina);

                        List<Questao> questoes = Simulado.ObterQuestoes(prova.CodDisciplina, prova.QteQuestoes);

                        prova.SimProvaQuestao.Clear();

                        foreach (Questao questao in questoes)
                        {
                            prova.SimProvaQuestao.Add(new SimProvaQuestao()
                            {
                                Questao = questao
                            });
                        }

                        if (questoes.Count >= prova.QteQuestoes)
                        {
                            diaRealizacao.SimProva.Add(prova);
                            Repositorio.Commit();

                            Lembrete.AdicionarNotificacao($"Prova cadastrada com sucesso neste simulado!", Lembrete.POSITIVO);
                        }
                        else
                        {
                            Lembrete.AdicionarNotificacao($"Foi gerada uma quantidade menor de questões para a prova deste simulado!", Lembrete.NEGATIVO);
                        }
                    }
                }
            }

            return RedirectToAction("Provas", new { codigo = codigo });
        }

        [HttpPost]
        public ActionResult EditarProva(string codigo, int codDia, int codProva, FormCollection form)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                Simulado sim = Simulado.ListarPorCodigo(codigo);

                if (sim != null && sim.Colaborador.MatrColaborador == Sessao.UsuarioMatricula)
                {
                    string ddlDisciplina = form["ddlDisciplina"];
                    string txtQteQuestoes = form["txtQteQuestoes"];
                    string txtTitulo = form["txtTitulo"];
                    string txtDescricao = form["txtDescricao"];

                    if (!StringExt.IsNullOrWhiteSpace(ddlDisciplina, txtQteQuestoes, txtTitulo))
                    {
                        SimDiaRealizacao diaRealizacao = sim.SimDiaRealizacao.FirstOrDefault(s => s.CodDiaRealizacao == codDia);

                        SimProva prova = diaRealizacao.SimProva.FirstOrDefault(p => p.CodProva == codProva);

                        //prova.CodProva = diaRealizacao.SimProva.Count > 0 ? diaRealizacao.SimProva.Max(p => p.CodProva) + 1 : 1;
                        prova.Titulo = txtTitulo;
                        prova.Descricao = String.IsNullOrWhiteSpace(txtDescricao) ? String.Empty : txtDescricao;
                        prova.QteQuestoes = int.Parse(txtQteQuestoes);
                        prova.CodDisciplina = int.Parse(ddlDisciplina);

                        List<Questao> questoes = Simulado.ObterQuestoes(prova.CodDisciplina, prova.QteQuestoes);

                        prova.SimProvaQuestao.Clear();

                        foreach (Questao questao in questoes)
                        {
                            prova.SimProvaQuestao.Add(new SimProvaQuestao()
                            {
                                Questao = questao
                            });
                        }

                        if (questoes.Count >= prova.QteQuestoes)
                        {
                            diaRealizacao.SimProva.Add(prova);
                            Repositorio.Commit();

                            Lembrete.AdicionarNotificacao($"Prova editada com sucesso neste simulado!", Lembrete.POSITIVO);
                        }
                        else
                        {
                            Lembrete.AdicionarNotificacao($"Foi gerada uma quantidade menor de questões para a prova deste simulado!", Lembrete.NEGATIVO);
                        }
                    }
                }
            }

            return RedirectToAction("Provas", new { codigo = codigo });
        }

        [HttpPost]
        public ActionResult RemoverProva(string codigo, int codDia, int codProva)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                Simulado sim = Simulado.ListarPorCodigo(codigo);

                if (sim != null && sim.Colaborador.MatrColaborador == Sessao.UsuarioMatricula)
                {
                    SimDiaRealizacao diaRealizacao = sim.SimDiaRealizacao.FirstOrDefault(s => s.CodDiaRealizacao == codDia);

                    if (diaRealizacao!= null)
                    {
                        SimProva prova = diaRealizacao.SimProva.FirstOrDefault(p => p.CodProva == codProva);
                        prova.SimProvaQuestao.Clear();
                        diaRealizacao.SimProva.Remove(prova);
                        Repositorio.Commit();
                    }
                }
            }
            return RedirectToAction("Provas", new { codigo = codigo });
        }

    }
}