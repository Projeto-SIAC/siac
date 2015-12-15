using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIAC.Controllers
{
    public class ErroController : Controller
    {
        // GET: Erro
        [OutputCache(CacheProfile = "PorUsuario")]
        public ActionResult Index(int code = 0)
        {
            switch (code)
            {
                case 1:
                    ViewBag.Codigo = code;
                    ViewBag.Title = "Você está realizando uma avaliação.";
                    ViewBag.Mensagem = "Infelizmente, por você está realizando uma avaliação, você não pode acessar o resto do Sistema.";
                    break;
                case 403:
                    ViewBag.Codigo = code;
                    ViewBag.Title = "Acesso proibido";
                    ViewBag.Mensagem = "A página solicitada é proibida";
                    break;
                case 404:
                    ViewBag.Codigo = code;
                    ViewBag.Title = "Não encontrado";
                    ViewBag.Mensagem = "A página solicitada não foi encontrada";
                    break;
                default:
                    ViewBag.Codigo = "desconhecido";
                    ViewBag.Title = "Tente novamente";
                    ViewBag.Mensagem = "Ocorreu um erro inesperado";
                    break;
            }
            return View();
        }
    }
}