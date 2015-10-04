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
                name: "QuestaoDetalhe",
                url: "Historico/Questao/{codigo}",
                defaults: new { controller = "Questao", action = "Detalhe", codigo = UrlParameter.Optional },
                constraints: new { controller = @"^(Questao)$", codigo = @"^[0-9]+$" }
            );
            
            routes.MapRoute(
                name: "HistoricoAvaliacao",
                url: "Historico/Avaliacao/{controller}/{action}/{codigo}",
                defaults: new { controller = "Historico", action = "Index", codigo = UrlParameter.Optional },
                constraints: new { controller = @"^(Academica|Reposicao|Certificacao|Institucional)$" }
            );

            routes.MapRoute(
                name: "Historico",
                url: "Historico/{controller}/{action}/{codigo}",
                defaults: new { controller = "Historico", action = "Index", codigo = UrlParameter.Optional },
                constraints: new { controller = @"^(Questao|Autoavaliacao)$" }
            );


            routes.MapRoute(
                name: "DashboardAvaliacao",
                url: "Dashboard/Avaliacao/{controller}/{action}/{codigo}",
                defaults: new { controller = "Dashboard", action = "Index", codigo = UrlParameter.Optional },
                constraints: new { controller = @"^(Academica|Reposicao|Certificacao|Institucional)$" }
            );

            routes.MapRoute(
                name: "Dashboard",
                url: "Dashboard/{controller}/{action}/{codigo}",
                defaults: new { controller = "Dashboard", action = "Index", codigo = UrlParameter.Optional },
                constraints: new { controller = @"^(Questao|Autoavaliacao)$" }
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
                constraints: new { controller = @"^(Dashboard|Acesso|Erro|Configuracoes)$" }
            );

            routes.MapRoute(
                name: "RecuperarTemasPorCodDisciplina",
                url: "Tema/RecuperarTemasPorCodDisciplina/",
                defaults: new { controller = "Tema", action = "RecuperarTemasPorCodDisciplina" },
                namespaces: new[] { "SIAC.Web.Controllers" }
            );

            routes.MapRoute(
                name: "RecuperarTemasPorCodDisciplinaTemQuestao",
                url: "Tema/RecuperarTemasPorCodDisciplinaTemQuestao/",
                defaults: new { controller = "Tema", action = "RecuperarTemasPorCodDisciplinaTemQuestao" },
                namespaces: new[] { "SIAC.Web.Controllers" }
            );

            routes.MapRoute(
                name: "LeroLero",
                url: "LeroLero",
                defaults: new { controller = "LeroLero", action = "Index" },
                namespaces: new[] { "SIAC.Web.Controllers" }
            );

            routes.MapRoute(
                name: "PalavrasChave",
                url: "PalavrasChave",
                defaults: new { controller = "Questao", action = "PalavrasChave" },
                namespaces: new[] { "SIAC.Web.Controllers" }
            );

            routes.MapRoute(
                name: "Controle Não Encontrado",
                url: "{*all}",
                defaults: new { controller = "Erro", action = "Index", code = "404" }
            );
        }
    }
}
