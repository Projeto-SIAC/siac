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

        public static void Enviar(string para, string assunto, string viewname, object model = null)
        {
            string corpo = Engine.Razor.RunCompile(LerView(viewname), $"{para}.{viewname}", null, model);

            FluentEmail.Email
                .From(Criptografia.Base64Decode(Parametro.Obter().SmtpUsuario))
                .To(para)
                .Subject(assunto)
                .Body(corpo)
                .BodyAsHtml()
                .UsingClient(client)
                .Send();
        }

        public static void EnviarParaMuitos(string[] para, string assunto, string viewname, object model = null)
        {
            string corpo = Engine.Razor.RunCompile(LerView(viewname), $"{para}.{viewname}", null, model);

            FluentEmail.Email
                .From(Criptografia.Base64Decode(Parametro.Obter().SmtpUsuario))
                .To(para.Select(p=> new MailAddress(p)).ToList())
                .Subject(assunto)
                .Body(corpo)
                .BodyAsHtml()
                .UsingClient(client)
                .Send();
        }

        public static async Task Cadastro(string email, string nome)
        {
            var model = new
            {
                Nome = nome
            };
            await Task.Run(() => Enviar(email, "Cadastro no SIAC Simulados", "Cadastro", model));
        }

        public static async Task Inscricao(string email, string nome, string simuladoUrl, string simuladoTitulo)
        {
            var model = new
            {
                Nome = nome,
                SimuladoUrl = simuladoUrl,
                SimuladoTitulo = simuladoTitulo
            };
            await Task.Run(() => Enviar(email, "Inscrição realizada no SIAC Simulados", "Inscricao", model));
        }

        public static async Task NovoSimuladoDisponivel(List<Candidato> candidatos, string simuladoUrl, string simuladoTitulo)
        {
            var model = new { SimuladoUrl = simuladoUrl, SimuladoTitulo = simuladoTitulo };
            var assunto = "Novo simulado disponível no SIAC Simulados";
            var viewname = "SimuladoDisponivel";
            await Task.Run(() => EnviarParaMuitos(candidatos.Select(c=>c.Email).ToArray(), assunto, viewname, model));
        }

        public static async Task MensagemParaCandidatos(List<Candidato> candidatos, string mensagem, string simuladoUrl, string simuladoTitulo)
        {
            var model = new { Mensagem = mensagem, SimuladoUrl = simuladoUrl, SimuladoTitulo = simuladoTitulo };
            var assunto = "Mensagem sobre simulado no SIAC Simulados";
            var viewname = "Mensagem";
            await Task.Run(() => EnviarParaMuitos(candidatos.Select(c => c.Email).ToArray(), assunto, viewname, model));
        }

        public static async Task CartaoDeInscricaoDisponivel(List<Candidato> candidatos, string simuladoUrl, string simuladoTitulo)
        {
            var model = new { SimuladoUrl = simuladoUrl, SimuladoTitulo = simuladoTitulo };
            var assunto = "Cartão de Inscrição disponível no SIAC Simulados";
            var viewname = "CartaoDeInscricaoDisponivel";
            await Task.Run(() => EnviarParaMuitos(candidatos.Select(c => c.Email).ToArray(), assunto, viewname, model));
        }
    }
}