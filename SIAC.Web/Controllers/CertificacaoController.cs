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
            ViewBag.Disciplinas = /*Helpers.Sessao.UsuarioCategoriaCodigo == 2 ? */Disciplina.ListarPorProfessor(Helpers.Sessao.UsuarioMatricula)/*: Disciplina.ListarOrdenadamente()*/;
            ViewBag.Dificuldades = Dificuldade.ListarOrdenadamente();
            ViewBag.Termo = Parametro.Obter().NotaUso;
            return View();
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

                ViewBag.QteQuestoes = lstQuestoes.Count;
                ViewBag.QuestoesDaAvaliacao = lstQuestoes;

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
                        ViewBag.Dificuldades = Dificuldade.ListarOrdenadamente();
                        ViewBag.TiposQuestao = TipoQuestao.ListarOrdenadamente();
                        return View(cert);
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
            if(!String.IsNullOrEmpty(codigo) && questoes.Count() > 0)
            {
                Avaliacao.AtualizarQuestoes(codigo, questoes);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        // POST: Certificacao/CarregarQuestoes/CERT201520001/{temas}/{dificuldade}/{tipo}
        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [Filters.AutenticacaoFilter(Categorias = new[] { 2/*, 3*/ })]
        public ActionResult CarregarQuestoes(string codigo, int[] temas, int dificuldade, int tipo)
        {
            if (!String.IsNullOrEmpty(codigo) && temas.Count() > 0 && dificuldade > 0 && tipo > 0 )
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
        [AcceptVerbs(HttpVerbs.Post)]
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

            return RedirectToAction("Pessoas", new { codigo = codigo }); // Redirecionar para Pessoas
        }

        [HttpGet]
        [Filters.AutenticacaoFilter(Categorias = new[] { 2 })]
        public ActionResult Pessoas(string codigo)
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

        [AcceptVerbs(HttpVerbs.Post)]
        [Filters.AutenticacaoFilter(Categorias = new[] { 2 })]
        public ActionResult Filtrar(int filtro)
        {
            object lstResultado = null;

            switch (filtro)
            {
                case 1:
                    lstResultado = Usuario.Listar().Select(a => new {
                        cod = a.CodPessoaFisica,
                        description = a.Matricula,
                        title = a.PessoaFisica.Nome,
                        category = "Pessoa"
                    });
                    break;
                case 2:
                    lstResultado = Turma.ListarOrdenadamente().Select(a => new { 
                        cod = a.CodTurma,
                        description = a.CodTurma,
                        title = $"{a.Curso.Descricao} ({a.CodTurma})",
                        category = "Turma"
                    });
                    break;
                case 3:
                    lstResultado = Curso.ListarOrdenadamente().Select(a=>new {
                        cod = a.CodCurso,
                        description = a.Sigla,
                        title = a.Descricao,
                        category = "Curso"
                    });
                    break;
                case 4:
                    lstResultado = Diretoria.ListarOrdenadamente().Select(a => new {
                        cod = a.CodComposto,
                        description = $"{a.Campus.PessoaJuridica.NomeFantasia} ({a.Campus.Instituicao.Sigla})",
                        title = a.PessoaJuridica.NomeFantasia,
                        category = "Diretoria"
                    });
                    break;
                case 5:
                    lstResultado = Campus.ListarOrdenadamente().Select(a=>new {
                        cod =a.CodComposto,
                        description = a.Instituicao.PessoaJuridica.NomeFantasia,
                        title = a.PessoaJuridica.NomeFantasia,
                        category = "Campus"
                    });
                    break;
                case 6:
                    var lst = Reitoria.ListarOrdenadamente().Select(a => new {
                        cod = a.CodComposto,
                        description = a.Instituicao.PessoaJuridica.NomeFantasia,
                        title = a.PessoaJuridica.NomeFantasia,
                        category = "Reitoria"
                    });
                    lstResultado = lst.Union(ProReitoria.ListarOrdenadamente().Select(a => new {
                        cod = a.CodComposto,
                        description = a.Instituicao.PessoaJuridica.NomeFantasia,
                        title = a.PessoaJuridica.NomeFantasia,
                        category = "Pró-Reitoria"
                    }));                    
                    break;
                case 7:
                    lstResultado = Instituicao.ListarOrdenadamente().Select(a => new {
                        cod = a.CodInstituicao,
                        description = a.Sigla,
                        title = a.PessoaJuridica.NomeFantasia,
                        category = "Instituição"
                    });
                    break;
                default:
                    break;
            }

            return Json(lstResultado, JsonRequestBehavior.AllowGet);
        }
    }
}