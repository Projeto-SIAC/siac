using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIAC.Models;
using SIAC.Helpers;

namespace SIAC.Controllers
{
    [Filters.AutenticacaoFilter(Categorias = new[] { 1, 2 })]
    public class ReposicaoController : Controller
    {
        // GET: Reposicao
        public ActionResult Index()
        {
            return View();
        }


        // GET: Reposicao/Justificar/ACAD201520002
        [HttpGet]
        [Filters.AutenticacaoFilter(Categorias = new[] { 2 })]
        public ActionResult Justificar(string codigo)
        {
            var aval = AvalAcademica.ListarPorCodigoAvaliacao(codigo);
            if (aval.Professor.Usuario.Matricula == Sessao.UsuarioMatricula)
            {
                return View(aval);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { 2 })]
        public ActionResult Justificar(string codigo, dynamic justificacao)
        {
            var aval = AvalAcademica.ListarPorCodigoAvaliacao(codigo);
            if (aval.Professor.Usuario.Matricula == Sessao.UsuarioMatricula)
            {
                if (Usuario.Verificar((string)justificacao.senha))
                {
                    Aluno aluno = Aluno.ListarPorMatricula((string)justificacao.aluno);
                    AvalPessoaResultado apr = aval.Avaliacao.AvalPessoaResultado.FirstOrDefault(p => p.CodPessoaFisica == aluno.Usuario.CodPessoaFisica);
                    apr.Justificacao.Add(new Justificacao()
                    {
                        Professor = aval.Professor,
                        DtCadastro = DateTime.Parse((string)justificacao.cadastro),
                        DtConfirmacao = DateTime.Parse((string)justificacao.confirmacao),
                        Descricao = (string)justificacao.confirmacao
                    });
                    Repositorio.GetInstance().SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }
    }
}