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
using System.Web.Mvc;

namespace SIAC.Tests
{
    [TestClass]
    public class ImpressaoControllerTest
    {
        ImpressaoController controller = new ImpressaoController();

        [TestMethod]
        public void TestIndexRedirecionamento()
        {
            RedirectToRouteResult resultado = controller.Index() as RedirectToRouteResult;
            Assert.AreEqual("Principal", resultado.RouteValues["controller"]);
        }

        [TestMethod]
        public void TestInstitucionalComCodigoNulo()
        {
            RedirectToRouteResult resultado = controller.Institucional(null) as RedirectToRouteResult;
            Assert.AreEqual("Principal", resultado.RouteValues["controller"]);
        }


        [TestMethod]
        public void TestAvaliacaoComCodigoNulo()
        {
            RedirectToRouteResult resultado = controller.Avaliacao(null) as RedirectToRouteResult;
            Assert.AreEqual("Principal", resultado.RouteValues["controller"]);
        }

        [TestMethod]
        public void TestInstitucionalComCodigoVazio()
        {
            RedirectToRouteResult resultado = controller.Institucional("") as RedirectToRouteResult;
            Assert.AreEqual("Principal", resultado.RouteValues["controller"]);
        }

        [TestMethod]
        public void TestAvaliacaoComCodigoVazio()
        {
            RedirectToRouteResult resultado = controller.Avaliacao("") as RedirectToRouteResult;
            Assert.AreEqual("Principal", resultado.RouteValues["controller"]);
        }
    }
}
