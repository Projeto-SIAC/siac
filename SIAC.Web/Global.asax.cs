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
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ViewEngines.Engines.Add(new SIACViewEngine());
            Models.Parametro.Obter();
        }

        protected void Session_End(object sender, EventArgs e)
        {
            Models.Sistema.MatriculaAtivo.Remove((string)Session["UsuarioMatricula"]);
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
