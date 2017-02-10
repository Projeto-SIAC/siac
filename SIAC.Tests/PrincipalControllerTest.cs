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
using SIAC.Models;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SIAC.Tests
{
    [TestClass]
    public class PrincipalControllerTest : BaseControllerTest
    {
        private PrincipalController controller;

        [TestInitialize]
        public override void SetupTest()
        {
            base.SetupTest();
            this.controller = new PrincipalController();
            var context = new ControllerContext(this.moqHttpContext.Object, new RouteData(), this.controller);
            this.controller.ControllerContext = context;
        }

        [TestMethod]
        public void TestAvaliacaoRedirect()
        {
            string esperado = "Index";
            RedirectToRouteResult resultado = controller.Avaliacao() as RedirectToRouteResult;
            Assert.AreEqual(esperado, resultado.RouteValues["Action"]);
        }

    }
}
