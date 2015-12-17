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
        public void TestIndexComValoresVazios()
        {
            var controller = new AcessoController();

            FormCollection form = new FormCollection();
            form["txtMatricula"] = String.Empty;
            form["txtSenha"] = String.Empty;

            var result = (controller.Index(form) as ViewResult).Model as ViewModels.AcessoIndexViewModel;

            Assert.AreEqual(true, result.Erro);
        }

        [TestMethod]
        public void TestIndexComValoresNulos()
        {
            var controller = new AcessoController();

            FormCollection form = new FormCollection();
            form["txtMatricula"] = null;
            form["txtSenha"] = null;

            var result = (controller.Index(form) as ViewResult).Model as ViewModels.AcessoIndexViewModel;

            Assert.AreEqual(true, result.Erro);
        }


        [TestMethod]
        public void TestIndexComValoresEspaco()
        {
            var controller = new AcessoController();

            FormCollection form = new FormCollection();
            form["txtMatricula"] = " ";
            form["txtSenha"] = " ";

            var result = (controller.Index(form) as ViewResult).Model as ViewModels.AcessoIndexViewModel;

            Assert.AreEqual(true, result.Erro);
        }

        [TestMethod]
        public void TestIndexComFormularioSemChaves()
        {
            var controller = new AcessoController();

            FormCollection form = new FormCollection();

            var result = (controller.Index(form) as ViewResult).Model as ViewModels.AcessoIndexViewModel;

            Assert.AreEqual(true, result.Erro);
        }

        [TestMethod]
        public void TestSairRedirecionamento()
        {
            var controller = new AcessoController();
            var result = controller.Sair() as RedirectResult;
            Assert.AreEqual("~/", result.Url);
        }
    }
}
