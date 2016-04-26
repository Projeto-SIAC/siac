using SIAC.Helpers;
using SIAC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIAC.Controllers
{
    [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.COLABORADOR })]
    public class SimuladoController : Controller
    {
        public ActionResult Index() => View();
        
        public ActionResult Novo() => View();

        [HttpPost]
        public ActionResult Novo(FormCollection form)
        {
            if (!StringExt.IsNullOrWhiteSpace(form["txtTitulo"]))
            {
                Simulado sim = new Simulado();
                DateTime hoje = DateTime.Now;
                /* Chave */
                sim.Ano = hoje.Year;
                sim.NumIdentificador = Simulado.ObterNumIdentificador();
                sim.DtCadastro = hoje;
                
                /* Simulado */
                sim.Titulo = form["txtTitulo"];
                sim.Descricao = form["txtDescricao"];

                /* Colaborador */
                sim.Colaborador = Colaborador.ListarPorMatricula(Sessao.UsuarioMatricula);

                Simulado.Inserir(sim);
                Lembrete.AdicionarNotificacao($"Simulado cadastrado com sucesso.", Lembrete.POSITIVO);
                return RedirectToAction("Provas");
            }
            return RedirectToAction("Novo");
        }
    }
}