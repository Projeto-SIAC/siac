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
    public class CertificacaoController : Controller
    {
        public List<AvalCertificacao> Certificacoes
        {
            get
            {
                var matr = Sessao.UsuarioMatricula;
                if (Helpers.Sessao.UsuarioCategoriaCodigo == 2)
                {                    
                    int codProfessor = Professor.ListarPorMatricula(matr).CodProfessor;
                    var lstProfessor = AvalCertificacao.ListarPorProfessor(codProfessor);
                    int codPessoaFisica = Usuario.ObterPessoaFisica(matr);
                    return AvalCertificacao.ListarPorPessoa(codPessoaFisica).Union(lstProfessor).Distinct().ToList();
                }
                else
                {
                    int codPessoaFisica = Usuario.ObterPessoaFisica(matr);
                    return AvalCertificacao.ListarPorPessoa(codPessoaFisica);
                }
            }
        }

        // GET: Certificacao
        public ActionResult Index()
        {
            if (Request.Url.ToString().ToLower().Contains("dashboard"))
            {
                return Redirect("~/historico/avaliacao/certificacao");
            }
            var model = new ViewModels.AvaliacaoIndexViewModel();
            model.Disciplinas = Certificacoes.Select(a => a.Disciplina).Distinct().ToList();
            return View(model);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Listar(int? pagina, string pesquisa, string ordenar, string[] categorias, string disciplina)
        {
            var qte = 12;
            var certificacoes = Certificacoes;
            pagina = pagina ?? 1;
            if (!String.IsNullOrWhiteSpace(pesquisa))
            {
                certificacoes = certificacoes.Where(a => a.Avaliacao.CodAvaliacao.ToLower().Contains(pesquisa.ToLower())).ToList();
            }

            if (!String.IsNullOrWhiteSpace(disciplina))
            {
                certificacoes = certificacoes.Where(a => a.CodDisciplina == int.Parse(disciplina)).ToList();
            }

            if (categorias != null)
            {
                if (categorias.Contains("agendada") && !categorias.Contains("arquivo") && !categorias.Contains("realizada"))
                {
                    certificacoes = certificacoes.Where(a => a.Avaliacao.FlagAgendada).ToList();
                }
                else if (!categorias.Contains("agendada") && categorias.Contains("arquivo") && !categorias.Contains("realizada"))
                {
                    certificacoes = certificacoes.Where(a => a.Avaliacao.FlagArquivo).ToList();
                }
                else if (!categorias.Contains("agendada") && !categorias.Contains("arquivo") && categorias.Contains("realizada"))
                {
                    certificacoes = certificacoes.Where(a => a.Avaliacao.FlagRealizada).ToList();
                }
                else if (!categorias.Contains("agendada") && categorias.Contains("arquivo") && categorias.Contains("realizada"))
                {
                    certificacoes = certificacoes.Where(a => a.Avaliacao.FlagRealizada || a.Avaliacao.FlagArquivo).ToList();
                }
                else if (categorias.Contains("agendada") && !categorias.Contains("arquivo") && categorias.Contains("realizada"))
                {
                    certificacoes = certificacoes.Where(a => a.Avaliacao.FlagRealizada || a.Avaliacao.FlagAgendada).ToList();
                }
                else if (categorias.Contains("agendada") && categorias.Contains("arquivo") && !categorias.Contains("realizada"))
                {
                    certificacoes = certificacoes.Where(a => a.Avaliacao.FlagArquivo || a.Avaliacao.FlagAgendada).ToList();
                }
            }

            switch (ordenar)
            {
                case "data_desc":
                    certificacoes = certificacoes.OrderByDescending(a => a.Avaliacao.DtCadastro).ToList();
                    break;
                case "data":
                    certificacoes = certificacoes.OrderBy(a => a.Avaliacao.DtCadastro).ToList();
                    break;
                default:
                    certificacoes = certificacoes.OrderByDescending(a => a.Avaliacao.DtCadastro).ToList();
                    break;
            }

            return PartialView("_ListaCertificacao", certificacoes.Skip((qte * pagina.Value) - qte).Take(qte).ToList());
        }

        // GET: Certificacao/Gerar
        [Filters.AutenticacaoFilter(Categorias = new[] { 2/*, 3*/ })]
        public ActionResult Gerar()
        {
            var model = new ViewModels.AvaliacaoGerarViewModel();
            model.Disciplinas = /*Helpers.Sessao.UsuarioCategoriaCodigo == 2 ? */Disciplina.ListarPorProfessor(Helpers.Sessao.UsuarioMatricula)/*: Disciplina.ListarOrdenadamente()*/;
            model.Dificuldades = Dificuldade.ListarOrdenadamente();
            model.Termo = Parametro.Obter().NotaUso;
            return View(model);
        }

        // POST: Certificacao/Confirmar
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { 2/*, 3*/ })]
        public ActionResult Confirmar(FormCollection formCollection)
        {
            AvalCertificacao cert = new AvalCertificacao();
            if (formCollection.HasKeys())
            {
                DateTime hoje = DateTime.Now;

                /* Chave */
                cert.Avaliacao = new Avaliacao();
                cert.Avaliacao.TipoAvaliacao = TipoAvaliacao.ListarPorCodigo(3);
                cert.Avaliacao.Ano = hoje.Year;
                cert.Avaliacao.Semestre = hoje.Month > 6 ? 2 : 1;
                cert.Avaliacao.NumIdentificador = Avaliacao.ObterNumIdentificador(3);
                cert.Avaliacao.DtCadastro = hoje;

                /* Professor */
                string strMatr = Helpers.Sessao.UsuarioMatricula;
                cert.Professor = Professor.ListarPorMatricula(strMatr);

                /* Dados */
                int codDisciplina = int.Parse(formCollection["ddlDisciplina"]);

                cert.CodDisciplina = codDisciplina;

                /* Dificuldade */
                int codDificuldade = int.Parse(formCollection["ddlDificuldade"]);

                /* Quantidade */
                int qteObjetiva = 0;
                int qteDiscursiva = 0;
                if (formCollection["ddlTipo"] == "3")
                {
                    int.TryParse(formCollection["txtQteObjetiva"], out qteObjetiva);
                    int.TryParse(formCollection["txtQteDiscursiva"], out qteDiscursiva);
                }
                else if (formCollection["ddlTipo"] == "2")
                {
                    int.TryParse(formCollection["txtQteDiscursiva"], out qteDiscursiva);
                }
                else if (formCollection["ddlTipo"] == "1")
                {
                    int.TryParse(formCollection["txtQteObjetiva"], out qteObjetiva);
                }

                /* Temas */
                string[] arrTemaCods = formCollection["ddlTemas"].Split(',');

                /* Questões */
                List<QuestaoTema> lstQuestoes = new List<QuestaoTema>();

                if (qteObjetiva > 0)
                {
                    lstQuestoes.AddRange(Questao.ListarPorDisciplina(codDisciplina, arrTemaCods, codDificuldade, 1, qteObjetiva));
                }
                if (qteDiscursiva > 0)
                {
                    lstQuestoes.AddRange(Questao.ListarPorDisciplina(codDisciplina, arrTemaCods, codDificuldade, 2, qteDiscursiva));
                }

                foreach (var strTemaCod in arrTemaCods)
                {
                    AvaliacaoTema avalTema = new AvaliacaoTema();
                    avalTema.Tema = Tema.ListarPorCodigo(codDisciplina, int.Parse(strTemaCod));
                    foreach (var queTma in lstQuestoes.Where(q => q.CodTema == int.Parse(strTemaCod)))
                    {
                        AvalTemaQuestao avalTemaQuestao = new AvalTemaQuestao();
                        avalTemaQuestao.QuestaoTema = queTma;
                        avalTema.AvalTemaQuestao.Add(avalTemaQuestao);
                    }
                    cert.Avaliacao.AvaliacaoTema.Add(avalTema);
                }

                AvalCertificacao.Inserir(cert);
            }

            //return View(cert);
            return RedirectToAction("Configurar", new { codigo = cert.Avaliacao.CodAvaliacao });
        }

        // GET: Certificacao/Configurar/CERT201520001
        [Filters.AutenticacaoFilter(Categorias = new[] { 2/*, 3*/ })]
        public ActionResult Configurar(string codigo)
        {
            if (!String.IsNullOrEmpty(codigo))
            {
                AvalCertificacao cert = AvalCertificacao.ListarPorCodigoAvaliacao(codigo);
                if (cert != null && !cert.Avaliacao.FlagRealizada)
                {
                    Professor prof = Professor.ListarPorMatricula(Helpers.Sessao.UsuarioMatricula);
                    if (prof != null && prof.CodProfessor == cert.Professor.CodProfessor)
                    {
                        var model = new ViewModels.CertificacaoConfigurarViewModel();
                        model.Avaliacao = cert.Avaliacao;
                        model.Dificuldades = Dificuldade.ListarOrdenadamente();
                        model.TiposQuestao = TipoQuestao.ListarOrdenadamente();
                        return View(model);
                    }
                }
            }
            return RedirectToAction("Index");
        }

        //POST: Certificacao/Configurar/CERT201520001
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] {2})]
        public ActionResult Configurar(string codigo,int[] questoes)
        {
            if(!String.IsNullOrEmpty(codigo) && questoes.Length > 0)
            {
                Avaliacao.AtualizarQuestoes(codigo, questoes);
                return Json(true);
            }
            return Json(false);
        }

        // POST: Certificacao/CarregarQuestoes/CERT201520001/{temas}/{dificuldade}/{tipo}
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { 2/*, 3*/ })]
        public ActionResult CarregarQuestoes(string codigo, int[] temas, int dificuldade, int tipo)
        {
            if (!String.IsNullOrEmpty(codigo) && temas.Length > 0 && dificuldade > 0 && tipo > 0 )
            {
                AvalCertificacao cert = AvalCertificacao.ListarPorCodigoAvaliacao(codigo);
                if (cert != null)
                {
                    List<Questao> questoes = Questao.ListarQuestoesFiltradas(cert.CodDisciplina, temas, dificuldade, tipo);
                    if (questoes != null)
                    {
                        return PartialView("_ListaQuestaoCard", questoes);
                    }
                }
            }
            return null;
        }

        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { 2 })]
        public ActionResult CarregarQuestao(int codQuestao)
        {
            if (codQuestao > 0)
            {
                Questao questao = Questao.ListarPorCodigo(codQuestao);

                return PartialView("~/Views/Questao/Partials/_Questao.cshtml", questao); 
            }
            return null;
        }

        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { 2 })]
        public ActionResult CarregarListaQuestaoDetalhe(int[] codQuestoes)
        {
            if (codQuestoes.Length > 0)
            {
                List<Questao> questoes = Questao.ListarPorCodigos(codQuestoes);
                
                if(questoes.Count > 0)
                {
                    return PartialView("_ListaQuestaoDetalhe", questoes);
                }
            }
            return null;
        }

        // GET: Certificacao/Agendar/CERT201520001
        [HttpGet]
        [Filters.AutenticacaoFilter(Categorias = new [] { 2 })]
        public ActionResult Agendar(string codigo)
        {
            if (!String.IsNullOrEmpty(codigo))
            {
                AvalCertificacao cert = AvalCertificacao.ListarPorCodigoAvaliacao(codigo);
                
                if (cert.Professor.MatrProfessor == Helpers.Sessao.UsuarioMatricula)
                {
                    var model = new ViewModels.AvaliacaoAgendarViewModel();

                    model.Avaliacao = cert.Avaliacao;
                    model.Salas = Sala.ListarOrdenadamente();

                    return View(model);
                }
                
            }
            return RedirectToAction("Index");
        }
        
        // POST: Certificacao/Agendar/CERT201520001
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { 2 })]
        public ActionResult Agendar(string codigo, FormCollection form)
        {
            string strCodSala = form["ddlSala"];
            string strData = form["txtData"];
            string strHoraInicio = form["txtHoraInicio"];
            string strHoraTermino = form["txtHoraTermino"];
            if (!StringExt.IsNullOrWhiteSpace(strCodSala, strData, strHoraInicio, strHoraTermino))
            {
                AvalCertificacao aval = AvalCertificacao.ListarPorCodigoAvaliacao(codigo);

                string strMatr = Helpers.Sessao.UsuarioMatricula;
                Professor prof = Professor.ListarPorMatricula(strMatr);

                if (aval.CodProfessor == prof.CodProfessor)
                {
                    // Sala
                    int codSala;
                    int.TryParse(strCodSala, out codSala);
                    Sala sala = Sala.ListarPorCodigo(codSala);
                    if (sala != null)
                    {
                        aval.Sala = sala;
                    }

                    // Data de Aplicacao
                    DateTime dtAplicacao = DateTime.Parse(strData + " " + strHoraInicio);
                    DateTime dtAplicacaoTermino = DateTime.Parse(strData + " " + strHoraTermino);

                    if (dtAplicacao.IsFuture() && dtAplicacaoTermino.IsFuture() && dtAplicacaoTermino > dtAplicacao)
                    {
                        aval.Avaliacao.DtAplicacao = dtAplicacao;
                        aval.Avaliacao.Duracao = Convert.ToInt32((dtAplicacaoTermino - aval.Avaliacao.DtAplicacao.Value).TotalMinutes);
                    }

                    aval.Avaliacao.FlagLiberada = false;

                    Repositorio.GetInstance().SaveChanges();
                }
            }

            return RedirectToAction("Avaliados", new { codigo = codigo }); // Redirecionar para Pessoas
        }

        // GET: Certificacao/Avaliados/CERT201520001
        [HttpGet]
        [Filters.AutenticacaoFilter(Categorias = new[] { 2 })]
        public ActionResult Avaliados(string codigo)
        {
            if (!String.IsNullOrEmpty(codigo))
            {
                AvalCertificacao cert = AvalCertificacao.ListarPorCodigoAvaliacao(codigo);
                if (cert.Professor.MatrProfessor == Sessao.UsuarioMatricula)
                {
                    return View(cert);
                }
            }
            return RedirectToAction("Index");
        }

        // POST: Certificacao/Avaliados/CERT201520001
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { 2 })]
        public ActionResult Avaliados(string codigo, List<Selecao> selecao)
        {
            if (!String.IsNullOrEmpty(codigo))
            {
                AvalCertificacao cert = AvalCertificacao.ListarPorCodigoAvaliacao(codigo);
                if (cert.Professor.MatrProfessor == Sessao.UsuarioMatricula)
                {
                    cert.PessoaFisica.Clear();
                    List<PessoaFisica> lstPessoaFisica = new List<PessoaFisica>();

                    foreach (var item in selecao)
                    {
                        switch (item.category)
                        {
                            case "Pessoa":
                                lstPessoaFisica.Add(PessoaFisica.ListarPorCodigo(int.Parse(item.id)));
                                break;
                            case "Turma":
                                lstPessoaFisica.AddRange(PessoaFisica.ListarPorTurma(item.id));
                                break;
                            case "Curso":
                                lstPessoaFisica.AddRange(PessoaFisica.ListarPorCurso(int.Parse(item.id)));
                                break;
                            case "Diretoria":
                                lstPessoaFisica.AddRange(PessoaFisica.ListarPorDiretoria(item.id));
                                break;
                            case "Campus":
                                lstPessoaFisica.AddRange(PessoaFisica.ListarPorCampus(item.id));
                                break;
                            default:
                                break;
                        }
                    }

                    cert.PessoaFisica = lstPessoaFisica.Distinct().ToList();
                    Repositorio.GetInstance().SaveChanges();
                }
            }
            return Json("/historico/avaliacao/certificacao/detalhe/"+codigo);
        }

        // POST: Certificacao/Avaliados/CERT201520001
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { 2 })]
        public ActionResult Filtrar(int codigo)
        {
            object lstResultado = null;

            switch (codigo)
            {
                case 1:
                    lstResultado = Usuario.Listar().Select(a => new Selecao {
                        id = a.CodPessoaFisica.ToString(),
                        description = a.Matricula,
                        title = a.PessoaFisica.Nome,
                        category = "Pessoa"
                    });
                    break;
                case 2:
                    lstResultado = Turma.ListarOrdenadamente().Select(a => new Selecao {
                        id = a.CodTurma,
                        description = a.CodTurma,
                        title = $"{a.Curso.Descricao} ({a.CodTurma})",
                        category = "Turma"
                    });
                    break;
                case 3:
                    lstResultado = Curso.ListarOrdenadamente().Select(a=>new Selecao {
                        id = a.CodCurso.ToString(),
                        description = a.Sigla,
                        title = a.Descricao,
                        category = "Curso"
                    });
                    break;
                case 4:
                    lstResultado = Diretoria.ListarOrdenadamente().Select(a => new Selecao {
                        id = a.CodComposto,
                        description = $"{a.Campus.PessoaJuridica.NomeFantasia} ({a.Campus.Instituicao.Sigla})",
                        title = a.PessoaJuridica.NomeFantasia,
                        category = "Diretoria"
                    });
                    break;
                case 5:
                    lstResultado = Campus.ListarOrdenadamente().Select(a=>new Selecao {
                        id = a.CodComposto,
                        description = a.Instituicao.PessoaJuridica.NomeFantasia,
                        title = a.PessoaJuridica.NomeFantasia,
                        category = "Campus"
                    });
                    break;
                default:
                    break;
            }
           
            return Json(lstResultado);
        }

        // GET: Certificacao/Agendada/CERT201520001
        [Filters.AutenticacaoFilter(Categorias = new[] { 1, 2 })]
        public ActionResult Agendada(string codigo)
        {
            if (!String.IsNullOrEmpty(codigo))
            {
                var usuario = Usuario.ListarPorMatricula(Sessao.UsuarioMatricula);
                var cert = AvalCertificacao.ListarAgendadaPorUsuario(usuario).FirstOrDefault(a => a.Avaliacao.CodAvaliacao.ToLower() == codigo.ToLower());
                if (cert != null)
                {
                    return View(cert);
                }
            }
            return RedirectToAction("Detalhe", new { codigo = codigo });
        }

        // POST: Certificacao/AlternarLiberar
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { 2 })]
        public ActionResult AlternarLiberar(string codAvaliacao)
        {
            return Json(AvalCertificacao.AlternarLiberar(codAvaliacao));
        }

        // POST: Certificacao/ContagemRegressiva
        [HttpPost]
        public ActionResult ContagemRegressiva(string codAvaliacao)
        {
            AvalCertificacao cert = AvalCertificacao.ListarPorCodigoAvaliacao(codAvaliacao);
            string strTempo = cert.Avaliacao.DtAplicacao.Value.ToLeftTimeString();
            int qteMilissegundo = 0;
            bool flagLiberada = cert.Avaliacao.FlagLiberada && cert.Avaliacao.DtAplicacao.Value.AddMinutes(cert.Avaliacao.Duracao.Value) > DateTime.Now;
            if (strTempo != "Agora")
            {
                char tipo = strTempo[(strTempo.IndexOf(' ')) + 1];
                switch (tipo)
                {
                    case 'd':
                        qteMilissegundo = 0;
                        break;
                    case 'h':
                        qteMilissegundo = 1 * 60 * 60 * 1000;
                        break;
                    case 'm':
                        qteMilissegundo = 1 * 60 * 1000;
                        break;
                    case 's':
                        qteMilissegundo = 1 * 1000;
                        break;
                    default:
                        break;
                }
            }
            return Json(new { Tempo = strTempo, Intervalo = qteMilissegundo, FlagLiberada = flagLiberada });
        }

        // GET: Certificacao/Realizar/
        [Filters.AutenticacaoFilter(Categorias = new[] { 1 })]
        public ActionResult Realizar(string codigo)
        {
            if (!String.IsNullOrEmpty(codigo))
            {
                AvalCertificacao cert = AvalCertificacao.ListarPorCodigoAvaliacao(codigo);
                if (cert.Avaliacao.FlagPendente 
                    && cert.Avaliacao.FlagLiberada 
                    && cert.Avaliacao.FlagAgora  
                    && cert.PessoaFisica.FirstOrDefault(p=>p.CodPessoa == Usuario.ObterPessoaFisica(Sessao.UsuarioMatricula)) != null)
                {
                    Helpers.Sessao.Inserir("RealizandoAvaliacao", true);
                    Helpers.Sessao.Inserir("UsuarioAvaliacao", codigo);
                    return View(cert);
                }
            }
            return RedirectToAction("Agendada", new { codigo = codigo });
        }

        //GET: Certificacao/Acompanhar/CERT201520007
        [Filters.AutenticacaoFilter(Categorias = new[] { 2 })]
        public ActionResult Acompanhar(string codigo)
        {
            if (!String.IsNullOrEmpty(codigo))
            {
                AvalCertificacao cert = AvalCertificacao.ListarPorCodigoAvaliacao(codigo);
                if (cert != null && cert.Professor.MatrProfessor == Sessao.UsuarioMatricula && cert.Avaliacao.FlagAgendada && cert.Avaliacao.FlagAgora)
                {
                    return View(cert);
                }
            }
            return RedirectToAction("Agendada", new {codigo = codigo });
        }

        //POST: Certificacao/Printar
        [HttpPost]
        public ActionResult Printar(string codAvaliacao, string imageData)
        {
            var cert = AvalCertificacao.ListarPorCodigoAvaliacao(codAvaliacao);
            var flagProfessor = cert.Professor.MatrProfessor == Sessao.UsuarioMatricula;
            if (!flagProfessor)
            {
                Sistema.TempDataUrlImage[codAvaliacao] = imageData;
                return Json(true);
            }
            else if (flagProfessor)
            {
                string temp = Sistema.TempDataUrlImage[codAvaliacao];
                Sistema.TempDataUrlImage[codAvaliacao] = String.Empty;
                return Json(temp);
            }
            return Json(false);
        }

        // POST: Certificacao/Desistir/CERT201520016
        [HttpPost]
        public void Desistir(string codigo)
        {
            int codPessoaFisica = Usuario.ObterPessoaFisica(Sessao.UsuarioMatricula);
            if (!String.IsNullOrEmpty(codigo))
            {
                AvalCertificacao aval = AvalCertificacao.ListarPorCodigoAvaliacao(codigo);
                if (aval.PessoaFisica.FirstOrDefault(a => a.CodPessoa == codPessoaFisica) != null 
                    && aval.Avaliacao.AvalPessoaResultado.FirstOrDefault(a => a.CodPessoaFisica == codPessoaFisica) == null)
                {
                    AvalPessoaResultado avalPessoaResultado = new AvalPessoaResultado();
                    avalPessoaResultado.CodPessoaFisica = codPessoaFisica;
                    avalPessoaResultado.HoraTermino = DateTime.Now;
                    avalPessoaResultado.QteAcertoObj = 0;
                    avalPessoaResultado.Nota = 0;

                    foreach (var avaliacaoTema in aval.Avaliacao.AvaliacaoTema)
                    {
                        foreach (var avalTemaQuestao in avaliacaoTema.AvalTemaQuestao)
                        {
                            AvalQuesPessoaResposta avalQuesPessoaResposta = new AvalQuesPessoaResposta();
                            avalQuesPessoaResposta.CodPessoaFisica = codPessoaFisica;
                            if (avalTemaQuestao.QuestaoTema.Questao.CodTipoQuestao == 1) avalQuesPessoaResposta.RespAlternativa = -1;
                            avalQuesPessoaResposta.RespNota = 0;
                            avalTemaQuestao.AvalQuesPessoaResposta.Add(avalQuesPessoaResposta);
                        }
                    }

                    aval.Avaliacao.AvalPessoaResultado.Add(avalPessoaResultado);

                    Repositorio.GetInstance().SaveChanges();
                    Sessao.Inserir("RealizandoAvaliacao", false);
                    Sessao.Inserir("UsuarioAvaliacao", String.Empty);
                }
            }
        }

        // POST: Certificacao/Resultado/CERT201520001
        [HttpPost]
        public ActionResult Resultado(string codigo, FormCollection form)
        {
            int codPessoaFisica = Usuario.ObterPessoaFisica(Helpers.Sessao.UsuarioMatricula);
            if (!String.IsNullOrEmpty(codigo))
            {
                AvalCertificacao aval = AvalCertificacao.ListarPorCodigoAvaliacao(codigo);
                if (aval.PessoaFisica.FirstOrDefault(a => a.CodPessoa == codPessoaFisica) != null
                    && aval.Avaliacao.AvalPessoaResultado.FirstOrDefault(a => a.CodPessoaFisica == codPessoaFisica) == null)
                {
                    AvalPessoaResultado avalPessoaResultado = new AvalPessoaResultado();
                    avalPessoaResultado.CodPessoaFisica = codPessoaFisica;
                    avalPessoaResultado.HoraTermino = DateTime.Now;
                    avalPessoaResultado.QteAcertoObj = 0;

                    double qteObjetiva = 0;

                    foreach (var avaliacaoTema in aval.Avaliacao.AvaliacaoTema)
                    {
                        foreach (var avalTemaQuestao in avaliacaoTema.AvalTemaQuestao)
                        {
                            AvalQuesPessoaResposta avalQuesPessoaResposta = new AvalQuesPessoaResposta();
                            avalQuesPessoaResposta.CodPessoaFisica = codPessoaFisica;
                            if (avalTemaQuestao.QuestaoTema.Questao.CodTipoQuestao == 1)
                            {
                                qteObjetiva++;
                                int respAlternativa = -1;
                                string strRespAlternativa = form["rdoResposta" + avalTemaQuestao.QuestaoTema.Questao.CodQuestao];
                                if (!String.IsNullOrEmpty(strRespAlternativa))
                                {
                                    int.TryParse(strRespAlternativa, out respAlternativa);
                                }
                                avalQuesPessoaResposta.RespAlternativa = respAlternativa;
                                if (avalTemaQuestao.QuestaoTema.Questao.Alternativa.First(q => q.FlagGabarito.HasValue && q.FlagGabarito.Value).CodOrdem == avalQuesPessoaResposta.RespAlternativa)
                                {
                                    avalQuesPessoaResposta.RespNota = 10;
                                    avalPessoaResultado.QteAcertoObj++;
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
                            avalQuesPessoaResposta.RespComentario = !String.IsNullOrEmpty(form["txtComentario" + avalTemaQuestao.QuestaoTema.Questao.CodQuestao]) ? form["txtComentario" + avalTemaQuestao.QuestaoTema.Questao.CodQuestao].Trim() : null;
                            avalTemaQuestao.AvalQuesPessoaResposta.Add(avalQuesPessoaResposta);
                        }

                    }

                    var lstAvalQuesPessoaResposta = aval.Avaliacao.PessoaResposta.Where(r => r.CodPessoaFisica == codPessoaFisica);

                    avalPessoaResultado.Nota = lstAvalQuesPessoaResposta.Average(r => r.RespNota);
                    aval.Avaliacao.AvalPessoaResultado.Add(avalPessoaResultado);

                    Repositorio.GetInstance().SaveChanges();

                    var model = new ViewModels.AvaliacaoResultadoViewModel();
                    model.Avaliacao = aval.Avaliacao;
                    model.Porcentagem = (avalPessoaResultado.QteAcertoObj.Value / qteObjetiva) * 100;

                    Helpers.Sessao.Inserir("RealizandoAvaliacao", false);
                    Helpers.Sessao.Inserir("UsuarioAvaliacao", String.Empty);

                    return View(model);
                }
                return RedirectToAction("Detalhe", new { codigo = aval.Avaliacao.CodAvaliacao });
            }
            return RedirectToAction("Index");
        }
    }
}