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
        [OutputCache(CacheProfile = "PorUsuario")]
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
    }
}