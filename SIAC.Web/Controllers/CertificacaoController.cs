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
    [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.ESTUDANTE,Categoria.PROFESSOR,Categoria.COLABORADOR })]
    public class CertificacaoController : Controller
    {
        public List<AvalCertificacao> Certificacoes
        {
            get
            {
                string matricula = Sessao.UsuarioMatricula;
                if (Sessao.UsuarioCategoriaCodigo == Categoria.PROFESSOR)
                {                    
                    int codProfessor = Professor.ListarPorMatricula(matricula).CodProfessor;
                    List<AvalCertificacao> lstProfessor = AvalCertificacao.ListarPorProfessor(codProfessor);
                    int codPessoaFisica = Usuario.ObterPessoaFisica(matricula);
                    return AvalCertificacao.ListarPorPessoa(codPessoaFisica).Union(lstProfessor).Distinct().ToList();
                }
                else
                {
                    int codPessoaFisica = Usuario.ObterPessoaFisica(matricula);
                    return AvalCertificacao.ListarPorPessoa(codPessoaFisica);
                }
            }
        }

        // GET: historico/avaliacao/certificacao
        public ActionResult Index()
        {
            if (Request.Url.ToString().ToLower().Contains("principal"))
            {
                return Redirect("~/historico/avaliacao/certificacao");
            }
            AvaliacaoIndexViewModel model = new ViewModels.AvaliacaoIndexViewModel();
            model.Disciplinas = Certificacoes.Select(a => a.Disciplina).Distinct().ToList();
            return View(model);
        }

        // POST: historico/avaliacao/certificacao/listar
        [HttpPost]
        public ActionResult Listar(int? pagina, string pesquisa, string ordenar, string[] categorias, string disciplina)
        {
            int quantidade = 12;
            List<AvalCertificacao> certificacoes = Certificacoes;
            pagina = pagina ?? 1;
            if (!String.IsNullOrWhiteSpace(pesquisa))
            {
                certificacoes = certificacoes.Where(a => a.Avaliacao.CodAvaliacao.ToLower().Contains(pesquisa.ToLower())).ToList();
            }

            if (!String.IsNullOrWhiteSpace(disciplina))
            {
                certificacoes = certificacoes.Where(a => a.CodDisciplina == int.Parse(disciplina)).ToList();
            }

            if (categorias != null)
            {
                if (categorias.Contains("agendada") && !categorias.Contains("arquivo") && !categorias.Contains("realizada"))
                {
                    certificacoes = certificacoes.Where(a => a.Avaliacao.FlagAgendada).ToList();
                }
                else if (!categorias.Contains("agendada") && categorias.Contains("arquivo") && !categorias.Contains("realizada"))
                {
                    certificacoes = certificacoes.Where(a => a.Avaliacao.FlagArquivo).ToList();
                }
                else if (!categorias.Contains("agendada") && !categorias.Contains("arquivo") && categorias.Contains("realizada"))
                {
                    certificacoes = certificacoes.Where(a => a.Avaliacao.FlagRealizada).ToList();
                }
                else if (!categorias.Contains("agendada") && categorias.Contains("arquivo") && categorias.Contains("realizada"))
                {
                    certificacoes = certificacoes.Where(a => a.Avaliacao.FlagRealizada || a.Avaliacao.FlagArquivo).ToList();
                }
                else if (categorias.Contains("agendada") && !categorias.Contains("arquivo") && categorias.Contains("realizada"))
                {
                    certificacoes = certificacoes.Where(a => a.Avaliacao.FlagRealizada || a.Avaliacao.FlagAgendada).ToList();
                }
                else if (categorias.Contains("agendada") && categorias.Contains("arquivo") && !categorias.Contains("realizada"))
                {
                    certificacoes = certificacoes.Where(a => a.Avaliacao.FlagArquivo || a.Avaliacao.FlagAgendada).ToList();
                }
            }

            switch (ordenar)
            {
                case "data_desc":
                    certificacoes = certificacoes.OrderByDescending(a => a.Avaliacao.DtCadastro).ToList();
                    break;
                case "data":
                    certificacoes = certificacoes.OrderBy(a => a.Avaliacao.DtCadastro).ToList();
                    break;
                default:
                    certificacoes = certificacoes.OrderByDescending(a => a.Avaliacao.DtCadastro).ToList();
                    break;
            }

            return PartialView("_ListaCertificacao", certificacoes.Skip((quantidade * pagina.Value) - quantidade).Take(quantidade).ToList());
        }

        // GET: principal/avaliacao/certificacao/gerar
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.PROFESSOR/*, 3*/ })]
        public ActionResult Gerar()
        {
            AvaliacaoGerarViewModel model = new AvaliacaoGerarViewModel();
            model.Disciplinas = /*Helpers.Sessao.UsuarioCategoriaCodigo == 2 ? */Disciplina.ListarPorProfessor(Sessao.UsuarioMatricula)/*: Disciplina.ListarOrdenadamente()*/;
            model.Dificuldades = Dificuldade.ListarOrdenadamente();
            model.Termo = Parametro.Obter().NotaUsoCertificacao;
            return View(model);
        }

        // POST: principal/avaliacao/certificacao/confirmar
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.PROFESSOR/*, 3*/ })]
        public ActionResult Confirmar(FormCollection formCollection)
        {
            AvalCertificacao cert = new AvalCertificacao();
            if (formCollection.HasKeys())
            {
                DateTime hoje = DateTime.Now;

                /* Chave */
                cert.Avaliacao = new Avaliacao();
                cert.Avaliacao.TipoAvaliacao = TipoAvaliacao.ListarPorCodigo(TipoAvaliacao.CERTIFICACAO);
                cert.Avaliacao.Ano = hoje.Year;
                cert.Avaliacao.Semestre = hoje.SemestreAtual();
                cert.Avaliacao.NumIdentificador = Avaliacao.ObterNumIdentificador(TipoAvaliacao.CERTIFICACAO);
                cert.Avaliacao.DtCadastro = hoje;

                /* Professor */
                string matricula = Sessao.UsuarioMatricula;
                cert.Professor = Professor.ListarPorMatricula(matricula);

                /* Dados */
                int codDisciplina = int.Parse(formCollection["ddlDisciplina"]);

                cert.CodDisciplina = codDisciplina;

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
                string[] temasCodigo = formCollection["ddlTemas"].Split(',');

                /* Questões */
                List<QuestaoTema> lstQuestoes = new List<QuestaoTema>();

                if (quantidadeObjetiva > 0)
                {
                    lstQuestoes.AddRange(Questao.ListarPorDisciplina(codDisciplina, temasCodigo, codDificuldade, TipoQuestao.OBJETIVA, quantidadeObjetiva));
                }
                if (quantidadeDiscursiva > 0)
                {
                    lstQuestoes.AddRange(Questao.ListarPorDisciplina(codDisciplina, temasCodigo, codDificuldade, TipoQuestao.DISCURSIVA, quantidadeDiscursiva));
                }

                foreach (string temaCodigo in temasCodigo)
                {
                    AvaliacaoTema avalTema = new AvaliacaoTema();
                    avalTema.Tema = Tema.ListarPorCodigo(codDisciplina, int.Parse(temaCodigo));
                    foreach (QuestaoTema questaoTema in lstQuestoes.Where(q => q.CodTema == int.Parse(temaCodigo)))
                    {
                        AvalTemaQuestao avalTemaQuestao = new AvalTemaQuestao();
                        avalTemaQuestao.QuestaoTema = questaoTema;
                        avalTema.AvalTemaQuestao.Add(avalTemaQuestao);
                    }
                    cert.Avaliacao.AvaliacaoTema.Add(avalTema);
                }

                AvalCertificacao.Inserir(cert);
            }
            
            return RedirectToAction("Configurar", new { codigo = cert.Avaliacao.CodAvaliacao });
        }

        // GET: principal/avaliacao/certificacao/configurar/CERT201520001
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.PROFESSOR/*, 3*/ })]
        public ActionResult Configurar(string codigo)
        {
            if (!String.IsNullOrWhiteSpace(codigo) && !Sistema.AvaliacaoUsuario.ContainsKey(codigo))
            {
                AvalCertificacao cert = AvalCertificacao.ListarPorCodigoAvaliacao(codigo);
                if (cert != null && !cert.Avaliacao.FlagRealizada)
                {
                    Professor prof = Professor.ListarPorMatricula(Sessao.UsuarioMatricula);
                    if (prof != null && prof.CodProfessor == cert.Professor.CodProfessor)
                    {
                        CertificacaoConfigurarViewModel model = new CertificacaoConfigurarViewModel();
                        model.Avaliacao = cert.Avaliacao;
                        model.Dificuldades = Dificuldade.ListarOrdenadamente();
                        model.TiposQuestao = TipoQuestao.ListarOrdenadamente();
                        return View(model);
                    }
                }
            }
            return RedirectToAction("Index");
        }

        // POST: principal/avaliacao/certificacao/configurar/CERT201520001
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.PROFESSOR/*, 3*/})]
        public ActionResult Configurar(string codigo,int[] questoes)
        {
            if(!String.IsNullOrWhiteSpace(codigo) && questoes.Length > 0)
            {
                Avaliacao.AtualizarQuestoes(codigo, questoes);
                return Json(true);
            }
            return Json(false);
        }

        // POST: principal/avaliacao/certificacao/carregarquestoes/CERT201520001
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.PROFESSOR/*, 3*/ })]
        public ActionResult CarregarQuestoes(string codigo, int[] temas, int dificuldade, int tipo)
        {
            if (!String.IsNullOrWhiteSpace(codigo) && temas.Length > 0 && dificuldade > 0 && tipo > 0 )
            {
                AvalCertificacao cert = AvalCertificacao.ListarPorCodigoAvaliacao(codigo);
                if (cert != null)
                {
                    List<Questao> questoes = Questao.ListarQuestoesFiltradas(cert.CodDisciplina, temas, dificuldade, tipo);
                    if (questoes != null)
                    {
                        return PartialView("_ListaQuestaoCard", questoes);
                    }
                }
            }
            return null;
        }

        // POST: principal/avaliacao/certificacao/carregarquestao
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.PROFESSOR })]
        public ActionResult CarregarQuestao(int codQuestao)
        {
            if (codQuestao > 0)
            {
                Questao questao = Questao.ListarPorCodigo(codQuestao);

                if (questao != null)
                {

                    return PartialView("~/Views/Questao/Partials/_Questao.cshtml", questao);
                }
            }
            return null;
        }

        // POST: principal/avaliacao/certificacao/carregarlistaquestaodetalhe
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.PROFESSOR })]
        public ActionResult CarregarListaQuestaoDetalhe(int[] codQuestoes)
        {
            if (codQuestoes.Length > 0)
            {
                List<Questao> questoes = Questao.ListarPorCodigos(codQuestoes);
                
                if(questoes.Count > 0)
                {
                    return PartialView("_ListaQuestaoDetalhe", questoes);
                }
            }
            return null;
        }

        // GET: principal/avaliacao/certificacao/agendar/CERT201520001
        [HttpGet]
        [Filters.AutenticacaoFilter(Categorias = new [] { Categoria.PROFESSOR })]
        public ActionResult Agendar(string codigo)
        {
            if (!String.IsNullOrWhiteSpace(codigo) && !Sistema.AvaliacaoUsuario.ContainsKey(codigo))
            {
                AvalCertificacao cert = AvalCertificacao.ListarPorCodigoAvaliacao(codigo);
                
                if (cert.Professor.MatrProfessor == Sessao.UsuarioMatricula)
                {
                    AvaliacaoAgendarViewModel model = new AvaliacaoAgendarViewModel();

                    model.Avaliacao = cert.Avaliacao;
                    model.Salas = Sala.ListarOrdenadamente();

                    return View(model);
                }
                
            }
            return RedirectToAction("Index");
        }
        
        // POST: principal/avaliacao/certificacao/agendar/CERT201520001
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
                AvalCertificacao aval = AvalCertificacao.ListarPorCodigoAvaliacao(codigo);

                string matricula = Sessao.UsuarioMatricula;
                Professor professor = Professor.ListarPorMatricula(matricula);

                if (aval.CodProfessor == professor.CodProfessor)
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
                    DateTime dtAplicacao = DateTime.Parse(data + " " + horaInicio, new CultureInfo("pt-BR"));
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

            return RedirectToAction("Avaliados", new { codigo = codigo }); // Redirecionar para Pessoas
        }

        // GET: principal/avaliacao/certificacao/avaliados/CERT201520001
        [HttpGet]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.PROFESSOR })]
        public ActionResult Avaliados(string codigo)
        {
            if (!String.IsNullOrWhiteSpace(codigo) && !Sistema.AvaliacaoUsuario.ContainsKey(codigo))
            {
                AvalCertificacao cert = AvalCertificacao.ListarPorCodigoAvaliacao(codigo);
                if (cert.Professor.MatrProfessor == Sessao.UsuarioMatricula)
                {
                    return View(cert);
                }
            }
            return RedirectToAction("Index");
        }

        // POST: principal/avaliacao/certificacao/avaliados/CERT201520001
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.PROFESSOR })]
        public ActionResult Avaliados(string codigo, List<Selecao> selecao)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                AvalCertificacao cert = AvalCertificacao.ListarPorCodigoAvaliacao(codigo);
                if (cert.Professor.MatrProfessor == Sessao.UsuarioMatricula)
                {
                    cert.PessoaFisica.Clear();
                    List<PessoaFisica> lstPessoaFisica = new List<PessoaFisica>();

                    foreach (Selecao item in selecao)
                    {
                        switch (item.category)
                        {
                            case "Pessoa":
                                lstPessoaFisica.Add(PessoaFisica.ListarPorCodigo(int.Parse(item.id)));
                                break;
                            case "Turma":
                                lstPessoaFisica.AddRange(PessoaFisica.ListarPorTurma(item.id));
                                break;
                            case "Curso":
                                lstPessoaFisica.AddRange(PessoaFisica.ListarPorCurso(int.Parse(item.id)));
                                break;
                            case "Diretoria":
                                lstPessoaFisica.AddRange(PessoaFisica.ListarPorDiretoria(item.id));
                                break;
                            case "Campus":
                                lstPessoaFisica.AddRange(PessoaFisica.ListarPorCampus(item.id));
                                break;
                            default:
                                break;
                        }
                    }

                    cert.PessoaFisica = lstPessoaFisica.Distinct().ToList();
                    Repositorio.GetInstance().SaveChanges();
                }
            }
            return Json("/historico/avaliacao/certificacao/detalhe/"+codigo);
        }
        
        // POST: principal/avaliacao/certificacao/filtrar/CERT201520001
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.PROFESSOR })]
        public ActionResult Filtrar(int codigo)
        {
            object lstResultado = null;

            switch (codigo)
            {
                case 1:
                    lstResultado = Usuario.Listar().Select(a => new Selecao {
                        id = a.CodPessoaFisica.ToString(),
                        description = a.Matricula,
                        title = a.PessoaFisica.Nome,
                        category = "Pessoa"
                    });
                    break;
                case 2:
                    lstResultado = Turma.ListarOrdenadamente().Select(a => new Selecao {
                        id = a.CodTurma,
                        description = a.CodTurma,
                        title = $"{a.Curso.Descricao} ({a.CodTurma})",
                        category = "Turma"
                    });
                    break;
                case 3:
                    lstResultado = Curso.ListarOrdenadamente().Select(a=>new Selecao {
                        id = a.CodCurso.ToString(),
                        description = a.Sigla,
                        title = a.Descricao,
                        category = "Curso"
                    });
                    break;
                case 4:
                    lstResultado = Diretoria.ListarOrdenadamente().Select(a => new Selecao {
                        id = a.CodComposto,
                        description = $"{a.Campus.PessoaJuridica.NomeFantasia} ({a.Campus.Instituicao.Sigla})",
                        title = a.PessoaJuridica.NomeFantasia,
                        category = "Diretoria"
                    });
                    break;
                case 5:
                    lstResultado = Campus.ListarOrdenadamente().Select(a=>new Selecao {
                        id = a.CodComposto,
                        description = a.Instituicao.PessoaJuridica.NomeFantasia,
                        title = a.PessoaJuridica.NomeFantasia,
                        category = "Campus"
                    });
                    break;
                default:
                    break;
            }
           
            return Json(lstResultado);
        }

        // GET: historico/avaliacao/certificacao/agendada/CERT201520001
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.ESTUDANTE, Categoria.PROFESSOR })]
        public ActionResult Agendada(string codigo)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                Usuario usuario = Usuario.ListarPorMatricula(Sessao.UsuarioMatricula);
                AvalCertificacao cert = AvalCertificacao.ListarAgendadaPorUsuario(usuario).FirstOrDefault(a => a.Avaliacao.CodAvaliacao.ToLower() == codigo.ToLower());
                if (cert != null && cert.PessoaFisica.Count > 0)
                {
                    return View(cert);
                }
            }
            return RedirectToAction("Detalhe", new { codigo = codigo });
        }

        // POST: principal/avaliacao/certificacao/alternarliberar
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

        // POST: principal/avaliacao/certificacao/contagemregressiva
        [HttpPost]
        public ActionResult ContagemRegressiva(string codAvaliacao)
        {
            AvalCertificacao cert = AvalCertificacao.ListarPorCodigoAvaliacao(codAvaliacao);
            string tempo = cert.Avaliacao.DtAplicacao.Value.ToLeftTimeString();
            int quantidadeMilissegundo = 0;
            bool flagLiberada = cert.Avaliacao.FlagLiberada && cert.Avaliacao.DtTermino > DateTime.Now;
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

        // GET: principal/avaliacao/certificacao/realizar/CERT201520001
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.ESTUDANTE })]
        public ActionResult Realizar(string codigo)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                AvalCertificacao cert = AvalCertificacao.ListarPorCodigoAvaliacao(codigo);
                if (cert.Avaliacao.FlagPendente 
                    && cert.Avaliacao.FlagLiberada 
                    && cert.Avaliacao.FlagAgora  
                    && cert.PessoaFisica.FirstOrDefault(p=>p.CodPessoa == Usuario.ObterPessoaFisica(Sessao.UsuarioMatricula)) != null)
                {
                    Sessao.Inserir("RealizandoAvaliacao", true);
                    return View(cert);
                }
            }
            return RedirectToAction("Agendada", new { codigo = codigo });
        }

        // GET: principal/avaliacao/certificacao/acompanhar/CERT201520001
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.PROFESSOR })]
        public ActionResult Acompanhar(string codigo)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                AvalCertificacao cert = AvalCertificacao.ListarPorCodigoAvaliacao(codigo);
                if (cert != null && cert.Professor.MatrProfessor == Sessao.UsuarioMatricula && cert.Avaliacao.FlagAgendada && cert.Avaliacao.FlagAgora)
                {
                    return View(cert);
                }
            }
            return RedirectToAction("Agendada", new {codigo = codigo });
        }

        // POST: principal/avaliacao/certificacao/printar
        [HttpPost]
        public ActionResult Printar(string codAvaliacao, string imageData)
        {
            AvalCertificacao cert = AvalCertificacao.ListarPorCodigoAvaliacao(codAvaliacao);
            bool flagProfessor = cert.Professor.MatrProfessor == Sessao.UsuarioMatricula;
            if (!flagProfessor)
            {
                Sistema.TempDataUrlImage[codAvaliacao] = imageData;
                return Json(true);
            }
            else if (flagProfessor)
            {
                string temp = Sistema.TempDataUrlImage[codAvaliacao];
                Sistema.TempDataUrlImage[codAvaliacao] = String.Empty;
                return Json(temp);
            }
            return Json(false);
        }

        // POST: principal/avaliacao/certificacao/desistir/CERT201520001
        [HttpPost]
        public void Desistir(string codigo)
        {
            int codPessoaFisica = Usuario.ObterPessoaFisica(Sessao.UsuarioMatricula);
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                AvalCertificacao aval = AvalCertificacao.ListarPorCodigoAvaliacao(codigo);
                if (aval.PessoaFisica.FirstOrDefault(a => a.CodPessoa == codPessoaFisica) != null 
                    && aval.Avaliacao.AvalPessoaResultado.FirstOrDefault(a => a.CodPessoaFisica == codPessoaFisica) == null)
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
                            if (avalTemaQuestao.QuestaoTema.Questao.CodTipoQuestao == 1) avalQuesPessoaResposta.RespAlternativa = -1;
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

        // POST: principal/avaliacao/certificacao/resultado/CERT201520001
        [HttpPost]
        public ActionResult Resultado(string codigo, FormCollection form)
        {
            int codPessoaFisica = Usuario.ObterPessoaFisica(Helpers.Sessao.UsuarioMatricula);
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                AvalCertificacao aval = AvalCertificacao.ListarPorCodigoAvaliacao(codigo);
                if (aval.PessoaFisica.FirstOrDefault(a => a.CodPessoa == codPessoaFisica) != null
                    && aval.Avaliacao.AvalPessoaResultado.FirstOrDefault(a => a.CodPessoaFisica == codPessoaFisica) == null)
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

        // GET: historico/avaliacao/certificacao/corrigir/CERT201520001
        [HttpGet]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.PROFESSOR })]
        public ActionResult Corrigir(string codigo)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                AvalCertificacao cert = AvalCertificacao.ListarPorCodigoAvaliacao(codigo);

                if (cert != null && cert.Avaliacao.FlagCorrecaoPendente && cert.Professor.MatrProfessor == Sessao.UsuarioMatricula)
                {
                    return View(cert);
                }
            }
            return RedirectToAction("Index");
        }

        // POST: principal/avaliacao/certificacao/Avaliacao/CarregarAlunos/CERT201520001
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.PROFESSOR })]
        public ActionResult CarregarAlunos(string codigo)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                AvalCertificacao cert = AvalCertificacao.ListarPorCodigoAvaliacao(codigo);
                var retorno = from a in cert.PessoasRealizaram
                             select new
                             {
                                 Matricula = a.CodPessoa,
                                 Nome = a.Nome,
                                 FlagCorrecaoPendente = cert.Avaliacao.AvalPessoaResultado.Single(r => r.CodPessoaFisica == a.CodPessoa).FlagParcial
                             };
                return Json(retorno);
            }
            return Json(null);
        }

        // POST: principal/avaliacao/certificacao/carregarquestoesdiscursivas/CERT201520001
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.PROFESSOR })]
        public ActionResult CarregarQuestoesDiscursivas(string codigo)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                AvalCertificacao cert = AvalCertificacao.ListarPorCodigoAvaliacao(codigo);
                var retorno = from questao in cert.Avaliacao.Questao
                             where questao.CodTipoQuestao == TipoQuestao.DISCURSIVA
                             orderby questao.CodQuestao
                             select new
                             {
                                 codQuestao = questao.CodQuestao,
                                 questaoEnunciado = questao.Enunciado,
                                 questaoChaveResposta = questao.ChaveDeResposta,
                                 flagCorrecaoPendente = cert.Avaliacao.PessoaResposta.Where(r => r.CodQuestao == questao.CodQuestao && !r.RespNota.HasValue).Count() > 0
                             };
                return Json(retorno);
            }
            return Json(null);
        }

        // POST: principal/avaliacao/certificacao/carregarrespostasdiscursivas/CERT201520001
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.PROFESSOR})]
        public ActionResult CarregarRespostasDiscursivas(string codigo, string matrAluno)
        {
            if (!StringExt.IsNullOrWhiteSpace(codigo,matrAluno))
            {
                AvalCertificacao cert = AvalCertificacao.ListarPorCodigoAvaliacao(codigo);
                PessoaFisica pessoa = PessoaFisica.ListarPorCodigo(int.Parse(matrAluno));
                int codPessoaFisica = pessoa.CodPessoa;

                var retorno = from alunoResposta in cert.Avaliacao.PessoaResposta
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

        // POST: principal/avaliacao/certificacao/carregarrespostasporquestao/CERT201520001
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.PROFESSOR })]
        public ActionResult CarregarRespostasPorQuestao(string codigo, string codQuestao)
        {
            if (!StringExt.IsNullOrWhiteSpace(codigo,codQuestao))
            {
                AvalCertificacao cert = AvalCertificacao.ListarPorCodigoAvaliacao(codigo);
                int codQuestaoTemp = int.Parse(codQuestao);

                var retorno = from questao in cert.Avaliacao.PessoaResposta
                             orderby questao.PessoaFisica.Nome
                             where questao.CodQuestao == codQuestaoTemp
                                && questao.AvalTemaQuestao.QuestaoTema.Questao.CodTipoQuestao == TipoQuestao.DISCURSIVA
                             select new
                             {
                                 alunoMatricula = cert.PessoasRealizaram.FirstOrDefault(a => a.CodPessoa == questao.CodPessoaFisica).CodPessoa,
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

        // POST: principal/avaliacao/certificacao/corrigirquestaoaluno/CERT201520001
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.PROFESSOR })]
        public ActionResult CorrigirQuestaoAluno(string codigo, string matrAluno, string codQuestao, string notaObtida, string profObservacao)
        {
            if (!StringExt.IsNullOrWhiteSpace(codigo,matrAluno,codQuestao))
            {
                int codQuesTemp = int.Parse(codQuestao);
                double nota = Double.Parse(notaObtida.Replace('.', ','));

                bool retorno = AvalCertificacao.CorrigirQuestaoAluno(codigo, matrAluno, codQuesTemp, nota, profObservacao);

                return Json(retorno);
            }
            return Json(false);
        }

        // GET: principal/avaliacao/certificacao/imprimir/CERT201520001
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.PROFESSOR })]
        public ActionResult Imprimir(string codigo)
        {
            if (!String.IsNullOrWhiteSpace(codigo) && !Sistema.AvaliacaoUsuario.ContainsKey(codigo))
            {
                AvalCertificacao cert = AvalCertificacao.ListarPorCodigoAvaliacao(codigo);
                if (cert != null)
                {
                    string matricula = Sessao.UsuarioMatricula;
                    Professor professor = Professor.ListarPorMatricula(matricula);
                    if (professor.CodProfessor == cert.CodProfessor)
                    {
                        return View(cert);
                    }
                }
            }
            return RedirectToAction("Index");
        }

        // POST: principal/avaliacao/certificacao/arquivar/CERT201520001
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

        // GET: historico/avaliacao/certificacao/Detalhe/CERT201520001
        public ActionResult Detalhe(string codigo)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                AvalCertificacao cert = AvalCertificacao.ListarPorCodigoAvaliacao(codigo);
                if (cert != null)
                {
                    return View(cert);
                }
            }

            return RedirectToAction("Index");
        }

        // POST: principal/avaliacao/certificacao/detalheindividual/CERT201520001
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.PROFESSOR})]
        public ActionResult DetalheIndividual(string codigo, int pessoa)
        {
            if (!StringExt.IsNullOrWhiteSpace(codigo))
            {
                AvalCertificacao cert = AvalCertificacao.ListarPorCodigoAvaliacao(codigo);
                if (cert != null)
                {
                    AvalPessoaResultado model = cert.Avaliacao.AvalPessoaResultado.FirstOrDefault(r => r.CodPessoaFisica == pessoa);
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