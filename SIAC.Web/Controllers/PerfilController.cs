using SIAC.Helpers;
using SIAC.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace SIAC.Controllers
{
    [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.ESTUDANTE, Categoria.PROFESSOR, Categoria.COLABORADOR})]
    public class PerfilController : Controller
    {
        // GET: perfil
        public ActionResult Index()
        {
            return View(Sistema.UsuarioAtivo[Sessao.UsuarioMatricula].Usuario);
        }
        
        // GET: perfil/estatisticas
        [OutputCache(CacheProfile = "PorUsuario")]
        public ActionResult Estatisticas()
        {
            return PartialView("_Estatisticas", Sistema.UsuarioAtivo[Sessao.UsuarioMatricula].Usuario);
        }

        // POST: perfil/enviaropiniao
        [HttpPost]
        public void EnviarOpiniao(string opiniao)
        {
            if (!String.IsNullOrWhiteSpace(opiniao))
            {
                Usuario usuario = Sistema.UsuarioAtivo[Sessao.UsuarioMatricula].Usuario;
                usuario.UsuarioOpiniao.Add(new UsuarioOpiniao
                {
                    CodOrdem = usuario.UsuarioOpiniao.Count > 0 ? usuario.UsuarioOpiniao.Max(o => o.CodOrdem) + 1 : 1,
                    DtEnvio = DateTime.Now,
                    Opiniao = opiniao.Trim()
                });
                Repositorio.GetInstance().SaveChanges(false);
            }
        }

        // POST: perfil/alterarsenha
        [HttpPost]
        public ActionResult AlterarSenha(string senhaAtual, string senhaNova, string senhaConfirmacao)
        {
            string lembrete = Lembrete.NEGATIVO;
            string mensagem = "Ocorreu um erro ao tentar alterar a senha.";

            if (!StringExt.IsNullOrWhiteSpace(senhaAtual, senhaNova, senhaConfirmacao))
            {
                Usuario usuario = Sistema.UsuarioAtivo[Sessao.UsuarioMatricula].Usuario;
                
                string hashSenhaAtual = Criptografia.RetornarHash(senhaAtual);
                if (hashSenhaAtual == usuario.Senha)
                {
                    if (senhaNova == senhaConfirmacao)
                    {
                        string hashSenhaNova = Criptografia.RetornarHash(senhaNova);
                        usuario.Senha = hashSenhaNova;
                        Repositorio.Commit();

                        lembrete = Lembrete.POSITIVO;
                        mensagem = "Senha alterada com sucesso.";
                    }
                    else
                    {
                        mensagem = "A confirmação da senha deve ser igual a senha nova.";
                    }
                }
                else
                {
                    mensagem = "A senha atual informada está incorreta.";
                }
            }
            else
            {
                mensagem = "Todos os campos são necessários para alterar a senha.";
            }

            Lembrete.AdicionarNotificacao(mensagem, lembrete);
            return RedirectToAction("Index");
        }
    }
}