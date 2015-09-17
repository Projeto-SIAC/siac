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
                name: "Configuracoes",
                url: "Configuracoes/{action}",
                defaults: new { controller = "Configuracoes", action = "Index" }
            );

            routes.MapRoute(
                name: "Questao",
                url: "Dashboard/Questao/{codigo}",
                defaults: new { controller = "Questao", action = "Detalhe", codigo = UrlParameter.Optional },
                constraints: new { controller = @"^(Questao)$", codigo = @"^[0-9]+$" }
            );

            routes.MapRoute(
                name: "Dashboard",
                url: "Dashboard/{controller}/{action}/{codigo}",
                defaults: new { controller = "Dashboard", action = "Index", codigo = UrlParameter.Optional },
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

            routes.MapRoute(
                name: "RecuperarTemasPorCodDisciplina",
                url: "Questao/RecuperarTemasPorCodDisciplina/",
                defaults: new { controller = "Questao", action = "RecuperarTemasPorCodDisciplina" },
                namespaces: new[] { "SIAC.Web.Controllers" }
            );
            
            routes.MapRoute(
                name: "LeroLero",
                url: "LeroLero",
                defaults: new { controller = "LeroLero", action = "Index" },
                namespaces: new[] { "SIAC.Web.Controllers" }
            );
        }
    }
}
