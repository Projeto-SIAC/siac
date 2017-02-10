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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIAC.Controllers
{
    [Filters.AutenticacaoFilter]
    public class TemaController : Controller
    {
        // GET: Tema
        public ActionResult Index() => RedirectToAction("Index", "Acesso");

        [HttpPost]
        public ActionResult RecuperarTemasPorCodDisciplina(string codDisciplina)
        {
            if (!String.IsNullOrEmpty(codDisciplina))
            {
                int cod = 0;
                if (int.TryParse(codDisciplina, out cod) && cod > 0)
                {
                    var temas = Tema.ListarPorDisciplina(cod);
                    var result = from t in temas select new { CodTema = t.CodTema, Descricao = t.Descricao };
                    return Json(result.ToList());
                }
            }
            return Json(null);
        }

        [HttpPost]
        public ActionResult RecuperarTemasPorCodDisciplinaTemQuestao(string codDisciplina)
        {
            if (!String.IsNullOrEmpty(codDisciplina))
            {
                int cod = 0;
                if (int.TryParse(codDisciplina, out cod) && cod > 0)
                {
                    var temas = Tema.ListarPorDisciplinaTemQuestao(cod);
                    var result = from t in temas select new { CodTema = t.CodTema, Descricao = t.Descricao };
                    return Json(result.ToList());
                }
            }
            return Json(null);
        }
    }
}