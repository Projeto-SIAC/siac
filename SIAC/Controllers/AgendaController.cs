using SIAC.Helpers;
using SIAC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SIAC.Controllers
{
    [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.ALUNO, Categoria.PROFESSOR, Categoria.COLABORADOR })]
    public class AgendaController : Controller
    {
        // GET: principal/agenda
        public ActionResult Index() => View("Index");

        // POST: principal/agenda/academicas?start=2013-12-01&end=2014-01-12
        [HttpPost]
        public ActionResult Academicas(string start, string end)
        {
            DateTime inicio = DateTime.Parse(start);
            DateTime termino = DateTime.Parse(end);

            Usuario usuario = Sistema.UsuarioAtivo[Sessao.UsuarioMatricula].Usuario;
            List<AvalAcademica> lstAgendadas = AvalAcademica.ListarAgendadaPorUsuario(usuario, inicio, termino);

            IEnumerable<Evento> retorno = lstAgendadas.Select(a => new Evento
            {
                id = a.Avaliacao.CodAvaliacao,
                title = a.Avaliacao.CodAvaliacao,
                start = a.Avaliacao.DtAplicacao.Value.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"),
                end = a.Avaliacao.DtTermino.Value.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"),
                url = Url.Action("Agendada", "Academica", new { codigo = a.Avaliacao.CodAvaliacao })
            });

            return Json(retorno);
        }

        // POST: principal/agenda/reposicoes?start=2013-12-01&end=2014-01-12
        [HttpPost]
        public ActionResult Reposicoes(string start, string end)
        {
            DateTime inicio = DateTime.Parse(start);
            DateTime termino = DateTime.Parse(end);

            Usuario usuario = Sistema.UsuarioAtivo[Sessao.UsuarioMatricula].Usuario;
            List<AvalAcadReposicao> lstAgendadas = AvalAcadReposicao.ListarAgendadaPorUsuario(usuario, inicio, termino);

            IEnumerable<Evento> retorno = lstAgendadas.Select(a => new Evento
            {
                id = a.Avaliacao.CodAvaliacao,
                title = a.Avaliacao.CodAvaliacao,
                start = a.Avaliacao.DtAplicacao.Value.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"),
                end = a.Avaliacao.DtTermino.Value.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"),
                url = Url.Action("Agendada", "Reposicao", new { codigo = a.Avaliacao.CodAvaliacao })
            });

            return Json(retorno);
        }

        // POST: principal/agenda/certificacoes?start=2013-12-01&end=2014-01-12
        [HttpPost]
        public ActionResult Certificacoes(string start, string end)
        {
            DateTime inicio = DateTime.Parse(start);
            DateTime termino = DateTime.Parse(end);

            Usuario usuario = Sistema.UsuarioAtivo[Sessao.UsuarioMatricula].Usuario;
            List<AvalCertificacao> lstAgendadas = AvalCertificacao.ListarAgendadaPorUsuario(usuario, inicio, termino);

            IEnumerable<Evento> retorno = lstAgendadas.Select(a => new Evento
            {
                id = a.Avaliacao.CodAvaliacao,
                title = a.Avaliacao.CodAvaliacao,
                start = a.Avaliacao.DtAplicacao.Value.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"),
                end = a.Avaliacao.DtTermino.Value.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"),
                url = Url.Action("Agendada", "Certificacao", new { codigo = a.Avaliacao.CodAvaliacao })
            });

            return Json(retorno);
        }

        // POST: principal/agenda/horarios?start=2013-12-01&end=2014-01-12
        [HttpPost]
        public ActionResult Horarios(string start, string end)
        {
            DateTime inicio = DateTime.Parse(start);
            DateTime termino = DateTime.Parse(end);
            List<Evento> retorno = new List<Evento>();

            if (Sessao.UsuarioCategoriaCodigo == Categoria.ALUNO || Sessao.UsuarioCategoriaCodigo == Categoria.PROFESSOR)
            {
                Usuario usuario = Sistema.UsuarioAtivo[Sessao.UsuarioMatricula].Usuario;
                List<TurmaDiscProfHorario> lstHorarios = TurmaDiscProfHorario.ListarPorUsuario(usuario);

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
            }

            return Json(retorno);
        }

        // POST: principal/agenda/horarios?start=2013-12-01&end=2014-01-12
        [HttpPost]
        public ActionResult Conflitos(string start, string end)
        {
            IEnumerable<Evento> retorno = ((JsonResult)Academicas(start, end)).Data as IEnumerable<Evento>;
            retorno = retorno.Union(((JsonResult)Reposicoes(start, end)).Data as IEnumerable<Evento>);
            retorno = retorno.Union(((JsonResult)Certificacoes(start, end)).Data as IEnumerable<Evento>);
            return Json(retorno);
        }
    }
}