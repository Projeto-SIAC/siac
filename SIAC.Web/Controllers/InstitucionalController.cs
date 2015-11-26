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
                if (avi != null)
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
        public ActionResult EditarQuestao(string codigo,FormCollection form)/*, int modulo, int categoria, int indicador, int ordem)*/
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
    }
}