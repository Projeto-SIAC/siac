using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SIAC.Web.Helpers;

namespace SIAC.Web.Models
{
    public partial class Usuario
    {
        public static bool SAutenticado;
        public static string SMatricula;
        public static string SNome;
        public static string SCategoria;
        public static int SCategoriaCodigo;
    
        public static Usuario Autenticar(string matricula, string senha)
        {
            dbSIACEntities contexto = DataContextSIAC.GetInstance();

            Usuario usuario = contexto.Usuario.SingleOrDefault(u => u.Matricula == matricula);

            if (usuario != null)
            {
                string strSenha = Criptografia.RetornarHash(senha);

                if (usuario.Senha == strSenha)
                {
                    SAutenticado = true;
                    SMatricula = usuario.Matricula;
                    SNome = usuario.PessoaFisica.Nome;
                    SCategoria = usuario.Categoria.Descricao;
                    SCategoriaCodigo = usuario.CodCategoria;

                    return usuario;
                }
            }

            return null;
        }

        public static void Sair()
        {
            SAutenticado = false;
            SMatricula = null;
            SNome = null;
            SCategoria = null;
            SCategoriaCodigo = 0;
        }
    }
}