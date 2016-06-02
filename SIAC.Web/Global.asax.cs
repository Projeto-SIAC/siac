using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SIAC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleTable.EnableOptimizations = false;
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ViewEngines.Engines.Add(new SIACViewEngine());
            Models.Parametro.Obter();
        }

        protected void Session_End(object sender, EventArgs e)
        {
            string matricula = (string)Session["UsuarioMatricula"] ?? "";
            if (!String.IsNullOrWhiteSpace(matricula))
            {
                Models.Sistema.UsuarioAtivo.Remove(matricula);
                Models.Sistema.RemoverCookie(matricula);
                Models.Sistema.Notificacoes.Remove(matricula);
                Hubs.LembreteHub.Limpar(matricula);
            }
        }

        protected void Application_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            if (Context.Handler is System.Web.SessionState.IRequiresSessionState || Context.Handler is System.Web.SessionState.IReadOnlySessionState)
            {
                if (Helpers.Sessao.UsuarioMatricula != null && Models.Sistema.UsuarioAtivo.Keys.Contains(Helpers.Sessao.UsuarioMatricula))
                {
                    if (!HttpContext.Current.Request.Path.ToLower().Contains("lembrete"))
                    {
                        var acesso = Models.Sistema.UsuarioAtivo[Helpers.Sessao.UsuarioMatricula];
                        var acessos = acesso.UsuarioAcessoPagina;
                        int numIdentificador = acessos.Count > 0 ? acesso.UsuarioAcessoPagina.Max(a => a.NumIdentificador) : 0;
                        acesso.UsuarioAcessoPagina.Add(new Models.UsuarioAcessoPagina()
                        {
                            NumIdentificador = numIdentificador + 1,
                            Pagina = HttpContext.Current.Request.Url.PathAndQuery.ToString(),
                            DtAbertura = DateTime.Now,
                            PaginaReferencia = HttpContext.Current.Request.UrlReferrer?.PathAndQuery.ToString(),
                            Dados = HttpContext.Current.Request.Form.HasKeys() ? HttpContext.Current.Request.Form.ToString() : null
                        });
                        Models.Repositorio.GetInstance().SaveChanges(false);
                    }
                }
            }
        }

        public override string GetVaryByCustomString(HttpContext context, string custom)
        {
            if (custom.ToLower() == "usuario")
            {
                var cookie = context.Request.Cookies["SIAC_Session"];
                if (cookie != null && Models.Sistema.CookieUsuario.ContainsKey(cookie.Value))
                    return "usuario=" + Models.Sistema.CookieUsuario[cookie.Value];
            }
            return base.GetVaryByCustomString(context, custom);
        }
    }

    public class SIACViewEngine : RazorViewEngine
    {
        private static string[] NewPartialViewFormats = new[] {
            "~/Views/{1}/Partials/{0}.cshtml",
            "~/Views/Shared/Partials/{0}.cshtml"
        };

        public SIACViewEngine()
        {
            base.PartialViewLocationFormats = base.PartialViewLocationFormats.Union(NewPartialViewFormats).ToArray();
        }
    }
}