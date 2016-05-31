using SIAC.Helpers;
using SIAC.Models;
using SIAC.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace SIAC.Controllers
{
    [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.ESTUDANTE,Categoria.PROFESSOR })]
    public class ReposicaoController : Controller
    {
        public List<AvalAcadReposicao> Reposicoes
        {
            get
            {
                if (Sessao.UsuarioCategoriaCodigo == Categoria.PROFESSOR)
                {
                    int codProfessor = Professor.ListarPorMatricula(Sessao.UsuarioMatricula).CodProfessor;
                    return AvalAcadReposicao.ListarPorProfessor(codProfessor);
                }
                else
                {
                    int codAluno = Aluno.ListarPorMatricula(Sessao.UsuarioMatricula).CodAluno;
                    return AvalAcadReposicao.ListarPorAluno(codAluno);
                }
            }
        }

        // POST: principal/avaliacao/reposicao/listar            
        [HttpPost]
        public ActionResult Listar(int? pagina, string pesquisa, string ordenar, string categoria, string disciplina)
        {
            int quantidade = 12;
            List<AvalAcadReposicao> reposicoes = Reposicoes;
            pagina = pagina ?? 1;
            if (!String.IsNullOrWhiteSpace(pesquisa))
            {
                reposicoes = reposicoes.Where(a => a.Avaliacao.CodAvaliacao.ToLower().Contains(pesquisa.ToLower())).ToList();
            }

            if (!String.IsNullOrWhiteSpace(disciplina))
            {
                reposicoes = reposicoes.Where(a => a.Disciplina.CodDisciplina == int.Parse(disciplina)).ToList();
            }

            switch (categoria)
            {
                case "agendada":
                    reposicoes = reposicoes.Where(a => a.Avaliacao.FlagAgendada).ToList();
                    break;
                case "realizada":
                    reposicoes = reposicoes.Where(a => a.Avaliacao.FlagRealizada).ToList();
                    break;
                case "arquivo":
                    reposicoes = reposicoes.Where(a => a.Avaliacao.FlagArquivo).ToList();
                    break;
            }

            switch (ordenar)
            {
                case "data_desc":
                    reposicoes = reposicoes.OrderByDescending(a => a.Avaliacao.DtCadastro).ToList();
                    break;
                case "data":
                    reposicoes = reposicoes.OrderBy(a => a.Avaliacao.DtCadastro).ToList();
                    break;
                default:
                    reposicoes = reposicoes.OrderByDescending(a => a.Avaliacao.DtCadastro).ToList();
                    break;
            }

            return PartialView("_ListaReposicao", reposicoes.Skip((quantidade * pagina.Value) - quantidade).Take(quantidade).ToList());
        }

        // GET: historico/avaliacao/reposicao/
        public ActionResult Index()
        {
            if (Request.Url.ToString().ToLower().Contains("principal"))
            {
                return Redirect("~/historico/avaliacao/reposicao");
            }
            AvaliacaoIndexViewModel model = new AvaliacaoIndexViewModel();
            model.Disciplinas = Reposicoes.Select(a => a.Disciplina).Distinct().ToList();
            return View(model);
        }

        // GET: principal/avaliacao/reposicao/justificar/REPO201520001
        [HttpGet]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.PROFESSOR })]
        public ActionResult Justificar(string codigo)
        {
            AvalAcademica aval = AvalAcademica.ListarPorCodigoAvaliacao(codigo);
            if (aval.Professor.Usuario.Matricula == Sessao.UsuarioMatricula)
            {
                return View(aval);
            }
            return RedirectToAction("Index");
        }

        // POST: principal/avaliacao/reposicao/justificar/REPO201520001
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.PROFESSOR })]
        public ActionResult Justificar(string codigo, Dictionary<string, string> justificacao)
        {
            AvalAcademica aval = AvalAcademica.ListarPorCodigoAvaliacao(codigo);
            if (aval.Professor.Usuario.Matricula == Sessao.UsuarioMatricula)
            {
                if (Usuario.Verificar(justificacao["senha"]))
                {
                    Aluno aluno = Aluno.ListarPorMatricula(justificacao["aluno"]);
                    AvalPessoaResultado apr = aval.Avaliacao.AvalPessoaResultado.FirstOrDefault(p => p.CodPessoaFisica == aluno.Usuario.CodPessoaFisica);
                    if (apr == null)
                    {
                        AvalPessoaResultado avalPessoaResultado = new AvalPessoaResultado();
                        avalPessoaResultado.CodPessoaFisica = aluno.Usuario.CodPessoaFisica;
                        avalPessoaResultado.HoraTermino = aval.Avaliacao.DtAplicacao;
                        avalPessoaResultado.QteAcertoObj = 0;
                        avalPessoaResultado.Nota = 0;

                        avalPessoaResultado.Justificacao.Add(new Justificacao()
                        {
                            Professor = aval.Professor,
                            DtCadastro = DateTime.Parse(justificacao["cadastro"], new CultureInfo("pt-BR")),
                            DtConfirmacao = DateTime.Parse(justificacao["confirmacao"], new CultureInfo("pt-BR")),
                            Descricao = justificacao["descricao"]
                        });

                        aval.Avaliacao.AvalPessoaResultado.Add(avalPessoaResultado);

                        Repositorio.GetInstance().SaveChanges();
                    }
                }
            }
            return null;
        }
        
        // POST: principal/avaliacao/reposicao/gerar/REPO201520001
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.PROFESSOR })]
        public string Gerar(string codigo, int[] justificaticoes, bool nova = false)
        {
            DateTime hoje = DateTime.Now;

            AvalAcadReposicao aval = new AvalAcadReposicao();
            AvalAcademica acad = AvalAcademica.ListarPorCodigoAvaliacao(codigo);

            aval.Avaliacao = new Avaliacao();

            aval.Avaliacao.TipoAvaliacao = TipoAvaliacao.ListarPorCodigo(TipoAvaliacao.REPOSICAO);
            aval.Avaliacao.Ano = hoje.Year;
            aval.Avaliacao.Semestre = hoje.SemestreAtual();
            aval.Avaliacao.NumIdentificador = Avaliacao.ObterNumIdentificador(TipoAvaliacao.REPOSICAO);
            aval.Avaliacao.DtCadastro = hoje;

            if (nova)
            {
                List<QuestaoTema> lstQuestoes = new List<QuestaoTema>();
                List<AvaliacaoTema> lstAvaliacaoTema = acad.Avaliacao.AvaliacaoTema.ToList();
                int quantidadeObjetiva = lstAvaliacaoTema.QteQuestoesPorTipo(TipoQuestao.OBJETIVA);
                int quantidadeDiscursiva = lstAvaliacaoTema.QteQuestoesPorTipo(TipoQuestao.DISCURSIVA);
                string[] temasCodigo = lstAvaliacaoTema.Select(a => a.CodTema.ToString()).ToArray();
                int codDificuldade = acad.Avaliacao.Questao.Max(a => a.CodDificuldade);

                if (quantidadeObjetiva > 0)
                {
                    lstQuestoes.AddRange(Questao.ListarPorDisciplina(acad.CodDisciplina, temasCodigo, codDificuldade, TipoQuestao.OBJETIVA, quantidadeObjetiva));
                }
                if (quantidadeDiscursiva > 0)
                {
                    lstQuestoes.AddRange(Questao.ListarPorDisciplina(acad.CodDisciplina, temasCodigo, codDificuldade, TipoQuestao.DISCURSIVA, quantidadeDiscursiva));
                }

                foreach (string temaCodigo in temasCodigo)
                {
                    AvaliacaoTema avalTema = new AvaliacaoTema();
                    avalTema.Tema = Tema.ListarPorCodigo(acad.CodDisciplina, int.Parse(temaCodigo));
                    foreach (QuestaoTema queTma in lstQuestoes.Where(q => q.CodTema == int.Parse(temaCodigo)))
                    {
                        AvalTemaQuestao avalTemaQuestao = new AvalTemaQuestao();
                        avalTemaQuestao.QuestaoTema = queTma;
                        avalTema.AvalTemaQuestao.Add(avalTemaQuestao);
                    }
                    aval.Avaliacao.AvaliacaoTema.Add(avalTema);
                }
            }
            else
            {
                foreach (AvaliacaoTema avaliacaoTema in acad.Avaliacao.AvaliacaoTema)
                {
                    aval.Avaliacao.AvaliacaoTema.Add(new AvaliacaoTema
                    {
                        Tema = avaliacaoTema.Tema,
                        AvalTemaQuestao = avaliacaoTema.AvalTemaQuestao.Select(a => new AvalTemaQuestao { QuestaoTema = a.QuestaoTema }).ToList()
                    });
                }
            }

            foreach (int codJustificacao in justificaticoes)
            {
                aval.Justificacao.Add(acad.Justificacoes.First(j => j.CodJustificacao == codJustificacao));
            }

            Repositorio.GetInstance().AvalAcadReposicao.Add(aval);
            Repositorio.GetInstance().SaveChanges();

            return nova ? Url.Action("Configurar", new { codigo = aval.Avaliacao.CodAvaliacao }) : Url.Action("Agendar", new { codigo = aval.Avaliacao.CodAvaliacao });
        }

        // GET: principal/avaliacao/reposicao/configurar/REPO201520001
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.PROFESSOR })]
        public ActionResult Configurar(string codigo)
        {
            TempData["listaQuestoesAntigas"] = new List<AvalTemaQuestao>();
            TempData["listaQuestoesNovas"] = new List<AvalTemaQuestao>();
            TempData["listaQuestoesPossiveisObj"] = new List<QuestaoTema>();
            TempData["listaQuestoesPossiveisDisc"] = new List<QuestaoTema>();
            TempData["listaQuestoesIndices"] = new List<int>();
            TempData["listaQuestoesRecentes"] = new List<int>();

            if (!String.IsNullOrWhiteSpace(codigo) && !Sistema.AvaliacaoUsuario.ContainsKey(codigo))
            {
                AvalAcadReposicao repo = AvalAcadReposicao.ListarPorCodigoAvaliacao(codigo);
                if (repo != null && repo.Professor.MatrProfessor == Sessao.UsuarioMatricula && repo.Avaliacao.AvalPessoaResultado.Count == 0)
                {
                    return View(repo);
                }
            }
            return RedirectToAction("Index");
        }

        // POST: principal/avaliacao/reposicao/trocarquestao/REPO201520001
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.PROFESSOR })]
        public ActionResult TrocarQuestao(string codigoAvaliacao, int tipo, int indice, int codQuestao)
        {
            List<AvalTemaQuestao> antigas = (List<AvalTemaQuestao>)TempData["listaQuestoesAntigas"];
            List<AvalTemaQuestao> novas = (List<AvalTemaQuestao>)TempData["listaQuestoesNovas"];
            List<QuestaoTema> questoesTrocaObj = (List<QuestaoTema>)TempData["listaQuestoesPossiveisObj"];
            List<QuestaoTema> questoesTrocaDisc = (List<QuestaoTema>)TempData["listaQuestoesPossiveisDisc"];
            List<int> indices = (List<int>)TempData["listaQuestoesIndices"];
            List<int> recentes = (List<int>)TempData["listaQuestoesRecentes"];

            TempData.Keep();
            Random r = new Random();

            if (!String.IsNullOrEmpty(codigoAvaliacao))
            {
                AvalAcadReposicao repo = AvalAcadReposicao.ListarPorCodigoAvaliacao(codigoAvaliacao);
                if (repo != null && repo.Professor.MatrProfessor == Sessao.UsuarioMatricula)
                {
                    List<QuestaoTema> AvalQuestTema = repo.Avaliacao.QuestaoTema;

                    QuestaoTema questao = null;

                    if (tipo == TipoQuestao.OBJETIVA)
                    {
                        if (questoesTrocaObj.Count <= 0)
                        {
                            TempData["listaQuestoesPossiveisObj"] = Questao.ObterNovasQuestoes(AvalQuestTema, tipo);
                            questoesTrocaObj = (List<QuestaoTema>)TempData["listaQuestoesPossiveisObj"];
                        }

                        int random = r.Next(0, questoesTrocaObj.Count);
                        questao = questoesTrocaObj.ElementAtOrDefault(random);
                    }
                    else if (tipo == TipoQuestao.DISCURSIVA)
                    {
                        if (questoesTrocaDisc.Count <= 0)
                        {
                            TempData["listaQuestoesPossiveisDisc"] = Questao.ObterNovasQuestoes(AvalQuestTema, tipo);
                            questoesTrocaDisc = (List<QuestaoTema>)TempData["listaQuestoesPossiveisDisc"];
                        }

                        int random = r.Next(0, questoesTrocaDisc.Count);
                        questao = questoesTrocaDisc.ElementAtOrDefault(random);
                    }

                    if (questao != null)
                    {
                        if (!indices.Contains(indice))
                        {
                            AvalTemaQuestao aqtAntiga = (from atq in Repositorio.GetInstance().AvalTemaQuestao
                                                         where atq.Ano == repo.Ano
                                                         && atq.Semestre == repo.Semestre
                                                         && atq.CodTipoAvaliacao == repo.CodTipoAvaliacao
                                                         && atq.NumIdentificador == repo.NumIdentificador
                                                         && atq.CodQuestao == codQuestao
                                                         select atq).FirstOrDefault();
                            antigas.Add(aqtAntiga);
                            indices.Add(indice);
                        }

                        int index = indices.IndexOf(indice);

                        AvalTemaQuestao atqNova = new AvalTemaQuestao();
                        atqNova.Ano = repo.Avaliacao.Ano;
                        atqNova.Semestre = repo.Avaliacao.Semestre;
                        atqNova.CodTipoAvaliacao = repo.Avaliacao.CodTipoAvaliacao;
                        atqNova.NumIdentificador = repo.Avaliacao.NumIdentificador;
                        atqNova.QuestaoTema = questao;

                        if (novas.Count > index)
                        {
                            novas.RemoveAt(index);
                        }
                        if (recentes.Count > index)
                        {
                            recentes.RemoveAt(index);
                        }

                        novas.Insert(index, atqNova);
                        recentes.Insert(index, codQuestao);

                        ViewData["Index"] = indice;
                        return PartialView("_Questao", questao.Questao);
                    }
                }
            }

            return Json(String.Empty);
        }

        // POST: principal/avaliacao/reposicao/desfazer/REPO201520001
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.PROFESSOR })]
        public ActionResult Desfazer(string codigoAvaliacao, int tipo, int indice, int codQuestao)
        {
            List<AvalTemaQuestao> antigas = (List<AvalTemaQuestao>)TempData["listaQuestoesAntigas"];
            List<AvalTemaQuestao> novas = (List<AvalTemaQuestao>)TempData["listaQuestoesNovas"];
            List<QuestaoTema> questoesTrocaObj = (List<QuestaoTema>)TempData["listaQuestoesPossiveisObj"];
            List<QuestaoTema> questoesTrocaDisc = (List<QuestaoTema>)TempData["listaQuestoesPossiveisDisc"];
            List<int> indices = (List<int>)TempData["listaQuestoesIndices"];
            List<int> recentes = (List<int>)TempData["listaQuestoesRecentes"];

            TempData.Keep();

            if (!String.IsNullOrEmpty(codigoAvaliacao))
            {
                int codQuestaoRecente = recentes[indices.IndexOf(indice)];

                QuestaoTema questao = null;

                if (tipo == TipoQuestao.OBJETIVA)
                {
                    questao = questoesTrocaObj.FirstOrDefault(qt => qt.CodQuestao == codQuestaoRecente);
                    if (questao == null)
                    {
                        questao = antigas[indices.IndexOf(indice)].QuestaoTema;
                    }
                }
                else if (tipo == TipoQuestao.DISCURSIVA)
                {
                    questao = questoesTrocaDisc.FirstOrDefault(qt => qt.CodQuestao == codQuestaoRecente);
                    if (questao == null)
                    {
                        questao = antigas[indices.IndexOf(indice)].QuestaoTema;
                    }
                }

                if (questao != null)
                {
                    novas[indices.IndexOf(indice)].QuestaoTema = questao;

                    ViewData["Index"] = indice;
                    return PartialView("_Questao", questao.Questao);
                }
            }

            return Json(String.Empty);
        }

        // POST: principal/avaliacao/reposicao/salvar/REPO201520001
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.PROFESSOR })]
        public ActionResult Salvar(string codigo)
        {
            List<AvalTemaQuestao> antigas = (List<AvalTemaQuestao>)TempData["listaQuestoesAntigas"];
            List<AvalTemaQuestao> novas = (List<AvalTemaQuestao>)TempData["listaQuestoesNovas"];

            if (antigas.Count != 0 && novas.Count != 0)
            {
                Contexto contexto = Repositorio.GetInstance();
                for (int i = 0; i < antigas.Count && i < novas.Count; i++)
                {
                    contexto.AvalTemaQuestao.Remove(antigas.ElementAtOrDefault(i));
                    contexto.AvalTemaQuestao.Add(novas.ElementAtOrDefault(i));
                }
                contexto.SaveChanges();
            }
            TempData.Clear();

            return RedirectToAction("Agendar", new { codigo = codigo });
        }

        // GET: principal/avaliacao/reposicao/agendar/REPO201520001
        [HttpGet]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.PROFESSOR })]
        public ActionResult Agendar(string codigo)
        {
            if (String.IsNullOrWhiteSpace(codigo) || Sistema.AvaliacaoUsuario.ContainsKey(codigo))
            {
                return RedirectToAction("Index");
            }

            AvalAcadReposicao aval = AvalAcadReposicao.ListarPorCodigoAvaliacao(codigo);

            if (aval.Professor.MatrProfessor == Sessao.UsuarioMatricula)
            {
                AvaliacaoAgendarViewModel model = new AvaliacaoAgendarViewModel();

                model.Avaliacao = aval.Avaliacao;
                model.Salas = Sala.ListarOrdenadamente();

                return View(model);
            }
            return RedirectToAction("Index");
        }

        // POST: principal/avaliacao/reposicao/agendar/REPO201520001
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.PROFESSOR })]
        public ActionResult Agendar(string codigo, FormCollection form)
        {
            string strCodSala = form["ddlSala"];
            string data = form["txtData"];
            string horaInicio = form["txtHoraInicio"];
            string horaTermino = form["txtHoraTermino"];
            if (!StringExt.IsNullOrWhiteSpace(strCodSala, data, horaInicio, horaTermino))
            {
                AvalAcadReposicao aval = AvalAcadReposicao.ListarPorCodigoAvaliacao(codigo);

                if (aval.Professor.MatrProfessor == Sessao.UsuarioMatricula)
                {
                    // Sala
                    int codSala;
                    int.TryParse(strCodSala, out codSala);
                    Sala sala = Sala.ListarPorCodigo(codSala);
                    if (sala != null)
                    {
                        aval.Sala = sala;
                    }

                    // Data de Aplicacao
                    DateTime dtAplicacao = DateTime.Parse(data+ " " + horaInicio, new CultureInfo("pt-BR"));
                    DateTime dtAplicacaoTermino = DateTime.Parse(data + " " + horaTermino, new CultureInfo("pt-BR"));

                    if (dtAplicacao.IsFuture() && dtAplicacaoTermino.IsFuture() && dtAplicacaoTermino > dtAplicacao)
                    {
                        aval.Avaliacao.DtAplicacao = dtAplicacao;
                        aval.Avaliacao.Duracao = Convert.ToInt32((dtAplicacaoTermino - aval.Avaliacao.DtAplicacao.Value).TotalMinutes);
                    }

                    aval.Avaliacao.FlagLiberada = false;

                    Repositorio.GetInstance().SaveChanges();
                }
            }

            return RedirectToAction("Agendada", new { codigo = codigo });
        }

        // GET: historico/avaliacao/reposicao/salvar/REPO201520001
        [HttpGet]
        public ActionResult Agendada(string codigo)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                Usuario usuario = Usuario.ListarPorMatricula(Sessao.UsuarioMatricula);
                AvalAcadReposicao repo = AvalAcadReposicao.ListarAgendadaPorUsuario(usuario).FirstOrDefault(a => a.Avaliacao.CodAvaliacao.ToLower() == codigo.ToLower());
                if (repo != null)
                {
                    return View(repo);
                }
            }
            return RedirectToAction("Detalhe", new { codigo = codigo });
        }

        // POST: principal/avaliacao/reposicao/arquivar/REPO201520001
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.PROFESSOR })]
        public ActionResult Arquivar(string codigo)
        {
            if (!String.IsNullOrWhiteSpace(codigo) && !Sistema.AvaliacaoUsuario.ContainsKey(codigo))
            {
                return Json(Avaliacao.AlternarFlagArquivo(codigo));
            }
            return Json(false);
        }

        // POST: principal/avaliacao/reposicao/contagemregressiva/REPO201520001
        [HttpPost]
        public ActionResult ContagemRegressiva(string codAvaliacao)
        {
            AvalAcadReposicao aval = AvalAcadReposicao.ListarPorCodigoAvaliacao(codAvaliacao);
            string tempo = aval.Avaliacao.DtAplicacao.Value.ToLeftTimeString();
            int quantidadeMilissegundo = 0;
            bool flagLiberada = aval.Avaliacao.FlagLiberada && aval.Avaliacao.DtTermino > DateTime.Now;
            if (tempo != "Agora")
            {
                char tipo = tempo[(tempo.IndexOf(' ')) + 1];
                switch (tipo)
                {
                    case 'd':
                        quantidadeMilissegundo = 0;
                        break;
                    case 'h':
                        quantidadeMilissegundo = 1 * 60 * 60 * 1000;
                        break;
                    case 'm':
                        quantidadeMilissegundo = 1 * 60 * 1000;
                        break;
                    case 's':
                        quantidadeMilissegundo = 1 * 1000;
                        break;
                    default:
                        break;
                }
            }
            return Json(new { Tempo = tempo, Intervalo = quantidadeMilissegundo, FlagLiberada = flagLiberada });
        }

        // POST: principal/avaliacao/reposicao/alternarliberar/REPO201520001
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.PROFESSOR })]
        public ActionResult AlternarLiberar(string codAvaliacao)
        {
            if (!String.IsNullOrWhiteSpace(codAvaliacao))
            {
                return Json(Avaliacao.AlternarFlagLiberada(codAvaliacao));
            }
            return Json(false);
        }

        // GET: principal/avaliacao/reposicao/imprimir/REPO201520001
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.PROFESSOR })]
        public ActionResult Imprimir(string codigo)
        {
            if (!String.IsNullOrWhiteSpace(codigo) && !Sistema.AvaliacaoUsuario.ContainsKey(codigo))
            {
                AvalAcadReposicao aval = AvalAcadReposicao.ListarPorCodigoAvaliacao(codigo);
                if (aval != null)
                {
                    if (Sessao.UsuarioMatricula == aval.Professor.MatrProfessor)
                    {
                        return View(aval);
                    }
                }
            }
            return RedirectToAction("Index");
        }

        // GET: principal/avaliacao/reposicao/realizar/REPO201520001
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.ESTUDANTE })]
        public ActionResult Realizar(string codigo)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                AvalAcadReposicao aval = AvalAcadReposicao.ListarPorCodigoAvaliacao(codigo);
                if (aval.Avaliacao.FlagPendente
                    && aval.Avaliacao.FlagLiberada
                    && aval.Avaliacao.FlagAgora
                    && aval.Alunos.FirstOrDefault(a => a.MatrAluno == Sessao.UsuarioMatricula) != null)
                {
                    Sessao.Inserir("RealizandoAvaliacao", true);
                    return View(aval);
                }
            }
            return RedirectToAction("Agendada", new { codigo = codigo });
        }

        // POST: principal/avaliacao/reposicao/desistir/REPO201520001
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.ESTUDANTE })]
        public void Desistir(string codigo)
        {
            int codPessoaFisica = Usuario.ObterPessoaFisica(Sessao.UsuarioMatricula);
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                AvalAcadReposicao aval = AvalAcadReposicao.ListarPorCodigoAvaliacao(codigo);
                if (aval.Alunos.SingleOrDefault(a => a.MatrAluno == Sessao.UsuarioMatricula) != null && aval.Avaliacao.AvalPessoaResultado.SingleOrDefault(a => a.CodPessoaFisica == codPessoaFisica) == null)
                {
                    AvalPessoaResultado avalPessoaResultado = new AvalPessoaResultado();
                    avalPessoaResultado.CodPessoaFisica = codPessoaFisica;
                    avalPessoaResultado.HoraTermino = DateTime.Now;
                    avalPessoaResultado.QteAcertoObj = 0;
                    avalPessoaResultado.Nota = 0;

                    foreach (AvaliacaoTema avaliacaoTema in aval.Avaliacao.AvaliacaoTema)
                    {
                        foreach (AvalTemaQuestao avalTemaQuestao in avaliacaoTema.AvalTemaQuestao)
                        {
                            AvalQuesPessoaResposta avalQuesPessoaResposta = new AvalQuesPessoaResposta();
                            avalQuesPessoaResposta.CodPessoaFisica = codPessoaFisica;
                            if (avalTemaQuestao.QuestaoTema.Questao.CodTipoQuestao == TipoQuestao.OBJETIVA) avalQuesPessoaResposta.RespAlternativa = -1;
                            avalQuesPessoaResposta.RespNota = 0;
                            avalTemaQuestao.AvalQuesPessoaResposta.Add(avalQuesPessoaResposta);
                        }
                    }

                    aval.Avaliacao.AvalPessoaResultado.Add(avalPessoaResultado);

                    Repositorio.GetInstance().SaveChanges();
                    Sessao.Inserir("RealizandoAvaliacao", false);
                }
            }
        }

        // POST: principal/avaliacao/reposicao/printar/REPO201520001
        [HttpPost]
        public ActionResult Printar(string codAvaliacao, string imageData)
        {
            if (Sessao.UsuarioCategoriaCodigo == Categoria.ESTUDANTE)
            {
                Sistema.TempDataUrlImage[codAvaliacao] = imageData;
                return Json(true);
            }
            else if (Sessao.UsuarioCategoriaCodigo == Categoria.PROFESSOR)
            {
                string temp = Sistema.TempDataUrlImage[codAvaliacao];
                Sistema.TempDataUrlImage[codAvaliacao] = String.Empty;
                return Json(temp);
            }
            return Json(false);
        }

        // GET: principal/avaliacao/reposicao/acompanhar/REPO201520001
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.PROFESSOR })]
        public ActionResult Acompanhar(string codigo)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                AvalAcadReposicao aval = AvalAcadReposicao.ListarPorCodigoAvaliacao(codigo);
                if (aval != null && aval.Professor.MatrProfessor == Sessao.UsuarioMatricula && aval.Avaliacao.FlagAgendada && aval.Avaliacao.FlagAgora)
                {
                    return View(aval);
                }
            }
            return RedirectToAction("Agendada", new { codigo = codigo });
        }

        // POST: principal/avaliacao/reposicao/resultado/REPO201520001
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.ESTUDANTE })]
        public ActionResult Resultado(string codigo, FormCollection form)
        {
            int codPessoaFisica = Usuario.ObterPessoaFisica(Sessao.UsuarioMatricula);
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                AvalAcadReposicao aval = AvalAcadReposicao.ListarPorCodigoAvaliacao(codigo);
                if (aval.Alunos.SingleOrDefault(a => a.MatrAluno == Sessao.UsuarioMatricula) != null && aval.Avaliacao.AvalPessoaResultado.SingleOrDefault(a => a.CodPessoaFisica == codPessoaFisica) == null)
                {
                    AvalPessoaResultado avalPessoaResultado = new AvalPessoaResultado();
                    avalPessoaResultado.CodPessoaFisica = codPessoaFisica;
                    avalPessoaResultado.HoraTermino = DateTime.Now;
                    avalPessoaResultado.QteAcertoObj = 0;

                    double quantidadeObjetiva = 0;

                    foreach (AvaliacaoTema avaliacaoTema in aval.Avaliacao.AvaliacaoTema)
                    {
                        foreach (AvalTemaQuestao avalTemaQuestao in avaliacaoTema.AvalTemaQuestao)
                        {
                            AvalQuesPessoaResposta avalQuesPessoaResposta = new AvalQuesPessoaResposta();
                            avalQuesPessoaResposta.CodPessoaFisica = codPessoaFisica;
                            if (avalTemaQuestao.QuestaoTema.Questao.CodTipoQuestao == TipoQuestao.OBJETIVA)
                            {
                                quantidadeObjetiva++;
                                int respAlternativa = -1;
                                string strRespAlternativa = form["rdoResposta" + avalTemaQuestao.QuestaoTema.Questao.CodQuestao];
                                if (!String.IsNullOrWhiteSpace(strRespAlternativa))
                                {
                                    int.TryParse(strRespAlternativa, out respAlternativa);
                                }
                                avalQuesPessoaResposta.RespAlternativa = respAlternativa;
                                if (avalTemaQuestao.QuestaoTema.Questao.Alternativa.First(q => q.FlagGabarito).CodOrdem == avalQuesPessoaResposta.RespAlternativa)
                                {
                                    avalQuesPessoaResposta.RespNota = 10;
                                    avalPessoaResultado.QteAcertoObj++;
                                }
                                else
                                {
                                    avalQuesPessoaResposta.RespNota = 0;
                                }
                            }
                            else
                            {
                                avalQuesPessoaResposta.RespDiscursiva = form["txtResposta" + avalTemaQuestao.QuestaoTema.Questao.CodQuestao].Trim();
                            }
                            avalQuesPessoaResposta.RespComentario = !String.IsNullOrWhiteSpace(form["txtComentario" + avalTemaQuestao.QuestaoTema.Questao.CodQuestao]) ? form["txtComentario" + avalTemaQuestao.QuestaoTema.Questao.CodQuestao].Trim() : null;
                            avalTemaQuestao.AvalQuesPessoaResposta.Add(avalQuesPessoaResposta);
                        }

                    }

                    IEnumerable<AvalQuesPessoaResposta> lstAvalQuesPessoaResposta = aval.Avaliacao.PessoaResposta.Where(r => r.CodPessoaFisica == codPessoaFisica);

                    avalPessoaResultado.Nota = lstAvalQuesPessoaResposta.Average(r => r.RespNota);
                    aval.Avaliacao.AvalPessoaResultado.Add(avalPessoaResultado);

                    Repositorio.GetInstance().SaveChanges();

                    AvaliacaoResultadoViewModel model = new AvaliacaoResultadoViewModel();
                    model.Avaliacao = aval.Avaliacao;
                    model.Porcentagem = (avalPessoaResultado.QteAcertoObj.Value / quantidadeObjetiva) * 100;

                    Sessao.Inserir("RealizandoAvaliacao", false);

                    return View(model);
                }
                return RedirectToAction("Detalhe", new { codigo = aval.Avaliacao.CodAvaliacao });
            }
            return RedirectToAction("Index");
        }

        // GET: historico/avaliacao/reposicao/corrigir/REPO201520001
        [HttpGet]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.PROFESSOR })]
        public ActionResult Corrigir(string codigo)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                AvalAcadReposicao aval = AvalAcadReposicao.ListarPorCodigoAvaliacao(codigo);
                if (aval != null && aval.Avaliacao.FlagCorrecaoPendente && aval.Professor.MatrProfessor == Sessao.UsuarioMatricula)
                {
                    return View(aval);
                }
            }
            return RedirectToAction("Index");
        }

        // POST: principal/avaliacao/reposicao/carregaralunos/REPO201520001
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.PROFESSOR })]
        public ActionResult CarregarAlunos(string codigo)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                AvalAcadReposicao aval = AvalAcadReposicao.ListarPorCodigoAvaliacao(codigo);
                var result = from alunos in aval.AlunosRealizaram
                             select new
                             {
                                 Matricula = alunos.MatrAluno,
                                 Nome = alunos.Usuario.PessoaFisica.Nome,
                                 FlagCorrecaoPendente = aval.Avaliacao.AvalPessoaResultado.Single(r => r.CodPessoaFisica == alunos.Usuario.CodPessoaFisica).FlagParcial
                             };
                return Json(result);
            }
            return Json(null);
        }

        // POST: principal/avaliacao/reposicao/carregarquestoesdiscursivas/REPO201520001
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.PROFESSOR })]
        public ActionResult CarregarQuestoesDiscursivas(string codigo)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                AvalAcadReposicao aval = AvalAcadReposicao.ListarPorCodigoAvaliacao(codigo);
                var result = from questao in aval.Avaliacao.Questao
                             where questao.CodTipoQuestao == TipoQuestao.DISCURSIVA
                             orderby questao.CodQuestao
                             select new
                             {
                                 codQuestao = questao.CodQuestao,
                                 questaoEnunciado = questao.Enunciado,
                                 questaoChaveResposta = questao.ChaveDeResposta,
                                 flagCorrecaoPendente = aval.Avaliacao.PessoaResposta.Where(r => r.CodQuestao == questao.CodQuestao && !r.RespNota.HasValue).Count() > 0
                             };
                return Json(result);
            }
            return Json(null);
        }

        // POST: principal/avaliacao/reposicao/carregarrespostasdiscursivas/REPO201520001
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.PROFESSOR })]
        public ActionResult CarregarRespostasDiscursivas(string codigo, string matrAluno)
        {
            if (!StringExt.IsNullOrWhiteSpace(codigo,matrAluno))
            {
                AvalAcadReposicao aval = AvalAcadReposicao.ListarPorCodigoAvaliacao(codigo);
                Aluno aluno = Aluno.ListarPorMatricula(matrAluno);
                int codPessoaFisica = aluno.Usuario.PessoaFisica.CodPessoa;

                var retorno = from alunoResposta in aval.Avaliacao.PessoaResposta
                             orderby alunoResposta.CodQuestao
                             where alunoResposta.CodPessoaFisica == codPessoaFisica
                                && alunoResposta.AvalTemaQuestao.QuestaoTema.Questao.CodTipoQuestao == TipoQuestao.DISCURSIVA
                             select new
                             {
                                 codQuestao = alunoResposta.CodQuestao,
                                 questaoEnunciado = alunoResposta.AvalTemaQuestao.QuestaoTema.Questao.Enunciado,
                                 questaoChaveResposta = alunoResposta.AvalTemaQuestao.QuestaoTema.Questao.ChaveDeResposta,
                                 alunoResposta = alunoResposta.RespDiscursiva,
                                 notaObtida = alunoResposta.RespNota.HasValue ? alunoResposta.RespNota.Value.ToValueHtml() : "",
                                 correcaoComentario = alunoResposta.ProfObservacao != null ? alunoResposta.ProfObservacao : "",
                                 flagCorrigida = alunoResposta.RespNota != null ? true : false
                             };
                return Json(retorno);
            }
            return Json(null);
        }

        // POST: principal/avaliacao/reposicao/carregarrespostasporquestao/REPO201520001
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.PROFESSOR })]
        public ActionResult CarregarRespostasPorQuestao(string codigo, string codQuestao)
        {
            if (!StringExt.IsNullOrWhiteSpace(codigo,codQuestao))
            {
                AvalAcadReposicao aval = AvalAcadReposicao.ListarPorCodigoAvaliacao(codigo);
                int codQuestaoTemp = int.Parse(codQuestao);

                var retorno = from questao in aval.Avaliacao.PessoaResposta
                             orderby questao.PessoaFisica.Nome
                             where questao.CodQuestao == codQuestaoTemp
                                && questao.AvalTemaQuestao.QuestaoTema.Questao.CodTipoQuestao == TipoQuestao.DISCURSIVA
                             select new
                             {
                                 alunoMatricula = aval.AlunosRealizaram.FirstOrDefault(a => a.Usuario.CodPessoaFisica == questao.CodPessoaFisica).Usuario.Matricula,
                                 alunoNome = questao.PessoaFisica.Nome,
                                 codQuestao = questao.CodQuestao,
                                 questaoEnunciado = questao.AvalTemaQuestao.QuestaoTema.Questao.Enunciado,
                                 questaoChaveResposta = questao.AvalTemaQuestao.QuestaoTema.Questao.ChaveDeResposta,
                                 alunoResposta = questao.RespDiscursiva,
                                 notaObtida = questao.RespNota.HasValue ? questao.RespNota.Value.ToValueHtml() : "",
                                 correcaoComentario = questao.ProfObservacao != null ? questao.ProfObservacao : "",
                                 flagCorrigida = questao.RespNota != null ? true : false
                             };
                return Json(retorno);
            }
            return Json(null);
        }

        // POST: principal/avaliacao/reposicao/corrigirquestaoaluno/REPO201520001
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.PROFESSOR })]
        public ActionResult CorrigirQuestaoAluno(string codigo, string matrAluno, int codQuestao, string notaObtida, string profObservacao)
        {
            if (!StringExt.IsNullOrWhiteSpace(codigo, matrAluno) && codQuestao > 0)
            {
                double nota = Double.Parse(notaObtida.Replace('.', ','));

                bool retorno = AvalAcadReposicao.CorrigirQuestaoAluno(codigo, matrAluno, codQuestao, nota, profObservacao);

                return Json(retorno);
            }
            return Json(false);
        }

        // GET: historico/avaliacao/reposicao/detalhe/REPO201520001
        public ActionResult Detalhe(string codigo)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                AvalAcadReposicao aval = AvalAcadReposicao.ListarPorCodigoAvaliacao(codigo);
                if (aval != null)
                {
                    return View(aval);
                }
            }

            return RedirectToAction("Index");
        }

        // POST: principal/avaliacao/reposicao/detalheindividual/REPO201520001
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.PROFESSOR })]
        public ActionResult DetalheIndividual(string codigo, string matricula)
        {
            if (!StringExt.IsNullOrWhiteSpace(codigo, matricula))
            {
                AvalAcadReposicao aval = AvalAcadReposicao.ListarPorCodigoAvaliacao(codigo);
                if (aval != null)
                {
                    int codPessoaFisica = Usuario.ObterPessoaFisica(matricula);
                    AvalPessoaResultado model = aval.Avaliacao.AvalPessoaResultado.FirstOrDefault(r => r.CodPessoaFisica == codPessoaFisica);
                    if (model != null)
                    {
                        return PartialView("_Individual", model);
                    }
                }
            }
            return null;
        }
    }
}
