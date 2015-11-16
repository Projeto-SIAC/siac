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
        // GET: institucional/Configurar
        [Filters.AutenticacaoFilter(Categorias = new[] { 3 })]
        public ActionResult Configurar()
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
        // GET: institucional/Gerar
        [Filters.AutenticacaoFilter(Categorias = new[] { 3 })]
        public ActionResult Gerar()
        {
            ViewModels.InstitucionalGerarQuestaoViewModel model = new ViewModels.InstitucionalGerarQuestaoViewModel();
            model.Modulos = AviModulo.ListarOrdenadamente();
            model.Categorias = AviCategoria.ListarOrdenadamente();
            model.Indicadores = AviIndicador.ListarOrdenadamente();
            model.Tipos = TipoQuestao.ListarOrdenadamente();

            return View(model);
        }
    }
}