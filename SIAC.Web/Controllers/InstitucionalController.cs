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
            AvalAvi avi = new AvalAvi();
            /* Chave */
            avi.Avaliacao = new Avaliacao();
            DateTime hoje = DateTime.Now;
            avi.Avaliacao.TipoAvaliacao = TipoAvaliacao.ListarPorCodigo(4);
            avi.Avaliacao.Ano = hoje.Year;
            avi.Avaliacao.Semestre = hoje.Month > 6 ? 2 : 1;
            avi.Avaliacao.NumIdentificador = Avaliacao.ObterNumIdentificador(4);
            avi.Avaliacao.DtCadastro = hoje;
            avi.Avaliacao.FlagLiberada = true;

            /* AVI */
            avi.Titulo = form["txtTitulo"];
            avi.Objetivo = form["txtObjetivo"];
            avi.DtTermino = DateTime.Parse(form["txtDataTermino"]);

            /* Colaborador */
            Colaborador colaborador = Colaborador.ListarPorMatricula(Helpers.Sessao.UsuarioMatricula);
            avi.CodColabCoordenador = colaborador.CodColaborador;
            avi.Colaborador = colaborador;

            AvalAvi.Inserir(avi);
            
            return RedirectToAction("Configurar");
        }

        // GET: institucional/Configurar
        [Filters.AutenticacaoFilter(Categorias = new[] { 3 })]
        public ActionResult Configurar()
        {
            ViewModels.InstitucionalGerarQuestaoViewModel model = new ViewModels.InstitucionalGerarQuestaoViewModel();
            model.Modulos = AviModulo.ListarOrdenadamente();
            model.Categorias = AviCategoria.ListarOrdenadamente();
            model.Indicadores = AviIndicador.ListarOrdenadamente();
            model.Tipos = TipoQuestao.ListarOrdenadamente();

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

            return RedirectToAction("Configurar");
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

            return RedirectToAction("Configurar");
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

            return RedirectToAction("Configurar");
        }
        
        // POST: institucional/CadastrarQuestao
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { 3 })]
        public ActionResult CadastrarQuestao(FormCollection form)
        {
            AviQuestao questao = new AviQuestao();
            questao.CodAviModulo = int.Parse(form["ddlModulo"]);
            questao.CodAviCategoria = int.Parse(form["ddlCategoria"]);
            questao.CodAviIndicador = int.Parse(form["ddlIndicador"]);
            

            questao.Enunciado = form["txtEnunciado"];
            questao.Observacao = form["txtObservacao"];
            
            if(int.Parse(form["ddlTipo"]) == 1)
            {

            }
            return Json(true);
        }
    }
}