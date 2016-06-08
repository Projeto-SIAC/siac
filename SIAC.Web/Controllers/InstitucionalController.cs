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
    public class InstitucionalController : Controller
    {
        public List<AvalAvi> Institucionais =>
            Sessao.UsuarioCategoriaCodigo == Categoria.COLABORADOR ? AvalAvi.ListarPorColaborador(Sessao.UsuarioMatricula) : new List<AvalAvi>();

        // GET: institucional/
        public ActionResult Index()
        {
            Usuario usuario = Sistema.UsuarioAtivo[Sessao.UsuarioMatricula].Usuario;
            if ((usuario.CodCategoria == Categoria.COLABORADOR || usuario.CodCategoria == Categoria.SUPERUSUARIO) && usuario.FlagCoordenadorAvi)
                return View(usuario);
            else
                return RedirectToAction("Andamento");
        }

        // GET: institucional/historico
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.COLABORADOR }, SomenteOcupacaoAvi = true)]
        public ActionResult Historico() => View();

        // GET: institucional/andamento
        public ActionResult Andamento() => View(AvalAvi.ListarPorUsuario(Sessao.UsuarioMatricula));

        // GET: institucional/configuracao
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.SUPERUSUARIO, Categoria.COLABORADOR }, SomenteOcupacaoAvi = true)]
        public ActionResult Configuracao()
        {
            InstitucionalGerarQuestaoViewModel model = new InstitucionalGerarQuestaoViewModel();
            model.Modulos = AviModulo.ListarOrdenadamente();
            model.Categorias = AviCategoria.ListarOrdenadamente();
            model.Indicadores = AviIndicador.ListarOrdenadamente();

            return View(model);
        }

        // POST: institucional/listar
        [HttpPost]
        public ActionResult Listar(int? pagina, string pesquisa, string ordenar, string categoria)
        {
            int quantidade = 12;
            List<AvalAvi> institucionais = Institucionais;
            pagina = pagina ?? 1;
            if (!String.IsNullOrWhiteSpace(pesquisa))
                institucionais = Institucionais
                    .Where(a => a.Avaliacao.CodAvaliacao.ToLower().Contains(pesquisa.ToLower())
                    || a.Titulo.ToLower().Contains(pesquisa.ToLower()))
                    .ToList();

            switch (categoria)
            {
                case "gerada":
                    institucionais = institucionais.Where(a => !a.FlagAndamento && !a.FlagConcluida).ToList();
                    break;

                case "andamento":
                    institucionais = institucionais.Where(a => a.FlagAndamento && !a.FlagConcluida).ToList();
                    break;

                case "concluida":
                    institucionais = institucionais.Where(a => a.FlagConcluida).ToList();
                    break;
            }

            switch (ordenar)
            {
                case "data_desc":
                    institucionais = institucionais.OrderByDescending(a => a.Avaliacao.DtCadastro).ToList();
                    break;

                case "data":
                    institucionais = institucionais.OrderBy(a => a.Avaliacao.DtCadastro).ToList();
                    break;

                default:
                    institucionais = institucionais.OrderByDescending(a => a.Avaliacao.DtCadastro).ToList();
                    break;
            }

            return PartialView("_ListaInstitucional", institucionais.Skip((quantidade * pagina.Value) - quantidade).Take(quantidade).ToList());
        }

        // POST: institucional/informacao/AVI201520002
        [HttpPost]
        public ActionResult Informacao(string codigo)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                AvalAvi avi = AvalAvi.ListarPorCodigoAvaliacao(codigo);
                if (avi != null)
                    return PartialView("_InstitucionalInformacoes", avi);
            }
            return null;
        }

        // POST: institucional/cadastrarmodulo
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.SUPERUSUARIO, Categoria.COLABORADOR }, SomenteOcupacaoAvi = true)]
        public ActionResult CadastrarModulo(FormCollection form)
        {
            if (!StringExt.IsNullOrWhiteSpace(form["txtTitulo"], form["txtObjetivo"]))
            {
                AviModulo modulo = new AviModulo();
                modulo.Descricao = form["txtTitulo"];
                modulo.Objetivo = form["txtObjetivo"];
                modulo.Observacao = String.IsNullOrWhiteSpace(form["txtObservacao"]) ? null : form["txtObservacao"];
                AviModulo.Inserir(modulo);
                Lembrete.AdicionarNotificacao($"Módulo <b>{modulo.Descricao}</b> cadastrado com sucesso.", Lembrete.POSITIVO);
            }
            return RedirectToAction("Configuracao");
        }

        // POST: institucional/cadastrarcategoria
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.SUPERUSUARIO, Categoria.COLABORADOR }, SomenteOcupacaoAvi = true)]
        public ActionResult CadastrarCategoria(FormCollection form)
        {
            if (!String.IsNullOrWhiteSpace(form["txtTitulo"]))
            {
                AviCategoria categoria = new AviCategoria();
                categoria.Descricao = form["txtTitulo"];
                categoria.Observacao = String.IsNullOrWhiteSpace(form["txtObservacao"]) ? null : form["txtObservacao"];
                AviCategoria.Inserir(categoria);
                Lembrete.AdicionarNotificacao($"Categoria <b>{categoria.Descricao}</b> cadastrada com sucesso.", Lembrete.POSITIVO);
            }
            return RedirectToAction("Configuracao");
        }

        // POST: institucional/cadastrarindicador
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.SUPERUSUARIO, Categoria.COLABORADOR }, SomenteOcupacaoAvi = true)]
        public ActionResult CadastrarIndicador(FormCollection form)
        {
            if (!String.IsNullOrWhiteSpace(form["txtTitulo"]))
            {
                AviIndicador indicador = new AviIndicador();
                indicador.Descricao = form["txtTitulo"];
                indicador.Observacao = form["txtObservacao"];
                AviIndicador.Inserir(indicador);
                Lembrete.AdicionarNotificacao($"Indicador <b>{indicador.Descricao}</b> cadastrado com sucesso.", Lembrete.POSITIVO);
            }
            return RedirectToAction("Configuracao");
        }

        // GET: institucional/gerar
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.COLABORADOR }, SomenteOcupacaoAvi = true)]
        public ActionResult Gerar() => View();

        // POST: institucional/gerar
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.COLABORADOR }, SomenteOcupacaoAvi = true)]
        [HttpPost]
        public ActionResult Gerar(FormCollection form)
        {
            if (!StringExt.IsNullOrWhiteSpace(form["txtTitulo"], form["txtObjetivo"]))
            {
                AvalAvi avi = new AvalAvi();
                /* Chave */
                avi.Avaliacao = new Avaliacao();
                DateTime hoje = DateTime.Now;
                avi.Avaliacao.TipoAvaliacao = TipoAvaliacao.ListarPorCodigo(TipoAvaliacao.AVI);
                avi.Avaliacao.Ano = hoje.Year;
                avi.Avaliacao.Semestre = hoje.SemestreAtual();
                avi.Avaliacao.NumIdentificador = Avaliacao.ObterNumIdentificador(TipoAvaliacao.AVI);
                avi.Avaliacao.DtCadastro = hoje;
                avi.Avaliacao.FlagLiberada = false;

                /* AVI */
                avi.Titulo = form["txtTitulo"];
                avi.Objetivo = form["txtObjetivo"];

                /* Colaborador */
                Colaborador colaborador = Colaborador.ListarPorMatricula(Sessao.UsuarioMatricula);
                avi.CodColabCoordenador = colaborador.CodColaborador;
                avi.Colaborador = colaborador;

                AvalAvi.Inserir(avi);
                Lembrete.AdicionarNotificacao($"Avaliação Institucional cadastrada com sucesso.", Lembrete.POSITIVO);
                return RedirectToAction("Questionario", new { codigo = avi.Avaliacao.CodAvaliacao });
            }
            return RedirectToAction("Gerar");
        }

        // GET: institucional/detalhe
        public ActionResult Detalhe(string codigo)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                AvalAvi avi = AvalAvi.ListarPorCodigoAvaliacao(codigo);
                if (avi != null)
                {
                    if (avi.Colaborador.Usuario.Matricula == Sessao.UsuarioMatricula)
                    {
                        return View(avi);
                    }
                }
            }

            return RedirectToAction("Historico");
        }

        // GET: institucional/questionario
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.COLABORADOR }, SomenteOcupacaoAvi = true)]
        public ActionResult Questionario(string codigo)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                AvalAvi avi = AvalAvi.ListarPorCodigoAvaliacao(codigo);
                if (avi != null && !avi.FlagAndamento)
                {
                    ViewModels.InstitucionalGerarQuestaoViewModel model = new ViewModels.InstitucionalGerarQuestaoViewModel();
                    model.Modulos = AviModulo.ListarOrdenadamente();
                    model.Categorias = AviCategoria.ListarOrdenadamente();
                    model.Indicadores = AviIndicador.ListarOrdenadamente();
                    model.Tipos = TipoQuestao.ListarOrdenadamente();
                    model.Avi = avi;

                    return View(model);
                }
            }
            return RedirectToAction("Index");
        }

        // POST: institucional/cadastrarquestao/AVI201520002
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.COLABORADOR }, SomenteOcupacaoAvi = true)]
        public ActionResult CadastrarQuestao(string codigo, FormCollection form)
        {
            AvalAvi avi = AvalAvi.ListarPorCodigoAvaliacao(codigo);
            if (avi != null && form.HasKeys())
            {
                AviQuestao questao = new AviQuestao();

                /* Chave */
                questao.AvalAvi = avi;
                questao.CodAviModulo = int.Parse(form["ddlModulo"]);
                questao.CodAviCategoria = int.Parse(form["ddlCategoria"]);
                questao.CodAviIndicador = int.Parse(form["ddlIndicador"]);
                questao.CodOrdem = AviQuestao.ObterNovaOrdem(avi);

                questao.Enunciado = form["txtEnunciado"].Trim();
                questao.Observacao = !String.IsNullOrWhiteSpace(form["txtObservacao"]) ? form["txtObservacao"].RemoveSpaces() : null;

                if (int.Parse(form["ddlTipo"]) == TipoQuestao.OBJETIVA)
                {
                    int qteAlternativas = int.Parse(form["txtQtdAlternativas"]);

                    for (int i = 1; i <= qteAlternativas; i++)
                    {
                        string enunciado = form["txtAlternativaEnunciado" + i];
                        questao.AviQuestaoAlternativa.Add(new AviQuestaoAlternativa
                        {
                            AviQuestao = questao,
                            CodAlternativa = i,
                            Enunciado = enunciado,
                            FlagAlternativaDiscursiva = false
                        });
                    }

                    if (form["chkAlternativaDiscursiva"] == "on")
                    {
                        int codAlternativa = qteAlternativas + 1;
                        string enunciado = form["txtAlternativaDiscursiva"];
                        questao.AviQuestaoAlternativa.Add(new AviQuestaoAlternativa
                        {
                            AviQuestao = questao,
                            CodAlternativa = codAlternativa,
                            Enunciado = enunciado,
                            FlagAlternativaDiscursiva = true
                        });
                    }
                }
                else if (int.Parse(form["ddlTipo"]) == TipoQuestao.DISCURSIVA)
                {
                    questao.FlagDiscursiva = true;
                }
                AviQuestao.Inserir(questao);
                return Json(questao.CodOrdem);
            }
            return Json(false);
        }

        // POST: institucional/removerquestao/AVI201520002
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.COLABORADOR }, SomenteOcupacaoAvi = true)]
        public ActionResult RemoverQuestao(string codigo, int modulo, int categoria, int indicador, int ordem)
        {
            AvalAvi avi = AvalAvi.ListarPorCodigoAvaliacao(codigo);
            if (avi != null)
            {
                AviQuestao questao = new AviQuestao();

                /* Chave */
                questao.Ano = avi.Ano;
                questao.Semestre = avi.Semestre;
                questao.CodTipoAvaliacao = avi.CodTipoAvaliacao;
                questao.NumIdentificador = avi.NumIdentificador;
                questao.CodAviModulo = modulo;
                questao.CodAviCategoria = categoria;
                questao.CodAviIndicador = indicador;
                questao.CodOrdem = ordem;

                AviQuestao.Remover(questao);
                return Json(questao.CodOrdem);
            }
            return Json(false);
        }

        // POST: institucional/editarquestao/AVI201520002
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.COLABORADOR }, SomenteOcupacaoAvi = true)]
        public ActionResult EditarQuestao(string codigo, FormCollection form)
        {
            AvalAvi avi = AvalAvi.ListarPorCodigoAvaliacao(codigo);
            if (avi != null && form.HasKeys())
            {
                int modulo = int.Parse(Request.QueryString["modulo"]);
                int categoria = int.Parse(Request.QueryString["categoria"]);
                int indicador = int.Parse(Request.QueryString["indicador"]);
                int ordem = int.Parse(Request.QueryString["ordem"]);

                AviQuestao questao = avi.ObterQuestao(modulo, categoria, indicador, ordem);

                if (questao != null)
                {
                    questao.Enunciado = form["txtEditarEnunciado"];
                    questao.Observacao = !String.IsNullOrWhiteSpace(form["txtEditarObservacao"]) ? form["txtEditarObservacao"] : null;

                    int indice = 1;
                    while (!String.IsNullOrWhiteSpace(form["txtEditarAlternativa" + indice]))
                    {
                        AviQuestaoAlternativa alternativa = questao.AviQuestaoAlternativa.FirstOrDefault(a => a.CodAlternativa == indice);
                        alternativa.Enunciado = form["txtEditarAlternativa" + indice];
                        indice++;
                    }

                    if (!String.IsNullOrWhiteSpace(form["txtEditarAlternativaDiscursiva"]))
                    {
                        AviQuestaoAlternativa alternativa = questao.AviQuestaoAlternativa.FirstOrDefault(a => a.FlagAlternativaDiscursiva);
                        alternativa.Enunciado = form["txtEditarAlternativaDiscursiva"];
                    }

                    AviQuestao.Atualizar(questao);
                }
                return Json(form);
            }
            return Json(false);
        }

        // GET: institucional/configurar/AVI201520002
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.COLABORADOR }, SomenteOcupacaoAvi = true)]
        public ActionResult Configurar(string codigo)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                AvalAvi avi = AvalAvi.ListarPorCodigoAvaliacao(codigo);

                if (avi != null && !avi.FlagAndamento)
                {
                    if (avi.FlagQuestionario)
                        return View(avi);
                    else
                        return RedirectToAction("Questionario", new { codigo = codigo });
                }
            }
            return RedirectToAction("Index");
        }

        // POST: institucional/configurar/AVI201520002
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.COLABORADOR }, SomenteOcupacaoAvi = true)]
        public ActionResult Configurar(string codigo, string[] questoes)
        {
            if (!String.IsNullOrWhiteSpace(codigo) && questoes.Length > 0)
            {
                AvalAvi avi = AvalAvi.ListarPorCodigoAvaliacao(codigo);

                if (avi != null && !avi.FlagAndamento)
                {
                    avi.OrdenarQuestoes(questoes);
                    return View(avi);
                }
            }
            return RedirectToAction("Index");
        }

        // GET: institucional/publico/AVI201520002
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.COLABORADOR }, SomenteOcupacaoAvi = true)]
        public ActionResult Publico(string codigo)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                AvalAvi avi = AvalAvi.ListarPorCodigoAvaliacao(codigo);

                if (avi != null && !avi.FlagAndamento)
                {
                    if (avi.FlagQuestionario)
                    {
                        InstitucionalPublicoViewModel viewModel = new InstitucionalPublicoViewModel();
                        viewModel.Avi = avi;
                        viewModel.TiposPublico = AviTipoPublico.ListarOrdenadamente();
                        return View(viewModel);
                    }
                    else
                        return RedirectToAction("Questionario", new { codigo = codigo });
                }
            }
            return RedirectToAction("Index");
        }

        // GET: institucional/agendar/AVI201520002
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.COLABORADOR }, SomenteOcupacaoAvi = true)]
        public ActionResult Agendar(string codigo)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                AvalAvi avi = AvalAvi.ListarPorCodigoAvaliacao(codigo);

                if (avi != null && !avi.FlagRealizada)
                {
                    if (avi.FlagPublico)
                        return View(avi);
                    return RedirectToAction("Publico", new { codigo = codigo });
                }
            }
            return RedirectToAction("Index");
        }

        // POST: institucional/agendar/AVI201520002
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.COLABORADOR }, SomenteOcupacaoAvi = true)]
        public ActionResult Agendar(string codigo, FormCollection form)
        {
            if (!StringExt.IsNullOrWhiteSpace(codigo, form["txtDataInicio"], form["txtDataTermino"]))
            {
                AvalAvi avi = AvalAvi.ListarPorCodigoAvaliacao(codigo);

                if (avi != null && !avi.FlagRealizada)
                {
                    if (avi.Questoes.Count > 0)
                    {
                        avi.Avaliacao.DtAplicacao = DateTime.Parse(form["txtDataInicio"] + " 00:00", new CultureInfo("pt-BR"));
                        avi.DtTermino = DateTime.Parse(form["txtDataTermino"] + " 23:59", new CultureInfo("pt-BR"));

                        Repositorio.Commit();
                        Lembrete.AdicionarNotificacao($"Avaliação Institucional agendada com sucesso.", Lembrete.POSITIVO);
                        return RedirectToAction("Historico");
                    }
                    return RedirectToAction("Questionario", new { codigo = codigo });
                }
            }
            return RedirectToAction("Index");
        }

        // POST: institucional/filtrarpublico/AVI201520001
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.COLABORADOR }, SomenteOcupacaoAvi = true)]
        public ActionResult FiltrarPublico(int codigo)
        {
            object lstResultado = null;

            switch (codigo)
            {
                case AviTipoPublico.PESSOA:
                    lstResultado = Usuario.Listar().Select(a => new Selecao
                    {
                        id = a.CodPessoaFisica.ToString(),
                        description = a.Matricula,
                        title = a.PessoaFisica.Nome,
                        category = "Pessoa"
                    });
                    break;

                case AviTipoPublico.TURMA:
                    lstResultado = Turma.ListarOrdenadamente().Select(a => new Selecao
                    {
                        id = a.CodTurma,
                        description = a.CodTurma,
                        title = $"{a.Curso.Descricao} ({a.CodTurma})",
                        category = "Turma"
                    });
                    break;

                case AviTipoPublico.CURSO:
                    lstResultado = Curso.ListarOrdenadamente().Select(a => new Selecao
                    {
                        id = a.CodCurso.ToString(),
                        description = a.Sigla,
                        title = a.Descricao,
                        category = "Curso"
                    });
                    break;

                case AviTipoPublico.DIRETORIA:
                    lstResultado = Diretoria.ListarOrdenadamente().Select(a => new Selecao
                    {
                        id = a.CodComposto,
                        description = $"{a.Campus.PessoaJuridica.NomeFantasia} ({a.Campus.Instituicao.Sigla})",
                        title = a.PessoaJuridica.NomeFantasia,
                        category = "Diretoria"
                    });
                    break;

                case AviTipoPublico.CAMPUS:
                    lstResultado = Campus.ListarOrdenadamente().Select(a => new Selecao
                    {
                        id = a.CodComposto,
                        description = a.Instituicao.PessoaJuridica.NomeFantasia,
                        title = a.PessoaJuridica.NomeFantasia,
                        category = "Campus"
                    });
                    break;

                case AviTipoPublico.PRO_REITORIA:
                    lstResultado = ProReitoria.ListarOrdenadamente().Select(a => new Selecao
                    {
                        id = a.CodComposto,
                        description = a.Instituicao.PessoaJuridica.NomeFantasia,
                        title = a.PessoaJuridica.NomeFantasia,
                        category = "Pró-Reitoria"
                    });
                    break;

                case AviTipoPublico.REITORIA:
                    lstResultado = Reitoria.ListarOrdenadamente().Select(a => new Selecao
                    {
                        id = a.CodComposto,
                        description = a.Instituicao.PessoaJuridica.NomeFantasia,
                        title = a.PessoaJuridica.NomeFantasia,
                        category = "Reitoria"
                    });
                    break;

                case AviTipoPublico.INSTITUICAO:
                    lstResultado = Instituicao.ListarOrdenadamente().Select(a => new Selecao
                    {
                        id = a.CodInstituicao.ToString(),
                        description = a.PessoaJuridica.NomeFantasia,
                        title = a.Sigla,
                        category = "Instituição"
                    });
                    break;

                default:
                    break;
            }

            return Json(lstResultado);
        }

        // POST: institucional/salvarpublico/AVI201520001
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.COLABORADOR }, SomenteOcupacaoAvi = true)]
        public ActionResult SalvarPublico(string codigo, List<Selecao> selecao)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                AvalAvi avi = AvalAvi.ListarPorCodigoAvaliacao(codigo);
                if (avi != null && !avi.FlagAndamento)
                    if (avi.Colaborador.MatrColaborador == Sessao.UsuarioMatricula)
                    {
                        avi.AviPublico.Clear();
                        avi.InserirPublico(selecao);
                    }
            }
            return Json("/institucional/agendar/" + codigo);
        }

        // GET: institucional/realizar/AVI201520002
        public ActionResult Realizar(string codigo)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                AvalAvi avi = AvalAvi.ListarPorCodigoAvaliacao(codigo);
                if (avi != null && avi.FlagAndamento)
                {
                    PessoaFisica pessoa = PessoaFisica.ListarPorMatricula(Sessao.UsuarioMatricula);
                    if (avi.Pessoas.Contains(pessoa))
                    {
                        InstitucionalRealizarViewModel viewModel = new InstitucionalRealizarViewModel();
                        viewModel.Avi = avi;
                        viewModel.Respostas = AviQuestaoPessoaResposta.ObterRespostasPessoa(avi, pessoa);
                        return View(viewModel);
                    }
                }
            }
            return RedirectToAction("Andamento");
        }

        // POST: institucional/enviarrespostaobjetiva/AVI201520002
        [HttpPost]
        public void EnviarRespostaObjetiva(string codigo, int ordem, int alternativa)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                AvalAvi avi = AvalAvi.ListarPorCodigoAvaliacao(codigo);
                if (avi != null && avi.FlagAndamento)
                {
                    AviQuestao questao = avi.ObterQuestao(ordem);
                    PessoaFisica pessoa = Usuario.ListarPorMatricula(Sessao.UsuarioMatricula)?.PessoaFisica;
                    AviQuestaoPessoaResposta.InserirResposta(questao, pessoa, alternativa);
                }
            }
        }

        // POST: institucional/enviarrespostadiscursiva/AVI201520002
        [HttpPost]
        public void EnviarRespostaDiscursiva(string codigo, int ordem, string resposta)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                AvalAvi avi = AvalAvi.ListarPorCodigoAvaliacao(codigo);
                if (avi != null && avi.FlagAndamento)
                {
                    AviQuestao questao = avi.ObterQuestao(ordem);
                    AviQuestaoPessoaResposta.InserirResposta(questao, PessoaFisica.ListarPorMatricula(Sessao.UsuarioMatricula), resposta);
                }
            }
        }

        // POST: institucional/enviaralternativadiscursiva/AVI201520002
        [HttpPost]
        public void EnviarAlternativaDiscursiva(string codigo, int ordem, int alternativa, string resposta)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                AvalAvi avi = AvalAvi.ListarPorCodigoAvaliacao(codigo);
                if (avi != null && avi.FlagAndamento)
                {
                    AviQuestao questao = avi.ObterQuestao(ordem);
                    AviQuestaoPessoaResposta.InserirResposta(questao, PessoaFisica.ListarPorMatricula(Sessao.UsuarioMatricula), alternativa, resposta);
                }
            }
        }

        // GET: institucional/resultado/AVI201520002
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.COLABORADOR }, SomenteOcupacaoAvi = true)]
        public ActionResult Resultado(string codigo)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                AvalAvi avi = AvalAvi.ListarPorCodigoAvaliacao(codigo);
                if (avi != null)
                {
                    Colaborador colaborador = Colaborador.ListarPorMatricula(Sessao.UsuarioMatricula);
                    if (avi.CodColabCoordenador == colaborador.CodColaborador)
                        return View(avi);
                }
            }
            return RedirectToAction("Andamento");
        }
    }
}