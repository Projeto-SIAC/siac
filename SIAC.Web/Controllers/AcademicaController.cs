using SIAC.Helpers;
using SIAC.Models;
using SIAC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SIAC.Controllers
{
    [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.ESTUDANTE, Categoria.PROFESSOR })]
    public class AcademicaController : Controller
    {
        public List<AvalAcademica> Academicas
        {
            get
            {
                if (Helpers.Sessao.UsuarioCategoriaCodigo == Categoria.PROFESSOR)
                {
                    int codProfessor = Professor.ListarPorMatricula(Helpers.Sessao.UsuarioMatricula).CodProfessor;
                    return AvalAcademica.ListarPorProfessor(codProfessor);
                }
                else
                {
                    int codAluno = Aluno.ListarPorMatricula(Helpers.Sessao.UsuarioMatricula).CodAluno;
                    return AvalAcademica.ListarPorAluno(codAluno);
                }
            }
        }

        // POST: academica/listar
        [HttpPost]
        public ActionResult Listar(int? pagina, string pesquisa, string ordenar, string[] categorias, string disciplina)
        {
            int quantidade = 12;
            List<AvalAcademica> academicas = Academicas;

            pagina = pagina ?? 1;
            if (!String.IsNullOrWhiteSpace(pesquisa))
            {
                academicas = academicas.Where(a => a.Avaliacao.CodAvaliacao.ToLower().Contains(pesquisa.ToLower())).ToList();
            }

            if (!String.IsNullOrWhiteSpace(disciplina))
            {
                academicas = academicas.Where(a => a.CodDisciplina == int.Parse(disciplina)).ToList();
            }

            if (categorias != null)
            {
                if (categorias.Contains("agendada") && !categorias.Contains("arquivo") && !categorias.Contains("realizada"))
                {
                    academicas = academicas.Where(a => a.Avaliacao.FlagAgendada).ToList();
                }
                else if (!categorias.Contains("agendada") && categorias.Contains("arquivo") && !categorias.Contains("realizada"))
                {
                    academicas = academicas.Where(a => a.Avaliacao.FlagArquivo).ToList();
                }
                else if (!categorias.Contains("agendada") && !categorias.Contains("arquivo") && categorias.Contains("realizada"))
                {
                    academicas = academicas.Where(a => a.Avaliacao.FlagRealizada).ToList();
                }
                else if (!categorias.Contains("agendada") && categorias.Contains("arquivo") && categorias.Contains("realizada"))
                {
                    academicas = academicas.Where(a => a.Avaliacao.FlagRealizada || a.Avaliacao.FlagArquivo).ToList();
                }
                else if (categorias.Contains("agendada") && !categorias.Contains("arquivo") && categorias.Contains("realizada"))
                {
                    academicas = academicas.Where(a => a.Avaliacao.FlagRealizada || a.Avaliacao.FlagAgendada).ToList();
                }
                else if (categorias.Contains("agendada") && categorias.Contains("arquivo") && !categorias.Contains("realizada"))
                {
                    academicas = academicas.Where(a => a.Avaliacao.FlagArquivo || a.Avaliacao.FlagAgendada).ToList();
                }
            }

            switch (ordenar)
            {
                case "data_desc":
                    academicas = academicas.OrderByDescending(a => a.Avaliacao.DtCadastro).ToList();
                    break;
                case "data":
                    academicas = academicas.OrderBy(a => a.Avaliacao.DtCadastro).ToList();
                    break;
                default:
                    academicas = academicas.OrderByDescending(a => a.Avaliacao.DtCadastro).ToList();
                    break;
            }

            return PartialView("_ListaAcademica", academicas.Skip((quantidade * pagina.Value) - quantidade).Take(quantidade).ToList());
        }

        // GET: historico/avaliacao/academica
        [OutputCache(CacheProfile = "PorUsuario")]
        public ActionResult Index()
        {
            if (Request.Url.ToString().ToLower().Contains("principal"))
            {
                return Redirect("~/historico/avaliacao/academica");
            }

            AvaliacaoIndexViewModel model = new AvaliacaoIndexViewModel();
            model.Disciplinas = Academicas.Select(a => a.Disciplina).Distinct().ToList();
            return View(model);
        }

        // GET: principal/avaliacao/academica/gerar
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.PROFESSOR })]
        public ActionResult Gerar()
        {
            AvaliacaoGerarViewModel model = new AvaliacaoGerarViewModel();
            model.Disciplinas = Disciplina.ListarPorProfessor(Helpers.Sessao.UsuarioMatricula);
            model.Dificuldades = Dificuldade.ListarOrdenadamente();
            model.Termo = Parametro.Obter().NotaUsoAcademica;

            return View(model);
        }

        // POST: principal/avaliacao/academica/confirmar
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.PROFESSOR })]
        public ActionResult Confirmar(FormCollection formCollection)
        {
            if (Helpers.Sessao.UsuarioCategoriaCodigo != Categoria.PROFESSOR)
            {
                if (Session["UrlReferrer"] != null)
                {
                    return Redirect(Session["UrlReferrer"].ToString());
                }
                else return RedirectToAction("Index", "Principal");
            }
            AvalAcademica acad = new AvalAcademica();
            if (formCollection.HasKeys())
            {
                DateTime hoje = DateTime.Now;

                /* Chave */
                acad.Avaliacao = new Avaliacao();
                acad.Avaliacao.TipoAvaliacao = TipoAvaliacao.ListarPorCodigo(TipoAvaliacao.ACADEMICA);
                acad.Avaliacao.Ano = hoje.Year;
                acad.Avaliacao.Semestre = hoje.SemestreAtual();
                acad.Avaliacao.NumIdentificador = Avaliacao.ObterNumIdentificador(TipoAvaliacao.ACADEMICA);
                acad.Avaliacao.DtCadastro = hoje;

                /* Professor */
                string matricula = Helpers.Sessao.UsuarioMatricula;
                acad.Professor = Professor.ListarPorMatricula(matricula);

                /* Dados */
                int codDisciplina = int.Parse(formCollection["ddlDisciplina"]);

                acad.CodDisciplina = codDisciplina;

                /* Dificuldade */
                int codDificuldade = int.Parse(formCollection["ddlDificuldade"]);

                /* Quantidade */
                int quantidadeObjetiva = 0;
                int quantidadeDiscursiva = 0;

                if (formCollection["ddlTipo"] == "3")
                {
                    int.TryParse(formCollection["txtQteObjetiva"], out quantidadeObjetiva);
                    int.TryParse(formCollection["txtQteDiscursiva"], out quantidadeDiscursiva);
                }
                else if (formCollection["ddlTipo"] == "2")
                {
                    int.TryParse(formCollection["txtQteDiscursiva"], out quantidadeDiscursiva);
                }
                else if (formCollection["ddlTipo"] == "1")
                {
                    int.TryParse(formCollection["txtQteObjetiva"], out quantidadeObjetiva);
                }

                /* Temas */
                string[] temaCodigos = formCollection["ddlTemas"].Split(',');

                /* Questões */
                List<QuestaoTema> lstQuestoes = new List<QuestaoTema>();

                if (quantidadeObjetiva > 0)
                {
                    lstQuestoes.AddRange(Questao.ListarPorDisciplina(codDisciplina, temaCodigos, codDificuldade, TipoQuestao.OBJETIVA, quantidadeObjetiva));
                }
                if (quantidadeDiscursiva > 0)
                {
                    lstQuestoes.AddRange(Questao.ListarPorDisciplina(codDisciplina, temaCodigos, codDificuldade, TipoQuestao.DISCURSIVA, quantidadeDiscursiva));
                }

                foreach (string temaCodigo in temaCodigos)
                {
                    AvaliacaoTema avalTema = new AvaliacaoTema();
                    avalTema.Tema = Tema.ListarPorCodigo(codDisciplina, int.Parse(temaCodigo));
                    foreach (QuestaoTema questaoTema in lstQuestoes.Where(q => q.CodTema == int.Parse(temaCodigo)))
                    {
                        AvalTemaQuestao avalTemaQuestao = new AvalTemaQuestao();
                        avalTemaQuestao.QuestaoTema = questaoTema;
                        avalTema.AvalTemaQuestao.Add(avalTemaQuestao);
                    }
                    acad.Avaliacao.AvaliacaoTema.Add(avalTema);
                }

                AvalAcademica.Inserir(acad);
            }

            return View(acad);
        }

        // GET: principal/avaliacao/academica/agendar/ACAD2015100002
        [HttpGet]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.PROFESSOR })]
        public ActionResult Agendar(string codigo)
        {
            if (String.IsNullOrWhiteSpace(codigo) || Sistema.AvaliacaoUsuario.ContainsKey(codigo))
            {
                return RedirectToAction("Index");
            }
            AvalAcademica acad = AvalAcademica.ListarPorCodigoAvaliacao(codigo);

            string matricula = Helpers.Sessao.UsuarioMatricula;
            Professor professor = Professor.ListarPorMatricula(matricula);
            if (acad.CodProfessor == professor.CodProfessor)
            {
                AvaliacaoAgendarViewModel model = new AvaliacaoAgendarViewModel();

                model.Avaliacao = acad.Avaliacao;
                model.Turmas = professor.TurmaDiscProfHorario.Select(d => d.Turma).Distinct().OrderBy(t => t.Curso.Descricao).ToList();
                model.Salas = Sala.ListarOrdenadamente();

                return View(model);
            }
            return RedirectToAction("Index");
        }

        // POST: principal/avaliacao/academica/agendar/ACAD2015100002
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.PROFESSOR })]
        public ActionResult Agendar(string codigo, FormCollection form)
        {
            if (Helpers.Sessao.UsuarioCategoriaCodigo != Categoria.PROFESSOR)
            {
                if (Session["UrlReferrer"] != null)
                {
                    return Redirect(Session["UrlReferrer"].ToString());
                }
                else return RedirectToAction("Index", "Principal");
            }
            string codTurma = form["ddlTurma"];
            string strCodSala = form["ddlSala"];
            string data = form["txtData"];
            string horaInicio = form["txtHoraInicio"];
            string horaTermino = form["txtHoraTermino"];
            if (!Helpers.StringExt.IsNullOrWhiteSpace(codTurma, strCodSala, data,horaInicio,horaTermino))
            {
                AvalAcademica acad = AvalAcademica.ListarPorCodigoAvaliacao(codigo);

                string matricula = Helpers.Sessao.UsuarioMatricula;
                Professor professor = Professor.ListarPorMatricula(matricula);

                if (acad.CodProfessor == professor.CodProfessor)
                {
                    // Turma
                    Turma turma = Turma.ListarPorCodigo(codTurma);
                    if (turma != null)
                    {
                        acad.Turma = turma;
                    }

                    // Sala
                    int codSala;
                    int.TryParse(strCodSala, out codSala);
                    Sala sala = Sala.ListarPorCodigo(codSala);
                    if (sala != null)
                    {
                        acad.Sala = sala;
                    }

                    // Data de Aplicacao
                    DateTime dtAplicacao = DateTime.Parse(data + " " + horaInicio);
                    DateTime dtAplicacaoTermino = DateTime.Parse(data + " " + horaInicio);

                    if (dtAplicacao.IsFuture() && dtAplicacaoTermino.IsFuture() && dtAplicacaoTermino > dtAplicacao)
                    {
                        acad.Avaliacao.DtAplicacao = dtAplicacao;
                        acad.Avaliacao.Duracao = Convert.ToInt32((dtAplicacaoTermino - acad.Avaliacao.DtAplicacao.Value).TotalMinutes);
                    }

                    acad.Avaliacao.FlagLiberada = false;

                    Repositorio.GetInstance().SaveChanges();
                }
            }

            return RedirectToAction("Agendada", new { codigo = codigo });
        }

        // GET: avaliacao/academica/detalhe/ACAD201520001
        public ActionResult Detalhe(string codigo)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                AvalAcademica acad = AvalAcademica.ListarPorCodigoAvaliacao(codigo);
                if (acad != null)
                {
                    return View(acad);
                }
            }

            return RedirectToAction("Index");
        }

        // GET: avaliacao/academica/configurar/ACAD201520001
        [HttpGet]
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
                AvalAcademica acad = AvalAcademica.ListarPorCodigoAvaliacao(codigo);
                if (acad != null && acad.Avaliacao.AvalPessoaResultado.Count == 0)
                {
                    string matricula = Helpers.Sessao.UsuarioMatricula;
                    Professor professor = Professor.ListarPorMatricula(matricula);
                    if (professor != null)
                    {
                        if (professor.CodProfessor == acad.CodProfessor)
                        {
                            return View(acad);
                        }
                    }
                }
            }
            return RedirectToAction("Index");
        }

        // GET: avaliacao/academica/imprimir/ACAD201520001
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.PROFESSOR })]
        public ActionResult Imprimir(string codigo)
        {
            if (!String.IsNullOrWhiteSpace(codigo) && !Sistema.AvaliacaoUsuario.ContainsKey(codigo))
            {
                AvalAcademica acad = AvalAcademica.ListarPorCodigoAvaliacao(codigo);
                if (acad != null)
                {
                    string matricula = Helpers.Sessao.UsuarioMatricula;
                    Professor professor = Professor.ListarPorMatricula(matricula);
                    if (professor.CodProfessor == acad.CodProfessor)
                    {
                        return View(acad);
                    }
                }
            }
            return RedirectToAction("Index");
        }

        // GET: avaliacao/academica/agendada/ACAD201520001
        public ActionResult Agendada(string codigo)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                Usuario usuario = Usuario.ListarPorMatricula(Helpers.Sessao.UsuarioMatricula);
                AvalAcademica acad = AvalAcademica.ListarAgendadaPorUsuario(usuario).FirstOrDefault(a=>a.Avaliacao.CodAvaliacao.ToLower() == codigo.ToLower());
                if (acad != null)
                {
                    return View(acad);
                }
            }
            return RedirectToAction("Detalhe", new { codigo = codigo });
        }

        // POST: avaliacao/academica/trocar/ACAD201520001
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.PROFESSOR })]
        public ActionResult TrocarQuestao(string codigoAvaliacao, int tipo, int indice, int codQuestao)
        {
            List<AvalTemaQuestao> antigas = (List<AvalTemaQuestao>)TempData["listaQuestoesAntigas"];
            List<AvalTemaQuestao> novas = (List<AvalTemaQuestao>)TempData["listaQuestoesNovas"];
            List<QuestaoTema> questoesTrocaObjetiva = (List<QuestaoTema>)TempData["listaQuestoesPossiveisObj"];
            List<QuestaoTema> questoesTrocaDiscursiva = (List<QuestaoTema>)TempData["listaQuestoesPossiveisDisc"];
            List<int> indices = (List<int>)TempData["listaQuestoesIndices"];
            List<int> recentes = (List<int>)TempData["listaQuestoesRecentes"];

            TempData.Keep();
            Random r = new Random();

            if (!String.IsNullOrWhiteSpace(codigoAvaliacao))
            {
                AvalAcademica acad = AvalAcademica.ListarPorCodigoAvaliacao(codigoAvaliacao);
                if (acad != null)
                {
                    List<QuestaoTema> avalQuestTema = acad.Avaliacao.QuestaoTema;

                    QuestaoTema questao = null;

                    if (tipo == TipoQuestao.OBJETIVA)
                    {
                        if (questoesTrocaObjetiva.Count <= 0)
                        {
                            TempData["listaQuestoesPossiveisObj"] = Questao.ObterNovasQuestoes(avalQuestTema, tipo);
                            questoesTrocaObjetiva = (List<QuestaoTema>)TempData["listaQuestoesPossiveisObj"];
                        }

                        int random = r.Next(0, questoesTrocaObjetiva.Count);
                        questao = questoesTrocaObjetiva.ElementAtOrDefault(random);
                    }
                    else if (tipo == TipoQuestao.DISCURSIVA)
                    {
                        if (questoesTrocaDiscursiva.Count <= 0)
                        {
                            TempData["listaQuestoesPossiveisDisc"] = Questao.ObterNovasQuestoes(avalQuestTema, tipo);
                            questoesTrocaDiscursiva = (List<QuestaoTema>)TempData["listaQuestoesPossiveisDisc"];
                        }

                        int random = r.Next(0, questoesTrocaDiscursiva.Count);
                        questao = questoesTrocaDiscursiva.ElementAtOrDefault(random);
                    }

                    if (questao != null)
                    {
                        if (!indices.Contains(indice))
                        {
                            AvalTemaQuestao aqtAntiga = (from atq in Repositorio.GetInstance().AvalTemaQuestao
                                                         where atq.Ano == acad.Ano
                                                         && atq.Semestre == acad.Semestre
                                                         && atq.CodTipoAvaliacao == acad.CodTipoAvaliacao
                                                         && atq.NumIdentificador == acad.NumIdentificador
                                                         && atq.CodQuestao == codQuestao
                                                         select atq).FirstOrDefault();
                            antigas.Add(aqtAntiga);
                            indices.Add(indice);
                        }

                        int index = indices.IndexOf(indice);

                        AvalTemaQuestao atqNova = new AvalTemaQuestao();
                        atqNova.Ano = acad.Avaliacao.Ano;
                        atqNova.Semestre = acad.Avaliacao.Semestre;
                        atqNova.CodTipoAvaliacao = acad.Avaliacao.CodTipoAvaliacao;
                        atqNova.NumIdentificador = acad.Avaliacao.NumIdentificador;
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

        // POST: avaliacao/academica/salvar/ACAD201520001
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.PROFESSOR })]
        public ActionResult Salvar(string codigo)
        {
            List<AvalTemaQuestao> antigas = (List<AvalTemaQuestao>)TempData["listaQuestoesAntigas"];
            List<AvalTemaQuestao> novas = (List<AvalTemaQuestao>)TempData["listaQuestoesNovas"];

            if (antigas.Count != 0 && novas.Count != 0)
            {
                dbSIACEntities contexto = Repositorio.GetInstance();
                for (int i = 0; i < antigas.Count && i < novas.Count; i++)
                {
                    contexto.AvalTemaQuestao.Remove(antigas.ElementAtOrDefault(i));
                    contexto.AvalTemaQuestao.Add(novas.ElementAtOrDefault(i));
                }
                contexto.SaveChanges();
            }
            TempData.Clear();

            return RedirectToAction("Detalhe", new { codigo = codigo });
        }
        
        // POST: avaliacao/academica/desfazer/ACAD201520001
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.PROFESSOR })]
        public ActionResult Desfazer(string codigoAvaliacao, int tipoQuestao, int indice, int codQuestao)
        {
            List<AvalTemaQuestao> antigas = (List<AvalTemaQuestao>)TempData["listaQuestoesAntigas"];
            List<AvalTemaQuestao> novas = (List<AvalTemaQuestao>)TempData["listaQuestoesNovas"];
            List<QuestaoTema> questoesTrocaObj = (List<QuestaoTema>)TempData["listaQuestoesPossiveisObj"];
            List<QuestaoTema> questoesTrocaDisc = (List<QuestaoTema>)TempData["listaQuestoesPossiveisDisc"];
            List<int> indices = (List<int>)TempData["listaQuestoesIndices"];
            List<int> recentes = (List<int>)TempData["listaQuestoesRecentes"];

            TempData.Keep();

            if (!String.IsNullOrWhiteSpace(codigoAvaliacao))
            {
                int codQuestaoRecente = recentes[indices.IndexOf(indice)];

                QuestaoTema questao = null;

                if (tipoQuestao == TipoQuestao.OBJETIVA)
                {
                    questao = questoesTrocaObj.FirstOrDefault(qt => qt.CodQuestao == codQuestaoRecente);
                    if (questao == null)
                    {
                        questao = antigas[indices.IndexOf(indice)].QuestaoTema;
                    }
                }
                else if (tipoQuestao == TipoQuestao.DISCURSIVA)
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

        // POST: contagemregressiva
        [HttpPost]
        public ActionResult ContagemRegressiva(string codAvaliacao)
        {
            AvalAcademica avalAcad = AvalAcademica.ListarPorCodigoAvaliacao(codAvaliacao);
            string tempo = avalAcad.Avaliacao.DtAplicacao.Value.ToLeftTimeString();
            int quantidadeMilissegundo = 0;
            bool flagLiberada = avalAcad.Avaliacao.FlagLiberada && avalAcad.Avaliacao.DtTermino > DateTime.Now;
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

        // POST: avaliacao/academica/liberar/ACAD201520001
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

        // GET: avaliacao/academica/acompanhar/ACAD201520007
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.PROFESSOR })]
        public ActionResult Acompanhar(string codigo)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                if (Helpers.Sessao.UsuarioCategoriaCodigo == Categoria.PROFESSOR)
                {
                    AvalAcademica acad = AvalAcademica.ListarPorCodigoAvaliacao(codigo);
                    if (acad != null && acad.Avaliacao.FlagAgendada && acad.Avaliacao.FlagAgora)
                    {
                        return View(acad);
                    }
                }
            }
            return RedirectToAction("Agendada", new { codigo = codigo });
        }

        // POST: academica/printar/ACAD201520007/20150123
        [HttpPost]
        public ActionResult Printar(string codAvaliacao, string imageData)
        {
            if (Helpers.Sessao.UsuarioCategoriaCodigo == Categoria.ESTUDANTE)
            {
                Sistema.TempDataUrlImage[codAvaliacao] = imageData;
                return Json(true);
            }
            else if (Helpers.Sessao.UsuarioCategoriaCodigo == Categoria.PROFESSOR)
            {
                string temp = Sistema.TempDataUrlImage[codAvaliacao];
                Sistema.TempDataUrlImage[codAvaliacao] = String.Empty;
                return Json(temp);
            }
            return Json(false);
        }

        // GET: academica/realizar
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.ESTUDANTE })]
        public ActionResult Realizar(string codigo)
        {
            if (Helpers.Sessao.UsuarioCategoriaCodigo == Categoria.ESTUDANTE && !String.IsNullOrWhiteSpace(codigo))
            {
                AvalAcademica avalAcad = AvalAcademica.ListarPorCodigoAvaliacao(codigo);
                if (avalAcad.Avaliacao.FlagPendente 
                    && avalAcad.Avaliacao.FlagLiberada 
                    && avalAcad.Avaliacao.FlagAgora
                    && avalAcad.Alunos.FirstOrDefault(a=>a.MatrAluno == Helpers.Sessao.UsuarioMatricula) != null)
                {
                    Helpers.Sessao.Inserir("RealizandoAvaliacao", true);
                    return View(avalAcad);
                }
            }
            return RedirectToAction("Agendada", new { codigo = codigo });
        }

        // POST: academica/resultado/ACAD201520001
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.ESTUDANTE })]
        public ActionResult Resultado(string codigo, FormCollection form)
        {
            int codPessoaFisica = Usuario.ObterPessoaFisica(Helpers.Sessao.UsuarioMatricula);
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                AvalAcademica aval = AvalAcademica.ListarPorCodigoAvaliacao(codigo);
                if (aval.Alunos.SingleOrDefault(a => a.MatrAluno == Helpers.Sessao.UsuarioMatricula) != null && aval.Avaliacao.AvalPessoaResultado.SingleOrDefault(a => a.CodPessoaFisica == codPessoaFisica) == null)
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
                                if (avalTemaQuestao.QuestaoTema.Questao.Alternativa.First(q => q.FlagGabarito.HasValue && q.FlagGabarito.Value).CodOrdem == avalQuesPessoaResposta.RespAlternativa)
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

        // POST: academica/desistir/ACAD201520016
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.ESTUDANTE })]
        public void Desistir(string codigo)
        {
            int codPessoaFisica = Usuario.ObterPessoaFisica(Sessao.UsuarioMatricula);
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                AvalAcademica aval = AvalAcademica.ListarPorCodigoAvaliacao(codigo);
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

        // POST: academica/pendente
        [HttpGet]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.PROFESSOR })]
        public ActionResult Pendente()
        {
            string matricula = Sessao.UsuarioMatricula;
            int codProfessor = Professor.ListarPorMatricula(matricula).CodProfessor;
            return View(AvalAcademica.ListarCorrecaoPendentePorProfessor(codProfessor));
        }

        // GET: avaliacao/academica/corrigir/ACAD201520016
        [HttpGet]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.PROFESSOR })]
        public ActionResult Corrigir(string codigo)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                AvalAcademica acad = AvalAcademica.ListarPorCodigoAvaliacao(codigo);

                if (acad != null && acad.Avaliacao.FlagCorrecaoPendente && acad.Professor.MatrProfessor == Sessao.UsuarioMatricula)
                {
                    return View(acad);
                }
            }
            return RedirectToAction("Index");
        }
        
        // POST: academica/arquivar
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

        // POST: academica/avaliacao/carregaralunos/ACAD201520016
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.PROFESSOR })]
        public ActionResult CarregarAlunos(string codigo)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                AvalAcademica acad = AvalAcademica.ListarPorCodigoAvaliacao(codigo);
                var retorno = from alunos in acad.AlunosRealizaram
                             select new
                             {
                                 Matricula = alunos.MatrAluno,
                                 Nome = alunos.Usuario.PessoaFisica.Nome,
                                 FlagCorrecaoPendente = acad.Avaliacao.AvalPessoaResultado.Single(r => r.CodPessoaFisica == alunos.Usuario.CodPessoaFisica).FlagParcial
                             };
                return Json(retorno);
            }
            return Json(null);
        }

        // POST: academica/avaliacao/carregarquestoesdiscursivas/ACAD201520016
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.PROFESSOR})]
        public ActionResult CarregarQuestoesDiscursivas(string codigo)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                AvalAcademica acad = AvalAcademica.ListarPorCodigoAvaliacao(codigo);
                var retorno = from questao in acad.Avaliacao.Questao
                             where questao.CodTipoQuestao == TipoQuestao.DISCURSIVA
                             orderby questao.CodQuestao
                             select new
                             {
                                 codQuestao = questao.CodQuestao,
                                 questaoEnunciado = questao.Enunciado,
                                 questaoChaveResposta = questao.ChaveDeResposta,
                                 flagCorrecaoPendente = acad.Avaliacao.PessoaResposta.Where(r => r.CodQuestao == questao.CodQuestao && !r.RespNota.HasValue).Count() > 0
                             };
                return Json(retorno);
            }
            return Json(null);
        }

        // POST: academica/avaliacao/carregarrespostasdiscursivas/ACAD201520016/20150123
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.PROFESSOR })]
        public ActionResult CarregarRespostasDiscursivas(string codigo, string matrAluno)
        {
            if (!String.IsNullOrWhiteSpace(codigo) && !String.IsNullOrWhiteSpace(matrAluno))
            {
                AvalAcademica acad = AvalAcademica.ListarPorCodigoAvaliacao(codigo);
                Aluno aluno = Aluno.ListarPorMatricula(matrAluno);
                int codPessoaFisica = aluno.Usuario.PessoaFisica.CodPessoa;

                var retorno = from alunoResposta in acad.Avaliacao.PessoaResposta
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

        // POST: academica/avaliacao/carregarrespostasporquestao/ACAD201520016/2699
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.PROFESSOR})]
        public ActionResult CarregarRespostasPorQuestao(string codigo, string codQuestao)
        {
            if (!StringExt.IsNullOrWhiteSpace(codigo,codQuestao))
            {
                AvalAcademica acad = AvalAcademica.ListarPorCodigoAvaliacao(codigo);
                int codQuestaoTemp = int.Parse(codQuestao);

                var retorno = from questao in acad.Avaliacao.PessoaResposta
                             orderby questao.PessoaFisica.Nome
                             where questao.CodQuestao == codQuestaoTemp
                                && questao.AvalTemaQuestao.QuestaoTema.Questao.CodTipoQuestao == TipoQuestao.DISCURSIVA
                             select new
                             {
                                 alunoMatricula = acad.AlunosRealizaram.FirstOrDefault(a => a.Usuario.CodPessoaFisica == questao.CodPessoaFisica).Usuario.Matricula,
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

        // POST: academica/avaliacao/corrigirquestaoaluno/ACAD201520016/20120065/2699
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.PROFESSOR })]
        public ActionResult CorrigirQuestaoAluno(string codigo, string matrAluno, string codQuestao, string notaObtida, string profObservacao)
        {
            if (!StringExt.IsNullOrWhiteSpace(codigo,matrAluno,codQuestao))
            {
                int codQuesTemp = int.Parse(codQuestao);
                double nota = Double.Parse(notaObtida.Replace('.', ','));

                bool retorno = AvalAcademica.CorrigirQuestaoAluno(codigo, matrAluno, codQuesTemp, nota, profObservacao);

                return Json(retorno);
            }
            return Json(false);
        }

        // POST: academica/avaliacao/detalheindividual/ACAD201520016/20120065
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.PROFESSOR })]
        public ActionResult DetalheIndividual(string codigo, string matricula)
        {
            if (!StringExt.IsNullOrWhiteSpace(codigo, matricula))
            {
                AvalAcademica acad = AvalAcademica.ListarPorCodigoAvaliacao(codigo);
                if (acad != null)
                {
                    int codPessoaFisica = Usuario.ObterPessoaFisica(matricula);
                    AvalPessoaResultado model = acad.Avaliacao.AvalPessoaResultado.SingleOrDefault(r => r.CodPessoaFisica == codPessoaFisica);
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
