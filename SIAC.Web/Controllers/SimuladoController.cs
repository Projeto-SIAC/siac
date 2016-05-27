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
            string titulo = form["txtTitulo"];
            string descricao = form["txtDescricao"];
            string strQteVagas = form["txtQteVagas"];

            if (!StringExt.IsNullOrWhiteSpace(titulo, strQteVagas))
            {
                int qteVagas = int.Parse(strQteVagas);

                Simulado sim = new Simulado();
                DateTime hoje = DateTime.Now;
                /* Chave */
                sim.Ano = hoje.Year;
                sim.NumIdentificador = Simulado.ObterNumIdentificador();
                sim.DtCadastro = hoje;

                /* Simulado */
                sim.Titulo = titulo;
                sim.Descricao = descricao;
                sim.QteVagas = qteVagas;
                sim.FlagInscricaoEncerrado = true;

                /* Colaborador */
                sim.Colaborador = Colaborador.ListarPorMatricula(Sessao.UsuarioMatricula);

                Simulado.Inserir(sim);
                Lembrete.AdicionarNotificacao("Simulado cadastrado com sucesso.", Lembrete.POSITIVO);
                return RedirectToAction("Datas", new { codigo = sim.Codigo });
            }
            Lembrete.AdicionarNotificacao("Ocorreu um erro na operação.", Lembrete.NEGATIVO);
            return RedirectToAction("Novo");
        }

        [HttpPost]
        public ActionResult Editar(string codigo, FormCollection form)
        {
            string mensagem = "Ocorreu um erro na operação.";
            string estilo = Lembrete.NEGATIVO;

            if (!String.IsNullOrWhiteSpace(codigo))
            {
                Simulado sim = Simulado.ListarPorCodigo(codigo);

                if (sim != null && sim.Colaborador.MatrColaborador == Sessao.UsuarioMatricula && !sim.FlagSimuladoEncerrado)
                {
                    string titulo = form["txtTitulo"];
                    string descricao = form["txtDescricao"];
                    string strQteVagas = form["txtQteVagas"];

                    if (!StringExt.IsNullOrWhiteSpace(titulo, strQteVagas))
                    {
                        int qteVagas = int.Parse(strQteVagas);

                        sim.Titulo = titulo;
                        sim.Descricao = descricao;

                        if (qteVagas < sim.SimCandidato.Count)
                        {
                            mensagem = "A quantidade de vagas fornecida é inferior à quantidade de candidatos já inscritos.";
                            estilo = Lembrete.NEGATIVO;
                        }
                        else
                        {
                            sim.QteVagas = qteVagas;
                            Repositorio.Commit();

                            mensagem = "Simulado editado com sucesso.";
                            estilo = Lembrete.POSITIVO;
                        }
                    }
                    else
                    {
                        mensagem = "O campo de Título e Quantidade de vagas são obrigatórios.";
                        estilo = Lembrete.NEGATIVO;
                    }
                }
            }
            Lembrete.AdicionarNotificacao(mensagem, estilo);
            return RedirectToAction("Detalhe", new { codigo = codigo });
        }

        public ActionResult Provas(string codigo)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                Simulado sim = Simulado.ListarPorCodigo(codigo);

                if (sim != null && sim.Colaborador.MatrColaborador == Sessao.UsuarioMatricula && !sim.FlagSimuladoEncerrado)
                {
                    return View(new SimuladoProvaViewModel()
                    {
                        Simulado = sim,
                        Disciplinas = Disciplina.ListarOrdenadamente()
                    });
                }
            }

            return RedirectToAction("", "Arquivo");
        }

        public ActionResult Datas(string codigo)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                Simulado sim = Simulado.ListarPorCodigo(codigo);

                if (sim != null && sim.Colaborador.MatrColaborador == Sessao.UsuarioMatricula && !sim.FlagSimuladoEncerrado)
                {
                    if (sim.DtInicioInscricao <= DateTime.Now)
                    {
                        Lembrete.AdicionarNotificacao("Não é possível gerenciar a data após o início do período de inscrições", Lembrete.NEGATIVO);
                        return RedirectToAction("Detalhe", new { codigo = codigo });
                    }

                    return View(sim);
                }
            }

            return RedirectToAction("", "Arquivo");
        }

        [HttpPost]
        public ActionResult Datas(string codigo, FormCollection form)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                Simulado sim = Simulado.ListarPorCodigo(codigo);

                if (sim != null && sim.Colaborador.MatrColaborador == Sessao.UsuarioMatricula && !sim.FlagSimuladoEncerrado)
                {
                    string inicioInscricao = form["txtInicioInscricao"];
                    string terminoInscricao = form["txtTerminoInscricao"];

                    if (!StringExt.IsNullOrWhiteSpace(inicioInscricao, terminoInscricao))
                    {
                        DateTime inicio = DateTime.Parse(inicioInscricao, new CultureInfo("pt-BR"));
                        DateTime termino = DateTime.Parse($"{terminoInscricao} 23:59:59", new CultureInfo("pt-BR"));

                        if (sim.DtInicioInscricao <= DateTime.Now)
                        {
                            Lembrete.AdicionarNotificacao("As incrições já foram iniciadas, você não pode mais alterar o início.", Lembrete.NEGATIVO);
                            return View(sim);
                        }

                        if (sim.DtTerminoInscricao >= sim.SimDiaRealizacao?.FirstOrDefault()?.DtRealizacao)
                        {
                            Lembrete.AdicionarNotificacao("A data de término das inscrições tem que ser antes do primeiro dia de prova.", Lembrete.NEGATIVO);
                            return View(sim);
                        }

                        /* Simulado */
                        sim.DtInicioInscricao = inicio;
                        sim.DtTerminoInscricao = termino;

                        Repositorio.GetInstance().SaveChanges();

                        Lembrete.AdicionarNotificacao("As datas foram atualizadas.", Lembrete.POSITIVO);

                        return RedirectToAction("Salas", new { codigo = sim.Codigo });
                    }
                }
            }

            return RedirectToAction("", "Arquivo");
        }

        public ActionResult Salas(string codigo)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                Simulado sim = Simulado.ListarPorCodigo(codigo);

                if (sim != null && sim.Colaborador.MatrColaborador == Sessao.UsuarioMatricula && !sim.FlagSimuladoEncerrado)
                {
                    SimuladoSalasViewModel model = new SimuladoSalasViewModel();
                    model.Simulado = sim;
                    model.Campi = Campus.ListarOrdenadamente();
                    model.Blocos = Bloco.ListarOrdenadamente();
                    model.Salas = Sala.ListarOrdenadamente();

                    return View(model);
                }
            }

            return RedirectToAction("", "Arquivo");
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

                if (sim != null && sim.Colaborador.MatrColaborador == Sessao.UsuarioMatricula && !sim.FlagSimuladoEncerrado)
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
                    Lembrete.AdicionarNotificacao("A sala foi adicionada com sucesso.", Lembrete.POSITIVO);
                    return RedirectToAction("Salas", new { codigo = codigo });
                }
            }
            Lembrete.AdicionarNotificacao("Ocorreu um erro na operação.", Lembrete.NEGATIVO);
            return RedirectToAction("Salas", new { codigo = codigo });
        }

        [HttpPost]
        public ActionResult RemoverSala(string codigo, int codSala)
        {
            if (!StringExt.IsNullOrWhiteSpace(codigo, codSala.ToString()))
            {
                Simulado sim = Simulado.ListarPorCodigo(codigo);

                if (sim != null && sim.Colaborador.MatrColaborador == Sessao.UsuarioMatricula && !sim.FlagSimuladoEncerrado)
                {
                    SimSala simSala = contexto.SimSala
                        .FirstOrDefault(s => s.Ano == sim.Ano && s.NumIdentificador == sim.NumIdentificador && s.CodSala == codSala);

                    sim.SimSala.Remove(simSala);
                    Repositorio.Commit();

                    Lembrete.AdicionarNotificacao("A sala foi removida com sucesso.", Lembrete.POSITIVO);
                    return RedirectToAction("Salas", new { codigo = codigo });
                }
            }

            Lembrete.AdicionarNotificacao("Ocorreu um erro na operação.", Lembrete.NEGATIVO);
            return RedirectToAction("Salas", new { codigo = codigo });
        }

        [HttpPost]
        public ActionResult CarregarDia(string codigo, int codDia)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                Simulado sim = Simulado.ListarPorCodigo(codigo);

                if (sim != null && sim.Colaborador.MatrColaborador == Sessao.UsuarioMatricula && !sim.FlagSimuladoEncerrado)
                {
                    SimDiaRealizacao diaRealizacao = sim.SimDiaRealizacao.FirstOrDefault(s => s.CodDiaRealizacao == codDia);

                    if (diaRealizacao == null) diaRealizacao = new SimDiaRealizacao();

                    return PartialView("_SimuladoDiaCarregar", diaRealizacao);
                }
            }

            return RedirectToAction("Provas", new { codigo = codigo });
        }

        [HttpPost]
        public ActionResult NovoDia(string codigo, FormCollection form)
        {
            string mensagem = "Ocorreu um erro na operação.";
            string estilo = Lembrete.NEGATIVO;

            if (!String.IsNullOrWhiteSpace(codigo))
            {
                Simulado sim = Simulado.ListarPorCodigo(codigo);

                if (sim != null && sim.Colaborador.MatrColaborador == Sessao.UsuarioMatricula && !sim.FlagSimuladoEncerrado)
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

                        if (dataRealizacao <= sim.DtTerminoInscricao)
                        {
                            mensagem = "A data da prova tem que ser superior à data de termino de inscrição do simulado.";
                            estilo = Lembrete.NEGATIVO;
                        }
                        else if (diaRealizacao != null && inicio >= diaRealizacao.DtRealizacao.TimeOfDay && inicio <= diaRealizacao.DtRealizacao.AddMinutes(diaRealizacao.Duracao).TimeOfDay)
                        {
                            mensagem = $"Já existe uma data marcada com a realização nesse período: {dataRealizacao.ToShortDateString()} ({inicio.ToString("HH:mm")} até {termino.ToString("HH:mm")}).";
                            estilo = Lembrete.NEGATIVO;
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

                            mensagem = "O dia foi adicionado com sucesso.";
                            estilo = Lembrete.POSITIVO;
                        }
                    }
                }
            }
            Lembrete.AdicionarNotificacao(mensagem, estilo);
            return RedirectToAction("Provas", new { codigo = codigo });
        }

        [HttpPost]
        public ActionResult EditarDia(string codigo, int codDia, FormCollection form)
        {
            string mensagem = "Ocorreu um erro na operação.";
            string estilo = Lembrete.NEGATIVO;

            if (!String.IsNullOrWhiteSpace(codigo))
            {
                Simulado sim = Simulado.ListarPorCodigo(codigo);

                if (sim != null && sim.Colaborador.MatrColaborador == Sessao.UsuarioMatricula && !sim.FlagSimuladoEncerrado)
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

                        SimDiaRealizacao diaRealizacao = sim.SimDiaRealizacao.FirstOrDefault(s => s.DtRealizacao.Date == dataRealizacao.Date && s.CodDiaRealizacao != codDia);

                        if (dataRealizacao >= sim.DtTerminoInscricao)
                        {
                            mensagem = "A data da prova tem que ser superior à data de termino de inscrição do simulado.";
                            estilo = Lembrete.NEGATIVO;
                        }
                        else if (diaRealizacao != null && inicio >= diaRealizacao.DtRealizacao.TimeOfDay && inicio <= diaRealizacao.DtRealizacao.AddMinutes(diaRealizacao.Duracao).TimeOfDay)
                        {
                            mensagem = $"Já existe uma data marcada com a realização nesse período: {dataRealizacao.ToShortDateString()} ({inicio.ToString("HH:mm")} até {termino.ToString("HH:mm")}).";
                            estilo = Lembrete.NEGATIVO;
                        }
                        else
                        {
                            int codDiaRealizacao = sim.SimDiaRealizacao.Count > 0 ? sim.SimDiaRealizacao.Max(s => s.CodDiaRealizacao) + 1 : 1;

                            diaRealizacao = sim.SimDiaRealizacao.FirstOrDefault(d => d.CodDiaRealizacao == codDia);
                            diaRealizacao.DtRealizacao = dataRealizacao;
                            diaRealizacao.CodTurno = "V";
                            diaRealizacao.Duracao = int.Parse((termino - dataRealizacao.TimeOfDay).TotalMinutes.ToString());

                            Repositorio.Commit();

                            mensagem = "O dia foi editado com sucesso.";
                            estilo = Lembrete.POSITIVO;
                        }
                    }
                }
            }
            Lembrete.AdicionarNotificacao(mensagem, estilo);
            return RedirectToAction("Provas", new { codigo = codigo });
        }

        [HttpPost]
        public ActionResult RemoverDia(string codigo, int codDia)
        {
            string mensagem = "Ocorreu um erro na operação.";
            string estilo = Lembrete.NEGATIVO;

            if (!String.IsNullOrWhiteSpace(codigo))
            {
                Simulado sim = Simulado.ListarPorCodigo(codigo);

                if (sim != null && sim.Colaborador.MatrColaborador == Sessao.UsuarioMatricula && !sim.FlagSimuladoEncerrado)
                {
                    SimDiaRealizacao diaRealizacao = sim.SimDiaRealizacao.FirstOrDefault(s => s.CodDiaRealizacao == codDia);

                    sim.SimDiaRealizacao.Remove(diaRealizacao);
                    Repositorio.GetInstance().SaveChanges();

                    mensagem = "O dia foi removido com sucesso.";
                    estilo = Lembrete.POSITIVO;
                }
            }

            Lembrete.AdicionarNotificacao(mensagem, estilo);
            return RedirectToAction("Provas", new { codigo = codigo });
        }

        [HttpPost]
        public ActionResult CarregarProvas(string codigo, int codDia)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                Simulado sim = Simulado.ListarPorCodigo(codigo);

                if (sim != null && sim.Colaborador.MatrColaborador == Sessao.UsuarioMatricula && !sim.FlagSimuladoEncerrado)
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

                if (sim != null && sim.Colaborador.MatrColaborador == Sessao.UsuarioMatricula && !sim.FlagSimuladoEncerrado)
                {
                    SimDiaRealizacao diaRealizacao = sim.SimDiaRealizacao.FirstOrDefault(s => s.CodDiaRealizacao == codDia);

                    if (diaRealizacao != null)
                    {
                        SimProva prova = diaRealizacao.SimProva.FirstOrDefault(p => p.CodProva == codProva);

                        if (prova == null) prova = new SimProva();

                        return PartialView("_SimuladoProvaCarregar", new SimuladoProvaViewModel()
                        {
                            Simulado = sim,
                            Prova = prova,
                            Disciplinas = Disciplina.ListarOrdenadamente()
                        });
                    }
                }
            }
            return RedirectToAction("Provas", new { codigo = codigo });
        }

        [HttpPost]
        public ActionResult NovaProva(string codigo, int codDia, FormCollection form)
        {
            string mensagem = "Ocorreu um erro na operação.";
            string estilo = Lembrete.NEGATIVO;

            if (!String.IsNullOrWhiteSpace(codigo))
            {
                Simulado sim = Simulado.ListarPorCodigo(codigo);

                if (sim != null && sim.Colaborador.MatrColaborador == Sessao.UsuarioMatricula && !sim.FlagSimuladoEncerrado)
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

                        List<int> questoesCodigos = Simulado.ObterQuestoesCodigos(prova.CodDisciplina, prova.QteQuestoes);

                        prova.SimProvaQuestao.Clear();

                        foreach (int codQuestao in questoesCodigos)
                        {
                            prova.SimProvaQuestao.Add(new SimProvaQuestao()
                            {
                                CodQuestao = codQuestao
                            });
                        }

                        if (questoesCodigos.Count >= prova.QteQuestoes)
                        {
                            mensagem = "Prova cadastrada com sucesso neste simulado.";
                            estilo = Lembrete.POSITIVO;

                        }
                        else
                        {
                            mensagem = "Foi gerada uma quantidade menor de questões para a prova deste simulado.";
                            estilo = Lembrete.NEGATIVO;
                        }

                        diaRealizacao.SimProva.Add(prova);
                        Repositorio.Commit();
                    }
                }
            }

            Lembrete.AdicionarNotificacao(mensagem, estilo);
            return RedirectToAction("Provas", new { codigo = codigo });
        }

        [HttpPost]
        public ActionResult EditarProva(string codigo, int codDia, int codProva, FormCollection form)
        {
            string mensagem = "Ocorreu um erro na operação.";
            string estilo = Lembrete.NEGATIVO;

            if (!String.IsNullOrWhiteSpace(codigo))
            {
                Simulado sim = Simulado.ListarPorCodigo(codigo);

                if (sim != null && sim.Colaborador.MatrColaborador == Sessao.UsuarioMatricula && !sim.FlagSimuladoEncerrado)
                {
                    string ddlDisciplina = form["ddlDisciplina"];
                    string txtQteQuestoes = form["txtQteQuestoes"];
                    string txtTitulo = form["txtTitulo"];
                    string txtDescricao = form["txtDescricao"];

                    if (!StringExt.IsNullOrWhiteSpace(ddlDisciplina, txtQteQuestoes, txtTitulo))
                    {
                        SimDiaRealizacao diaRealizacao = sim.SimDiaRealizacao.FirstOrDefault(s => s.CodDiaRealizacao == codDia);

                        SimProva prova = diaRealizacao.SimProva.FirstOrDefault(p => p.CodProva == codProva);

                        prova.Titulo = txtTitulo;
                        prova.Descricao = String.IsNullOrWhiteSpace(txtDescricao) ? String.Empty : txtDescricao;
                        prova.QteQuestoes = int.Parse(txtQteQuestoes);
                        prova.CodDisciplina = int.Parse(ddlDisciplina);

                        List<int> questoesCodigos = Simulado.ObterQuestoesCodigos(prova.CodDisciplina, prova.QteQuestoes);

                        prova.SimProvaQuestao.Clear();

                        foreach (int codQuestao in questoesCodigos)
                        {
                            prova.SimProvaQuestao.Add(new SimProvaQuestao()
                            {
                                CodQuestao = codQuestao
                            });
                        }

                        if (questoesCodigos.Count >= prova.QteQuestoes)
                        {
                            mensagem = "Prova editada com sucesso neste simulado.";
                            estilo = Lembrete.POSITIVO;
                        }
                        else
                        {
                            mensagem = "Foi gerada uma quantidade menor de questões para a prova deste simulado.";
                            estilo = Lembrete.NEGATIVO;
                        }

                        diaRealizacao.SimProva.Add(prova);
                        Repositorio.Commit();
                    }
                }
            }

            Lembrete.AdicionarNotificacao(mensagem, estilo);
            return RedirectToAction("Provas", new { codigo = codigo });
        }

        [HttpPost]
        public ActionResult RemoverProva(string codigo, int codDia, int codProva)
        {
            string mensagem = "Ocorreu um erro na operação.";
            string estilo = Lembrete.NEGATIVO;

            if (!String.IsNullOrWhiteSpace(codigo))
            {
                Simulado sim = Simulado.ListarPorCodigo(codigo);

                if (sim != null && sim.Colaborador.MatrColaborador == Sessao.UsuarioMatricula && !sim.FlagSimuladoEncerrado)
                {
                    SimDiaRealizacao diaRealizacao = sim.SimDiaRealizacao.FirstOrDefault(s => s.CodDiaRealizacao == codDia);

                    if (diaRealizacao != null)
                    {
                        SimProva prova = diaRealizacao.SimProva.FirstOrDefault(p => p.CodProva == codProva);
                        prova.SimProvaQuestao.Clear();
                        diaRealizacao.SimProva.Remove(prova);
                        Repositorio.Commit();

                        mensagem = "A prova foi removida com sucesso.";
                        estilo = Lembrete.POSITIVO;
                    }
                }
            }

            Lembrete.AdicionarNotificacao(mensagem, estilo);
            return RedirectToAction("Provas", new { codigo = codigo });
        }

        public ActionResult Detalhe(string codigo)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                Simulado sim = Simulado.ListarPorCodigo(codigo);

                if (sim != null && sim.Colaborador.MatrColaborador == Sessao.UsuarioMatricula)
                {
                    return View(sim);
                }
            }

            return RedirectToAction("", "Arquivo");
        }

        [HttpPost]
        public ActionResult Encerrar(string codigo)
        {
            string mensagem = "Ocorreu um erro na operação.";
            string estilo = Lembrete.NEGATIVO;

            if (!String.IsNullOrWhiteSpace(codigo))
            {
                Simulado sim = Simulado.ListarPorCodigo(codigo);

                if (sim != null && sim.Colaborador.MatrColaborador == Sessao.UsuarioMatricula)
                {
                    sim.FlagSimuladoEncerrado = true;
                    Repositorio.Commit();
                    mensagem = "O simulado foi encerrado com sucesso.";
                    estilo = Lembrete.INFO;
                }
            }

            Lembrete.AdicionarNotificacao(mensagem, estilo);
            return RedirectToAction("Detalhe", new { codigo = codigo });
        }

        [HttpPost]
        public ActionResult AlterarPermissaoInscricao(string codigo, string acao)
        {
            string mensagem = "Ocorreu um erro na operação.";
            string estilo = Lembrete.NEGATIVO;

            if (!String.IsNullOrWhiteSpace(codigo))
            {
                Simulado sim = Simulado.ListarPorCodigo(codigo);

                if (sim != null && sim.Colaborador.MatrColaborador == Sessao.UsuarioMatricula)
                {
                    switch (acao)
                    {
                        case "Bloquear":
                            sim.FlagInscricaoEncerrado = true;
                            mensagem = "As inscrições foram bloqueadas com sucesso.";
                            estilo = Lembrete.NEGATIVO;
                            break;
                        case "Liberar":
                            if (sim.CadastroCompleto)
                            {
                                if (sim.QteVagas <= sim.SimSala.Sum(s => s.Sala.Capacidade))
                                {
                                    bool valido = true;
                                    foreach(SimProva prova in sim.Provas)
                                    {
                                        if(prova.QteQuestoes != prova.SimProvaQuestao.Count)
                                        {
                                            valido = false;
                                            Lembrete.AdicionarNotificacao($"A prova de {prova.Titulo} não tem questões suficientes para realização.", Lembrete.NEGATIVO);
                                        }
                                    }
                                    if (valido)
                                    {
                                        sim.FlagInscricaoEncerrado = false;
                                        mensagem = "As inscrições foram liberadas com sucesso.";
                                        estilo = Lembrete.POSITIVO;

                                        string url = Request.Url.ToString();
                                        string simuladoUrl = url.Remove(url.IndexOf("/", url.IndexOf("//") + 2)) + Url.Action("Confirmar", "Inscricao", new { codigo = sim.Codigo });
                                        Helpers.EnviarEmail.NovoSimuladoDisponivel(Candidato.Listar(), simuladoUrl, sim.Titulo);
                                    }
                                }
                                else
                                {
                                    mensagem = "É necessário ter a capacidade das salas superior a quantidade de vagas do simulado.";
                                    estilo = Lembrete.NEGATIVO;
                                }
                            }
                            else
                            {
                                mensagem = "É necessário terminar o cadastro do simulado.";
                                estilo = Lembrete.NEGATIVO;
                            }
                            break;
                        default:
                            break;
                    }
                    Repositorio.Commit();

                }
            }

            Lembrete.AdicionarNotificacao(mensagem, estilo);
            return RedirectToAction("Detalhe", new { codigo = codigo });
        }

        [HttpPost]
        public ActionResult AlterarPrazo(string codigo, FormCollection form)
        {
            string mensagem = "Ocorreu um erro na operação.";
            string estilo = Lembrete.NEGATIVO;
            string txtNovoPrazo = form["txtNovoPrazo"];

            if (!StringExt.IsNullOrWhiteSpace(codigo, txtNovoPrazo))
            {
                Simulado sim = Simulado.ListarPorCodigo(codigo);

                if (sim != null && sim.Colaborador.MatrColaborador == Sessao.UsuarioMatricula)
                {
                    DateTime novoPrazo = DateTime.Parse($"{txtNovoPrazo} 23:59:59", new CultureInfo("pt-BR"));

                    if (novoPrazo < DateTime.Now)
                    {
                        mensagem = "A nova data de término não pode ser inferior à hoje.";
                        estilo = Lembrete.NEGATIVO;
                    }
                    else if (novoPrazo >= sim.SimDiaRealizacao?.First().DtRealizacao)
                    {
                        mensagem = "A data de termino de inscrição tem que ser antes do primeiro dia de prova do simulado.";
                        estilo = Lembrete.NEGATIVO;
                    }

                    else
                    {
                        mensagem = "O prazo foi alterado com sucesso.";
                        estilo = Lembrete.POSITIVO;
                        sim.DtTerminoInscricao = novoPrazo;
                        Repositorio.Commit();
                    }
                }
            }
            else
            {
                mensagem = "A nova data de término da inscrição é obrigatória.";
                estilo = Lembrete.NEGATIVO;
            }

            Lembrete.AdicionarNotificacao(mensagem, estilo);
            return RedirectToAction("Detalhe", new { codigo = codigo });
        }

        public ActionResult Respostas(string codigo)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                Simulado sim = Simulado.ListarPorCodigo(codigo);

                if (sim != null && sim.Colaborador.MatrColaborador == Sessao.UsuarioMatricula && !sim.FlagSimuladoEncerrado)
                {
                    SimuladoRespostasViewModel model = new SimuladoRespostasViewModel();
                    model.Simulado = sim;
                    model.Provas = sim.Provas;
                    model.Candidatos = sim.SimCandidato.OrderBy(c => c.Candidato.Nome).ToList();

                    return View(model);
                }
            }

            return RedirectToAction("", "Arquivo");
        }

        [HttpPost]
        public ActionResult CorrecaoPorProva(string codigo, int codDia, int codProva)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                Simulado sim = Simulado.ListarPorCodigo(codigo);

                if (sim != null && sim.Colaborador.MatrColaborador == Sessao.UsuarioMatricula && !sim.FlagSimuladoEncerrado)
                {
                    SimDiaRealizacao diaRealizacao = sim.SimDiaRealizacao.FirstOrDefault(s => s.CodDiaRealizacao == codDia);

                    if (diaRealizacao != null)
                    {
                        SimProva prova = diaRealizacao.SimProva.FirstOrDefault(p => p.CodProva == codProva);

                        return PartialView("_SimuladoRespostasProva", prova.SimCandidatoProva.OrderBy(p => p.SimCandidato.Candidato.Nome).ToList());
                    }
                }

            }
            return null;
        }

        [HttpPost]
        public ActionResult CorrecaoPorCandidato(string codigo, int codDia, int codProva, int codCandidato)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                Simulado sim = Simulado.ListarPorCodigo(codigo);

                if (sim != null && sim.Colaborador.MatrColaborador == Sessao.UsuarioMatricula && !sim.FlagSimuladoEncerrado)
                {
                    SimDiaRealizacao diaRealizacao = sim.SimDiaRealizacao.FirstOrDefault(s => s.CodDiaRealizacao == codDia);

                    if (diaRealizacao != null)
                    {
                        SimProva prova = diaRealizacao.SimProva.FirstOrDefault(p => p.CodProva == codProva);
                        SimCandidatoProva candidato = contexto.SimCandidatoProva
                                                        .FirstOrDefault(p => p.Ano == sim.Ano
                                                        && p.NumIdentificador == sim.NumIdentificador
                                                        && p.CodProva == codProva
                                                        && p.CodDiaRealizacao == codDia
                                                        && p.CodCandidato == codCandidato);

                        SimuladoRespostasCandidatoViewModel model = new SimuladoRespostasCandidatoViewModel();

                        model.Simulado = sim;
                        model.Prova = prova;
                        model.Candidato = candidato.SimCandidato.Candidato;
                        model.Questoes = prova.SimProvaQuestao.OrderBy(q => q.CodQuestao).ToList();
                        model.Respostas = candidato.SimCandidatoQuestao.OrderBy(q => q.CodQuestao).ToList();
                        model.CandidatoProva = candidato;

                        return PartialView("_SimuladoRespostasCandidato", model);
                    }
                }

            }
            return null;
        }

        [HttpPost]
        public ActionResult InserirRespostasProva(string codigo, int codDia, int codProva, FormCollection form)
        {
            string mensagem = "Ocorreu um erro na operação.";
            string estilo = Lembrete.NEGATIVO;

            if (!String.IsNullOrWhiteSpace(codigo))
            {
                Simulado sim = Simulado.ListarPorCodigo(codigo);

                if (sim != null && sim.Colaborador.MatrColaborador == Sessao.UsuarioMatricula && !sim.FlagSimuladoEncerrado)
                {
                    SimDiaRealizacao diaRealizacao = sim.SimDiaRealizacao.FirstOrDefault(s => s.CodDiaRealizacao == codDia);

                    if (diaRealizacao != null)
                    {
                        SimProva prova = diaRealizacao.SimProva.FirstOrDefault(p => p.CodProva == codProva);

                        foreach (SimCandidatoProva candidato in prova.SimCandidatoProva)
                        {
                            string flagAusente = form[candidato.CodCandidato + "ausente"];

                            if (flagAusente != null && flagAusente == "on")
                            {
                                candidato.QteAcertos = null;
                                candidato.FlagPresente = false;
                            }
                            else
                            {
                                int qteAcerto = -1;
                                string qteAcertoForm = form[candidato.CodCandidato.ToString()];
                                if (!String.IsNullOrEmpty(qteAcertoForm) && qteAcertoForm.IsNumber())
                                {
                                    int.TryParse(qteAcertoForm, out qteAcerto);
                                }
                                if (qteAcerto > -1)
                                {
                                    candidato.QteAcertos = qteAcerto;
                                    candidato.FlagPresente = true;
                                }
                            }
                        }

                        Repositorio.Commit();

                        mensagem = "O preenchimento ocorreu com sucesso.";
                        estilo = Lembrete.POSITIVO;
                    }
                }
            }
            Lembrete.AdicionarNotificacao(mensagem, estilo);
            return RedirectToAction("Respostas", new { codigo = codigo });
        }

        [HttpPost]
        public ActionResult InserirRespostasCandidato(string codigo, int codDia, int codProva, int codCandidato, FormCollection form)
        {
            string mensagem = "Ocorreu um erro na operação.";
            string estilo = Lembrete.NEGATIVO;

            if (!String.IsNullOrWhiteSpace(codigo))
            {
                Simulado sim = Simulado.ListarPorCodigo(codigo);

                if (sim != null && sim.Colaborador.MatrColaborador == Sessao.UsuarioMatricula && !sim.FlagSimuladoEncerrado)
                {
                    SimDiaRealizacao diaRealizacao = sim.SimDiaRealizacao.FirstOrDefault(s => s.CodDiaRealizacao == codDia);

                    if (diaRealizacao != null)
                    {
                        SimProva prova = diaRealizacao.SimProva.FirstOrDefault(p => p.CodProva == codProva);
                        SimCandidatoProva candidato = contexto.SimCandidatoProva
                                                        .FirstOrDefault(p => p.Ano == sim.Ano
                                                        && p.NumIdentificador == sim.NumIdentificador
                                                        && p.CodProva == codProva
                                                        && p.CodDiaRealizacao == codDia
                                                        && p.CodCandidato == codCandidato);

                        candidato.SimCandidatoQuestao.Clear();
                        int acertos = 0;
                        string flagAusente = form["ausente"];

                        if (flagAusente != null && flagAusente == "on")
                        {
                            candidato.FlagPresente = false;
                            candidato.QteAcertos = null;
                        }
                        else
                        {
                            foreach (SimProvaQuestao questao in prova.SimProvaQuestao)
                            {
                                string strQuestao = "questao" + questao.CodQuestao;
                                
                                if (form[strQuestao] != null)
                                {
                                    int alternativa = int.Parse(form[strQuestao]);

                                    candidato.SimCandidatoQuestao.Add(new SimCandidatoQuestao()
                                    {
                                        SimProvaQuestao = questao,
                                        RespAlternativa = alternativa
                                    });

                                    if (questao.Questao.Alternativa.First(a => a.FlagGabarito).CodOrdem == alternativa)
                                        acertos++;
                                }
                            }
                            candidato.FlagPresente = true;
                            candidato.QteAcertos = acertos;
                        }
                        Repositorio.Commit();

                        mensagem = "O preenchimento ocorreu com sucesso.";
                        estilo = Lembrete.POSITIVO;
                    }
                }

            }
            Lembrete.AdicionarNotificacao(mensagem, estilo);
            return RedirectToAction("Respostas", new { codigo = codigo });
        }

        [HttpPost]
        public ActionResult MapearSalas(string codigo)
        {
            string mensagem = "Ocorreu um erro na operação.";
            string estilo = Lembrete.NEGATIVO;

            if (!StringExt.IsNullOrWhiteSpace(codigo))
            {
                Simulado sim = Simulado.ListarPorCodigo(codigo);

                if (sim != null && sim.Colaborador.MatrColaborador == Sessao.UsuarioMatricula)
                {

                    List<SimCandidato> candidatos = sim.SimCandidato.OrderBy(c => c.Candidato.Nome).ToList();
                    List<SimSala> salas = sim.SimSala.ToList();

                    int indice = 1;
                    int salaIndex = 0;
                    SimSala sala = salas[salaIndex];

                    foreach(SimCandidato candidato in candidatos)
                    {
                        if (indice > sala.Sala.Capacidade)
                        {
                            sala = salas[++salaIndex];
                            indice = 1;
                        }

                        candidato.CodSala = sala.CodSala;
                        indice++;
                    }

                    mensagem = "As salas foram mapeadas com sucesso.";
                    estilo = Lembrete.POSITIVO;
                    Repositorio.Commit();

                    string url = Request.Url.ToString();
                    string simuladoUrl = url.Remove(url.IndexOf("/", url.IndexOf("//") + 2)) + Url.Action("Inscricoes", "Candidato", new { codigo = sim.Codigo });

                    Helpers.EnviarEmail.CartaoDeInscricaoDisponivel(candidatos.Select(sc => sc.Candidato).ToList(), simuladoUrl, sim.Titulo);
                }
            }

            Lembrete.AdicionarNotificacao(mensagem, estilo);
            return RedirectToAction("Detalhe", new { codigo = codigo });
        }

        [HttpPost]
        public ActionResult EnviarEmail(string codigo, string mensagemEmail)
        {
            string mensagem = "Um erro ocorreu na operação.";
            string estilo = Lembrete.NEGATIVO;

            if (!StringExt.IsNullOrWhiteSpace(codigo, mensagemEmail))
            {
                Simulado sim = Simulado.ListarPorCodigo(codigo);

                if (sim != null && sim.Colaborador.MatrColaborador == Sessao.UsuarioMatricula && !sim.FlagSimuladoEncerrado)
                {
                    string url = Request.Url.ToString();
                    string simuladoUrl = url.Remove(url.IndexOf("/", url.IndexOf("//") + 2)) + Url.Action("Inscricoes", "Candidato", new { codigo = sim.Codigo });

                    Helpers.EnviarEmail.MensagemParaCandidatos(sim.SimCandidato.Select(s => s.Candidato).ToList(), mensagemEmail.ToHtml("p"), simuladoUrl, sim.Titulo);

                    mensagem = "Mensagem enviada.";
                    estilo = Lembrete.POSITIVO;
                }
            }
            else
            {
                mensagem = "Não é possível enviar uma mensagem vazia.";
            }

            Lembrete.AdicionarNotificacao(mensagem, estilo);
            return RedirectToAction("Detalhe", new { codigo = codigo });
        }

        [HttpPost]
        public ActionResult ListarCandidatoPorSala(string codigo, int codSala)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                Simulado sim = Simulado.ListarPorCodigo(codigo);

                if (sim != null && sim.Colaborador.MatrColaborador == Sessao.UsuarioMatricula && !sim.FlagSimuladoEncerrado)
                {
                    SimSala sala = sim.SimSala.FirstOrDefault(s => s.CodSala == codSala);

                    return PartialView("_SimuladoSalaCandidatos", sala);
                }
            }
            return null;
        }

        [HttpPost]
        public ActionResult FinalizarProvas(string codigo)
        {
            string mensagem = "Ocorreu um erro na operação.";
            string estilo = Lembrete.NEGATIVO;

            if (!String.IsNullOrWhiteSpace(codigo))
            {
                Simulado sim = Simulado.ListarPorCodigo(codigo);

                if (sim != null && sim.Colaborador.MatrColaborador == Sessao.UsuarioMatricula)
                {
                    
                    sim.FlagProvaEncerrada = true;
                    mensagem = "As provas foram finalizadas com sucesso.";
                    estilo = Lembrete.POSITIVO;

                    Repositorio.Commit();

                }
            }

            Lembrete.AdicionarNotificacao(mensagem, estilo);
            return RedirectToAction("Detalhe", new { codigo = codigo });
        }

        [HttpPost]
        public ActionResult CalcularResultados(string codigo)
        {
            string mensagem = "Ocorreu um erro na operação.";
            string estilo = Lembrete.NEGATIVO;

            if (!String.IsNullOrWhiteSpace(codigo))
            {
                Simulado sim = Simulado.ListarPorCodigo(codigo);

                if (sim != null && sim.Colaborador.MatrColaborador == Sessao.UsuarioMatricula)
                {

                    foreach (SimProva prova in sim.Provas)
                    {
                        List<SimCandidatoProva> candidatos = prova.SimCandidatoProva.Where(c => c.FlagPresente.HasValue && c.FlagPresente.Value).ToList();

                        double qcpp = candidatos.Count; //QCPP = Quantidade de Candidatos Presentes à Prova
                        double maap = candidatos.Sum(c => c.QteAcertos.Value) / qcpp; //MAAP = Média Aritmética dos Acertos da Prova
                        double Eqac = candidatos.Select(c => Math.Pow(c.QteAcertos.Value, 2.0)).Sum(); //EQAC = Soma da Quantidade de Acertos dos Candidatos da Prova
                        double dpap = Math.Sqrt((Eqac / qcpp) - Math.Pow(maap, 2.0)); //DPAP = Desvio Padrão dos Acertos da Prova

                        prova.MediaAritmeticaAcerto = Convert.ToDecimal(maap);
                        prova.DesvioPadraoAcerto = Convert.ToDecimal(dpap);

                        foreach(SimCandidatoProva candidato in candidatos)
                        {
                            candidato.EscorePadronizado = Convert.ToDecimal(((candidato.QteAcertos - maap) / dpap) * 100.0 + 500.0);
                        }
                    }

                    foreach (SimCandidato candidato in sim.SimCandidato)
                    {
                        if ((bool)candidato.SimCandidatoProva.First().FlagPresente)
                        {
                            decimal? somaEscoreProvas = candidato.SimCandidatoProva.Sum(p => p.EscorePadronizado);

                            candidato.EscorePadronizadoFinal = somaEscoreProvas.Value / candidato.SimCandidatoProva.Count();
                        }
                    }

                    sim.FlagSimuladoEncerrado = true;
                    mensagem = "Os escores foram calculados com sucesso e o simulado foi encerrado com sucesso.";
                    estilo = Lembrete.POSITIVO;

                    Repositorio.Commit();

                }
            }

            Lembrete.AdicionarNotificacao(mensagem, estilo);
            return RedirectToAction("Detalhe", new { codigo = codigo });
        }

        [HttpPost]
        public ActionResult Classificacao(string codigo)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                Simulado sim = Simulado.ListarPorCodigo(codigo);

                if (sim != null && sim.Colaborador.MatrColaborador == Sessao.UsuarioMatricula)
                {
                    return PartialView("_SimuladoClassificacao", sim);
                }
            }
            return null;
        }

        [HttpPost]
        public ActionResult ClassificacaoCandidato(string codigo, int codCandidato)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                Simulado sim = Simulado.ListarPorCodigo(codigo);

                if (sim != null && sim.Colaborador.MatrColaborador == Sessao.UsuarioMatricula)
                {
                    SimCandidato candidato = sim.SimCandidato.FirstOrDefault(c => c.CodCandidato == codCandidato);

                    return PartialView("_SimuladoClassificacaoCandidato", candidato);
                }
            }
            return null;
        }

        public ActionResult Imprimir(string codigo)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                Simulado sim = Simulado.ListarPorCodigo(codigo);

                if (sim != null && sim.Colaborador.MatrColaborador == Sessao.UsuarioMatricula && !sim.FlagSimuladoEncerrado)
                {
                    if (sim.Provas.Count > 0 && !sim.FlagProvaEncerrada)
                    {
                        return View(sim);
                    }
                }
            }
            return RedirectToAction("Detalhe", new { codigo = codigo });
        }

        public ActionResult ImprimirProva(string codigo, int dia, int prova)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                Simulado sim = Simulado.ListarPorCodigo(codigo);

                if (sim != null && sim.Colaborador.MatrColaborador == Sessao.UsuarioMatricula && !sim.FlagSimuladoEncerrado)
                {
                    if (sim.Provas.Count > 0 && !sim.FlagProvaEncerrada)
                    {
                        SimProva model = sim.Provas.FirstOrDefault(p=>p.CodDiaRealizacao == dia && p.CodProva == prova);
                        if (model != null)
                        {
                            return PartialView("_ImprimirProva", model);
                        }
                    }
                }
            }
            return RedirectToAction("Detalhe", new { codigo = codigo });
        }

        public ActionResult ImprimirCartaoResposta(string codigo, int dia, int prova)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                Simulado sim = Simulado.ListarPorCodigo(codigo);

                if (sim != null && sim.Colaborador.MatrColaborador == Sessao.UsuarioMatricula && !sim.FlagSimuladoEncerrado)
                {
                    if (sim.Provas.Count > 0 && !sim.FlagProvaEncerrada)
                    {
                        SimProva model = sim.Provas.FirstOrDefault(p => p.CodDiaRealizacao == dia && p.CodProva == prova);
                        if (model != null)
                        {
                            return PartialView("_ImprimirCartaoResposta", model);
                        }
                    }
                }
            }
            return RedirectToAction("Detalhe", new { codigo = codigo });
        }

        public ActionResult ImprimirGabarito(string codigo, int dia, int prova)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                Simulado sim = Simulado.ListarPorCodigo(codigo);

                if (sim != null && sim.Colaborador.MatrColaborador == Sessao.UsuarioMatricula && !sim.FlagSimuladoEncerrado)
                {
                    if (sim.Provas.Count > 0 && !sim.FlagProvaEncerrada)
                    {
                        SimProva model = sim.Provas.FirstOrDefault(p => p.CodDiaRealizacao == dia && p.CodProva == prova);
                        if (model != null)
                        {
                            return PartialView("_ImprimirGabarito", model);
                        }
                    }
                }
            }
            return RedirectToAction("Detalhe", new { codigo = codigo });
        }

        public ActionResult ImprimirListaPresenca(string codigo)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                Simulado sim = Simulado.ListarPorCodigo(codigo);

                if (sim != null && sim.Colaborador.MatrColaborador == Sessao.UsuarioMatricula && !sim.FlagSimuladoEncerrado)
                {
                    return PartialView("_ImprimirListaPresenca", sim);
                }
            }
            return RedirectToAction("Detalhe", new { codigo = codigo });
        }
    }
}
