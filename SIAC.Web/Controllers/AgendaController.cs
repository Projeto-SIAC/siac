using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIAC.Controllers
{
    public class AgendaController : Controller
    {
        // GET: Agenda
        public ActionResult Index()
        {
            return View();
        }

        //?start=2013-12-01&end=2014-01-12&_=1386054751381
        public ActionResult Eventos(string start, string end)
        {
            var inicio = DateTime.Parse(start);
            var termino = DateTime.Parse(end);

            var lstAgendadas = Models.AvalAcademica.ListarAgendadaPorProfessor(Models.Professor.ListarPorMatricula(Helpers.Sessao.UsuarioMatricula).CodProfessor).Where(a=>a.Avaliacao.DtAplicacao > inicio && a.Avaliacao.DtAplicacao < termino);

            var retorno = lstAgendadas.Select(a=>new { title = a.Avaliacao.CodAvaliacao, start = a.Avaliacao.DtAplicacao.Value.ToString("yyyy-MM-ddTHH:mm:ss"), end = a.Avaliacao.DtAplicacao.Value.AddMinutes(a.Avaliacao.Duracao.Value).ToString("yyyy-MM-ddTHH:mm:ss") });

            return Json(retorno, JsonRequestBehavior.AllowGet);
        }
    }
}