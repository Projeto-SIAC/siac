using SIAC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIAC.Controllers
{
    [Filters.AutenticacaoFilter(Categorias = new[] { 1, 2, 3 })]
    public class InstitucionalController : Controller
    {
        // GET: institucional/
        public ActionResult Index()
        {
            Usuario usuario = Usuario.ListarPorMatricula(Helpers.Sessao.UsuarioMatricula);
            return View(usuario);
        }
        // GET: institucional/Configuracao
        [Filters.AutenticacaoFilter(Categorias = new[] { 3 })]
        public ActionResult Configuracao()
        {
            ViewModels.InstitucionalGerarQuestaoViewModel model = new ViewModels.InstitucionalGerarQuestaoViewModel();
            model.Modulos = AviModulo.ListarOrdenadamente();
            model.Categorias = AviCategoria.ListarOrdenadamente();
            model.Indicadores = AviIndicador.ListarOrdenadamente();

            return View(model);
        }
        // POST: institucional/CadastrarModulo
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { 3 })]
        public ActionResult CadastrarModulo(FormCollection form)
        {
            AviModulo modulo = new AviModulo();

            modulo.Descricao = form["txtTitulo"];
            modulo.Objetivo = form["txtObjetivo"];
            modulo.Observacao = form["txtObservacao"];

            AviModulo.Inserir(modulo);

            return RedirectToAction("Configuracao");
        }
        // POST: institucional/CadastrarCategoria
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { 3 })]
        public ActionResult CadastrarCategoria(FormCollection form)
        {
            AviCategoria categoria = new AviCategoria();

            categoria.Descricao = form["txtTitulo"];
            categoria.Observacao = form["txtObservacao"];

            AviCategoria.Inserir(categoria);

            return RedirectToAction("Configuracao");
        }
        // POST: institucional/CadastrarIndicador
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { 3 })]
        public ActionResult CadastrarIndicador(FormCollection form)
        {
            AviIndicador indicador = new AviIndicador();

            indicador.Descricao = form["txtTitulo"];
            indicador.Observacao = form["txtObservacao"];

            AviIndicador.Inserir(indicador);

            return RedirectToAction("Configuracao");
        }
        // GET: institucional/Gerar
        [Filters.AutenticacaoFilter(Categorias = new[] { 3 })]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Gerar()
        {
            return View();
        }
        // POST: institucional/Gerar
        [Filters.AutenticacaoFilter(Categorias = new[] { 3 })]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Gerar(FormCollection form)
        {
            if (!Helpers.StringExt.IsNullOrWhiteSpace(form["txtTitulo"], form["txtObjetivo"]))
            {
                AvalAvi avi = new AvalAvi();
                /* Chave */
                avi.Avaliacao = new Avaliacao();
                DateTime hoje = DateTime.Now;
                avi.Avaliacao.TipoAvaliacao = TipoAvaliacao.ListarPorCodigo(4);
                avi.Avaliacao.Ano = hoje.Year;
                avi.Avaliacao.Semestre = hoje.Month > 6 ? 2 : 1;
                avi.Avaliacao.NumIdentificador = Avaliacao.ObterNumIdentificador(4);
                avi.Avaliacao.DtCadastro = hoje;
                avi.Avaliacao.FlagLiberada = false;

                /* AVI */
                avi.Titulo = form["txtTitulo"];
                avi.Objetivo = form["txtObjetivo"];

                /* Colaborador */
                Colaborador colaborador = Colaborador.ListarPorMatricula(Helpers.Sessao.UsuarioMatricula);
                avi.CodColabCoordenador = colaborador.CodColaborador;
                avi.Colaborador = colaborador;

                AvalAvi.Inserir(avi);

                return RedirectToAction("Questionario", new { codigo = avi.Avaliacao.CodAvaliacao });
            }
            return RedirectToAction("Gerar");
        }
        // GET: institucional/Questionario
        [Filters.AutenticacaoFilter(Categorias = new[] { 3 })]
        public ActionResult Questionario(string codigo)
        {
            if (!String.IsNullOrEmpty(codigo))
            {
                AvalAvi avi = AvalAvi.ListarPorCodigoAvaliacao(codigo);
                if (avi != null /*&& !avi.FlagAndamento*/)
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
        // POST: institucional/CadastrarQuestao/{codigo}
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { 3 })]
        public ActionResult CadastrarQuestao(string codigo,FormCollection form)
        {
            AvalAvi avi = AvalAvi.ListarPorCodigoAvaliacao(codigo);
            if (avi != null)
            {
                AviQuestao questao = new AviQuestao();

                /* Chave */
                questao.AvalAvi = avi;
                questao.CodAviModulo = int.Parse(form["ddlModulo"]);
                questao.CodAviCategoria = int.Parse(form["ddlCategoria"]);
                questao.CodAviIndicador = int.Parse(form["ddlIndicador"]);
                questao.CodOrdem = AviQuestao.ObterNovaOrdem(avi, questao.CodAviModulo, questao.CodAviCategoria, questao.CodAviIndicador);

                questao.Enunciado = form["txtEnunciado"].Trim();
                questao.Observacao = !String.IsNullOrEmpty(form["txtObservacao"]) ? form["txtObservacao"].RemoveSpaces() : null;

                if (int.Parse(form["ddlTipo"]) == 1)
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
                else if (int.Parse(form["ddlTipo"]) == 2)
                {
                    questao.FlagDiscursiva = true;
                }
                AviQuestao.Inserir(questao);
                return Json(questao.CodOrdem);
            }
            return Json(false);
        }
        // POST: institucional/RemoverQuestao/{codigo}
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { 3 })]
        public ActionResult RemoverQuestao(string codigo,int modulo,int categoria, int indicador, int ordem)
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
        // POST: institucional/EditarQuestao/{codigo}
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { 3 })]
        public ActionResult EditarQuestao(string codigo,FormCollection form)
        {
            AvalAvi avi = AvalAvi.ListarPorCodigoAvaliacao(codigo);
            if (avi != null)
            {
                int modulo = int.Parse(Request.QueryString["modulo"]);
                int categoria = int.Parse(Request.QueryString["categoria"]);
                int indicador = int.Parse(Request.QueryString["indicador"]);
                int ordem = int.Parse(Request.QueryString["ordem"]);

                AviQuestao questao = avi.ObterQuestao(modulo, categoria, indicador, ordem);

                if(questao != null)
                {
                    questao.Enunciado = form["txtEditarEnunciado"];
                    questao.Observacao = !String.IsNullOrEmpty(form["txtEditarObservacao"]) ? form["txtEditarObservacao"] : null;

                    int indice = 1;
                    while(!String.IsNullOrEmpty(form["txtEditarAlternativa"+indice]))
                    {
                        AviQuestaoAlternativa alternativa = questao.AviQuestaoAlternativa.FirstOrDefault(a => a.CodAlternativa == indice);
                        alternativa.Enunciado = form["txtEditarAlternativa" + indice];
                        indice++;
                    }

                    if (!String.IsNullOrEmpty(form["txtEditarAlternativaDiscursiva"]))
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
        // GET: institucional/configurar/{codigo}
        [Filters.AutenticacaoFilter(Categorias = new[] { 3 })]
        public ActionResult Configurar(string codigo)
        {
            if (!String.IsNullOrEmpty(codigo))
            {
                AvalAvi avi = AvalAvi.ListarPorCodigoAvaliacao(codigo);

                if (avi != null/* && !avi.FlagAndamento*/)
                {
                    if (avi.FlagQuestionario)
                        return View(avi);
                    else
                        return RedirectToAction("Questionario", new { codigo = codigo });
                }
            }
            return RedirectToAction("Index");
        }
        // POST: institucional/configurar/{codigo}
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { 3 })]
        public ActionResult Configurar(string codigo,string[] questoes)
        {
            if (!String.IsNullOrEmpty(codigo) && questoes.Length > 0)
            {
                AvalAvi avi = AvalAvi.ListarPorCodigoAvaliacao(codigo);

                if (avi != null /*&& !avi.FlagAndamento*/)
                {
                    avi.OrdenarQuestoes(questoes);

                    return View(avi);
                }
            }
            return RedirectToAction("Index");
        }
        // GET: institucional/publico
        [Filters.AutenticacaoFilter(Categorias = new[] { 3 })]
        public ActionResult Publico(string codigo)
        {
            if (!String.IsNullOrEmpty(codigo))
            {
                AvalAvi avi = AvalAvi.ListarPorCodigoAvaliacao(codigo);

                if (avi != null /*&& !avi.FlagAndamento*/)
                {
                    if (avi.FlagQuestionario)
                    {
                        ViewModels.InstitucionalPublicoViewModel viewModel = new ViewModels.InstitucionalPublicoViewModel();
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
        // GET: institucional/agendar/{codigo}
        [Filters.AutenticacaoFilter(Categorias = new[] { 3 })]
        public ActionResult Agendar(string codigo)
        {
            if (!String.IsNullOrEmpty(codigo))
            {
                AvalAvi avi = AvalAvi.ListarPorCodigoAvaliacao(codigo);

                if (avi != null && !avi.FlagRealizada)
                {
                    if(avi.FlagPublico)
                    {
                        return View(avi);
                    }
                    return RedirectToAction("Publico", new { codigo = codigo });
                }
            }
            return RedirectToAction("Index");
        }
        // POST: institucional/agendar/{codigo}
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { 3 })]
        public ActionResult Agendar(string codigo,FormCollection form)
        {
            if (!String.IsNullOrEmpty(codigo) && !Helpers.StringExt.IsNullOrEmpty(form["txtDataInicio"], form["txtDataTermino"]))
            {
                AvalAvi avi = AvalAvi.ListarPorCodigoAvaliacao(codigo);

                if (avi != null && !avi.FlagRealizada)
                {
                    if (avi.Questoes.Count > 0)
                    {
                        avi.Avaliacao.DtAplicacao = DateTime.Parse(form["txtDataInicio"]);
                        avi.DtTermino = DateTime.Parse(form["txtDataTermino"]);

                        Repositorio.GetInstance().SaveChanges();

                        return RedirectToAction("Historico");
                    }
                    return RedirectToAction("Questionario", new { codigo = codigo });
                }
            }
            return RedirectToAction("Index");
        }
        // POST: Institucional/FiltrarPublico/AVI201520001
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { 3 })]
        public ActionResult FiltrarPublico(int codigo)
        {
            object lstResultado = null;

            switch (codigo)
            {
                case 8:
                    lstResultado = Usuario.Listar().Select(a => new Selecao
                    {
                        id = a.CodPessoaFisica.ToString(),
                        description = a.Matricula,
                        title = a.PessoaFisica.Nome,
                        category = "Pessoa"
                    });
                    break;
                case 7:
                    lstResultado = Turma.ListarOrdenadamente().Select(a => new Selecao
                    {
                        id = a.CodTurma,
                        description = a.CodTurma,
                        title = $"{a.Curso.Descricao} ({a.CodTurma})",
                        category = "Turma"
                    });
                    break;
                case 6:
                    lstResultado = Curso.ListarOrdenadamente().Select(a => new Selecao
                    {
                        id = a.CodCurso.ToString(),
                        description = a.Sigla,
                        title = a.Descricao,
                        category = "Curso"
                    });
                    break;
                case 5:
                    lstResultado = Diretoria.ListarOrdenadamente().Select(a => new Selecao
                    {
                        id = a.CodComposto,
                        description = $"{a.Campus.PessoaJuridica.NomeFantasia} ({a.Campus.Instituicao.Sigla})",
                        title = a.PessoaJuridica.NomeFantasia,
                        category = "Diretoria"
                    });
                    break;
                case 4:
                    lstResultado = Campus.ListarOrdenadamente().Select(a => new Selecao
                    {
                        id = a.CodComposto,
                        description = a.Instituicao.PessoaJuridica.NomeFantasia,
                        title = a.PessoaJuridica.NomeFantasia,
                        category = "Campus"
                    });
                    break;
                case 3:
                    lstResultado = ProReitoria.ListarOrdenadamente().Select(a => new Selecao
                    {
                        id = a.CodComposto,
                        description = a.Instituicao.PessoaJuridica.NomeFantasia,
                        title = a.PessoaJuridica.NomeFantasia,
                        category = "Pró-Reitoria"
                    });
                    break;
                case 2:
                    lstResultado = Reitoria.ListarOrdenadamente().Select(a => new Selecao
                    {
                        id = a.CodComposto,
                        description = a.Instituicao.PessoaJuridica.NomeFantasia,
                        title = a.PessoaJuridica.NomeFantasia,
                        category = "Reitoria"
                    });
                    break;
                case 1:
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
        // POST: Institucional/SalvarPublico/AVI201520001
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { 3 })]
        public ActionResult SalvarPublico(string codigo, List<Selecao> selecao)
        {
            if (!String.IsNullOrEmpty(codigo))
            {
                AvalAvi avi = AvalAvi.ListarPorCodigoAvaliacao(codigo);
                if (avi != null && !avi.FlagAndamento)
                {
                    if (avi.Colaborador.MatrColaborador == Helpers.Sessao.UsuarioMatricula)
                    {
                        avi.InserirPublico(selecao);
                    }
                }
            }
            return Json("/institucional/agendar/" + codigo);
            
        }
        //GET: institucional/realizar/{codigo}
        [Filters.AutenticacaoFilter(Categorias = new[] { 3 })]
        public ActionResult Realizar(string codigo)
        {
            if (!String.IsNullOrEmpty(codigo))
            {
                AvalAvi avi = AvalAvi.ListarPorCodigoAvaliacao(codigo);
                if (avi != null /*&& avi.FlagAndamento*/)
                {
                    ViewModels.InstitucionalRealizarViewModel viewModel = new ViewModels.InstitucionalRealizarViewModel();
                    viewModel.Avi = avi;
                    viewModel.Respostas = AviQuestaoPessoaResposta.ObterRespostasPessoa(avi, PessoaFisica.ListarPorMatricula(Helpers.Sessao.UsuarioMatricula));

                    return View(viewModel);
                }
            }
            return RedirectToAction("Index");
        }
        // POST: institucional/EnviarRespostaObjetiva/{codigo}
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { 3 })]
        public ActionResult EnviarRespostaObjetiva(string codigo,int ordem,int alternativa)
        {
            if (!String.IsNullOrEmpty(codigo))
            {
                AvalAvi avi = AvalAvi.ListarPorCodigoAvaliacao(codigo);
                if (avi != null /*&& !avi.FlagAndamento*/)
                {
                    AviQuestao questao = avi.ObterQuestao(ordem);

                    PessoaFisica pessoa = Usuario.ListarPorMatricula(Helpers.Sessao.UsuarioMatricula)?.PessoaFisica;

                    AviQuestaoPessoaResposta.InserirResposta(questao, pessoa, alternativa);
                }
            }
            return Json("/institucional/agendar/" + codigo);

        }
        // POST: institucional/EnviarRespostaDiscursiva/{codigo}
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { 3 })]
        public ActionResult EnviarRespostaDiscursiva(string codigo, int ordem, string resposta)
        {
            if (!String.IsNullOrEmpty(codigo))
            {
                AvalAvi avi = AvalAvi.ListarPorCodigoAvaliacao(codigo);
                if (avi != null /*&& !avi.FlagAndamento*/)
                {
                    AviQuestao questao = avi.ObterQuestao(ordem);
                    
                    AviQuestaoPessoaResposta.InserirResposta(questao, PessoaFisica.ListarPorMatricula(Helpers.Sessao.UsuarioMatricula), resposta);
                }
            }
            return Json("/institucional/agendar/" + codigo);

        }
    }
}