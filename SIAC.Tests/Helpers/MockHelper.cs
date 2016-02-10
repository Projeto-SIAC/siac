using SIAC.Helpers;
using SIAC.Models;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace SIAC.Tests
{
    public class MockHelper
    {
        public static HttpContext FakeHttpContext(string url)
        {
            var uri = new Uri(url);
            var httpRequest = new HttpRequest(string.Empty, uri.ToString(), uri.Query.TrimStart('?'));
            var stringWriter = new StringWriter();
            var httpResponse = new HttpResponse(stringWriter);
            var httpContext = new HttpContext(httpRequest, httpResponse);

            var sessionContainer = new HttpSessionStateContainer("id", new SessionStateItemCollection(),
                                                    new HttpStaticObjectsCollection(), 10, true,
                                                    HttpCookieMode.AutoDetect,
                                                    SessionStateMode.InProc, false);

            SessionStateUtility.AddHttpSessionStateToContext(
                                         httpContext, sessionContainer);

            return httpContext;
        }

        public static void FakeLoginUsuario(string matricula, string path)
        {
            HttpContext.Current = FakeHttpContext(String.Format("http://siac.apphb.com{0}", path));
            Usuario usuario = Repositorio.GetInstance().Usuario.Find(matricula);
            Sessao.Inserir("UsuarioMatricula", usuario.Matricula);
            Sessao.Inserir("UsuarioNome", usuario.PessoaFisica.Nome);
            Sessao.Inserir("UsuarioCategoriaCodigo", usuario.CodCategoria);
            Sessao.Inserir("UsuarioCategoria", usuario.Categoria.Descricao);
            Sistema.UsuarioAtivo[matricula] = usuario.UsuarioAcesso.Last();
        }
    }
}
