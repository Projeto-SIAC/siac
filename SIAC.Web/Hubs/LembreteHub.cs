using Microsoft.AspNet.SignalR;
using SIAC.Helpers;
using SIAC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Hubs
{
    public class LembreteHub : Hub
    {
        private const string LEMBRETE_ACADEMICA = "LembreteAcademica";
        private const string LEMBRETE_REPOSICAO = "LembreteReposicao";
        private const string LEMBRETE_CERTIFICACAO = "LembreteCertificacao";
        private const string LEMBRETE_INSTITUCIONAL = "LembreteInstitucional";

        public static Dictionary<string, Dictionary<string, object>> UsuarioCache { get; set; } = new Dictionary<string, Dictionary<string, object>>();
        public static Dictionary<string, Dictionary<string, object>> UsuarioLembrete { get; set; } = new Dictionary<string, Dictionary<string, object>>();
        public static Dictionary<string, List<string>> UsuarioLembreteVisualizado { get; set; } = new Dictionary<string, List<string>>();

        public void RecuperarNotificacoes(string matricula)
        {
            List<Dictionary<string, string>> notificacoes = new List<Dictionary<string, string>>();
            notificacoes.AddRange(Sistema.Notificacoes[matricula]);
            Sistema.Notificacoes[matricula].Clear();
            Clients.Client(Context.ConnectionId).receberNotificacoes(notificacoes);
        }

        public void RecuperarMenu(string matricula)
        {
            if (!UsuarioCache.ContainsKey(matricula))
            {
                UsuarioCache[matricula] = new Dictionary<string, object>();
            }
            if (!UsuarioCache[matricula].ContainsKey("menu"))
            {
                Dictionary<string, int> menu = new Dictionary<string, int>();
                menu.Add("avi", AvalAvi.ListarPorUsuario(matricula).Count);
                UsuarioCache[matricula]["menu"] = menu;
            }            
            Clients.Client(Context.ConnectionId).receberMenu(UsuarioCache[matricula]["menu"]);
        }

        public void RecuperarContadoresPrincipal(string matricula)
        {
            if (!UsuarioCache.ContainsKey(matricula))
            {
                UsuarioCache[matricula] = new Dictionary<string, object>();
            }
            if (!UsuarioCache[matricula].ContainsKey("principal"))
            {
                Dictionary<string, int> atalho = new Dictionary<string, int>();
                atalho.Add("autoavaliacao", AvalAuto.ListarNaoRealizadaPorPessoa(Sistema.UsuarioAtivo[matricula].Usuario.CodPessoaFisica).Count);
                if (Sistema.UsuarioAtivo[matricula].Usuario.CodCategoria == Categoria.PROFESSOR)
                {
                    int codProfessor = Models.Professor.ListarPorMatricula(matricula).CodProfessor;
                    var lst = AvalAcademica.ListarCorrecaoPendentePorProfessor(codProfessor).Select(a => a.Avaliacao);
                    lst = lst.Union(AvalCertificacao.ListarCorrecaoPendentePorProfessor(codProfessor).Select(a => a.Avaliacao));
                    lst = lst.Union(AvalAcadReposicao.ListarCorrecaoPendentePorProfessor(codProfessor).Select(a => a.Avaliacao));
                    atalho.Add("correcao", lst.Count());
                }
                UsuarioCache[matricula]["principal"] =atalho;
            }
            Clients.Client(Context.ConnectionId).receberContadores(UsuarioCache[matricula]["principal"]);
        }

        public void RecuperarContadoresInstitucional(string matricula)
        {
            if (!UsuarioCache.ContainsKey(matricula))
            {
                UsuarioCache[matricula] = new Dictionary<string, object>();
            }
            if (!UsuarioCache[matricula].ContainsKey("institucional"))
            {
                Dictionary<string, int> atalho = new Dictionary<string, int>();
                atalho.Add("andamento", AvalAvi.ListarPorUsuario(Sessao.UsuarioMatricula).Count);
                UsuarioCache[matricula]["institucional"] = atalho;
            }
            Clients.Client(Context.ConnectionId).receberContadores(UsuarioCache[matricula]["institucional"]);
        }

        public void RecuperarLembretes(string matricula)
        {
            if (!UsuarioLembreteVisualizado.ContainsKey(matricula))
            {
                UsuarioLembreteVisualizado[matricula] = new List<string>();
            }

            if (!UsuarioLembrete.ContainsKey(matricula))
            {
                UsuarioLembrete[matricula] = new Dictionary<string, object>();
            }

            Usuario usuario = Sistema.UsuarioAtivo[matricula].Usuario;
            if (!UsuarioLembreteVisualizado[matricula].Contains(LEMBRETE_INSTITUCIONAL))
            {
                if (!UsuarioLembrete[matricula].ContainsKey(LEMBRETE_INSTITUCIONAL))
                {
                    if (AvalAvi.ListarPorUsuario(usuario.Matricula).Count > 0)
                    {
                        UsuarioLembrete[matricula][LEMBRETE_INSTITUCIONAL] = new Dictionary<string, string>() {
                        { "Id", LEMBRETE_INSTITUCIONAL },
                        { "Mensagem", "Há Av. Institucionais em andamento no momento." },
                        { "Botao", "Visualizar" },
                        { "Url", "/institucional/andamento" }
                    };
                    }
                }
            }
            if (!UsuarioLembreteVisualizado[matricula].Contains(LEMBRETE_ACADEMICA))
            {
                if (!UsuarioLembrete[matricula].ContainsKey(LEMBRETE_ACADEMICA))
                {
                    if (AvalAcademica.ListarAgendadaPorUsuario(usuario, DateTime.Now, DateTime.Now.AddHours(24)).Count > 0)
                    {
                        UsuarioLembrete[matricula][LEMBRETE_ACADEMICA] = new Dictionary<string, string>() {
                        { "Id", LEMBRETE_ACADEMICA },
                        { "Mensagem", "Há Avaliações Acadêmicas agendadas para as próximas 24 horas." },
                        { "Botao", "Visualizar" },
                        { "Url", "/principal/agenda" }
                    };
                    }
                }
            }
            if (!UsuarioLembreteVisualizado[matricula].Contains(LEMBRETE_CERTIFICACAO))
            {
                if (!UsuarioLembrete[matricula].ContainsKey(LEMBRETE_CERTIFICACAO))
                {
                    if (AvalCertificacao.ListarAgendadaPorUsuario(usuario, DateTime.Now, DateTime.Now.AddHours(24)).Count > 0)
                    {
                        UsuarioLembrete[matricula][LEMBRETE_CERTIFICACAO] = new Dictionary<string, string>() {
                        { "Id", LEMBRETE_CERTIFICACAO },
                        { "Mensagem", "Há Avaliações de Certificações agendadas para as próximas 24 horas." },
                        { "Botao", "Visualizar" },
                        { "Url", "/principal/agenda" }
                    };
                    }
                }
            }
            if (!UsuarioLembreteVisualizado[matricula].Contains(LEMBRETE_REPOSICAO))
            {
                if (!UsuarioLembrete[matricula].ContainsKey(LEMBRETE_REPOSICAO))
                {
                    if (AvalAcadReposicao.ListarAgendadaPorUsuario(usuario, DateTime.Now, DateTime.Now.AddHours(24)).Count > 0)
                    {
                        UsuarioLembrete[matricula][LEMBRETE_REPOSICAO] = new Dictionary<string, string>() {
                        { "Id", LEMBRETE_REPOSICAO },
                        { "Mensagem", "Há Reposições agendadas para as próximas 24 horas." },
                        { "Botao", "Visualizar" },
                        { "Url", "/principal/agenda" }
                    };
                    }
                }
            }
            Clients.Client(Context.ConnectionId).receberLembretes(UsuarioLembrete[matricula]);
        }

        public void LembreteVisualizado(string matricula, bool clicado, string lembrete)
        {
            if (clicado)
            {

                UsuarioLembrete[matricula].Remove(lembrete);
                UsuarioLembreteVisualizado[matricula].Add(lembrete);
            }
        }

        public static void Limpar(string matricula)
        {
            UsuarioCache.Remove(matricula);
            UsuarioLembrete.Remove(matricula);
            UsuarioLembreteVisualizado.Remove(matricula);
        }
    }
}