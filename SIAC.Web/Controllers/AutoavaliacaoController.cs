using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIAC.Models;

namespace SIAC.Controllers
{
    [Filters.CategoriaFilter]
    public class AutoavaliacaoController : Controller
    {
        public List<AvalAuto> Autoavaliacoes
        {
            get
            {
                return AvalAuto.ListarPorPessoa(Usuario.ObterPessoaFisica(Helpers.Sessao.UsuarioMatricula));
            }
        }

        // GET: Autoavaliacao
        public ActionResult Index()
        {
            if (Request.Url.ToString().ToLower().Contains("dashboard"))
            {
                return Redirect("~/Historico/Autoavaliacao");
            }
            ViewBag.Dificuldades = Dificuldade.ListarOrdenadamente();
            List<Disciplina> tempLstDisciplina = new List<Disciplina>();
            foreach (var auto in Autoavaliacoes)
            {
                tempLstDisciplina.AddRange(auto.Disciplina);
            }
            ViewBag.Disciplinas = tempLstDisciplina.Distinct().ToList();
            return View();
        }

        // POST: Autoavaliacao/Listar
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Listar(int? pagina, string pesquisa, string ordenar, string[] categorias, string disciplina, string dificuldade)
        {
            var qte = 10;
            var autoavaliacoes = Autoavaliacoes;
            pagina = pagina ?? 1;
            if (!String.IsNullOrWhiteSpace(pesquisa))
            {
                autoavaliacoes = autoavaliacoes.Where(a => a.Avaliacao.CodAvaliacao.ToLower().Contains(pesquisa.ToLower())).ToList();
            }

            if (!String.IsNullOrWhiteSpace(disciplina))
            {
                autoavaliacoes = autoavaliacoes.Where(a => a.Disciplina.Where(d=>d.CodDisciplina == int.Parse(disciplina)).Count()>0).ToList();
            }

            if (!String.IsNullOrWhiteSpace(dificuldade))
            {
                autoavaliacoes = autoavaliacoes.Where(a => a.CodDificuldade == int.Parse(dificuldade)).ToList();
            }

            if (categorias != null)
            {
                if (categorias.Contains("pendente") && !categorias.Contains("arquivo") && !categorias.Contains("realizada"))
                {
                    autoavaliacoes = autoavaliacoes.Where(a => a.Avaliacao.FlagPendente).ToList();
                }
                else if (!categorias.Contains("pendente") && categorias.Contains("arquivo") && !categorias.Contains("realizada"))
                {
                    autoavaliacoes = autoavaliacoes.Where(a => a.Avaliacao.FlagArquivo).ToList();
                }
                else if (!categorias.Contains("pendente") && !categorias.Contains("arquivo") && categorias.Contains("realizada"))
                {
                    autoavaliacoes = autoavaliacoes.Where(a => a.Avaliacao.AvalPessoaResultado.Count > 0).ToList();
                }
                else if (!categorias.Contains("pendente") && categorias.Contains("arquivo") && categorias.Contains("realizada"))
                {
                    autoavaliacoes = autoavaliacoes.Where(a => a.Avaliacao.AvalPessoaResultado.Count > 0 || a.Avaliacao.FlagArquivo).ToList();
                }
                else if (categorias.Contains("pendente") && !categorias.Contains("arquivo") && categorias.Contains("realizada"))
                {
                    autoavaliacoes = autoavaliacoes.Where(a => a.Avaliacao.AvalPessoaResultado.Count > 0 || a.Avaliacao.FlagPendente).ToList();
                }
                else if (categorias.Contains("pendente") && categorias.Contains("arquivo") && !categorias.Contains("realizada"))
                {
                    autoavaliacoes = autoavaliacoes.Where(a => a.Avaliacao.FlagArquivo || a.Avaliacao.FlagPendente).ToList();
                }
            }

            switch (ordenar)
            {
                case "data_desc":
                    autoavaliacoes = autoavaliacoes.OrderByDescending(a => a.Avaliacao.DtCadastro).ToList();
                    break;
                case "data":
                    autoavaliacoes = autoavaliacoes.OrderBy(a => a.Avaliacao.DtCadastro).ToList();
                    break;
                default:
                    autoavaliacoes = autoavaliacoes.OrderByDescending(a => a.Avaliacao.DtCadastro).ToList();
                    break;
            }
            return PartialView("_ListaAutoavaliacao", autoavaliacoes.Skip((qte*pagina.Value)-qte).Take(qte).ToList());
        }        

        // GET: Autoavaliacao/Gerar
        public ActionResult Gerar()
        {
            ViewBag.Disciplinas = Disciplina.ListarTemQuestoes();
            ViewBag.Dificuldades = Dificuldade.ListarOrdenadamente().Select(d=>new {Codigo = d.CodDificuldade, Descricao = d.Descricao });
            return View();
        }

        // POST: Autoavaliacao/Confirmar
        [HttpPost]
        public ActionResult Confirmar(FormCollection formCollection)
        {
            //ViewBag.Form = formCollection;

            AvalAuto auto = new AvalAuto();

            var hoje = DateTime.Now;

            /* Chave */
            auto.Avaliacao = new Avaliacao();
            auto.Avaliacao.TipoAvaliacao = TipoAvaliacao.ListarPorCodigo(1);
            //auto.Avaliacao.CodTipoAvaliacao = 1;
            auto.Avaliacao.Ano = hoje.Year;
            auto.Avaliacao.Semestre = hoje.Month > 6 ? 2 : 1;
            auto.Avaliacao.NumIdentificador = Avaliacao.ObterNumIdentificador(1);

            /* Pessoa */
            var strMatr = Helpers.Sessao.UsuarioMatricula;
            auto.CodPessoaFisica = Usuario.ObterPessoaFisica(strMatr);

            var disciplinas = formCollection["ddlDisciplinas"].Split(',');
            /* Dados */
            List<int> dificuldades = new List<int>();
            //List<QuestaoTema> VBQuestoes = new List<QuestaoTema>();
            foreach (var strDisc in disciplinas)
            {
                /* Dificuldade */
                int codDificuldade = int.Parse(formCollection["ddlDificuldade" + strDisc]);
                dificuldades.Add(codDificuldade);

                /* Quantidade */
                int qteObjetiva = 0;
                int qteDiscursiva = 0;
                if (formCollection["ddlTipo"] == "3")
                {
                    int.TryParse(formCollection["txtQteObjetiva" + strDisc], out qteObjetiva);
                    int.TryParse(formCollection["txtQteDiscursiva" + strDisc], out qteDiscursiva);
                }
                else if (formCollection["ddlTipo"] == "2")
                {
                    int.TryParse(formCollection["txtQteDiscursiva" + strDisc], out qteDiscursiva);
                }
                else if (formCollection["ddlTipo"] == "1")
                {
                    int.TryParse(formCollection["txtQteObjetiva" + strDisc], out qteObjetiva);
                }

                /* Temas */
                string[] arrTemaCods = formCollection["ddlTemas" + strDisc].Split(',');

                /* Questões */
                List<QuestaoTema> lstQuestoes = new List<QuestaoTema>();

                if (qteObjetiva > 0)
                {
                    lstQuestoes.AddRange(Questao.ListarPorDisciplina(int.Parse(strDisc), arrTemaCods, codDificuldade, 1, qteObjetiva));
                }
                if (qteDiscursiva > 0)
                {
                    lstQuestoes.AddRange(Questao.ListarPorDisciplina(int.Parse(strDisc), arrTemaCods, codDificuldade, 2, qteDiscursiva));
                }

                foreach (var strTemaCod in arrTemaCods)
                {
                    AvaliacaoTema avalTema = new AvaliacaoTema();
                    avalTema.Tema = Tema.ListarPorCodigo(int.Parse(strDisc), int.Parse(strTemaCod));
                    foreach (var queTma in lstQuestoes.Where(q=>q.CodTema == int.Parse(strTemaCod)))
                    {
                        AvalTemaQuestao avalTemaQuestao = new AvalTemaQuestao();
                        avalTemaQuestao.QuestaoTema = queTma;
                        avalTema.AvalTemaQuestao.Add(avalTemaQuestao);
                    }                   
                    auto.Avaliacao.AvaliacaoTema.Add(avalTema);                    
                }
                //VBQuestoes.AddRange(lstQuestoes);
            }

            auto.Avaliacao.DtCadastro = hoje;
            auto.CodDificuldade = dificuldades.Max();

            //ViewBag.QteQuestoes = VBQuestoes.Count;
            //ViewBag.QuestoesDaAvaliacao = VBQuestoes;

            AvalAuto.Inserir(auto);

            return View(auto);
        }

        // GET: Autoavaliacao/Detalhe/AUTO201520001
        public ActionResult Detalhe(string codigo)
        {
            if (!String.IsNullOrEmpty(codigo))
            {
                AvalAuto auto = AvalAuto.ListarPorCodigoAvaliacao(codigo);
                int codPessoaFisica = Usuario.ObterPessoaFisica(Helpers.Sessao.UsuarioMatricula);
                if (auto != null)
                {
                    if (auto.CodPessoaFisica == codPessoaFisica)
                    {
                        if (auto.Avaliacao.AvalPessoaResultado.Count > 0)
                        {
                            double qteObjetiva = 0;
                            Dictionary<string, double> qteObjetivaDisciplina = new Dictionary<string, double>();
                            Dictionary<string, double> qteObjetivaAcertoDisciplina = new Dictionary<string, double>();

                            foreach (var avaliacaoTema in auto.Avaliacao.AvaliacaoTema)
                            {
                                if (!qteObjetivaDisciplina.ContainsKey(avaliacaoTema.Tema.Disciplina.Descricao))
                                {
                                    qteObjetivaDisciplina.Add(avaliacaoTema.Tema.Disciplina.Descricao, 0);
                                }
                                if (!qteObjetivaAcertoDisciplina.ContainsKey(avaliacaoTema.Tema.Disciplina.Descricao))
                                {
                                    qteObjetivaAcertoDisciplina.Add(avaliacaoTema.Tema.Disciplina.Descricao, 0);
                                }
                                foreach (var avalTemaQuestao in avaliacaoTema.AvalTemaQuestao)
                                {
                                    AvalQuesPessoaResposta avalQuesPessoaResposta = avalTemaQuestao.AvalQuesPessoaResposta.First();
                                    if (avalTemaQuestao.QuestaoTema.Questao.CodTipoQuestao == 1)
                                    {
                                        qteObjetivaDisciplina[avaliacaoTema.Tema.Disciplina.Descricao]++;
                                        qteObjetiva++;
                                        if (avalTemaQuestao.QuestaoTema.Questao.Alternativa.First(q => q.FlagGabarito.HasValue && q.FlagGabarito.Value).CodOrdem == avalQuesPessoaResposta.RespAlternativa)
                                        {
                                            qteObjetivaAcertoDisciplina[avaliacaoTema.Tema.Disciplina.Descricao]++;
                                        }
                                    }
                                }
                            }

                            ViewBag.Porcentagem = (auto.Avaliacao.AvalPessoaResultado.First().QteAcertoObj / qteObjetiva) * 100;
                            ViewBag.Desempenho = new Dictionary<string, double>();
                            foreach (var key in qteObjetivaDisciplina.Keys)
                            {
                                if (qteObjetivaDisciplina[key] > 0)
                                {
                                    ViewBag.Desempenho.Add(key, (qteObjetivaAcertoDisciplina[key] / qteObjetivaDisciplina[key]) * 100);
                                }
                            }
                        }

                        return View(auto);
                    }
                }
            }
            return RedirectToAction("Index");
        }

        // GET: Autoavaliacao/Realizar/AUTO201520001
        public ActionResult Realizar(string codigo)
        {
            if (!String.IsNullOrEmpty(codigo))
            {
                AvalAuto auto = AvalAuto.ListarPorCodigoAvaliacao(codigo);
                if (auto.CodPessoaFisica == Usuario.ObterPessoaFisica(Helpers.Sessao.UsuarioMatricula))
                {
                    return View(auto);
                }
            }
            int codPessoaFisica = Usuario.ObterPessoaFisica(Helpers.Sessao.UsuarioMatricula);
            ViewBag.Geradas = AvalAuto.ListarNaoRealizadaPorPessoa(codPessoaFisica);
            return View("Novo");
        }

        // GET: Autoavaliacao/Imprimir/AUTO201520001
        public ActionResult Imprimir(string codigo)
        {
            if (!String.IsNullOrEmpty(codigo))
            {
                AvalAuto auto = AvalAuto.ListarPorCodigoAvaliacao(codigo);
                if (auto.CodPessoaFisica == Usuario.ObterPessoaFisica(Helpers.Sessao.UsuarioMatricula))
                {
                    return View(auto);
                }
            }
            return RedirectToAction("index");
        }

        // POST: Autoavaliacao/Resultado/AUTO201520001
        [HttpPost]
        public ActionResult Resultado(string codigo, FormCollection form)
        {
            int codPessoaFisica = Usuario.ObterPessoaFisica(Helpers.Sessao.UsuarioMatricula);
            if (!String.IsNullOrEmpty(codigo))
            {
                AvalAuto auto = AvalAuto.ListarPorCodigoAvaliacao(codigo);
                if (auto.Avaliacao.AvalPessoaResultado.Count == 0 && auto.CodPessoaFisica == codPessoaFisica)
                {
                    AvalPessoaResultado avalPessoaResultado = new AvalPessoaResultado();
                    avalPessoaResultado.CodPessoaFisica = codPessoaFisica;
                    avalPessoaResultado.HoraTermino = DateTime.Now;
                    avalPessoaResultado.QteAcertoObj = 0;

                    double qteObjetiva = 0;
                    Dictionary<string, double> qteObjetivaDisciplina = new Dictionary<string, double>();
                    Dictionary<string, double> qteObjetivaAcertoDisciplina = new Dictionary<string, double>();

                    foreach (var avaliacaoTema in auto.Avaliacao.AvaliacaoTema)
                    {
                        if (!qteObjetivaDisciplina.ContainsKey(avaliacaoTema.Tema.Disciplina.Descricao))
                        {
                            qteObjetivaDisciplina.Add(avaliacaoTema.Tema.Disciplina.Descricao, 0);
                        }
                        if (!qteObjetivaAcertoDisciplina.ContainsKey(avaliacaoTema.Tema.Disciplina.Descricao))
                        {
                            qteObjetivaAcertoDisciplina.Add(avaliacaoTema.Tema.Disciplina.Descricao, 0);
                        }
                        foreach (var avalTemaQuestao in avaliacaoTema.AvalTemaQuestao)
                        {
                            AvalQuesPessoaResposta avalQuesPessoaResposta = new AvalQuesPessoaResposta();
                            avalQuesPessoaResposta.CodPessoaFisica = codPessoaFisica;
                            if (avalTemaQuestao.QuestaoTema.Questao.CodTipoQuestao == 1)
                            {
                                qteObjetivaDisciplina[avaliacaoTema.Tema.Disciplina.Descricao]++;
                                qteObjetiva++;
                                avalQuesPessoaResposta.RespAlternativa = int.Parse(form["rdoResposta" + avalTemaQuestao.QuestaoTema.Questao.CodQuestao]);
                                if (avalTemaQuestao.QuestaoTema.Questao.Alternativa.First(q => q.FlagGabarito.HasValue && q.FlagGabarito.Value).CodOrdem == avalQuesPessoaResposta.RespAlternativa)
                                {
                                    avalPessoaResultado.QteAcertoObj++;
                                    qteObjetivaAcertoDisciplina[avaliacaoTema.Tema.Disciplina.Descricao]++;
                                }
                            }
                            else
                            {
                                avalQuesPessoaResposta.RespDiscursiva = form["txtResposta" + avalTemaQuestao.QuestaoTema.Questao.CodQuestao].Trim();
                            }
                            avalQuesPessoaResposta.RespComentario = !String.IsNullOrEmpty(form["txtComentario" + avalTemaQuestao.QuestaoTema.Questao.CodQuestao]) ? form["txtComentario" + avalTemaQuestao.QuestaoTema.Questao.CodQuestao].Trim() : null;
                            avalTemaQuestao.AvalQuesPessoaResposta.Add(avalQuesPessoaResposta);
                        }

                    }

                    auto.Avaliacao.AvalPessoaResultado.Add(avalPessoaResultado);

                    Repositorio.GetInstance().SaveChanges();

                    ViewBag.Porcentagem = (avalPessoaResultado.QteAcertoObj / qteObjetiva) * 100;
                    ViewBag.Desempenho = new Dictionary<string, double>();
                    foreach (var key in qteObjetivaDisciplina.Keys)
                    {
                        if (qteObjetivaDisciplina[key] > 0)
                        {
                            ViewBag.Desempenho.Add(key, (qteObjetivaAcertoDisciplina[key] / qteObjetivaDisciplina[key]) * 100);
                        }
                    }
                    return View(auto);
                }
                return RedirectToAction("Detalhe", new { codigo = auto.Avaliacao.CodAvaliacao });
            }            
            ViewBag.Geradas = AvalAuto.ListarNaoRealizadaPorPessoa(codPessoaFisica);
            return View("Novo");
        }

        [HttpPost]
        public ActionResult Arquivar(string codigo)
        {
            if (!String.IsNullOrEmpty(codigo))
            {
                return Json(Avaliacao.AlternarFlagArquivo(codigo));
            }
            return Json(false);
        } 
    }
}