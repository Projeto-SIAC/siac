using SIAC.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using RazorEngine;
using RazorEngine.Templating;
using System.Threading.Tasks;

namespace SIAC.Helpers
{
    public class EnviarEmail
    {
        private static SmtpClient client = new SmtpClient()
        {
            Host = Parametro.Obter().SmtpEnderecoHost,
            Port = Parametro.Obter().SmtpPorta,
            EnableSsl = Parametro.Obter().SmtpFlagSSL,
            Credentials = new NetworkCredential(Criptografia.Base64Decode(Parametro.Obter().SmtpUsuario),
                Criptografia.Base64Decode(Parametro.Obter().SmtpSenha))
        };

        private static string LerView(string viewname)
        {
            string caminho = AppDomain.CurrentDomain.BaseDirectory + "\\Views\\Shared\\Email\\__" + viewname + ".cshtml";
            return File.ReadAllText(caminho);
        }

        public async static Task EnviarAsync(string para, string assunto, string viewname, object model = null)
        {
            string corpo = Engine.Razor.RunCompile(LerView(viewname), $"{para}.{viewname}", null, model);

            await Task.Run(() =>
                FluentEmail.Email
                    .From(Criptografia.Base64Decode(Parametro.Obter().SmtpUsuario))
                    .To(para)
                    .Subject(assunto)
                    .Body(corpo)
                    .BodyAsHtml()
                    .UsingClient(client)
                    .Send()
            );
        }

        public static void Cadastro(string email, string nome)
        {
            var model = new
            {
                Nome = nome
            };
            EnviarAsync(email, "Cadastro no SIAC Simulados", "Cadastro", model);
        }

        public static void Inscricao(string email, string nome, string simuladoUrl, string simuladoTitulo)
        {
            var model = new
            {
                Nome = nome,
                SimuladoUrl = simuladoUrl,
                SimuladoTitulo = simuladoTitulo
            };
            EnviarAsync(email, "Inscrição realizada no SIAC Simulados", "Inscricao", model);
        }

        public async static Task NovoSimuladoDisponivel(List<Candidato> candidatos, string simuladoUrl, string simuladoTitulo)
        {
            foreach (Candidato candidato in candidatos)
            {
                var model = new
                {
                    Nome = candidato.Nome,
                    SimuladoUrl = simuladoUrl,
                    SimuladoTitulo = simuladoTitulo
                };
                EnviarAsync(candidato.Email, "Novo simulado disponível no SIAC Simulados", "SimuladoDisponivel", model);
            }
        }
    }
}