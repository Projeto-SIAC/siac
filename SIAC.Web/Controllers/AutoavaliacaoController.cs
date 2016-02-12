using SIAC.Helpers;
using SIAC.Models;
using SIAC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SIAC.Controllers
{
    [Filters.AutenticacaoFilter]
    public class AutoavaliacaoController : Controller
    {
        public List<AvalAuto> Autoavaliacoes => 
            AvalAuto.ListarPorPessoa(Usuario.ObterPessoaFisica(Sessao.UsuarioMatricula));

        // GET: historico/autoavaliacao
        public ActionResult Index()
        {
            if (Request.Url.ToString().ToLower().Contains("principal"))
                return Redirect("~/historico/autoavaliacao");
            AvaliacaoIndexViewModel model = new AvaliacaoIndexViewModel();
            model.Dificuldades = Dificuldade.ListarOrdenadamente();
            List<Disciplina> disciplinas = new List<Disciplina>();
            foreach (AvalAuto auto in Autoavaliacoes)
                disciplinas.AddRange(auto.Disciplina);
            model.Disciplinas = disciplinas.Distinct().ToList();
            return View(model);
        }

        // POST: historico/autoavaliacao/listar
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

        // GET: principal/autoavaliacao/gerar
        public ActionResult Gerar()
        {
            AvaliacaoGerarViewModel model = new AvaliacaoGerarViewModel();
            model.Disciplinas = Disciplina.ListarTemQuestoes();
            model.Dificuldades = Dificuldade.ListarOrdenadamente();
            return View(model);
        }

        // POST: principal/autoavaliacao/confirmar
        [HttpPost]
        public ActionResult Confirmar(FormCollection formCollection)
        {
            if (!formCollection.HasKeys())
                return RedirectToAction("Index");

            if (!String.IsNullOrWhiteSpace(formCollection["chkAvalicoesSeparadas"]))
            {
                string[] disciplinas = formCollection["ddlDisciplinas"].Split(',');
                foreach (string disciplina in disciplinas)
                {
                    AvalAuto auto = new AvalAuto();

                    DateTime hoje = DateTime.Now;

                    /* Chave */
                    auto.Avaliacao = new Avaliacao();
                    auto.Avaliacao.TipoAvaliacao = TipoAvaliacao.ListarPorCodigo(TipoAvaliacao.AUTOAVALIACAO);
                    auto.Avaliacao.Ano = hoje.Year;
                    auto.Avaliacao.Semestre = hoje.SemestreAtual();
                    auto.Avaliacao.NumIdentificador = Avaliacao.ObterNumIdentificador(TipoAvaliacao.AUTOAVALIACAO);

                    /* Pessoa */
                    auto.CodPessoaFisica = Sistema.UsuarioAtivo[Sessao.UsuarioMatricula].Usuario.CodPessoaFisica;

                    /* Dados */
                    List<int> dificuldades = new List<int>();
                    
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
                    List<QuestaoTema> questoes = new List<QuestaoTema>();

                    if (qteObjetiva > 0)
                        questoes.AddRange(Questao.ListarPorDisciplina(int.Parse(disciplina), temas, codDificuldade, TipoQuestao.OBJETIVA, qteObjetiva));

                    if (qteDiscursiva > 0)
                        questoes.AddRange(Questao.ListarPorDisciplina(int.Parse(disciplina), temas, codDificuldade, TipoQuestao.DISCURSIVA, qteDiscursiva));

                    foreach (string tema in temas)
                    {
                        AvaliacaoTema avalTema = new AvaliacaoTema();
                        avalTema.Tema = Tema.ListarPorCodigo(int.Parse(disciplina), int.Parse(tema));
                        foreach (QuestaoTema questaoTema in questoes.Where(q => q.CodTema == int.Parse(tema)))
                        {
                            AvalTemaQuestao avalTemaQuestao = new AvalTemaQuestao();
                            avalTemaQuestao.QuestaoTema = questaoTema;
                            avalTema.AvalTemaQuestao.Add(avalTemaQuestao);
                        }
                        auto.Avaliacao.AvaliacaoTema.Add(avalTema);
                    }

                    auto.Avaliacao.DtCadastro = hoje;
                    auto.CodDificuldade = dificuldades.Max();

                    AvalAuto.Inserir(auto);
                    Lembrete.AdicionarNotificacao($"Autoavaliação {auto.Avaliacao.CodAvaliacao} gerada com sucesso.", Lembrete.POSITIVO);
                }
                return RedirectToAction("Index");
            }
            else
            {
                AvalAuto auto = new AvalAuto();

                DateTime hoje = DateTime.Now;

                /* Chave */
                auto.Avaliacao = new Avaliacao();
                auto.Avaliacao.TipoAvaliacao = TipoAvaliacao.ListarPorCodigo(TipoAvaliacao.AUTOAVALIACAO);
                auto.Avaliacao.Ano = hoje.Year;
                auto.Avaliacao.Semestre = hoje.SemestreAtual();
                auto.Avaliacao.NumIdentificador = Avaliacao.ObterNumIdentificador(TipoAvaliacao.AUTOAVALIACAO);

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
                    List<QuestaoTema> questoes = new List<QuestaoTema>();

                    if (qteObjetiva > 0)
                        questoes.AddRange(Questao.ListarPorDisciplina(int.Parse(disciplina), temas, codDificuldade, TipoQuestao.OBJETIVA, qteObjetiva));

                    if (qteDiscursiva > 0)
                        questoes.AddRange(Questao.ListarPorDisciplina(int.Parse(disciplina), temas, codDificuldade, TipoQuestao.DISCURSIVA, qteDiscursiva));

                    foreach (string tema in temas)
                    {
                        AvaliacaoTema avalTema = new AvaliacaoTema();
                        avalTema.Tema = Tema.ListarPorCodigo(int.Parse(disciplina), int.Parse(tema));
                        foreach (QuestaoTema questaoTema in questoes.Where(q => q.CodTema == int.Parse(tema)))
                        {
                            AvalTemaQuestao avalTemaQuestao = new AvalTemaQuestao();
                            avalTemaQuestao.QuestaoTema = questaoTema;
                            avalTema.AvalTemaQuestao.Add(avalTemaQuestao);
                        }
                        auto.Avaliacao.AvaliacaoTema.Add(avalTema);
                    }
                }

                auto.Avaliacao.DtCadastro = hoje;
                auto.CodDificuldade = dificuldades.Max();

                AvalAuto.Inserir(auto);
                Lembrete.AdicionarNotificacao($"Autoavaliação {auto.Avaliacao.CodAvaliacao} gerada com sucesso.", Lembrete.POSITIVO);
                return View(auto);
            }
        }

        // GET: principal/autoavaliacao/detalhe/AUTO201520001
        public ActionResult Detalhe(string codigo)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
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

                            foreach (AvaliacaoTema avaliacaoTema in auto.Avaliacao.AvaliacaoTema)
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
                                    if (avalTemaQuestao.QuestaoTema.Questao.CodTipoQuestao == TipoQuestao.OBJETIVA)
                                    {
                                        qteObjetivaDisciplina[avaliacaoTema.Tema.Disciplina.Descricao]++;
                                        qteObjetiva++;
                                        if (avalTemaQuestao.QuestaoTema.Questao.Alternativa.First(q => q.FlagGabarito.HasValue && q.FlagGabarito.Value).CodOrdem == avalQuesPessoaResposta.RespAlternativa)
                                            qteObjetivaAcertoDisciplina[avaliacaoTema.Tema.Disciplina.Descricao]++;
                                    }
                                }
                            }

                            model.Porcentagem = (auto.Avaliacao.AvalPessoaResultado.First().QteAcertoObj.Value / qteObjetiva) * 100;
                            foreach (string chave in qteObjetivaDisciplina.Keys)
                                if (qteObjetivaDisciplina[chave] > 0)
                                    model.Desempenho.Add(chave, (qteObjetivaAcertoDisciplina[chave] / qteObjetivaDisciplina[chave]) * 100);
                        }

                        return View(model);
                    }
                }
            }
            return RedirectToAction("Index");
        }

        // GET: principal/autoavaliacao/realizar/AUTO201520001
        public ActionResult Realizar(string codigo)
        {
            int codPessoaFisica = Sistema.UsuarioAtivo[Sessao.UsuarioMatricula].Usuario.CodPessoaFisica;
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                AvalAuto auto = AvalAuto.ListarPorCodigoAvaliacao(codigo);
                if (auto.CodPessoaFisica == codPessoaFisica)
                {
                    return View(auto);
                }
                return RedirectToAction("Index");
            }
            else
            {
                AutoavaliacaoNovoViewModel model = new AutoavaliacaoNovoViewModel();
                model.Geradas = AvalAuto.ListarNaoRealizadaPorPessoa(codPessoaFisica);
                return View("Novo", model);
            }
        }

        // GET: principal/autoavaliacao/imprimir/AUTO201520001
        public ActionResult Imprimir(string codigo)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                AvalAuto auto = AvalAuto.ListarPorCodigoAvaliacao(codigo);
                if (auto.CodPessoaFisica == Sistema.UsuarioAtivo[Sessao.UsuarioMatricula].Usuario.CodPessoaFisica)
                    return View(auto);
            }
            return RedirectToAction("index");
        }

        // POST: principal/autoavaliacao/resultado/AUTO201520001
        [HttpPost]
        public ActionResult Resultado(string codigo, FormCollection form)
        {
            int codPessoaFisica = Sistema.UsuarioAtivo[Sessao.UsuarioMatricula].Usuario.CodPessoaFisica;
            if (!String.IsNullOrWhiteSpace(codigo) && form.HasKeys())
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

                    foreach (AvaliacaoTema avaliacaoTema in auto.Avaliacao.AvaliacaoTema)
                    {
                        if (!qteObjetivaDisciplina.ContainsKey(avaliacaoTema.Tema.Disciplina.Descricao))
                        {
                            qteObjetivaDisciplina.Add(avaliacaoTema.Tema.Disciplina.Descricao, 0);
                        }
                        if (!qteObjetivaAcertoDisciplina.ContainsKey(avaliacaoTema.Tema.Disciplina.Descricao))
                        {
                            qteObjetivaAcertoDisciplina.Add(avaliacaoTema.Tema.Disciplina.Descricao, 0);
                        }
                        foreach (AvalTemaQuestao avalTemaQuestao in avaliacaoTema.AvalTemaQuestao)
                        {
                            AvalQuesPessoaResposta avalQuesPessoaResposta = avalTemaQuestao.AvalQuesPessoaResposta.FirstOrDefault(r => r.PessoaFisica.CodPessoa == codPessoaFisica);
                            if (avalQuesPessoaResposta == null)
                            {
                                avalQuesPessoaResposta = new AvalQuesPessoaResposta();
                            }
                            avalQuesPessoaResposta.CodPessoaFisica = codPessoaFisica;
                            if (avalTemaQuestao.QuestaoTema.Questao.CodTipoQuestao == TipoQuestao.OBJETIVA)
                            {
                                qteObjetivaDisciplina[avaliacaoTema.Tema.Disciplina.Descricao]++;
                                qteObjetiva++;
                                avalQuesPessoaResposta.RespAlternativa = int.Parse(form["rdoResposta" + avalTemaQuestao.QuestaoTema.Questao.CodQuestao]);
                                if (avalTemaQuestao.QuestaoTema.Questao.Alternativa.First(q => q.FlagGabarito.HasValue && q.FlagGabarito.Value).CodOrdem == avalQuesPessoaResposta.RespAlternativa)
                                {
                                    avalPessoaResultado.QteAcertoObj++;
                                    avalQuesPessoaResposta.RespNota = 10;
                                    qteObjetivaAcertoDisciplina[avaliacaoTema.Tema.Disciplina.Descricao]++;
                                }
                                else
                                {
                                    avalQuesPessoaResposta.RespNota = 0;
                                }
                            }
                            else
                            {
                                avalQuesPessoaResposta.RespDiscursiva = form["txtResposta" + avalTemaQuestao.QuestaoTema.Questao.CodQuestao].Trim();
                            }
                            avalQuesPessoaResposta.RespComentario = !String.IsNullOrWhiteSpace(form["txtComentario" + avalTemaQuestao.QuestaoTema.Questao.CodQuestao]) ? form["txtComentario" + avalTemaQuestao.QuestaoTema.Questao.CodQuestao].Trim() : null;
                            avalTemaQuestao.AvalQuesPessoaResposta.Add(avalQuesPessoaResposta);
                        }
                    }
                    IEnumerable<AvalQuesPessoaResposta> lstAvalQuesPessoaResposta = auto.Avaliacao.PessoaResposta.Where(r => r.CodPessoaFisica == codPessoaFisica);
                    avalPessoaResultado.Nota = lstAvalQuesPessoaResposta.Average(r => r.RespNota);
                    auto.Avaliacao.AvalPessoaResultado.Add(avalPessoaResultado);

                    Repositorio.GetInstance().SaveChanges();

                    AvaliacaoResultadoViewModel model = new AvaliacaoResultadoViewModel();
                    model.Avaliacao = auto.Avaliacao;
                    model.Porcentagem = (avalPessoaResultado.QteAcertoObj.Value / qteObjetiva) * 100;
                    foreach (string chave in qteObjetivaDisciplina.Keys)
                        if (qteObjetivaDisciplina[chave] > 0)
                            model.Desempenho.Add(chave, (qteObjetivaAcertoDisciplina[chave] / qteObjetivaDisciplina[chave]) * 100);
                    Lembrete.AdicionarNotificacao($"Autoavaliação {auto.Avaliacao.CodAvaliacao} realizada. Confira seu resultado!");
                    return View(model);
                }
                return RedirectToAction("Detalhe", new { codigo = auto.Avaliacao.CodAvaliacao });
            }
            return RedirectToAction("Realizar");
        }

        // POST: principal/autoavaliacao/arquivar/AUTO201520001
        [HttpPost]
        public ActionResult Arquivar(string codigo) => 
            !String.IsNullOrWhiteSpace(codigo) ? Json(Avaliacao.AlternarFlagArquivo(codigo)) : Json(false);

        // POST: principal/autoavaliacao/salvarresposta/AUTO201520001
        [HttpPost]
        public void SalvarResposta(string codigo, int questao, string resposta, string comentario) =>
            AvalQuesPessoaResposta.SalvarResposta(
                Avaliacao.ListarPorCodigoAvaliacao(codigo), 
                Questao.ListarPorCodigo(questao), 
                Sistema.UsuarioAtivo[Sessao.UsuarioMatricula].Usuario.PessoaFisica, 
                resposta, 
                comentario);
    }
}