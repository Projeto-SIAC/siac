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
    [Filters.AutenticacaoFilter]
    public class AutoavaliacaoController : Controller
    {
        private const int CodTipoAvaliacao = 1;

        public List<AvalAuto> Autoavaliacoes => 
            AvalAuto.ListarPorPessoa(Usuario.ObterPessoaFisica(Helpers.Sessao.UsuarioMatricula));

        // GET: Autoavaliacao
        [OutputCache(CacheProfile = "PorUsuario")]
        public ActionResult Index()
        {
            if (Request.Url.ToString().ToLower().Contains("principal"))
            {
                return Redirect("~/historico/autoavaliacao");
            }
            AvaliacaoIndexViewModel model = new AvaliacaoIndexViewModel();
            model.Dificuldades = Dificuldade.ListarOrdenadamente();
            List<Disciplina> tempLstDisciplina = new List<Disciplina>();
            foreach (var auto in Autoavaliacoes)
            {
                tempLstDisciplina.AddRange(auto.Disciplina);
            }
            model.Disciplinas = tempLstDisciplina.Distinct().ToList();
            return View(model);
        }

        // POST: Autoavaliacao/Listar
        [HttpPost]
        public ActionResult Listar(int? pagina, string pesquisa, string ordenar, string[] categorias, string disciplina, string dificuldade)
        {
            int quantidade = 10;
            List<AvalAuto> autoavaliacoes = Autoavaliacoes;
            pagina = pagina ?? 1;
            if (!String.IsNullOrWhiteSpace(pesquisa))
            {
                autoavaliacoes = autoavaliacoes.Where(a => a.Avaliacao.CodAvaliacao.ToLower().Contains(pesquisa.ToLower())).ToList();
            }

            if (!String.IsNullOrWhiteSpace(disciplina))
            {
                autoavaliacoes = autoavaliacoes.Where(a => a.Disciplina.Where(d => d.CodDisciplina == int.Parse(disciplina)).Count() > 0).ToList();
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
            return PartialView("_ListaAutoavaliacao", autoavaliacoes.Skip((quantidade * pagina.Value) - quantidade).Take(quantidade).ToList());
        }

        // GET: Autoavaliacao/Gerar
        public ActionResult Gerar()
        {
            AvaliacaoGerarViewModel model = new AvaliacaoGerarViewModel();
            model.Disciplinas = Disciplina.ListarTemQuestoes();
            model.Dificuldades = Dificuldade.ListarOrdenadamente();
            return View(model);
        }

        // POST: Autoavaliacao/Confirmar
        [HttpPost]
        public ActionResult Confirmar(FormCollection formCollection)
        {
            AvalAuto auto = new AvalAuto();

            DateTime hoje = DateTime.Now;

            /* Chave */
            auto.Avaliacao = new Avaliacao();
            auto.Avaliacao.TipoAvaliacao = TipoAvaliacao.ListarPorCodigo(CodTipoAvaliacao);
            auto.Avaliacao.Ano = hoje.Year;
            auto.Avaliacao.Semestre = hoje.Month > 6 ? 2 : 1;
            auto.Avaliacao.NumIdentificador = Avaliacao.ObterNumIdentificador(CodTipoAvaliacao);

            /* Pessoa */
            auto.CodPessoaFisica = Sistema.UsuarioAtivo[Sessao.UsuarioMatricula].Usuario.CodPessoaFisica;

            string[] disciplinas = formCollection["ddlDisciplinas"].Split(',');
            /* Dados */
            List<int> dificuldades = new List<int>();
            foreach (string disciplina in disciplinas)
            {
                /* Dificuldade */
                int codDificuldade = int.Parse(formCollection["ddlDificuldade" + disciplina]);
                dificuldades.Add(codDificuldade);

                /* Quantidade */
                int qteObjetiva = 0;
                int qteDiscursiva = 0;
                if (formCollection["ddlTipo"] == "3")
                {
                    int.TryParse(formCollection["txtQteObjetiva" + disciplina], out qteObjetiva);
                    int.TryParse(formCollection["txtQteDiscursiva" + disciplina], out qteDiscursiva);
                }
                else if (formCollection["ddlTipo"] == "2")
                {
                    int.TryParse(formCollection["txtQteDiscursiva" + disciplina], out qteDiscursiva);
                }
                else if (formCollection["ddlTipo"] == "1")
                {
                    int.TryParse(formCollection["txtQteObjetiva" + disciplina], out qteObjetiva);
                }

                /* Temas */
                string[] temas = formCollection["ddlTemas" + disciplina].Split(',');

                /* Questões */
                List<QuestaoTema> lstQuestoes = new List<QuestaoTema>();

                if (qteObjetiva > 0)
                {
                    lstQuestoes.AddRange(Questao.ListarPorDisciplina(int.Parse(disciplina), temas, codDificuldade, 1, qteObjetiva));
                }
                if (qteDiscursiva > 0)
                {
                    lstQuestoes.AddRange(Questao.ListarPorDisciplina(int.Parse(disciplina), temas, codDificuldade, 2, qteDiscursiva));
                }

                foreach (string tema in temas)
                {
                    AvaliacaoTema avalTema = new AvaliacaoTema();
                    avalTema.Tema = Tema.ListarPorCodigo(int.Parse(disciplina), int.Parse(tema));
                    foreach (var queTma in lstQuestoes.Where(q => q.CodTema == int.Parse(tema)))
                    {
                        AvalTemaQuestao avalTemaQuestao = new AvalTemaQuestao();
                        avalTemaQuestao.QuestaoTema = queTma;
                        avalTema.AvalTemaQuestao.Add(avalTemaQuestao);
                    }
                    auto.Avaliacao.AvaliacaoTema.Add(avalTema);
                }
            }

            auto.Avaliacao.DtCadastro = hoje;
            auto.CodDificuldade = dificuldades.Max();

            AvalAuto.Inserir(auto);
            Lembrete.AdicionarNotificacao($"Autoavaliação {auto.Avaliacao.CodAvaliacao} gerada com sucesso.", Lembrete.Positivo);
            return View(auto);
        }

        // GET: Autoavaliacao/Detalhe/AUTO201520001
        public ActionResult Detalhe(string codigo)
        {
            if (!String.IsNullOrEmpty(codigo))
            {
                AvalAuto auto = AvalAuto.ListarPorCodigoAvaliacao(codigo);
                int codPessoaFisica = Sistema.UsuarioAtivo[Sessao.UsuarioMatricula].Usuario.CodPessoaFisica;
                if (auto != null)
                {
                    if (auto.CodPessoaFisica == codPessoaFisica)
                    {
                        AutoavaliacaoDetalheViewModel model = new AutoavaliacaoDetalheViewModel();
                        model.Avaliacao = auto.Avaliacao;

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

                            model.Porcentagem = (auto.Avaliacao.AvalPessoaResultado.First().QteAcertoObj.Value / qteObjetiva) * 100;
                            foreach (string chave in qteObjetivaDisciplina.Keys)
                            {
                                if (qteObjetivaDisciplina[chave] > 0)
                                {
                                    model.Desempenho.Add(chave, (qteObjetivaAcertoDisciplina[chave] / qteObjetivaDisciplina[chave]) * 100);
                                }
                            }
                        }

                        return View(model);
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
                if (auto.CodPessoaFisica == Sistema.UsuarioAtivo[Sessao.UsuarioMatricula].Usuario.CodPessoaFisica)
                {
                    return View(auto);
                }
            }
            AutoavaliacaoNovoViewModel model = new AutoavaliacaoNovoViewModel();
            int codPessoaFisica = Sistema.UsuarioAtivo[Sessao.UsuarioMatricula].Usuario.CodPessoaFisica;
            model.Geradas = AvalAuto.ListarNaoRealizadaPorPessoa(codPessoaFisica);
            return View("Novo", model);
        }

        // GET: Autoavaliacao/Imprimir/AUTO201520001
        public ActionResult Imprimir(string codigo)
        {
            if (!String.IsNullOrEmpty(codigo))
            {
                AvalAuto auto = AvalAuto.ListarPorCodigoAvaliacao(codigo);
                if (auto.CodPessoaFisica == Sistema.UsuarioAtivo[Sessao.UsuarioMatricula].Usuario.CodPessoaFisica)
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
            int codPessoaFisica = Sistema.UsuarioAtivo[Sessao.UsuarioMatricula].Usuario.CodPessoaFisica;
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

                    AvaliacaoResultadoViewModel model = new AvaliacaoResultadoViewModel();
                    model.Avaliacao = auto.Avaliacao;
                    model.Porcentagem = (avalPessoaResultado.QteAcertoObj.Value / qteObjetiva) * 100;
                    foreach (var chave in qteObjetivaDisciplina.Keys)
                    {
                        if (qteObjetivaDisciplina[chave] > 0)
                        {
                            model.Desempenho.Add(chave, (qteObjetivaAcertoDisciplina[chave] / qteObjetivaDisciplina[chave]) * 100);
                        }
                    }
                    Lembrete.AdicionarNotificacao($"Autoavaliação {auto.Avaliacao.CodAvaliacao} realizada. Confira seu resultado!");
                    return View(model);
                }
                return RedirectToAction("Detalhe", new { codigo = auto.Avaliacao.CodAvaliacao });
            }
            return RedirectToAction("Realizar");
        }

        // POST: Autoavaliacao/Arquivar/AUTO201520001
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