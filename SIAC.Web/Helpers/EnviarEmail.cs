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
            string caminho = AppDomain.CurrentDomain.BaseDirectory + "\\Views\\Shared\\Email\\__" + viewname+ ".cshtml";
            return File.ReadAllText(caminho);
        }

        public static void Enviar(string para, string assunto, string viewname, object model = null)
        {
            string templated = Engine.Razor.RunCompile(LerView(viewname), $"{para}.{viewname}", null, model);

            FluentEmail.Email
                .From(Criptografia.Base64Decode(Parametro.Obter().SmtpUsuario))
                .To(para)
                .Subject(assunto)
                .Body(templated)
                .BodyAsHtml()
                .UsingClient(client)
                .Send();
        }

        public static void Cadastro(string email, string nome)
        {
            Enviar(email, "Cadastro no SIAC Simulados", "Cadastro", new { Nome = nome });
        }
    }
}