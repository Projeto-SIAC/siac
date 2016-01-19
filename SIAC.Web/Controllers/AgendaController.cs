using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using SIAC.Models;
using SIAC.Helpers;

namespace SIAC.Controllers
{
    [Filters.AutenticacaoFilter(Categorias = new[] { 1, 2, 3 })]
    public class AgendaController : Controller
    {
        // GET: Agenda
        [OutputCache(CacheProfile = "PorUsuario")]
        public ActionResult Index()
        {
            return View("Index");
        }

        // POST: Agenda/Academicas?start=2013-12-01&end=2014-01-12&_=1386054751381
        [HttpPost]
        public ActionResult Academicas(string start, string end)
        {
            var inicio = DateTime.Parse(start);
            var termino = DateTime.Parse(end);

            var usuario = Sistema.UsuarioAtivo[Sessao.UsuarioMatricula].Usuario;//Models.Usuario.ListarPorMatricula(Helpers.Sessao.UsuarioMatricula);
            var lstAgendadas = AvalAcademica.ListarAgendadaPorUsuario(usuario, inicio, termino);    

            var retorno = lstAgendadas.Select(a => new Evento
            {
                id = a.Avaliacao.CodAvaliacao,
                title = a.Avaliacao.CodAvaliacao,
                start = a.Avaliacao.DtAplicacao.Value.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"),
                end = a.Avaliacao.DtTermino.Value.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"),
                url = Url.Action("Agendada", "Academica", new { codigo = a.Avaliacao.CodAvaliacao })
            });

            return Json(retorno);
        }

        // POST: Agenda/Reposicoes?start=2013-12-01&end=2014-01-12&_=1386054751381
        [HttpPost]
        public ActionResult Reposicoes(string start, string end)
        {
            var inicio = DateTime.Parse(start);
            var termino = DateTime.Parse(end);

            var usuario = Sistema.UsuarioAtivo[Sessao.UsuarioMatricula].Usuario;//Models.Usuario.ListarPorMatricula(Helpers.Sessao.UsuarioMatricula);
            var lstAgendadas = AvalAcadReposicao.ListarAgendadaPorUsuario(usuario, inicio, termino);

            var retorno = lstAgendadas.Select(a => new Evento
            {
                id = a.Avaliacao.CodAvaliacao,
                title = a.Avaliacao.CodAvaliacao,
                start = a.Avaliacao.DtAplicacao.Value.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"),
                end = a.Avaliacao.DtTermino.Value.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"),
                url = Url.Action("Agendada", "Reposicao", new { codigo = a.Avaliacao.CodAvaliacao })
            });

            return Json(retorno);
        }

        // POST: Agenda/Certificacoes?start=2013-12-01&end=2014-01-12&_=1386054751381
        [HttpPost]
        public ActionResult Certificacoes(string start, string end)
        {
            var inicio = DateTime.Parse(start);
            var termino = DateTime.Parse(end);

            var usuario = Sistema.UsuarioAtivo[Sessao.UsuarioMatricula].Usuario;//Models.Usuario.ListarPorMatricula(Helpers.Sessao.UsuarioMatricula);
            var lstAgendadas = AvalCertificacao.ListarAgendadaPorUsuario(usuario, inicio, termino);

            var retorno = lstAgendadas.Select(a => new Evento
            {
                id = a.Avaliacao.CodAvaliacao,
                title = a.Avaliacao.CodAvaliacao,
                start = a.Avaliacao.DtAplicacao.Value.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"),
                end = a.Avaliacao.DtTermino.Value.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"),
                url = Url.Action("Agendada", "Certificacao", new { codigo = a.Avaliacao.CodAvaliacao })
            });

            return Json(retorno);
        }

        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { 1, 2 })]
        [OutputCache(CacheProfile = "PorUsuario")]
        public ActionResult Horarios(string start, string end)
        {
            var ano = DateTime.Now.Year;
            var semestre = DateTime.Now.Month > 6 ? 2 : 1;
            var inicio = DateTime.Parse(start);
            var termino = DateTime.Parse(end);

            var usuario = Sistema.UsuarioAtivo[Sessao.UsuarioMatricula].Usuario;//Models.Usuario.ListarPorMatricula(Helpers.Sessao.UsuarioMatricula);
            var lstHorarios = new List<TurmaDiscProfHorario>();

            switch (usuario.CodCategoria)
            {
                case 1:
                    var codAluno = usuario.Aluno.First().CodAluno;
                    lstHorarios = (from h in Repositorio.GetInstance().TurmaDiscProfHorario
                                   where h.Turma.TurmaDiscAluno.FirstOrDefault(t=>t.CodAluno == codAluno) != null
                                   && h.AnoLetivo == ano
                                   && h.SemestreLetivo == semestre
                                   select h).ToList();
                    break;
                case 2:
                    var codProfessor = usuario.Professor.First().CodProfessor;
                    lstHorarios = (from h in Repositorio.GetInstance().TurmaDiscProfHorario
                                  where h.CodProfessor == codProfessor
                                  && h.AnoLetivo == ano
                                  && h.SemestreLetivo == semestre
                                  select h).ToList();
                    break;
                default:
                    break;
            }            

            var retorno = new List<Evento>();

            while (inicio < termino)
            {
                foreach (var hor in lstHorarios.Where(h => h.CodDia - 1 == (int)inicio.DayOfWeek))
                {
                    retorno.Add(new Evento
                    {
                        title = hor.Turma.CodTurma,
                        start = inicio.ToString("yyyy'-'MM'-'dd") + hor.Horario.HoraInicio.Value.ToString("'T'HH':'mm':'ss"),
                        end = inicio.ToString("yyyy'-'MM'-'dd") + hor.Horario.HoraTermino.Value.ToString("'T'HH':'mm':'ss")
                    });
                }
                inicio = inicio.AddDays(1);
            }

            return Json(retorno);
        }

        [HttpPost]
        public ActionResult Conflitos(string start, string end)
        {
            var retorno = ((JsonResult)Academicas(start, end)).Data as IEnumerable<Evento>;
            retorno = retorno.Union(((JsonResult)Reposicoes(start, end)).Data as IEnumerable<Evento>);
            retorno = retorno.Union(((JsonResult)Certificacoes(start, end)).Data as IEnumerable<Evento>);
            return Json(retorno);
        }
    }
}