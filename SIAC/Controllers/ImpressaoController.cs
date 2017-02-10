/*
This file is part of SIAC.

Copyright (C) 2016 Felipe Mateus Freire Pontes <felipemfpontes@gmail.com>
Copyright (C) 2016 Francisco Bento da Silva Júnior <francisco.bento.jr@hotmail.com>

SIAC is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details. 
*/
using SIAC.Helpers;
using SIAC.Models;
using SIAC.ViewModels;
using System;
using System.Web.Mvc;

namespace SIAC.Controllers
{
    [Filters.AutenticacaoFilter]
    public class ImpressaoController : Controller
    {
        // GET: impressao
        public ActionResult Index() => RedirectToAction("Index", "Principal");

        // GET: impressao/avaliacao/aval201520001
        [HttpGet]
        public ActionResult Avaliacao(string codigo)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                var model = new ImpressaoAvaliacaoViewModel();
                model.Avaliacao = Models.Avaliacao.ListarPorCodigoAvaliacao(codigo);
                if (model.Avaliacao.CodTipoAvaliacao == TipoAvaliacao.AUTOAVALIACAO)
                {
                    Models.Avaliacao.AlternarFlagArquivo(codigo);
                    return View("Autoavaliacao", model);
                }
                else if (model.Avaliacao != null && model.Avaliacao.FlagPendente)
                {
                    if (model.Avaliacao.CodTipoAvaliacao > TipoAvaliacao.AUTOAVALIACAO && Sessao.UsuarioCategoriaCodigo < Categoria.PROFESSOR)
                        return RedirectToAction("Index", "Principal");
                    else if (model.Avaliacao.Professor.MatrProfessor != Sessao.UsuarioMatricula)
                        return RedirectToAction("Index", "Principal");
                    return View("PreImpressao", model);
                }
            }
            return RedirectToAction("Index", "Principal");
        }

        // POST: impressao/avaliacao/aval201520001
        [HttpPost]
        public ActionResult Avaliacao(string codigo, FormCollection form)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                var model = new ImpressaoAvaliacaoViewModel();
                model.Avaliacao = Models.Avaliacao.ListarPorCodigoAvaliacao(codigo);
                if (model.Avaliacao != null && !StringExt.IsNullOrWhiteSpace(form["txtTitulo"], form["txtInstituicao"], form["txtProfessor"]) && model.Avaliacao.FlagPendente)
                {
                    if (model.Avaliacao.CodTipoAvaliacao > TipoAvaliacao.AUTOAVALIACAO && Sessao.UsuarioCategoriaCodigo < Categoria.PROFESSOR)
                        return RedirectToAction("Index", "Principal");
                    else if (model.Avaliacao.Professor.MatrProfessor != Sessao.UsuarioMatricula)
                        return RedirectToAction("Index", "Principal");
                    model.Titulo = form["txtTitulo"];
                    model.Instituicao = form["txtInstituicao"];
                    model.Professor = form["txtProfessor"];
                    model.Arquivar = !String.IsNullOrWhiteSpace(form["chkArquivar"]);
                    if (!String.IsNullOrWhiteSpace(form["txtInstrucoes"]))
                        model.Instrucoes = form["txtInstrucoes"].Split('\n');
                    if (!String.IsNullOrWhiteSpace(form["ddlCampos"]))
                        model.Campos = form["ddlCampos"].Split(',');
                    if (model.Arquivar)
                    {
                        Models.Avaliacao.AlternarFlagArquivo(codigo);
                    }
                    return View(model);
                }
            }

            return RedirectToAction("Index", "Principal");
        }

        // GET: impressao/institucional/avi201520001
        public ActionResult Institucional(string codigo)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                AvalAvi model = AvalAvi.ListarPorCodigoAvaliacao(codigo);
                if (model != null && model.CodColabCoordenador == Colaborador.ListarPorMatricula(Sessao.UsuarioMatricula).CodColaborador)
                    return View(model);
            }
            return RedirectToAction("Index", "Principal");
        }
    }
}