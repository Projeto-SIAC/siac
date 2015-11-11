using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIAC.Controllers
{
    [Filters.AutenticacaoFilter(Categorias = new[] { 1, 2, 3 })]
    public class AgendaController : Controller
    {
        // GET: Agenda
        public ActionResult Index()
        {
            return View();
        }

        // POST: Agenda/Academicas?start=2013-12-01&end=2014-01-12&_=1386054751381
        [HttpPost]
        public ActionResult Academicas(string start, string end)
        {
            var inicio = DateTime.Parse(start);
            var termino = DateTime.Parse(end);

            var usuario = Models.Usuario.ListarPorMatricula(Helpers.Sessao.UsuarioMatricula);
            var lstAgendadas = new List<Models.AvalAcademica>();

            switch (usuario.CodCategoria)
            {
                case 1:
                    lstAgendadas = Models.AvalAcademica.ListarAgendadaPorAluno(usuario.Aluno.First().CodAluno)
                        .Where(a => a.Avaliacao.DtAplicacao > inicio && a.Avaliacao.DtAplicacao < termino)
                        .ToList();
                    break;
                case 2:
                    lstAgendadas = Models.AvalAcademica.ListarAgendadaPorProfessor(usuario.Professor.First().CodProfessor)
                        .Where(a => a.Avaliacao.DtAplicacao > inicio && a.Avaliacao.DtAplicacao < termino)
                        .ToList();
                    break;
                case 3:
                    lstAgendadas = Models.AvalAcademica.ListarAgendadaPorColaborador(usuario.Colaborador.First().CodColaborador)
                        .Where(a => a.Avaliacao.DtAplicacao > inicio && a.Avaliacao.DtAplicacao < termino)
                        .ToList();
                    break;
                default:
                    break;
            }           

            var retorno = lstAgendadas.Select(a => new Models.Evento
            {
                id = a.Avaliacao.CodAvaliacao,
                title = a.Avaliacao.CodAvaliacao,
                start = a.Avaliacao.DtAplicacao.Value.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"),
                end = a.Avaliacao.DtAplicacao.Value.AddMinutes(a.Avaliacao.Duracao.Value).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"),
                url = Url.Action("Agendada", "Academica", new { codigo = a.Avaliacao.CodAvaliacao })
            });

            return Json(retorno, JsonRequestBehavior.AllowGet);
        }

        // POST: Agenda/Certificacoes?start=2013-12-01&end=2014-01-12&_=1386054751381
        [HttpPost]
        public ActionResult Certificacoes(string start, string end)
        {
            var inicio = DateTime.Parse(start);
            var termino = DateTime.Parse(end);

            var usuario = Models.Usuario.ListarPorMatricula(Helpers.Sessao.UsuarioMatricula);
            var lstAgendadas = new List<Models.AvalCertificacao>();

            switch (usuario.CodCategoria)
            {
                case 1:
                    lstAgendadas = Models.AvalCertificacao.ListarAgendadaPorPessoa(usuario.CodPessoaFisica)
                        .Where(a => a.Avaliacao.DtAplicacao > inicio && a.Avaliacao.DtAplicacao < termino)
                        .ToList();
                    break;
                case 2:
                    lstAgendadas = Models.AvalCertificacao.ListarAgendadaPorProfessor(usuario.Professor.First().CodProfessor)
                        .Where(a => a.Avaliacao.DtAplicacao > inicio && a.Avaliacao.DtAplicacao < termino)
                        .ToList();
                    break;
                case 3:
                    lstAgendadas = Models.AvalCertificacao.ListarAgendadaPorPessoa(usuario.CodPessoaFisica)
                        .Union(Models.AvalCertificacao.ListarAgendadaPorColaborador(usuario.Colaborador.First().CodColaborador))
                        .Where(a => a.Avaliacao.DtAplicacao > inicio && a.Avaliacao.DtAplicacao < termino)
                        .ToList();
                    break;
                default:
                    break;
            }

            var retorno = lstAgendadas.Select(a => new Models.Evento
            {
                id = a.Avaliacao.CodAvaliacao,
                title = a.Avaliacao.CodAvaliacao,
                start = a.Avaliacao.DtAplicacao.Value.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"),
                end = a.Avaliacao.DtAplicacao.Value.AddMinutes(a.Avaliacao.Duracao.Value).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"),
                url = Url.Action("Detalhe", "Certificacao", new { codigo = a.Avaliacao.CodAvaliacao })
            });

            return Json(retorno, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { 1, 2 })]
        public ActionResult Horarios(string start, string end)
        {
            var ano = DateTime.Now.Year;
            var semestre = DateTime.Now.Month > 6 ? 2 : 1;
            var inicio = DateTime.Parse(start);
            var termino = DateTime.Parse(end);

            var usuario = Models.Usuario.ListarPorMatricula(Helpers.Sessao.UsuarioMatricula);
            var lstHorarios = new List<Models.TurmaDiscProfHorario>();

            switch (usuario.CodCategoria)
            {
                case 1:
                    var codAluno = usuario.Aluno.First().CodAluno;
                    lstHorarios = (from h in Models.Repositorio.GetInstance().TurmaDiscProfHorario
                                   where h.Turma.TurmaDiscAluno.FirstOrDefault(t=>t.CodAluno == codAluno) != null
                                   && h.AnoLetivo == ano
                                   && h.SemestreLetivo == semestre
                                   select h).ToList();
                    break;
                case 2:
                    var codProfessor = usuario.Professor.First().CodProfessor;
                    lstHorarios = (from h in Models.Repositorio.GetInstance().TurmaDiscProfHorario
                                  where h.CodProfessor == codProfessor
                                  && h.AnoLetivo == ano
                                  && h.SemestreLetivo == semestre
                                  select h).ToList();
                    break;
                default:
                    break;
            }            

            var retorno = new List<Models.Evento>();

            while (inicio < termino)
            {
                foreach (var hor in lstHorarios.Where(h => h.CodDia - 1 == (int)inicio.DayOfWeek))
                {
                    retorno.Add(new Models.Evento
                    {
                        title = hor.Turma.CodTurma,
                        start = inicio.ToString("yyyy'-'MM'-'dd") + hor.Horario.HoraInicio.Value.ToString("'T'HH':'mm':'ss"),
                        end = inicio.ToString("yyyy'-'MM'-'dd") + hor.Horario.HoraTermino.Value.ToString("'T'HH':'mm':'ss")
                    });
                }
                inicio = inicio.AddDays(1);
            }

            return Json(retorno, JsonRequestBehavior.AllowGet);
        }
    }
}