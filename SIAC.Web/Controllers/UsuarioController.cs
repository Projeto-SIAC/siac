using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using SIAC.Models;
using SIAC.ViewModels;
using SIAC.Helpers;

namespace SIAC.Controllers
{
    [Filters.AutenticacaoFilter(Categorias = new[] { 3 })]
    public class UsuarioController : Controller
    {
        private List<Usuario> Usuarios => Usuario.Listar();

        [HttpPost]
        public ActionResult Listar(int? pagina, string pesquisa, string ordenar, string categoria)
        {
            var qte = 10;
            var usuarios = Usuarios;
            pagina = pagina ?? 1;

            if (!String.IsNullOrWhiteSpace(pesquisa))
            {
                var strPesquisa = pesquisa.Trim().ToLower();
                usuarios = usuarios.Where(a => a.Matricula.ToLower().Contains(strPesquisa) || a.PessoaFisica.Nome.ToLower().Contains(strPesquisa)).ToList();
            }

            if (!String.IsNullOrWhiteSpace(categoria))
            {
                var codCategoria = int.Parse(categoria);
                usuarios = usuarios.Where(a => a.CodCategoria == codCategoria).ToList();
            }

            switch (ordenar)
            {
                case "data_desc":
                    usuarios = usuarios.OrderByDescending(a => a.UsuarioAcesso.LastOrDefault()?.DtAcesso).ToList();
                    break;
                case "data":
                    usuarios = usuarios.OrderBy(a => a.UsuarioAcesso.LastOrDefault()?.DtAcesso).ToList();
                    break;
                default:
                    usuarios = usuarios.OrderByDescending(a => a.UsuarioAcesso.LastOrDefault()?.DtAcesso).ToList();
                    break;
            }

            return PartialView("_ListaUsuario", usuarios.Skip((qte * pagina.Value) - qte).Take(qte).ToList());
        }

        // GET: Usuario
        public ActionResult Index()
        {
            var model = new UsuarioIndexViewModel();
            model.Categorias = Usuarios.Select(a => a.Categoria).Distinct().ToList();
            return View(model);
        }
    }
}