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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SIAC.Tests
{
    [TestClass]
    public class HistoricoControllerTest
    {
        HistoricoController controller = new HistoricoController();

        [TestMethod]
        public void TestAvaliacaoRedirect()
        {
            string esperado = "Index";
            RedirectToRouteResult resultado = controller.Avaliacao() as RedirectToRouteResult;
            Assert.AreEqual(esperado, resultado.RouteValues["Action"]);
        }
    }
}
