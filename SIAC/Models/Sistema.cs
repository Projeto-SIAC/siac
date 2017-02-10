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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Models
{
    public class Sistema
    {
        private static Random rnd = new Random();
        public static Random Random => rnd;

        public static Dictionary<string, string> CookieUsuario = new Dictionary<string, string>();
        public static Dictionary<string, UsuarioAcesso> UsuarioAtivo = new Dictionary<string, UsuarioAcesso>();
        public static Dictionary<int, int> NumIdentificador = new Dictionary<int, int>();
        public static List<string> AlertarMudanca = new List<string>();
        public static Dictionary<string, string> TempDataUrlImage = new Dictionary<string, string>();
        public static int? ProxCodVisitante = null;

        public static Dictionary<string, List<string>> AvaliacaoUsuario = new Dictionary<string, List<string>>();

        public static bool Autenticado(string matricula) => !String.IsNullOrEmpty(matricula) && UsuarioAtivo.Keys.Contains(matricula) && UsuarioAtivo[matricula].IpAcesso == HttpContextManager.Current.RecuperarIp();

        public static void RegistrarCookie(string matricula)
        {
            var cookie = HttpContextManager.Current.Request.Cookies["SIAC_Session"];
            if (cookie != null)
                CookieUsuario[cookie.Value] = matricula;
        }

        public static void RemoverCookie(string matricula)
        {
            string cookie = string.Empty;
            foreach (var chave in CookieUsuario.Keys)
            {
                if (CookieUsuario[chave] == matricula)
                {
                    cookie = chave;
                    break;
                }
            }
            if (!String.IsNullOrEmpty(cookie))
                CookieUsuario.Remove(cookie);
        }

        public static string GerarSenhaPadrao(Usuario usuario) => $"{usuario.PessoaFisica.PrimeiroNome.ToLower()}@{usuario.PessoaFisica.Cpf?.Substring(0, 3)}";

        // Notificações
        public static Dictionary<string, List<Dictionary<string, string>>> Notificacoes = new Dictionary<string, List<Dictionary<string, string>>>();

        // Simulados
        public static Dictionary<string, int> ProxInscricao = new Dictionary<string, int>();
    }
}