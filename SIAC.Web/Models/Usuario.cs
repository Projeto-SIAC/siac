using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SIAC.Web.Helpers;

namespace SIAC.Web.Models
{
    public partial class Usuario
    {
        public static Usuario Autenticar(string matricula, string senha)
        {
            DataClassesSIACDataContext contexto = DataContextSIAC.GetInstance();

            Usuario usuario = contexto.Usuarios.SingleOrDefault(u => u.Matricula == matricula);

            if (usuario != null)
            {
                string strSenha = Criptografia.RetornarHash(senha);

                if (usuario.Senha == strSenha)
                {
                    return usuario;
                }
            }

            else return null;

            return null;
        }
    }
}