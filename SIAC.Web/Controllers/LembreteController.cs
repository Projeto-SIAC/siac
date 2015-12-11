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
        //[OutputCache(Duration = 120)]
        public ActionResult Dashboard()
        {
            if (Sessao.Retornar("ContadoresDashboard") == null)
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

                Sessao.Inserir("ContadoresDashboard", Atalho);
            }

            return Json(Sessao.Retornar("ContadoresDashboard"));
        }

        [HttpPost]
        //[OutputCache(Duration = 120)]
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
        //[OutputCache(Duration = 600)]
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
            if (Sessao.Retornar("Lembretes") == null)
            {
                if (Sessao.Retornar("LembretesMensagem") == null)
                {
                    var usuario = Sistema.UsuarioAtivo[Sessao.UsuarioMatricula].Usuario;
                    string matricula = Sessao.UsuarioMatricula;
                    var Lembretes = new List<Dictionary<string, string>>();
                    if (AvalAvi.ListarPorUsuario(usuario.Matricula).Count > 0)
                    {
                        Lembretes.Add(new Dictionary<string, string>() {
                            { "Mensagem", "Há Av. Institucionais em andamento no momento." },
                            { "Botao", "Visualizar" },
                            { "Url", "/institucional/andamento" }
                        });
                    }
                    if (AvalAcademica.ListarAgendadaPorUsuario(usuario, DateTime.Now, DateTime.Now.AddHours(24)).Count > 0)
                    {
                        Lembretes.Add(new Dictionary<string, string>() {
                            { "Mensagem", "Há Avaliações Acadêmicas agendadas para as próximas 24 horas." },
                            { "Botao", "Visualizar" },
                            { "Url", "/dashboard/agenda" }
                        });
                    }
                    if (AvalCertificacao.ListarAgendadaPorUsuario(usuario, DateTime.Now, DateTime.Now.AddHours(24)).Count > 0)
                    {
                        Lembretes.Add(new Dictionary<string, string>() {
                            { "Mensagem", "Há Avaliações de Certificações agendadas para as próximas 24 horas." },
                            { "Botao", "Visualizar" },
                            { "Url", "/dashboard/agenda" }
                        });
                    }
                    if (AvalAcadReposicao.ListarAgendadaPorUsuario(usuario, DateTime.Now, DateTime.Now.AddHours(24)).Count > 0)
                    {
                        Lembretes.Add(new Dictionary<string, string>() {
                            { "Mensagem", "Há Reposições agendadas para as próximas 24 horas." },
                            { "Botao", "Visualizar" },
                            { "Url", "/dashboard/agenda" }
                        });
                    }
                    Sessao.Inserir("LembretesMensagem", Lembretes);
                }
                return Json(Sessao.Retornar("LembretesMensagem"));                
            }
            return null;
        }

        [HttpPost]
        public void LembretesVisualizados()
        {
            Sessao.Inserir("Lembretes", DateTime.Now);
        }
    }
}