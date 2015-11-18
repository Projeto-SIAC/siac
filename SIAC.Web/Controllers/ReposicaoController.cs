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
        public ActionResult Justificar(string codigo, Dictionary<string, string> justificacao)
        {
            var aval = AvalAcademica.ListarPorCodigoAvaliacao(codigo);
            if (aval.Professor.Usuario.Matricula == Sessao.UsuarioMatricula)
            {
                if (Usuario.Verificar(justificacao["senha"]))
                {
                    Aluno aluno = Aluno.ListarPorMatricula(justificacao["aluno"]);
                    AvalPessoaResultado apr = aval.Avaliacao.AvalPessoaResultado.FirstOrDefault(p => p.CodPessoaFisica == aluno.Usuario.CodPessoaFisica);
                    if (apr == null)
                    {
                        AvalPessoaResultado avalPessoaResultado = new AvalPessoaResultado();
                        avalPessoaResultado.CodPessoaFisica = aluno.Usuario.CodPessoaFisica;
                        avalPessoaResultado.HoraTermino = aval.Avaliacao.DtAplicacao;
                        avalPessoaResultado.QteAcertoObj = 0;
                        avalPessoaResultado.Nota = 0;

                        avalPessoaResultado.Justificacao.Add(new Justificacao()
                        {
                            Professor = aval.Professor,
                            DtCadastro = DateTime.Parse(justificacao["cadastro"]),
                            DtConfirmacao = DateTime.Parse(justificacao["confirmacao"]),
                            Descricao = justificacao["descricao"]
                        });

                        aval.Avaliacao.AvalPessoaResultado.Add(avalPessoaResultado);

                        Repositorio.GetInstance().SaveChanges();
                    }
                }
            }
            return null;
        }

        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { 2 })]
        public string Gerar(string codigo, int[] justificaticoes, bool nova = false)
        {
            var hoje = DateTime.Now;

            AvalAcadReposicao aval = new AvalAcadReposicao();
            AvalAcademica acad = AvalAcademica.ListarPorCodigoAvaliacao(codigo);

            aval.Avaliacao = new Avaliacao();

            aval.Avaliacao.TipoAvaliacao = TipoAvaliacao.ListarPorCodigo(5);
            aval.Avaliacao.Ano = hoje.Year;
            aval.Avaliacao.Semestre = hoje.Month > 6 ? 2 : 1;
            aval.Avaliacao.NumIdentificador = Avaliacao.ObterNumIdentificador(5);
            aval.Avaliacao.DtCadastro = hoje;

            if (nova)
            {
                List<QuestaoTema> lstQuestoes = new List<QuestaoTema>();
                var lstAvaliacaoTema = acad.Avaliacao.AvaliacaoTema.ToList();
                var qteObjetiva = lstAvaliacaoTema.QteQuestoesPorTipo(1);
                var qteDiscursiva = lstAvaliacaoTema.QteQuestoesPorTipo(2);
                var arrTemaCods = lstAvaliacaoTema.Select(a => a.CodTema.ToString()).ToArray();
                var codDificuldade = acad.Avaliacao.Questao.Max(a => a.CodDificuldade);

                if (qteObjetiva > 0)
                { 
                    lstQuestoes.AddRange(Questao.ListarPorDisciplina(acad.CodDisciplina, arrTemaCods, codDificuldade, 1, qteObjetiva));
                }
                if (qteDiscursiva > 0)
                {
                    lstQuestoes.AddRange(Questao.ListarPorDisciplina(acad.CodDisciplina, arrTemaCods, codDificuldade, 2, qteDiscursiva));
                }

                foreach (var strTemaCod in arrTemaCods)
                {
                    AvaliacaoTema avalTema = new AvaliacaoTema();
                    avalTema.Tema = Tema.ListarPorCodigo(acad.CodDisciplina, int.Parse(strTemaCod));
                    foreach (var queTma in lstQuestoes.Where(q => q.CodTema == int.Parse(strTemaCod)))
                    {
                        AvalTemaQuestao avalTemaQuestao = new AvalTemaQuestao();
                        avalTemaQuestao.QuestaoTema = queTma;
                        avalTema.AvalTemaQuestao.Add(avalTemaQuestao);
                    }
                    aval.Avaliacao.AvaliacaoTema.Add(avalTema);
                }
            }
            else
            {
                foreach (var avaliacaoTema in acad.Avaliacao.AvaliacaoTema)
                {
                    aval.Avaliacao.AvaliacaoTema.Add(new AvaliacaoTema
                    {
                        Tema = avaliacaoTema.Tema,
                        AvalTemaQuestao = avaliacaoTema.AvalTemaQuestao.Select(a=> new AvalTemaQuestao { QuestaoTema = a.QuestaoTema }).ToList()
                    });
                }
            }

            foreach (var codJustificacao in justificaticoes)
            {
                aval.Justificacao.Add(acad.Justificacoes.First(j => j.CodJustificacao == codJustificacao));
            }
            
            Repositorio.GetInstance().AvalAcadReposicao.Add(aval);
            Repositorio.GetInstance().SaveChanges();            
            
            return nova ? Url.Action("Configurar", new { codigo = aval.Avaliacao.CodAvaliacao }) : Url.Action("Agendar", new { codigo = aval.Avaliacao.CodAvaliacao });
        }

        [Filters.AutenticacaoFilter(Categorias = new[] { 2})]
        public ActionResult Configurar(string codigo)
        {
            return null;
        }

        [Filters.AutenticacaoFilter(Categorias = new[] { 2 })]
        public ActionResult Agendar(string codigo)
        {
            return null;
        }
    }
}