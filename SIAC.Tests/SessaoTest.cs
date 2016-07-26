using Microsoft.VisualStudio.TestTools.UnitTesting;
using SIAC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIAC.Tests
{
    [TestClass]
    public class SessaoTest : BaseControllerTest
    {
        [TestInitialize]
        public override void SetupTest()
        {
            base.SetupTest();
            this.moqSession.SetupGet(x => x["UsuarioMatricula"]).Returns("foo");
            this.moqSession.SetupGet(x => x["UsuarioNome"]).Returns("foo");
            this.moqSession.SetupGet(x => x["UsuarioCategoriaCodigo"]).Returns(5);
            this.moqSession.SetupGet(x => x["UsuarioCategoria"]).Returns("bar");
            this.moqSession.SetupGet(x => x["AjudaEstado"]).Returns(false);
            this.moqSession.SetupGet(x => x["Apresentacao"]).Returns(true);
            this.moqSession.SetupGet(x => x["SimuladoCandidato"]).Returns(new Candidato() { Email = "foo@bar.com" });
            this.moqSession.SetupGet(x => x["UsuarioSenhaPadrao"]).Returns(true);
        }

        [TestMethod]
        public void TestPropriedades()
        {
            Assert.AreEqual(Helpers.Sessao.UsuarioMatricula, "foo");
            Assert.AreEqual(Helpers.Sessao.UsuarioNome, "foo");
            Assert.AreEqual(Helpers.Sessao.UsuarioCategoria, "bar");
            Assert.AreEqual(Helpers.Sessao.UsuarioCategoriaCodigo, 5);
            Assert.AreEqual(Helpers.Sessao.AjudaEstado, false);
            Assert.AreEqual(Helpers.Sessao.Apresentacao, true);
            Assert.AreEqual(Helpers.Sessao.Candidato.Email, "foo@bar.com");
            Assert.AreEqual(Helpers.Sessao.UsuarioSenhaPadrao, true);
        }

        [TestMethod]
        public void TestRetornar()
        {
            Assert.AreEqual(Helpers.Sessao.Retornar("UsuarioMatricula") as string, "foo");
        }
    }
}
