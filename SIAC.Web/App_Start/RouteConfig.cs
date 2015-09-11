using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SIAC.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Dashboard",
                url: "Dashboard/{controller}/{action}/{id}",
                defaults: new { controller = "Dashboard", action = "Index", id = UrlParameter.Optional },
                constraints: new { controller = @"^(Questao)$" }
            );

            routes.MapRoute(
                name: "Acesso",
                url: "Acesso/{action}",
                defaults: new { controller = "Acesso", action = "Index" }
            );

            routes.MapRoute(
                name: "Erro",
                url: "Erro/{code}",
                defaults: new { controller = "Erro", action = "Index", code = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                defaults: new { controller = "Acesso", action = "Index" },
                constraints: new { controller = @"^(Dashboard|Acesso|Erro)$" }
            );
        }
    }
}
