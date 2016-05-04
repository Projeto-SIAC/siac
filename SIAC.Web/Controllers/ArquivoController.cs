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

        [HttpPost]
        public ActionResult Listar(int? pagina, string pesquisa, string ordenar, string categoria)
        {
            int quantidade = 12;
            List<Simulado> simulados = categoria == "abertos" ? SimNaoEncerrados : SimEncerrados;

            pagina = pagina ?? 1;
            if (!String.IsNullOrWhiteSpace(pesquisa))
            {
                simulados = simulados
                    .Where(a =>
                        a.Codigo.IndexOf(pesquisa, StringComparison.OrdinalIgnoreCase) > -1 || 
                        a.Titulo.IndexOf(pesquisa, StringComparison.OrdinalIgnoreCase) > -1 ||
                        a.Descricao.IndexOf(pesquisa, StringComparison.OrdinalIgnoreCase) > -1)
                    .ToList();
            }

            switch (ordenar)
            {
                case "data_desc":
                    simulados = simulados.OrderByDescending(a => a.DtCadastro).ToList();
                    break;
                case "data":
                    simulados = simulados.OrderBy(a => a.DtCadastro).ToList();
                    break;
                default:
                    simulados = simulados.OrderByDescending(a => a.DtCadastro).ToList();
                    break;
            }

            return PartialView("_ListaSimulado", simulados.Skip((quantidade * pagina.Value) - quantidade).Take(quantidade).ToList());
        }

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