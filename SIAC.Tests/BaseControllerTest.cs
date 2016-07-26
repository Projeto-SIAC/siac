using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SIAC.Models;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SIAC.Tests
{
    [TestClass]
    public class BaseControllerTest
    {
        protected Mock<HttpContextBase> moqHttpContext;
        protected Mock<HttpRequestBase> moqRequest;
        protected Mock<RequestContext> moqRequestContext;
        protected Mock<HttpResponseBase> moqResponse;
        protected Mock<HttpSessionStateBase> moqSession;
        protected Mock<HttpServerUtilityBase> moqServer;
        protected Mock<UrlHelper> moqUrlHelper;

        [TestInitialize]
        public virtual void SetupTest()
        {
            moqHttpContext = new Mock<HttpContextBase>();
            moqRequest = new Mock<HttpRequestBase>();
            moqResponse = new Mock<HttpResponseBase>();
            moqSession = new Mock<HttpSessionStateBase>();
            moqServer = new Mock<HttpServerUtilityBase>();
            moqUrlHelper = new Mock<UrlHelper>();
            moqRequestContext = new Mock<RequestContext>();

            moqRequestContext.Setup(x => x.HttpContext).Returns(moqHttpContext.Object);

            moqHttpContext.Setup(x => x.Request).Returns(moqRequest.Object);
            moqHttpContext.Setup(x => x.Response).Returns(moqResponse.Object);
            moqHttpContext.Setup(x => x.Session).Returns(moqSession.Object);
            moqHttpContext.Setup(x => x.Server).Returns(moqServer.Object);

            moqRequest.Setup(x => x.Url).Returns(new Uri("http://localhost"));
            moqRequest.Setup(x => x.RequestContext).Returns(moqRequestContext.Object);
            moqRequest.Setup(x => x.ServerVariables).Returns(new NameValueCollection
            {
                {"SERVER_NAME", "localhost"},
                {"SCRIPT_NAME", "localhost"},
                {"SERVER_PORT", "80"},
                {"REMOTE_ADDR", "127.0.0.1"},
                {"REMOTE_HOST", "127.0.0.1"}
            });
            moqRequest.Setup(x => x.QueryString).Returns(new NameValueCollection());
            moqRequest.Setup(x => x.Headers).Returns(new NameValueCollection());

            moqResponse.Setup(x => x.Cookies).Returns(new HttpCookieCollection());

            HttpContextManager.SetCurrentContext(moqHttpContext.Object);
        }

        public void Login(string matricula)
        {
            Usuario usuario = Repositorio.GetInstance().Usuario.Find(matricula);
            moqSession.SetupGet(x => x["UsuarioMatricula"]).Returns(usuario.Matricula);
            moqSession.SetupGet(x => x["UsuarioNome"]).Returns(usuario.PessoaFisica.Nome);
            moqSession.SetupGet(x => x["UsuarioCategoriaCodigo"]).Returns(usuario.CodCategoria);
            moqSession.SetupGet(x => x["UsuarioCategoria"]).Returns(usuario.Categoria.Descricao);
            Sistema.UsuarioAtivo[matricula] = usuario.UsuarioAcesso.Last();
        }
    }
}