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
            routes.MapMvcAttributeRoutes();

            routes.MapRoute(
                name: "AreaDeSimulados",
                url: "Simulado/{controller}/{action}/{codigo}",
                defaults: new { controller = "Gerencia", action = "Index", codigo = UrlParameter.Optional },
                constraints: new { controller = @"^(Gerencia|Inscricao|Candidato)$" }
            );

            //routes.MapRoute(
            //    name: "Simulado",
            //    url: "Simulado/{action}/{codigo}",
            //    defaults: new { controller = "Simulado", action = "Index", codigo = UrlParameter.Optional }
            //);

            routes.MapRoute(
                name: "Institucional",
                url: "Institucional/{action}/{codigo}",
                defaults: new { controller = "Institucional", action = "Index", codigo = UrlParameter.Optional },
                constraints: new { controller = @"^(Institucional)$" }
            );

            routes.MapRoute(
                name: "HistoricoAvaliacao",
                url: "Historico/Avaliacao/{controller}/{action}/{codigo}",
                defaults: new { controller = "Historico", action = "Index", codigo = UrlParameter.Optional },
                constraints: new { controller = @"^(Academica|Reposicao|Certificacao)$", action = @"^(Index|Minhas|Detalhe|Agendada|Corrigir)$" }
            );

            routes.MapRoute(
                name: "Historico",
                url: "Historico/{controller}/{action}/{codigo}",
                defaults: new { controller = "Historico", action = "Index", codigo = UrlParameter.Optional },
                constraints: new { controller = @"^(Questao|Autoavaliacao)$", action = @"^(Index|Detalhe|Editar|Listar)$" }
            );

            routes.MapRoute(
                name: "PrincipalAvaliacao",
                url: "Principal/Avaliacao/{controller}/{action}/{codigo}",
                defaults: new { controller = "Principal", action = "Index", codigo = UrlParameter.Optional },
                constraints: new { controller = @"^(Academica|Reposicao|Certificacao)$" }
            );

            routes.MapRoute(
                name: "Principal",
                url: "Principal/{controller}/{action}/{codigo}",
                defaults: new { controller = "Principal", action = "Index", codigo = UrlParameter.Optional },
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
                constraints: new { controller = @"^(Principal|Historico|Institucional|Perfil|Acesso|Erro|Configuracoes|Tema)$" }
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
                defaults: new { controller = "Erro", action = "Index", code = 404 }
            );
        }
    }
}