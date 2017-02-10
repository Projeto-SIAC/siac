/*
This file is part of SIAC.

Copyright (C) 2016 Felipe Mateus Freire Pontes <felipemfpontes@gmail.com>
Copyright (C) 2016 Francisco Bento da Silva Júnior <francisco.bento.jr@hotmail.com>

SIAC is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details. 
*/
using RazorEngine;
using RazorEngine.Templating;
using SIAC.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

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
            string caminho = $@"{AppDomain.CurrentDomain.BaseDirectory}\Templates\Email\__{viewname}.html";
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
                .To(para.Select(p => new MailAddress(p)).ToList())
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
            await Task.Run(() => EnviarParaMuitos(candidatos.Select(c => c.Email).ToArray(), assunto, viewname, model));
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

        public static async Task SimuladoEncerrado(List<Candidato> candidatos, string simuladoUrl, string simuladoTitulo)
        {
            var model = new { SimuladoUrl = simuladoUrl, SimuladoTitulo = simuladoTitulo };
            var assunto = $"Simulado {simuladoTitulo} foi encerrado no SIAC Simulados";
            var viewname = "SimuladoEncerrado";
            await Task.Run(() => EnviarParaMuitos(candidatos.Select(c => c.Email).ToArray(), assunto, viewname, model));
        }

        public static async Task SolicitarSenha(string email, string nome, string url)
        {
            var model = new
            {
                Nome = nome,
                Url = url
            };
            await Task.Run(() => Enviar(email, "Alterar senha no SIAC Simulados", "Senha", model));
        }
    }
}