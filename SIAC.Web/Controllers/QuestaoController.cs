using SIAC.Helpers;
using SIAC.Models;
using SIAC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SIAC.Controllers
{
    [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.PROFESSOR })]
    public class QuestaoController : Controller
    {
        public List<Questao> Questoes => Questao.ListarPorProfessor(Sessao.UsuarioMatricula);

        // GET: historico/questao
        public ActionResult Index()
        {
            if (Request.Url.ToString().ToLower().Contains("principal"))
                return Redirect("~/historico/questao");
            QuestaoIndexViewModel model = new QuestaoIndexViewModel();
            List<Questao> questoes = Questoes;
            model.Disciplinas = questoes.Select(q => q.Disciplina).Distinct().ToList();
            model.Dificuldades = questoes.Select(q => q.Dificuldade).Distinct().ToList();
            return View(model);
        }

        // POST: historico/questao/listar
        [HttpPost]
        public ActionResult Listar(int? pagina, string pesquisa, string ordenar, string tipo, string disciplina, string tema, string dificuldade)
        {
            int quantidade = 10;
            List<Questao> questoes = Questoes;
            pagina = pagina ?? 1;

            if (!String.IsNullOrWhiteSpace(pesquisa))
            {
                questoes = questoes.Where(q => q.Enunciado.ToLower().Contains(pesquisa.Trim().ToLower())).ToList();
            }

            if (!String.IsNullOrWhiteSpace(disciplina))
            {
                questoes = questoes.Where(q => q.Disciplina.CodDisciplina == int.Parse(disciplina)).ToList();
            }

            if (!String.IsNullOrWhiteSpace(tema))
            {
                questoes = questoes.Where(q => q.QuestaoTema.Where(t => t.CodTema == int.Parse(tema)).Count() > 0).ToList();
            }

            if (!String.IsNullOrWhiteSpace(dificuldade))
            {
                questoes = questoes.Where(q => q.CodDificuldade == int.Parse(dificuldade)).ToList();
            }

            switch (tipo)
            {
                case "objetiva":
                    questoes = questoes.Where(q => q.CodTipoQuestao == TipoQuestao.OBJETIVA).ToList();
                    break;

                case "discursiva":
                    questoes = questoes.Where(q => q.CodTipoQuestao == TipoQuestao.DISCURSIVA).ToList();
                    break;
            }

            switch (ordenar)
            {
                case "data_desc":
                    questoes = questoes.OrderByDescending(q => q.DtCadastro).ToList();
                    break;

                case "data":
                    questoes = questoes.OrderBy(q => q.DtCadastro).ToList();
                    break;

                default:
                    questoes = questoes.OrderByDescending(q => q.DtCadastro).ToList();
                    break;
            }
            return PartialView("_ListaQuestao", questoes.Skip((quantidade * pagina.Value) - quantidade).Take(quantidade).ToList());
        }

        //POST: principal/questao/palavraschave
        [HttpPost]
        public ActionResult PalavrasChave(string[] palavras)
        {
            if (palavras.Length == 0)
                return Json(new List<Questao>());
            List<Questao> resultado = Questao.ListarPorPalavraChave(palavras);
            var retorno = resultado.Select(q => new
            {
                CodQuestao = q.CodQuestao,
                Dificuldade = q.Dificuldade.Descricao,
                Disciplina = q.QuestaoTema.First().Tema.Disciplina.Descricao,
                Enunciado = q.Enunciado,
                TipoQuestao = q.TipoQuestao.Descricao,
                Professor = q.Professor.Usuario.PessoaFisica.Nome,
                DtCadastro = q.DtCadastro.ToBrazilianString(),
                FlagProprietario = q.Professor.MatrProfessor == Sessao.UsuarioMatricula
            });
            return Json(retorno);
        }

        // GET: principal/questao/cadastrar
        public ActionResult Cadastrar()
        {
            QuestaoCadastrarViewModel model = new QuestaoCadastrarViewModel();
            model.Captcha = Captcha.Novo();
            model.Termo = Parametro.Obter().TermoResponsabilidade;
            model.Disciplinas = Professor.ObterDisciplinas(Sessao.UsuarioMatricula);
            model.Tipos = TipoQuestao.ListarOrdenadamente();
            model.Dificuldades = Dificuldade.ListarOrdenadamente();
            model.TiposAnexo = TipoAnexo.ListarOrdenadamente();
            return View(model);
        }

        // POST: principal/questao/chequecaptcha
        [HttpPost]
        public ActionResult ChequeCaptcha(string captcha) => Json(captcha == (string)Sessao.Retornar("Captcha"));

        // POST: principal/questao/novocaptcha
        [HttpPost]
        public string NovoCaptcha()
        {
            return Captcha.Novo();
        }

        // POST: principal/questao/confirmar
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Confirmar(FormCollection formCollection)
        {
            if (!formCollection.HasKeys())
                return RedirectToAction("Index");

            Questao questao = new Questao();
            questao.Professor = Professor.ListarPorMatricula(Sessao.UsuarioMatricula);
            questao.CodProfessor = questao.Professor.CodProfessor;

            // Gerais
            questao.CodDificuldade = int.Parse(formCollection["ddlDificuldade"]);
            questao.Dificuldade = Dificuldade.ListarPorCodigo(questao.CodDificuldade);
            questao.CodTipoQuestao = int.Parse(formCollection["ddlTipo"]);
            questao.TipoQuestao = TipoQuestao.ListarPorCodigo(questao.CodTipoQuestao);

            int codDisciplina = int.Parse(formCollection["ddlDisciplina"]);
            string[] codTemas = formCollection["ddlTema"].Split(',');
            foreach (string strCodTema in codTemas)
            {
                int codTema = int.Parse(strCodTema);
                questao.QuestaoTema.Add(new QuestaoTema
                {
                    CodDisciplina = codDisciplina,
                    CodTema = codTema,
                    Tema = Tema.ListarPorCodigo(codDisciplina, codTema)
                });
            }

            // Detalhes
            questao.Enunciado = formCollection["txtEnunciado"].Trim();
            questao.Objetivo = !String.IsNullOrWhiteSpace(formCollection["txtObjetivo"]) ? formCollection["txtObjetivo"].RemoveSpaces() : null;

            // Discursiva
            if (questao.CodTipoQuestao == TipoQuestao.DISCURSIVA)
            {
                questao.ChaveDeResposta = formCollection["txtChaveDeResposta"].Trim();
                questao.Comentario = !String.IsNullOrWhiteSpace(formCollection["txtComentario"]) ? formCollection["txtComentario"].RemoveSpaces() : null;
            }

            // Objetiva
            if (questao.CodTipoQuestao == TipoQuestao.OBJETIVA)
            {
                int qteAlternativas = int.Parse(formCollection["txtQtdAlternativas"]);
                for (int i = 0; i < qteAlternativas; i++)
                {
                    questao.Alternativa.Add(new Alternativa
                    {
                        CodOrdem = i,
                        Enunciado = formCollection["txtAlternativaEnunciado" + (i + 1)].RemoveSpaces(),
                        Comentario = !String.IsNullOrWhiteSpace(formCollection["txtAlternativaComentario" + (i + 1)]) ? formCollection["txtAlternativaComentario" + (i + 1)].RemoveSpaces() : null,
                        FlagGabarito = !String.IsNullOrWhiteSpace(formCollection["chkAlternativaCorreta" + (i + 1)]) ? true : false
                    });
                }
            }

            // Anexos
            if (!String.IsNullOrWhiteSpace(formCollection["chkAnexos"]) && !String.IsNullOrWhiteSpace(formCollection["txtQtdAnexos"]))
            {
                int iAnexoImage = 0;
                int qteAnexos = int.Parse(formCollection["txtQtdAnexos"]);
                for (int i = 0; i < qteAnexos; i++)
                {
                    int tipoAnexo = int.Parse(formCollection["txtAnexoTipo" + (i + 1)]);
                    switch (tipoAnexo)
                    {
                        case TipoAnexo.IMAGEM:
                            questao.QuestaoAnexo.Add(new QuestaoAnexo
                            {
                                CodOrdem = i,
                                CodTipoAnexo = tipoAnexo,
                                Legenda = formCollection["txtAnexoLegenda" + (i + 1)].RemoveSpaces(),
                                Fonte = !String.IsNullOrWhiteSpace(formCollection["txtAnexoFonte" + (i + 1)]) ? formCollection["txtAnexoFonte" + (i + 1)].RemoveSpaces() : null,
                                Anexo = new System.IO.BinaryReader(Request.Files[iAnexoImage].InputStream).ReadBytes(Request.Files[iAnexoImage].ContentLength)
                            });
                            iAnexoImage++;
                            break;

                        case TipoAnexo.CODIGO:
                            questao.QuestaoAnexo.Add(new QuestaoAnexo
                            {
                                CodOrdem = i,
                                CodTipoAnexo = tipoAnexo,
                                Legenda = !String.IsNullOrWhiteSpace(formCollection["txtAnexoLegenda" + (i + 1)]) ? formCollection["txtAnexoLegenda" + (i + 1)].RemoveSpaces() : null,
                                Fonte = !String.IsNullOrWhiteSpace(formCollection["txtAnexoFonte" + (i + 1)]) ? formCollection["txtAnexoFonte" + (i + 1)].RemoveSpaces() : null,
                                Anexo = formCollection["txtAnexo" + (i + 1)].ToString().GetBytes()
                            });
                            break;

                        default:
                            break;
                    }
                }
            }

            Questao.Inserir(questao);
            Lembrete.AdicionarNotificacao($"Questão {questao.CodQuestao} cadastrada com sucesso.", Lembrete.POSITIVO);
            return RedirectToAction("Detalhe", new { codigo = questao.CodQuestao });
        }

        //GET: historico/questao/editar/5
        [HttpGet]
        public ActionResult Editar(string codigo)
        {
            int codQuestao = 0;
            int.TryParse(codigo, out codQuestao);
            Questao questao = null;
            if (codQuestao > 0)
                questao = Questao.ListarPorCodigo(codQuestao);
            if (questao == null)
                return RedirectToAction("index");
            Lembrete.AdicionarNotificacao("Observe que há alguns dados que não podem ser editados.", Lembrete.INFO);
            return View(questao);
        }

        //POST: historico/questao/editar/5
        [HttpPost]
        public ActionResult Editar(string codigo, FormCollection formCollection)
        {
            if (!formCollection.HasKeys() || String.IsNullOrWhiteSpace(codigo))
                return RedirectToAction("Index");

            int codQuestao = 0;
            int.TryParse(codigo, out codQuestao);
            Questao questao = null;
            if (codQuestao > 0)
                questao = Questao.ListarPorCodigo(codQuestao);

            questao.Enunciado = !String.IsNullOrWhiteSpace(formCollection["txtEnunciado"]) ? formCollection["txtEnunciado"].Trim() : questao.Enunciado;
            questao.Objetivo = !String.IsNullOrWhiteSpace(formCollection["txtObjetivo"]) ? formCollection["txtObjetivo"].RemoveSpaces() : questao.Objetivo;

            if (questao.CodTipoQuestao == 2)
            {
                questao.ChaveDeResposta = !String.IsNullOrWhiteSpace(formCollection["txtChaveDeResposta"]) ? formCollection["txtChaveDeResposta"].Trim() : questao.ChaveDeResposta;
                questao.Comentario = !String.IsNullOrWhiteSpace(formCollection["txtComentario"]) ? formCollection["txtComentario"].RemoveSpaces() : questao.Comentario;
            }

            if (questao.CodTipoQuestao == 1)
            {
                for (int i = 0; i < questao.Alternativa.Count; i++)
                {
                    questao.Alternativa.ElementAt(i).Enunciado = !String.IsNullOrWhiteSpace(formCollection["txtAlternativaEnunciado" + (i + 1)]) ? formCollection["txtAlternativaEnunciado" + (i + 1)].RemoveSpaces() : questao.Alternativa.ElementAt(i).Enunciado;
                    questao.Alternativa.ElementAt(i).Comentario = !String.IsNullOrWhiteSpace(formCollection["txtAlternativaComentario" + (i + 1)]) ? formCollection["txtAlternativaComentario" + (i + 1)].RemoveSpaces() : questao.Alternativa.ElementAt(i).Comentario;
                }
            }

            if (questao.QuestaoAnexo.Count > 0)
            {
                for (int i = 0; i < questao.QuestaoAnexo.Count; i++)
                {
                    questao.QuestaoAnexo.ElementAt(i).Legenda = !String.IsNullOrWhiteSpace(formCollection["txtAnexoLegenda" + (i + 1)]) ? formCollection["txtAnexoLegenda" + (i + 1)].RemoveSpaces() : questao.QuestaoAnexo.ElementAt(i).Legenda;
                    questao.QuestaoAnexo.ElementAt(i).Fonte = !String.IsNullOrWhiteSpace(formCollection["txtAnexoFonte" + (i + 1)]) ? formCollection["txtAnexoFonte" + (i + 1)].RemoveSpaces() : String.Empty;
                }
            }

            Questao.Atualizar(questao);
            Lembrete.AdicionarNotificacao($"Questão {questao.CodQuestao} editada com sucesso.", Lembrete.POSITIVO);
            return RedirectToAction("Detalhe", new { codigo = questao.CodQuestao });
        }

        //GET: historico/questao/detalhe/5
        public ActionResult Detalhe(string codigo)
        {
            int codQuestao = 0;
            int.TryParse(codigo, out codQuestao);
            Questao model = null;
            if (codQuestao > 0)
                model = Questao.ListarPorCodigo(codQuestao);
            if (model == null)
                return RedirectToAction("Index");
            return View(model);
        }

        //POST: principal/questao/arquivar/5
        [HttpPost]
        public ActionResult Arquivar(string codigo)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                int codQuestao = 0;
                int.TryParse(codigo, out codQuestao);
                if (codQuestao > 0)
                {
                    return Json(Questao.AlternarFlagArquivo(codQuestao));
                }
            }
            return Json(false);
        }

        // POST: principal/questao/apresentar/5
        [HttpPost]
        public ActionResult Apresentar(string codigo)
        {
            int codQuestao = 0;
            int.TryParse(codigo, out codQuestao);
            Questao model = null;
            if (codQuestao > 0)
                model = Questao.ListarPorCodigo(codQuestao);
            if (model != null)
                return PartialView("_Questao", model);
            return null;
        }

        #region Desenvolvedor

        [HttpGet]
        public ActionResult Gerar(string strQte)
        {
            if (!String.IsNullOrWhiteSpace(strQte))
            {
                int qte = int.Parse(strQte);
                List<Questao> lstQuestao = Helpers.DevGerarQuestao.GerarQuestao(qte);
                TempData["lstQuestao"] = lstQuestao;
                return Json(lstQuestao
                    .Select(q => new
                    {
                        CodQuestao = q.CodQuestao,
                        Professor = q.Professor.Usuario.PessoaFisica.Nome,
                        Disciplina = q.QuestaoTema.First().Tema.Disciplina.Descricao,
                        Dificuldade = new { q.Dificuldade.Descricao, q.Dificuldade.Comentario },
                        Tema = q.QuestaoTema.Select(qt => new { qt.Tema.Descricao, qt.Tema.Comentario }),
                        Enunciado = q.Enunciado,
                        Objetivo = q.Objetivo,
                        TipoQuestao = q.TipoQuestao.CodTipoQuestao,
                        Alternativa = q.Alternativa.Select(a => new { a.Enunciado, a.Comentario, a.FlagGabarito }),
                        ChaveDeResposta = q.ChaveDeResposta,
                        Comentario = q.Comentario
                    }), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult Gerar()
        {
            if (TempData.ContainsKey("lstQuestao"))
            {
                List<Questao> lstQuestao = (List<Questao>)TempData["lstQuestao"];
                Repositorio.GetInstance().Questao.AddRange(lstQuestao);
                Repositorio.GetInstance().SaveChanges();
            }
            return RedirectToAction("Index");
        }

        #endregion Desenvolvedor
    }
}