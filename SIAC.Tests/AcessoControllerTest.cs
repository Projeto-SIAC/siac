using System;
using System.Web.Mvc;
using SIAC.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SIAC.Tests
{
    [TestClass]
    public class AcessoControllerTest
    {
        [TestMethod]
        public void TestIndexWithEmptyValues()
        {
            var controller = new AcessoController();

            FormCollection form = new FormCollection();
            form["txtMatricula"] = String.Empty;
            form["txtSenha"] = String.Empty;

            var result = controller.Index(form) as ViewResult;

            Assert.AreEqual("Index", result.ViewName);
        }

        [TestMethod]
        public void TestIndexWithNullValues()
        {
            var controller = new AcessoController();

            FormCollection form = new FormCollection();
            form["txtMatricula"] = null;
            form["txtSenha"] = null;

            var result = controller.Index(form) as ViewResult;

            Assert.AreEqual("Index", result.ViewName);
        }


        [TestMethod]
        public void TestIndexWithWhiteSpaceValues()
        {
            var controller = new AcessoController();

            FormCollection form = new FormCollection();
            form["txtMatricula"] = " ";
            form["txtSenha"] = " ";

            var result = controller.Index(form) as ViewResult;

            Assert.AreEqual("Index", result.ViewName);
        }

        [TestMethod]
        public void TestIndexWithFormHasNotKeys()
        {
            var controller = new AcessoController();

            FormCollection form = new FormCollection();

            var result = controller.Index(form) as ViewResult;

            Assert.AreEqual("Index", result.ViewName);
        }

        [TestMethod]
        public void TestSairForRedirect()
        {
            var controller = new AcessoController();
            var result = controller.Sair() as RedirectResult;
            Assert.AreEqual("~/", result.Url);
        }
    }
}
