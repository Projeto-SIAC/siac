using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SIAC.Controllers;
using SIAC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace SIAC.Tests
{
    [TestClass]
    public class PerfilControllerTest : BaseControllerTest
    {
        private PerfilController controller;

        [TestInitialize]
        public override void SetupTest()
        {
            base.SetupTest();
            this.controller = new PerfilController();
            var context = new ControllerContext(this.moqHttpContext.Object, new RouteData(), this.controller);
            this.controller.ControllerContext = context;
        }

        [TestMethod]
        public void TestIndexModel()
        {
            string esperado = "200";
            this.Login(esperado);
            Usuario resultado = (controller.Index() as ViewResult).Model as Usuario;
            Assert.AreEqual(esperado, resultado.Matricula);
        }

        [TestMethod]
        public void TestEstatisticasModel()
        {
            string esperado = "200";
            this.Login(esperado);
            Usuario resultado = (controller.Estatisticas() as PartialViewResult).Model as Usuario;
            Assert.AreEqual(esperado, resultado.Matricula);
        }

        [TestMethod]
        public void TestEnviarOpiniaoEmpty()
        {
            string matricula = "200";
            this.Login(matricula);
            Contexto c = Repositorio.GetInstance();
            Usuario usuario = c.Usuario.Find(matricula);
            int quantidade = usuario.UsuarioOpiniao.Count;
            controller.EnviarOpiniao("");
            Usuario novoUsuario = c.Usuario.Find(matricula);
            int novaQuantidade = novoUsuario.UsuarioOpiniao.Count;
            Assert.AreEqual(quantidade, novaQuantidade);
        }

        [TestMethod]
        public void TestEnviarOpiniao()
        {
            var moqUsuario = new Mock<Usuario>();
            var moqUsuarioAcesso = new Mock<UsuarioAcesso>();
            var moqUsuarioOpiniao = new Mock<ICollection<UsuarioOpiniao>>();

            moqUsuarioAcesso.Setup(x => x.Usuario).Returns(moqUsuario.Object);
            moqUsuario.Setup(x => x.UsuarioOpiniao).Returns(moqUsuarioOpiniao.Object);

            string matricula = "200";
            this.Login(matricula);
            Sistema.UsuarioAtivo[matricula] = moqUsuarioAcesso.Object;

            controller.EnviarOpiniao("Oi, minha opinião.");

            moqUsuarioOpiniao.Verify(x => x.Add(It.IsAny<UsuarioOpiniao>()), Times.Once());
        }
    }
}
