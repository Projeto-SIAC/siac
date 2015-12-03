using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SIAC.Helpers;

namespace SIAC.Models
{
    public partial class Usuario
    {
        public List<Ocupacao> Ocupacao => this.PessoaFisica.Ocupacao.ToList();

        public Dictionary<Disciplina, double?> DisciplinaMedia
        {
            get
            {
                Dictionary<Disciplina, double?> retorno = new Dictionary<Disciplina, double?>();
                List<Disciplina> lstDisciplina = this.PessoaFisica.AvalQuesPessoaResposta.Select(a => a.AvalTemaQuestao.AvaliacaoTema.Tema.Disciplina).Distinct().ToList();
                for (int i = 0, length = lstDisciplina.Count; i < length; i++)
                {
                    retorno.Add(lstDisciplina[i], this.PessoaFisica.AvalQuesPessoaResposta.Where(a=>a.CodDisciplina == lstDisciplina[i].CodDisciplina).Average(a=>a.RespNota));
                }
                return retorno;
            }
        }
        public Disciplina MelhorDisciplina
        {
            get
            {
                return DisciplinaMedia.FirstOrDefault(d => d.Value == DisciplinaMedia.Values.Max()).Key;
            }
        }
        public Disciplina PiorDisciplina
        {
            get
            {
                return DisciplinaMedia.FirstOrDefault(d => d.Value == DisciplinaMedia.Values.Min()).Key;
            }
        }

        private static dbSIACEntities contexto { get { return Repositorio.GetInstance(); } }

        public static Usuario Autenticar(string matricula, string senha)
        {
            if (!Sistema.UsuarioAtivo.Keys.Contains(matricula))
            {
                Usuario usuario = contexto.Usuario.SingleOrDefault(u => u.Matricula == matricula);

                if (usuario != null)
                {
                    string strSenha = Criptografia.RetornarHash(senha);

                    if (usuario.Senha == strSenha)
                    {
                        return usuario;
                    }
                }
            }
            return null;
        }

        public static void RegistrarAcesso(string matricula)
        {
            if (!String.IsNullOrWhiteSpace(matricula))
            {
                Usuario usuario = contexto.Usuario.SingleOrDefault(u => u.Matricula == matricula);

                if (usuario != null)
                {
                    var acesso = new UsuarioAcesso();
                    var acessos = contexto.UsuarioAcesso.Where(a => a.Matricula == matricula);
                    int codOrdem = acessos.Count() > 0 ? acessos.Max(a => a.CodOrdem) : 0;
                    acesso.CodOrdem = codOrdem + 1;
                    acesso.Usuario = usuario;
                    acesso.DtAcesso = DateTime.Now;
                    acesso.IpAcesso = HttpContext.Current.RecuperarIp();
                    contexto.UsuarioAcesso.Add(acesso);
                    contexto.SaveChanges();
                    Sistema.UsuarioAtivo.Add(matricula, acesso);
                }
            }
        }

        public static bool Verificar(string senha)
        {
            Usuario usuario = contexto.Usuario.SingleOrDefault(u => u.Matricula == Sessao.UsuarioMatricula);

            if (usuario != null)
            {
                string strSenha = Criptografia.RetornarHash(senha);

                if (usuario.Senha == strSenha)
                {
                    return true;
                }
            }

            return false;
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

        public static List<Usuario> Listar()
        {
            return contexto.Usuario.ToList();
        }

    }
}