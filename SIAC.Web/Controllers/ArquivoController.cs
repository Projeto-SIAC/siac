using SIAC.Models;
using SIAC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIAC.Controllers
{
    [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.COLABORADOR, Categoria.PROFESSOR })]
    public class ArquivoController : Controller
    {
        private List<Simulado> SimNaoEncerrados => Simulado.ListarNaoEncerradoOrdenadamente();
        private List<Simulado> SimEncerrados => Simulado.ListarEncerradoOrdenadamente();

        // GET: simulado/arquivo
        public ActionResult Index() =>
            View(new ArquivoIndexViewModel()
            {
                MaisAbertos = SimNaoEncerrados.Count > 3,
                Abertos = SimNaoEncerrados.Take(3).ToList(),
                MaisEncerrados = SimEncerrados.Count > 3,
                Encerrados = SimEncerrados.Take(3).ToList()
            });

        // TODO: método para ser utilizado ajax para atualizar as listas em Abertos e Encerrados
        public ActionResult Abertos() => View();
        public ActionResult Encerrados() => View();
    }
}