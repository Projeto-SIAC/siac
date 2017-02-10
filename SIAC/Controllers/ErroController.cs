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
using System.Web.Mvc;
using SIAC.ViewModels;

namespace SIAC.Controllers
{
    public class ErroController : Controller
    {
        // GET: erro
        [Route("erro/{code?}")]
        public ActionResult Index(int code = 0)
        {
            Response.StatusCode = 400;
            switch (code)
            {
                case 1:
                    return View(new ErroIndexViewModel(code.ToString(), "Você está realizando uma avaliação.", "Infelizmente, por você está realizando uma avaliação, você não pode acessar o resto do Sistema"));

                case 401:
                    Response.StatusCode = 401;
                    return View(new ErroIndexViewModel(code.ToString(), "Não autorizado", "Você não está autorizado pelo servidor"));

                case 403:
                    Response.StatusCode = 403;
                    return View(new ErroIndexViewModel(code.ToString(), "Acesso proibido", "A página solicitada é proibida para seu usuário"));

                case 404:
                    Response.StatusCode = 404;
                    return View(new ErroIndexViewModel(code.ToString(), "Não encontrado", "A página solicitada não foi encontrada"));

                case 500:
                    Response.StatusCode = 500;
                    return View(new ErroIndexViewModel(code.ToString(), "Erro interno", "Ocorreu um erro nos nossos servidores"));
            }
            return View(new ErroIndexViewModel());
        }
    }
}