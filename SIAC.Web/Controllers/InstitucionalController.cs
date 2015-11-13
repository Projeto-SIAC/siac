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
            ViewBag.Modulos = AviModulo.ListarOrdenadamente();
            ViewBag.Categorias = AviCategoria.ListarOrdenadamente();
            ViewBag.Indicadores = AviIndicador.ListarOrdenadamente();

            return View();
        }

        // OIST: institucional/CadastrarModulo
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { 3 })]
        public ActionResult CadastrarModulo(FormCollection form)
        {
            AviModulo modulo = new AviModulo();

            modulo.Descricao = form["txtTitulo"];
            modulo.Objetivo = form["txtObjetivo"];
            modulo.Observacao = form["txtObservacao"];

            AviModulo.Inserir(modulo);
            
            return View("Configurar");
        }

        // GET: institucional/Gerar
        [Filters.AutenticacaoFilter(Categorias = new[] { 3 })]
        public ActionResult Gerar()
        {
            return View();
        }
    }
}