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
using SIAC.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using HashidsNet;

namespace SIAC.Models
{
    public partial class Candidato
    {
        public enum Sexos
        {
            Feminino = 'F',
            Masculino = 'M',
            NaoInformado = 'N'
        };

        [NotMapped]
        public string PrimeiroNome => this.Nome.Split(' ').First();

        [NotMapped]
        public string UltimoNome => this.Nome.Split(' ').Last();

        [NotMapped]
        public bool PerfilCompleto
        {
            get
            {
                if (CodEstado != null && CodMunicipio != null && CodPais != null)
                {
                    if (RgDtExpedicao != null && RgNumero != null && RgOrgao != null)
                    {
                        if (DtNascimento != null && Sexo != null && FlagAdventista != null && FlagNecessidadeEspecial != null)
                        {
                            if (!String.IsNullOrWhiteSpace(TelefoneCelular) || !String.IsNullOrWhiteSpace(TelefoneFixo))
                            {
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
        }

        private static Contexto contexto => Repositorio.GetInstance();

        public static Candidato ListarPorCPF(string cpf) =>
            contexto.Candidato.FirstOrDefault(c => c.Cpf == cpf);

        public static List<Candidato> Listar() => contexto.Candidato.ToList();

        public static Candidato Autenticar(string cpf, string senha)
        {
            Candidato candidato = ListarPorCPF(Formate.DeCPF(cpf));
            if (candidato != null && Criptografia.ChecarSenha(senha, candidato.Senha))
                return candidato;
            return null;
        }

        public static int Inserir(Candidato candidato)
        {
            candidato.DtCadastro = DateTime.Now;
            contexto.Candidato.Add(candidato);
            contexto.SaveChanges();
            return candidato.CodCandidato;
        }

        private static Hashids HashidInstanciaParaToken => new Hashids((string)Configuracoes.Recuperar("SIAC_SECRET"), 15, "abcdefghijklmnopqrstuvwxyz1234567890");

        public static string GerarTokenParaAlterarSenha(Candidato c)
        {
            var candidato = contexto.Candidato.Find(c.CodCandidato);
            string token = HashidInstanciaParaToken.EncodeLong(new[] { c.CodCandidato, DateTime.UtcNow.AddMinutes(30).ToUnixTime() });
            candidato.AlterarSenha = Criptografia.RetornarHashSHA256(token);
            contexto.SaveChanges(false);
            return token;
        }

        public static Candidato LerTokenParaAlterarSenha(string token)
        {
            long[] valores = HashidInstanciaParaToken.DecodeLong(token);

            if (valores.Length == 2)
            {
                var candidato = contexto.Candidato.Find((int)valores[0]);
                var expirado = DateTime.UtcNow.ToUnixTime() > valores[1];
                if (!expirado && candidato.AlterarSenha == Criptografia.RetornarHashSHA256(token))
                {
                    return candidato;
                }
            }
            return null;
        }
    }
}