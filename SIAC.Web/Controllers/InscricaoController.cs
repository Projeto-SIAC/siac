using SIAC.Models;
using SIAC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIAC.Controllers
{
    public class InscricaoController : Controller
    {
        // GET: simulado/inscricao
        public ActionResult Index() => View(new InscricaoIndexViewModel()
        {
            Simulados = Simulado.ListarPorInscricoesAbertas()
        });

        // POST: simulado/inscricao/cadastro
        [HttpPost]
        public ActionResult Cadastro(FormCollection form)
        {
            return null;
        }
    }
}