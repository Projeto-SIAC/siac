using SIAC.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SIAC.Models
{
    public partial class Usuario
    {
        [NotMapped]
        public List<Ocupacao> Ocupacao => this.PessoaFisica.Ocupacao.ToList();

        [NotMapped]
        public Dictionary<Disciplina, double?> DisciplinaMedia
        {
            get
            {
                Dictionary<Disciplina, double?> retorno = new Dictionary<Disciplina, double?>();
                List<Disciplina> lstDisciplina = this.PessoaFisica.AvalQuesPessoaResposta.Select(a => a.AvalTemaQuestao.AvaliacaoTema.Tema.Disciplina).Distinct().ToList();
                for (int i = 0, length = lstDisciplina.Count; i < length; i++)
                {
                    retorno.Add(lstDisciplina[i], this.PessoaFisica.AvalQuesPessoaResposta.Where(a => a.CodDisciplina == lstDisciplina[i].CodDisciplina).Average(a => a.RespNota));
                }
                return retorno;
            }
        }

        [NotMapped]
        public Disciplina MelhorDisciplina => DisciplinaMedia.FirstOrDefault(d => d.Value == DisciplinaMedia.Values.Max()).Key;

        [NotMapped]
        public Disciplina PiorDisciplina => DisciplinaMedia.FirstOrDefault(d => d.Value == DisciplinaMedia.Values.Min()).Key;

        [NotMapped]
        public bool FlagCoordenadorAvi => this.Ocupacao.Count > 0 ? this.Ocupacao.Select(a => a.CodOcupacao).ToArray().ContainsOne(Parametro.Obter().OcupacaoCoordenadorAvi) : false;

        private static Contexto contexto => Repositorio.GetInstance();

        public static Usuario Autenticar(string matricula, string senha)
        {
            if (!Sistema.UsuarioAtivo.Keys.Contains(matricula))
            {
                Usuario usuario = ListarPorMatricula(matricula);
                if (usuario != null && usuario.Senha == Criptografia.RetornarHash(senha))
                    return usuario;
            }
            else if (HttpContext.Current.Request.Cookies.AllKeys.Contains("SIAC_Login"))
            {
                if (HttpContext.Current.Request.Cookies["SIAC_Login"].Value == Criptografia.RetornarHash(matricula))
                {
                    Usuario usuario = ListarPorMatricula(matricula);
                    if (usuario != null && usuario.Senha == Criptografia.RetornarHash(senha))
                        return usuario;
                }
            }
            return null;
        }

        public static void RegistrarAcesso(string matricula)
        {
            if (!String.IsNullOrWhiteSpace(matricula))
            {
                Usuario usuario = ListarPorMatricula(matricula);

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
                    //if (Sistema.UsuarioAtivo.ContainsKey(matricula))
                    //{
                    //    Sistema.UsuarioAtivo[matricula] = acesso;
                    //}
                    //else
                    //{
                    //    Sistema.UsuarioAtivo.Add(matricula, acesso);
                    //}
                    Sistema.UsuarioAtivo[matricula] = acesso;
                    Sistema.Notificacoes[matricula] = new List<Dictionary<string, string>>();
                }
            }
        }

        public static bool Verificar(string senha)
        {
            Usuario usuario = ListarPorMatricula(Sessao.UsuarioMatricula);

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

        public static Usuario ListarPorMatricula(string matricula) => contexto.Usuario.Find(matricula);

        public static int ObterPessoaFisica(string matricula) => contexto.Usuario.Find(matricula).CodPessoaFisica;

        public static List<Usuario> Listar() => contexto.Usuario.ToList();

        public void AtualizarSenha(string novaSenha)
        {
            Usuario usuario = contexto.Usuario.Find(this.Matricula);
            if (usuario != null)
            {
                usuario.Senha = Criptografia.RetornarHash(novaSenha);
                contexto.SaveChanges();
            }
        }
    }
}