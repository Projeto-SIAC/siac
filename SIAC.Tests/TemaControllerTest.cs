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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SIAC.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SIAC.Tests
{
    [TestClass]
    public class TemaControllerTest
    {
        TemaController controller = new TemaController();

        [TestMethod]
        public void TestIndexRedirecionamento()
        {
            RedirectToRouteResult resultado = controller.Index() as RedirectToRouteResult;
            Assert.AreEqual("Acesso", resultado.RouteValues["controller"]);
        }

        [TestMethod]
        public void TestRecuperarTemasPorCodDisciplinaZero()
        {
            JsonResult resultado = controller.RecuperarTemasPorCodDisciplina("0") as JsonResult;
            Assert.AreEqual(null, resultado.Data);
        }

        [TestMethod]
        public void TestRecuperarTemasPorCodDisciplinaVazio()
        {
            JsonResult resultado = controller.RecuperarTemasPorCodDisciplina("") as JsonResult;
            Assert.AreEqual(null, resultado.Data);
        }

        [TestMethod]
        public void TestRecuperarTemasPorCodDisciplinaNulo()
        {
            JsonResult resultado = controller.RecuperarTemasPorCodDisciplina(null) as JsonResult;
            Assert.AreEqual(null, resultado.Data);
        }

        [TestMethod]
        public void TestRecuperarTemasPorCodDisciplinaTemQuestaoZero()
        {
            JsonResult resultado = controller.RecuperarTemasPorCodDisciplina("0") as JsonResult;
            Assert.AreEqual(null, resultado.Data);
        }

        [TestMethod]
        public void TestRecuperarTemasPorCodDisciplinaTemQuestaoVazio()
        {
            JsonResult resultado = controller.RecuperarTemasPorCodDisciplina("") as JsonResult;
            Assert.AreEqual(null, resultado.Data);
        }

        [TestMethod]
        public void TestRecuperarTemasPorCodDisciplinaTemQuestaoNulo()
        {
            JsonResult resultado = controller.RecuperarTemasPorCodDisciplina(null) as JsonResult;
            Assert.AreEqual(null, resultado.Data);
        }
    }
}
