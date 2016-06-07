using System.Linq;
using System.Web.Mvc;

namespace SIAC.Filters
{
    public class AutenticacaoFilterAttribute : ActionFilterAttribute
    {
        public int[] Categorias { get; set; }
        public int[] Ocupacoes { get; set; }
        public bool CoordenadoresAvi { get; set; } = false;

        private ActionResult Redirecionar(ActionExecutingContext filterContext, string url = null)
        {
            if (filterContext.HttpContext.Request.HttpMethod == "GET")
            {
                if (string.IsNullOrEmpty(url))
                {
                    return new RedirectResult("~/?continuar=" + filterContext.HttpContext.Request.Path);
                }
                else
                {
                    return new RedirectResult(url);
                }
            }
            else
            {
                return new JsonResult();
            }
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var autenticado = Models.Sistema.Autenticado(Helpers.Sessao.UsuarioMatricula);

            if (!autenticado)
            {
                filterContext.Result = Redirecionar(filterContext);
            }
            else if (Helpers.Sessao.UsuarioCategoriaCodigo == Models.Categoria.SUPERUSUARIO)
            {
                // faça nada
            }
            else if (Helpers.Sessao.UsuarioCategoriaCodigo == Models.Categoria.VISITANTE && Helpers.Sessao.UsuarioSenhaPadrao)
            {
                filterContext.Result = Redirecionar(filterContext, "~/acesso/visitante");
            }
            else if (Categorias != null && !Categorias.Contains(Helpers.Sessao.UsuarioCategoriaCodigo))
            {
                filterContext.Result = Redirecionar(filterContext);
            }
            else if (Ocupacoes != null && !Ocupacoes.ContainsOne(Models.Sistema.UsuarioAtivo[Helpers.Sessao.UsuarioMatricula].Usuario.Ocupacao.Select(a => a.CodOcupacao).ToArray()))
            {
                filterContext.Result = Redirecionar(filterContext);
            }
            else if (CoordenadoresAvi && !Models.Parametro.Obter().OcupacaoCoordenadorAvi.ContainsOne(Models.Sistema.UsuarioAtivo[Helpers.Sessao.UsuarioMatricula].Usuario.Ocupacao.Select(a => a.CodOcupacao).ToArray()))
            {
                filterContext.Result = Redirecionar(filterContext);
            }
            else if (Helpers.Sessao.RealizandoAvaliacao)
            {
                string[] paths = filterContext.HttpContext.Request.Path.ToLower().Split('/');
                if (paths.Length > 0)
                {
                    string codigo = paths[paths.Length - 1];
                    if (paths.Contains("realizar"))
                    {
                        if (Models.Sistema.AvaliacaoUsuario.ContainsKey(codigo))
                        {
                            if (Models.Sistema.AvaliacaoUsuario[codigo].Contains(Helpers.Sessao.UsuarioMatricula))
                            {
                                filterContext.Result = new RedirectResult("~/erro/1");
                            }
                        }
                    }
                    else if (filterContext.HttpContext.Request.HttpMethod == "GET")
                    {
                        filterContext.Result = new RedirectResult("~/erro/1");
                    }
                }
                else
                {
                    filterContext.Result = new RedirectResult("~/erro/1");
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}