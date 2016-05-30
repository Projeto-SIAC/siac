using SIAC.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

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

        private static dbSIACEntities contexto => Repositorio.GetInstance();

        public static Candidato ListarPorCPF(string cpf) => 
            contexto.Candidato.FirstOrDefault(c => c.Cpf == cpf);

        public static List<Candidato> Listar() => contexto.Candidato.ToList();

        public static Candidato Autenticar(string cpf, string senha)
        {
            Candidato candidato = ListarPorCPF(Formate.DeCPF(cpf));
            if (candidato != null && candidato.Senha == Criptografia.RetornarHash(senha))
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

        public static string GerarTokenParaAlterarSenha(string cpf, string email)
        {
            string token = $"{Criptografia.Base64Encode(cpf)}.{Criptografia.Base64Encode(email)}.{Criptografia.Base64Encode(DateTime.Now.AddDays(7).ToUnixTime().ToString())}";
            return token;
        }

        public static dynamic LerTokenParaAlterarSenha(string token)
        {
            string[] valores = token.Split('.');

            string cpf = Criptografia.Base64Decode(valores[0]);
            string email = Criptografia.Base64Decode(valores[1]);
            string unixTime = Criptografia.Base64Decode(valores[2]);
            long expiracao = Convert.ToInt64(unixTime);
            bool expirado = DateTime.Now.ToUnixTime() > expiracao; 

            return new
            {
                Cpf = cpf,
                Email = email,
                Expirado = expirado
            };
        }
    }
}