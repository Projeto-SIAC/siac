using SIAC.Helpers;
using System;
using System.Collections.Generic;
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

        public string PrimeiroNome => this.Nome.Split(' ').First();
        public string UltimoNome => this.Nome.Split(' ').Last();

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
    }
}