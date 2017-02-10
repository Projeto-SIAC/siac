/*
This file is part of SIAC.

Copyright (C) 2016 Felipe Mateus Freire Pontes <felipemfpontes@gmail.com>
Copyright (C) 2016 Francisco Bento da Silva Júnior <francisco.bento.jr@hotmail.com>

SIAC is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details. 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIAC.Filters
{
    public class CandidatoFilterAttribute : ActionFilterAttribute
    {
        private ActionResult Redirecionar(ActionExecutingContext filterContext, string url = null)
        {
            if (filterContext.HttpContext.Request.HttpMethod == "GET")
            {
                if (string.IsNullOrEmpty(url))
                {
                    return new RedirectResult("~/simulado/candidato/acessar?continuar=" + filterContext.HttpContext.Request.Path);
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
            var autenticado = Helpers.Sessao.Candidato != null;

            if (!autenticado)
            {
                filterContext.Result = Redirecionar(filterContext);
            }

            base.OnActionExecuting(filterContext);
        }
    }
}