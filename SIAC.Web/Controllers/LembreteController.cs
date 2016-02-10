using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using SIAC.Models;
using SIAC.Helpers;

namespace SIAC.Controllers
{
    [Filters.AutenticacaoFilter]
    public class LembreteController : Controller
    {
        // GET: Lembrete
        public ActionResult Index() => RedirectToAction("Index", "Acesso");

        [HttpPost]
        public ActionResult Principal()
        {
            if (Sessao.Retornar("ContadoresPrincipal") == null)
            {
                string matricula = Sessao.UsuarioMatricula;
                var Atalho = new Dictionary<string, int>();
                Atalho.Add("autoavaliacao", AvalAuto.ListarNaoRealizadaPorPessoa(Sistema.UsuarioAtivo[matricula].Usuario.CodPessoaFisica).Count);

                if (Sessao.UsuarioCategoriaCodigo == 2)
                {
                    int codProfessor = Professor.ListarPorMatricula(matricula).CodProfessor;
                    var lst = AvalAcademica.ListarCorrecaoPendentePorProfessor(codProfessor).Select(a => a.Avaliacao);
                    lst = lst.Union(AvalCertificacao.ListarCorrecaoPendentePorProfessor(codProfessor).Select(a => a.Avaliacao));
                    lst = lst.Union(AvalAcadReposicao.ListarCorrecaoPendentePorProfessor(codProfessor).Select(a => a.Avaliacao));
                    Atalho.Add("correcao", lst.Count());
                }

                Sessao.Inserir("ContadoresPrincipal", Atalho);
            }

            return Json(Sessao.Retornar("ContadoresPrincipal"));
        }

        [HttpPost]
        public ActionResult Institucional()
        {
            if (Sessao.Retornar("ContadoresInstitucional") == null)
            {
                string matricula = Sessao.UsuarioMatricula;
                var Atalho = new Dictionary<string, int>();
                Atalho.Add("andamento", AvalAvi.ListarPorUsuario(Sessao.UsuarioMatricula).Count);
                Sessao.Inserir("ContadoresInstitucional", Atalho);
            }
            return Json(Sessao.Retornar("ContadoresInstitucional"));
        }

        [HttpPost]
        public ActionResult Menu()
        {
            if (Sessao.Retornar("ContadoresMenu") == null)
            {
                string matricula = Sessao.UsuarioMatricula;
                var Menu = new Dictionary<string, int>();
                Menu.Add("avi", AvalAvi.ListarPorUsuario(Sessao.UsuarioMatricula).Count);
                Sessao.Inserir("ContadoresMenu", Menu);
            }

            return Json(Sessao.Retornar("ContadoresMenu"));
        }

        [HttpPost]
        public ActionResult Lembretes()
        {
            var usuario = Sistema.UsuarioAtivo[Sessao.UsuarioMatricula].Usuario;
            string matricula = Sessao.UsuarioMatricula;
            var Lembretes = new List<Dictionary<string, string>>();
            if (Sessao.Retornar("LembreteInstitucional") == null)
            {
                if (AvalAvi.ListarPorUsuario(usuario.Matricula).Count > 0)
                {
                    Lembretes.Add(new Dictionary<string, string>() {
                            { "Id", "LembreteInstitucional" },
                            { "Mensagem", "Há Av. Institucionais em andamento no momento." },
                            { "Botao", "Visualizar" },
                            { "Url", "/institucional/andamento" }
                        });
                }
            }
            if (Sessao.Retornar("LembreteAcademica") == null)
            {
                if (AvalAcademica.ListarAgendadaPorUsuario(usuario, DateTime.Now, DateTime.Now.AddHours(24)).Count > 0)
                {
                    Lembretes.Add(new Dictionary<string, string>() {
                            { "Id", "LembreteAcademica" },
                            { "Mensagem", "Há Avaliações Acadêmicas agendadas para as próximas 24 horas." },
                            { "Botao", "Visualizar" },
                            { "Url", "/principal/agenda" }
                        });
                }
            }
            if (Sessao.Retornar("LembreteCertificacao") == null)
            {
                if (AvalCertificacao.ListarAgendadaPorUsuario(usuario, DateTime.Now, DateTime.Now.AddHours(24)).Count > 0)
                {
                    Lembretes.Add(new Dictionary<string, string>() {
                            { "Id", "LembreteCertificacao" },
                            { "Mensagem", "Há Avaliações de Certificações agendadas para as próximas 24 horas." },
                            { "Botao", "Visualizar" },
                            { "Url", "/principal/agenda" }
                        });
                }
            }
            if (Sessao.Retornar("LembreteReposicao") == null)
            {
                if (AvalAcadReposicao.ListarAgendadaPorUsuario(usuario, DateTime.Now, DateTime.Now.AddHours(24)).Count > 0)
                {
                    Lembretes.Add(new Dictionary<string, string>() {
                            { "Id", "LembreteReposicao" },
                            { "Mensagem", "Há Reposições agendadas para as próximas 24 horas." },
                            { "Botao", "Visualizar" },
                            { "Url", "/principal/agenda" }
                        });
                }
            }
            return Json(Lembretes);
        }

        [HttpPost]
        public void LembreteVisualizado(string id)
        {
            Sessao.Inserir(id, DateTime.Now);
        }

        [HttpPost]
        public ActionResult Notificacoes()
        {
            var notificacoes = Sessao.Retornar("Notificacoes");
            Sessao.Inserir("Notificacoes", null);
            return Json(notificacoes);
        }
    }
}