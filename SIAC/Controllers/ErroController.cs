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