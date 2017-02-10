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
using System;
using System.Web.Mvc;
using SIAC.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SIAC.ViewModels;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;

namespace SIAC.Tests
{
    [TestClass]
    public class AcessoControllerTest : BaseControllerTest
    {
        private AcessoController controller;

        [TestInitialize]
        public override void SetupTest()
        {
            base.SetupTest();
            this.controller = new AcessoController();
            var context = new ControllerContext(this.moqHttpContext.Object, new RouteData(), this.controller);
            this.controller.ControllerContext = context;           
        }


        [TestMethod]
        public async Task TestIndexComValoresVazios()
        {
            FormCollection form = new FormCollection();
            form["txtMatricula"] = String.Empty;
            form["txtSenha"] = String.Empty;
            AcessoIndexViewModel resultado = (await controller.Index(form) as ViewResult).Model as AcessoIndexViewModel;
            Assert.AreEqual(true, resultado.Erro);
        }

        [TestMethod]
        public async Task TestIndexComValoresNulos()
        {
            FormCollection form = new FormCollection();
            form["txtMatricula"] = null;
            form["txtSenha"] = null;
            AcessoIndexViewModel resultado = (await controller.Index(form) as ViewResult).Model as AcessoIndexViewModel;
            Assert.AreEqual(true, resultado.Erro);
        }


        [TestMethod]
        public async Task TestIndexComValoresEspaco()
        {
            FormCollection form = new FormCollection();
            form["txtMatricula"] = " ";
            form["txtSenha"] = " ";
            AcessoIndexViewModel resultado = (await controller.Index(form) as ViewResult).Model as AcessoIndexViewModel;
            Assert.AreEqual(true, resultado.Erro);
        }

        [TestMethod]
        public async Task TestIndexComFormularioSemChaves()
        {
            FormCollection form = new FormCollection();
            AcessoIndexViewModel resultado = (await controller.Index(form) as ViewResult).Model as AcessoIndexViewModel;
            Assert.AreEqual(true, resultado.Erro);
        }
        
        [TestMethod]
        public void TestSairRedirecionamento()
        {
            RedirectResult resultado = controller.Sair() as RedirectResult;
            Assert.AreEqual("~/", resultado.Url);
        }
    }
}
