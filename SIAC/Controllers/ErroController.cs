using SIAC.ViewModels;
using System.Web.Mvc;

namespace SIAC.Controllers
{
    public class ErroController : Controller
    {
        // GET: erro
        public ActionResult Index(int code = 0)
        {
            switch (code)
            {
                case 1:
                    return View(new ErroIndexViewModel(code.ToString(), "Você está realizando uma avaliação.", "Infelizmente, por você está realizando uma avaliação, você não pode acessar o resto do Sistema"));

                case 403:
                    return View(new ErroIndexViewModel(code.ToString(), "Acesso proibido", "A página solicitada é proibida para seu usuário"));

                case 404:
                    return View(new ErroIndexViewModel(code.ToString(), "Não encontrado", "A página solicitada não foi encontrada"));

                case 2699:
                    // developer tools run here
                    return RedirectToAction("Index", "Acesso");
            }
            return View(new ErroIndexViewModel());
        }
    }
}