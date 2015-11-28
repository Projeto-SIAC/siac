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
    public class VisitanteController : Controller
    {
        private List<Visitante> Visitantes
        {
            get
            {
                return new List<Visitante>();
            }
        }

        public ActionResult Listar()
        {
            return Json(null);
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Cadastrar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Confirmar(FormCollection form)
        {
            if (!StringExt.IsNullOrEmpty(form["txtNome"], form["txtCpf"]))
            {
                string nome = form["txtNome"].RemoveSpaces();
                string cpf = form["txtCpf"].Replace(".", "").Replace("-", "");

                Visitante visitante = PessoaFisica.ListarPorCpf(cpf)?.Usuario.FirstOrDefault(u => u.Visitante.FirstOrDefault() != null)?.Visitante.First();

                if (visitante == null)
                {
                    string matricula = $"VIS{Visitante.ProxCodigo.ToString("00000")}";
                    PessoaFisica pf = PessoaFisica.ListarPorCpf(cpf);
                    if (pf == null)
                    {
                        int codPessoa = Pessoa.Inserir(new Pessoa() { TipoPessoa = "F" });
                        pf = new PessoaFisica();
                        pf.CodPessoa = codPessoa;
                        pf.Nome = nome;
                        pf.Cpf = cpf;
                        pf.Categoria.Add(Categoria.ListarPorCodigo(4));
                        
                        PessoaFisica.Inserir(pf);
                    }

                    Usuario usuario = new Usuario();
                    usuario.Matricula = matricula;
                    usuario.PessoaFisica = pf;
                    usuario.CodCategoria = 4;
                    string senha = $"{pf.PrimeiroNome}@{cpf.Substring(0, 3)}"; // primeironome@3primeirosdigitosdocpf
                    usuario.Senha = Criptografia.RetornarHash(senha);

                    Usuario.Inserir(usuario);

                    visitante = new Visitante();
                    visitante.Usuario = usuario;

                    Visitante.Inserir(visitante);
                }

                if (!String.IsNullOrEmpty(form["txtDtNascimento"]) && !visitante.Usuario.PessoaFisica.DtNascimento.HasValue)
                {
                    visitante.Usuario.PessoaFisica.DtNascimento = DateTime.Parse(form["txtDtNascimento"]);
                }
                if (!String.IsNullOrEmpty(form["ddlSexo"]) && String.IsNullOrEmpty(visitante.Usuario.PessoaFisica.Sexo))
                {
                    visitante.Usuario.PessoaFisica.Sexo = form["ddlSexo"];
                }

                if (String.IsNullOrEmpty(form["chkDtValidade"]))
                {
                    visitante.DtValidade = null;
                }
                else
                {
                    visitante.DtValidade = DateTime.Parse(form["txtDtValidade"]);
                }

                Repositorio.GetInstance().SaveChanges();

                return View(visitante);
            }
            return RedirectToAction("Cadastrar");
        }

        [HttpPost]
        public ActionResult CarregarPessoa(string cpf)
        {
            if (!String.IsNullOrEmpty(cpf.Trim()))
            {
                var p = PessoaFisica.ListarPorCpf(cpf);
                if (p != null)
                {
                    var r = new
                    {
                        Nome = p.Nome,
                        DtNasc = p.DtNascimento?.ToString("YYYY-MM-DD"),
                        Sexo = p.Sexo
                    };
                    return Json(r);
                }
            }
            return null;
        }
    }
}