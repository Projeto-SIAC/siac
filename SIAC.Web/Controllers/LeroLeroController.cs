using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIAC.Web.Controllers
{
    public class LeroLeroController : Controller
    {
        // GET: LeroLero
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Index()
        {
            Helpers.LeroLero obj = new Helpers.LeroLero();
            string paragrafo = obj.Paragrafo();
            return Json(paragrafo);
        }
    }
}