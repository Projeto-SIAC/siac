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
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIAC.Controllers
{
    [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.SUPERUSUARIO, Categoria.COLABORADOR }, Ocupacoes = new[] { Ocupacao.SUPERUSUARIO, Ocupacao.REITOR, Ocupacao.PRO_REITOR, Ocupacao.DIRETOR_GERAL })]
    public class UsuarioController : Controller
    {
        private List<Usuario> Usuarios => Usuario.Listar();

        [HttpPost]
        public ActionResult Listar(int? pagina, string pesquisa, string ordenar, string categoria)
        {
            var qte = 10;
            var usuarios = Usuarios;
            pagina = pagina ?? 1;

            if (!String.IsNullOrWhiteSpace(pesquisa))
            {
                var strPesquisa = pesquisa.Trim().ToLower();
                usuarios = usuarios.Where(a => a.Matricula.ToLower().Contains(strPesquisa) || a.PessoaFisica.Nome.ToLower().Contains(strPesquisa)).ToList();
            }

            if (!String.IsNullOrWhiteSpace(categoria))
            {
                var codCategoria = int.Parse(categoria);
                usuarios = usuarios.Where(a => a.CodCategoria == codCategoria).ToList();
            }

            switch (ordenar)
            {
                case "data_desc":
                    usuarios = usuarios.OrderByDescending(a => a.UsuarioAcesso.LastOrDefault()?.DtAcesso).ToList();
                    break;

                case "data":
                    usuarios = usuarios.OrderBy(a => a.UsuarioAcesso.LastOrDefault()?.DtAcesso).ToList();
                    break;

                default:
                    usuarios = usuarios.OrderByDescending(a => a.UsuarioAcesso.LastOrDefault()?.DtAcesso).ToList();
                    break;
            }

            return PartialView("_ListaUsuario", usuarios.Skip((qte * pagina.Value) - qte).Take(qte).ToList());
        }

        // GET: Usuario
        public ActionResult Index()
        {
            var model = new UsuarioIndexViewModel();
            model.Categorias = Usuarios.Select(a => a.Categoria).Distinct().ToList();
            return View(model);
        }

        public ActionResult Detalhe(string codigo)
        {
            if (!String.IsNullOrEmpty(codigo))
            {
                var u = Usuario.ListarPorMatricula(codigo);
                if (u != null)
                {
                    return View(u);
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ListarAcessoPagina(string matricula, int codOrdem)
        {
            if (!String.IsNullOrEmpty(matricula))
            {
                var usuario = Usuario.ListarPorMatricula(matricula);
                if (usuario != null)
                {
                    return PartialView("_ListaAcessoUsuario", usuario.UsuarioAcesso.FirstOrDefault(a => a.CodOrdem == codOrdem));
                }
            }
            return null;
        }

        [HttpPost]
        public ActionResult ListarAcesso(string matricula, int? pagina)
        {
            var lista = Usuario.ListarPorMatricula(matricula)?.UsuarioAcesso.OrderByDescending(a => a.DtAcesso);

            var qte = 10;

            var usuarios = Usuarios;

            pagina = pagina ?? 1;

            return PartialView("_ListaAcesso", lista.Skip((qte * pagina.Value) - qte).Take(qte).ToList());
        }
    }
}