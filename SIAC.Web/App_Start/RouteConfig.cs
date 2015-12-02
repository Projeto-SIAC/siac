using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SIAC
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.LowercaseUrls = true;
            
            routes.MapRoute(
                name: "Institucional",
                url:"Institucional/{action}/{codigo}",
                defaults: new { controller = "Institucional", action = "Index", codigo = UrlParameter.Optional },
                constraints: new { controller = @"^(Institucional)$" }
            );

            routes.MapRoute(
                name: "HistoricoAvaliacao",
                url: "Historico/Avaliacao/{controller}/{action}/{codigo}",
                defaults: new { controller = "Historico", action = "Index", codigo = UrlParameter.Optional },
                constraints: new { controller = @"^(Academica|Reposicao|Certificacao)$", action = @"^(Index|Minhas|Detalhe|Agendada|Pendente|Corrigir)$" }
            );

            routes.MapRoute(
                name: "Historico",
                url: "Historico/{controller}/{action}/{codigo}",
                defaults: new { controller = "Historico", action = "Index", codigo = UrlParameter.Optional },
                constraints: new { controller = @"^(Questao|Autoavaliacao)$", action = @"^(Index|Detalhe|Editar|Listar)$" }
            );


            routes.MapRoute(
                name: "DashboardAvaliacao",
                url: "Dashboard/Avaliacao/{controller}/{action}/{codigo}",
                defaults: new { controller = "Dashboard", action = "Index", codigo = UrlParameter.Optional },
                constraints: new { controller = @"^(Academica|Reposicao|Certificacao)$" }
            );

            routes.MapRoute(
                name: "Dashboard",
                url: "Dashboard/{controller}/{action}/{codigo}",
                defaults: new { controller = "Dashboard", action = "Index", codigo = UrlParameter.Optional },
                constraints: new { controller = @"^(Questao|Autoavaliacao|Agenda|Impressao)$" }
            );


            routes.MapRoute(
                name: "Configuracoes",
                url: "Configuracoes/{controller}/{action}/{codigo}",
                defaults: new { controller = "Configuracoes", action = "Index", codigo = UrlParameter.Optional },
                constraints: new { controller = @"^(Visitante|Usuario)$" }
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
                url: "{controller}/{action}/{tab}",
                defaults: new { controller = "Acesso", action = "Index", tab = UrlParameter.Optional },
                constraints: new { controller = @"^(Dashboard|Historico|Institucional|Perfil|Acesso|Erro|Configuracoes|Tema)$" }
            );           

            routes.MapRoute(
                name: "LeroLero",
                url: "LeroLero",
                defaults: new { controller = "LeroLero", action = "Index" },
                namespaces: new[] { "SIAC.Controllers" }
            );

            routes.MapRoute(
                name: "Controle Não Encontrado",
                url: "{*all}",
                defaults: new { controller = "Erro", action = "Index", code = "404" }
            );
        }
    }
}
