using SIAC.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIAC.Web.Controllers
{
    public class PainelAdministrativoController : Controller
    {

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            bool autenticado = Usuario.SAutenticado;
            int codigo = Usuario.SCategoriaCodigo;
            
            if(!autenticado || codigo != 3)
            {
                filterContext.Result = RedirectToAction("Entrar", "Acesso");
            }

                
            base.OnActionExecuting(filterContext);
        }

        // GET: PainelAdministrativo
        public ActionResult Index()
        {
            return View();
        }
    }
}