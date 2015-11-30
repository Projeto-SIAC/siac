﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIAC.Filters
{
    public class AutenticacaoFilterAttribute : ActionFilterAttribute
    {
        public int[] Categorias { get; set; }
        public int[] Ocupacoes { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var autenticado = Models.Sistema.Autenticado(Helpers.Sessao.UsuarioMatricula);
                   
            if (!autenticado)
            {
                filterContext.Result = new RedirectResult("~/?continuar="+filterContext.HttpContext.Request.Path);
            }
            else if (Categorias != null && !Categorias.Contains(Helpers.Sessao.UsuarioCategoriaCodigo))
            {
                filterContext.Result = new RedirectResult("~/?continuar="+filterContext.HttpContext.Request.Path);
            }
            else if (Ocupacoes != null && !Ocupacoes.ContainsOne(Models.Sistema.UsuarioAtivo[Helpers.Sessao.UsuarioMatricula].Usuario.PessoaFisica.Ocupacao.Select(a=>a.CodOcupacao).ToArray()))
            {
                filterContext.Result = new RedirectResult("~/?continuar=" + filterContext.HttpContext.Request.Path);
            }
            else if (Helpers.Sessao.RealizandoAvaliacao)
            {
                string[] paths = filterContext.HttpContext.Request.Path.ToLower().Split('/');
                if (paths.Length > 0)
                {
                    string codigo = paths[paths.Length - 1];
                    if ((!paths.Contains("printar") && !paths.Contains("desistir") && !paths.Contains("resultado") && !paths.Contains("realizar")) && codigo != Helpers.Sessao.UsuarioAvaliacao) 
                    {
                        // como sei se é acadêmica ou certificacao ou reposicao? '-'
                        filterContext.Result = new RedirectResult("~/dashboard/avaliacao/academica/realizar/" + Helpers.Sessao.UsuarioAvaliacao);
                    }
                }
                else
                {
                    filterContext.Result = new RedirectResult("~/dashboard/avaliacao/academica/realizar/" + Helpers.Sessao.UsuarioAvaliacao);
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}