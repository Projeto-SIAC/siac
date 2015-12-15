using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using SIAC.Models;
using SIAC.Helpers;

namespace SIAC.Controllers
{
    [Filters.AutenticacaoFilter(Categorias = new[] { 1, 2, 3 })]
    public class PerfilController : Controller
    {
        // GET: Perfil
        [OutputCache(CacheProfile = "PorUsuario")]
        public ActionResult Index()
        {
            return View(Sistema.UsuarioAtivo[Sessao.UsuarioMatricula].Usuario);
        }

        [OutputCache(CacheProfile = "PorUsuario")]
        public ActionResult Estatisticas()
        {
            return PartialView("_Estatisticas", Sistema.UsuarioAtivo[Sessao.UsuarioMatricula].Usuario);
        }

        [HttpPost]
        public void EnviarOpiniao(string opiniao)
        {
            if (!String.IsNullOrWhiteSpace(opiniao))
            {
                var usuario = Sistema.UsuarioAtivo[Sessao.UsuarioMatricula].Usuario;
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