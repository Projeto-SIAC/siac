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
using SIAC.Models;
using SIAC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIAC.Controllers
{
    [RoutePrefix("simulado/arquivo")]
    [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.SUPERUSUARIO, Categoria.COLABORADOR }, SomenteOcupacaoSimulado = true)]
    public class ArquivoController : Controller
    {
        private List<Simulado> SimNaoEncerrados => Simulado.ListarNaoEncerradoOrdenadamente();
        private List<Simulado> SimEncerrados => Simulado.ListarEncerradoOrdenadamente();

        [HttpPost]
        [Route("listar")]
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

        [Route("")]
        public ActionResult Index() =>
            View(new ArquivoIndexViewModel()
            {
                MaisAbertos = SimNaoEncerrados.Count > 3,
                Abertos = SimNaoEncerrados.Take(3).ToList(),
                MaisEncerrados = SimEncerrados.Count > 3,
                Encerrados = SimEncerrados.Take(3).ToList()
            });

        [Route("abertos")]
        public ActionResult Abertos() => View();

        [Route("encerrados")]
        public ActionResult Encerrados() => View();
    }
}