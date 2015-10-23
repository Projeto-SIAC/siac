using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SIAC.Helpers;

namespace SIAC.Models
{
    public partial class Usuario
    {
        //private static dbSIACEntities contexto { get { return DataContextSIAC.GetInstance(); } }

        private static dbSIACEntities contexto { get { return Repositorio.GetInstance(); } }

        public static Usuario Autenticar(string matricula, string senha)
        {
            if (!Sistema.MatriculaAtivo.Contains(matricula))
            {
                Usuario usuario = contexto.Usuario.SingleOrDefault(u => u.Matricula == matricula);

                if (usuario != null)
                {
                    string strSenha = Criptografia.RetornarHash(senha);

                    if (usuario.Senha == strSenha)
                    {
                        Sistema.MatriculaAtivo.Add(matricula);
                        return usuario;
                    }
                }
            }
            return null;
        }

        public static int Inserir(Usuario usuario)
        {
            usuario.DtCadastro = DateTime.Now;
            contexto.Usuario.Add(usuario);
            contexto.SaveChanges();
            return usuario.CodPessoaFisica;
        }

        public static Usuario ListarPorMatricula(string matricula)
        {
            return contexto.Usuario.FirstOrDefault(u => u.Matricula == matricula);
        }

        public static int ObterPessoaFisica(string matricula)
        {
            return contexto.Usuario.FirstOrDefault(u => u.Matricula == matricula).CodPessoaFisica;
        }

    }
}