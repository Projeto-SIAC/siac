using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using SIAC.ViewModels;
using SIAC.Models;
using SIAC.Helpers;

namespace SIAC.Controllers
{
    [Filters.AutenticacaoFilter]
    public class ImpressaoController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Dashboard");
        }

        [HttpGet]
        public ActionResult Avaliacao(string codigo)
        {
            if (!String.IsNullOrEmpty(codigo))
            {
                var model = new ImpressaoAvaliacaoViewModel();
                model.Avaliacao = Models.Avaliacao.ListarPorCodigoAvaliacao(codigo);
                if (model.Avaliacao.CodTipoAvaliacao == 1)
                {
                    Models.Avaliacao.AlternarFlagArquivo(codigo);
                    Repositorio.GetInstance().SaveChanges();
                    return View("Autoavaliacao", model);
                }
                else if (model.Avaliacao != null && model.Avaliacao.FlagPendente)
                {
                    if (model.Avaliacao.CodTipoAvaliacao > 1 && Sessao.UsuarioCategoriaCodigo < 2)
                    {
                        return RedirectToAction("Index", "Dashboard");
                    }
                    else if (model.Avaliacao.Professor.MatrProfessor != Sessao.UsuarioMatricula)
                    {
                        return RedirectToAction("Index", "Dashboard");
                    }
                    return View("PreImpressao", model);
                }
            }
            return RedirectToAction("Index", "Dashboard");
        }

        [HttpPost]
        public ActionResult Avaliacao(string codigo, FormCollection form)
        {
            if (!String.IsNullOrEmpty(codigo))
            {
                var model = new ImpressaoAvaliacaoViewModel();
                model.Avaliacao = Models.Avaliacao.ListarPorCodigoAvaliacao(codigo);
                if (model.Avaliacao != null && !StringExt.IsNullOrWhiteSpace(form["txtTitulo"], form["txtInstituicao"], form["txtProfessor"]) && model.Avaliacao.FlagPendente)
                {
                    if (model.Avaliacao.CodTipoAvaliacao > 1 && Sessao.UsuarioCategoriaCodigo < 2)
                    {
                        return RedirectToAction("Index", "Dashboard");
                    }
                    else if (model.Avaliacao.Professor.MatrProfessor != Sessao.UsuarioMatricula)
                    {
                        return RedirectToAction("Index", "Dashboard");
                    }

                    model.Titulo = form["txtTitulo"];
                    model.Instituicao = form["txtInstituicao"];
                    model.Professor = form["txtProfessor"];
                    model.Arquivar = !String.IsNullOrEmpty(form["chkArquivar"]);
                    if (!String.IsNullOrEmpty(form["txtInstrucoes"]))
                    {
                        model.Instrucoes = form["txtInstrucoes"].Split('\n');
                    }
                    if (!String.IsNullOrEmpty(form["ddlCampos"]))
                    {
                        model.Campos = form["ddlCampos"].Split(',');
                    }

                    if (model.Arquivar)
                    {
                        Models.Avaliacao.AlternarFlagArquivo(codigo);
                        Repositorio.GetInstance().SaveChanges();
                    }

                    return View(model);
                    //switch (model.Avaliacao.CodTipoAvaliacao)
                    //{ 
                    //    case 2:
                    //        return View("Academica", model);
                    //    case 3:
                    //        return View("Certificacao", model);
                    //    case 5:
                    //        return View("Reposicao", model);
                    //    default:
                    //        break;
                    //}
                }
            }

            return RedirectToAction("Index", "Dashboard");
        }

        [HttpGet]
        public ActionResult Institucional(string codigo)
        {
            if (!String.IsNullOrEmpty(codigo))
            {
                AvalAvi model = AvalAvi.ListarPorCodigoAvaliacao(codigo);
                if (model != null && model.CodColabCoordenador == Colaborador.ListarPorMatricula(Helpers.Sessao.UsuarioMatricula).CodColaborador)
                {
                    return View(model);
                }
            }
            return RedirectToAction("Index", "Dashboard");
        }
    }
}