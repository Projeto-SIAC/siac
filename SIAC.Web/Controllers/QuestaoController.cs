using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIAC.Models;

namespace SIAC.Controllers
{
    [Filters.AutenticacaoFilter(Categorias = new[] { 2 })]
    public class QuestaoController : Controller
    {
        public List<Questao> Questoes {
            get
            {
                return Questao.ListarPorProfessor(Helpers.Sessao.UsuarioMatricula);
            }
        }         

        // GET: Questao
        public ActionResult Index()
        {
            if (Request.Url.ToString().ToLower().Contains("principal"))
            {
                return Redirect("~/Historico/Questao");
            }
            var model = new ViewModels.QuestaoIndexViewModel();
            model.Disciplinas = Questoes.Select(q => q.Disciplina).Distinct().ToList();
            model.Dificuldades = Questoes.Select(q => q.Dificuldade).Distinct().ToList();
            return View(model);
        }

        // POST: Questao/Listar
        [HttpPost]
        public ActionResult Listar(int? pagina, string pesquisa, string ordenar, string[] tipos, string disciplina, string tema, string dificuldade)
        {
            var qte = 10;
            var questoes = Questoes;
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
                questoes = questoes.Where(q => q.QuestaoTema.Where(t=>t.CodTema == int.Parse(tema)).Count() > 0).ToList();
            }

            if (!String.IsNullOrWhiteSpace(dificuldade))
            {
                questoes = questoes.Where(q => q.CodDificuldade == int.Parse(dificuldade)).ToList();
            }

            if (tipos != null)
            {
                if (tipos.Contains("objetiva") && !tipos.Contains("discursiva"))
                {
                    questoes = questoes.Where(q => q.CodTipoQuestao == 1).ToList();
                }
                else if (!tipos.Contains("objetiva") && tipos.Contains("discursiva"))
                {
                    questoes = questoes.Where(q => q.CodTipoQuestao == 2).ToList();
                }
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
            return PartialView("_ListaQuestao", questoes.Skip((qte*pagina.Value)-qte).Take(qte).ToList());
        }

        //POST: Questao/PalavrasChave
        [HttpPost]
        public ActionResult PalavrasChave(string[] palavras)
        {
            var resultado = Questao.ListarPorPalavraChave(palavras);
            var result = (from q in resultado select new
            {
                CodQuestao = q.CodQuestao,
                Dificuldade = q.Dificuldade.Descricao,
                Disciplina = q.QuestaoTema.First().Tema.Disciplina.Descricao,
                Enunciado = q.Enunciado,
                TipoQuestao = q.TipoQuestao.Descricao,
                Professor = q.Professor.Usuario.PessoaFisica.Nome,
                DtCadastro = q.DtCadastro.ToBrazilianString(),
                FlagProprietario = q.Professor.MatrProfessor == Helpers.Sessao.UsuarioMatricula
            });
            return Json(result);
        }

        // GET: Questao/Cadastrar
        public ActionResult Cadastrar()
        {
            var model = new ViewModels.QuestaoCadastrarViewModel();
            model.Captcha = Helpers.Captcha.Novo();
            model.Termo = Parametro.Obter().TermoResponsabilidade;
            model.Disciplinas = Professor.ObterDisciplinas(Helpers.Sessao.UsuarioMatricula);
            model.Tipos = TipoQuestao.ListarOrdenadamente();
            model.Dificuldades = Dificuldade.ListarOrdenadamente();
            model.TiposAnexo = TipoAnexo.ListarOrdenadamente();
            return View(model);
        }

        // GET: Questao/Captcha
        [HttpPost]
        public string ChequeCaptcha(string captcha)
        {
            if (captcha == Helpers.Sessao.Retornar("Captcha") as string)
            {
                return "true";
            }
            return "false";
        }

        // GET: Questao/Captcha
        [HttpPost]
        public string NovoCaptcha()
        {
            return Helpers.Captcha.Novo();
        }

        // POST: Questao/Confirmar
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Confirmar(FormCollection formCollection)
        {
            Questao questao = new Questao();

            questao.Professor = Professor.ListarPorMatricula(Helpers.Sessao.UsuarioMatricula);

            questao.CodProfessor = questao.Professor.CodProfessor;

            // Gerais
            questao.CodDificuldade = int.Parse(formCollection["ddlDificuldade"]);
            questao.Dificuldade = Dificuldade.ListarPorCodigo(questao.CodDificuldade);
            questao.CodTipoQuestao = int.Parse(formCollection["ddlTipo"]);
            questao.TipoQuestao = TipoQuestao.ListarPorCodigo(questao.CodTipoQuestao);

            var codDisciplina = int.Parse(formCollection["ddlDisciplina"]);
            var codTemas = formCollection["ddlTema"].Split(',');
            foreach (var strCod in codTemas)
            {
                var codTema = int.Parse(strCod);
                questao.QuestaoTema.Add(new QuestaoTema
                {
                    CodDisciplina = codDisciplina,
                    CodTema = codTema,
                    Tema = Tema.ListarPorCodigo(codDisciplina, codTema)
                });
            }

            // Detalhes
            questao.Enunciado = formCollection["txtEnunciado"].Trim();
            questao.Objetivo = !String.IsNullOrEmpty(formCollection["txtObjetivo"]) ? formCollection["txtObjetivo"].RemoveSpaces() : null;

            // Discursiva
            if (questao.CodTipoQuestao == 2)
            {
                questao.ChaveDeResposta = formCollection["txtChaveDeResposta"].Trim();
                questao.Comentario = !String.IsNullOrEmpty(formCollection["txtComentario"]) ? formCollection["txtComentario"].RemoveSpaces() : null;
            }

            // Objetiva
            if (questao.CodTipoQuestao == 1)
            {
                var qteAlternativas = int.Parse(formCollection["txtQtdAlternativas"]);
                for (int i = 0; i < qteAlternativas; i++)
                {
                    questao.Alternativa.Add(new Alternativa
                    {
                        CodOrdem = i,
                        Enunciado = formCollection["txtAlternativaEnunciado" + (i + 1)].RemoveSpaces(),
                        Comentario = !String.IsNullOrEmpty(formCollection["txtAlternativaComentario" + (i + 1)]) ? formCollection["txtAlternativaComentario" + (i + 1)].RemoveSpaces() : null,
                        FlagGabarito = !String.IsNullOrEmpty(formCollection["chkAlternativaCorreta" + (i + 1)]) ? true : false
                    });
                }
            }

            // Anexos
            if (!String.IsNullOrEmpty(formCollection["chkAnexos"]) && !String.IsNullOrEmpty(formCollection["txtQtdAnexos"]))
            {
                var qteAnexos = int.Parse(formCollection["txtQtdAnexos"]);
                for (int i = 0; i < qteAnexos; i++)
                {
                    var tipoAnexo = int.Parse(formCollection["txtAnexoTipo" + (i + 1)]);
                    switch (tipoAnexo)
                    {
                        case 1:
                            questao.QuestaoAnexo.Add(new QuestaoAnexo
                            {
                                CodOrdem = i,
                                CodTipoAnexo = tipoAnexo,
                                Legenda = formCollection["txtAnexoLegenda" + (i + 1)].RemoveSpaces(),
                                Fonte = !String.IsNullOrEmpty(formCollection["txtAnexoFonte" + (i + 1)]) ? formCollection["txtAnexoFonte" + (i + 1)].RemoveSpaces() : null,
                                Anexo = new System.IO.BinaryReader(Request.Files[i].InputStream).ReadBytes(Request.Files[i].ContentLength)
                            });
                            break;
                        case 2:
                            questao.QuestaoAnexo.Add(new QuestaoAnexo
                            {
                                CodOrdem = i,
                                CodTipoAnexo = tipoAnexo,
                                Legenda = !String.IsNullOrWhiteSpace(formCollection["txtAnexoLegenda" + (i + 1)]) ? formCollection["txtAnexoLegenda" + (i + 1)].RemoveSpaces() : null,
                                Fonte = !String.IsNullOrEmpty(formCollection["txtAnexoFonte" + (i + 1)]) ? formCollection["txtAnexoFonte" + (i + 1)].RemoveSpaces() : null,
                                Anexo = formCollection["txtAnexo" + (i + 1)].ToString().GetBytes()
                            });
                            break;
                        default:
                            break;
                    }
                }
            }

            //TempData["Questao"] = questao;

            Questao.Inserir(questao);

            return RedirectToAction("Detalhe", new { codigo = questao.CodQuestao });
        }

        //GET: Principal/Questão/Editar/5
        [HttpGet]
        public ActionResult Editar(string codigo)
        {
            int codQuestao = 0;
            int.TryParse(codigo, out codQuestao);
            Questao questao = null;
            if (codQuestao > 0)
            {
                questao = Questao.ListarPorCodigo(codQuestao);
            }
            if (questao == null)
            {
                return RedirectToAction("index");
            }
            return View(questao);
        }

        //POST: Principal/Questão/Editar/5
        [HttpPost]
        public ActionResult Editar(string codigo, FormCollection formCollection)
        {
            int codQuestao = 0;
            int.TryParse(codigo, out codQuestao);
            Questao questao = null;
            if (codQuestao > 0)
            {
                questao = Questao.ListarPorCodigo(codQuestao);
            }

            questao.Enunciado = !String.IsNullOrEmpty(formCollection["txtEnunciado"]) ? formCollection["txtEnunciado"].Trim() : questao.Enunciado;
            questao.Objetivo = !String.IsNullOrEmpty(formCollection["txtObjetivo"]) ? formCollection["txtObjetivo"].RemoveSpaces() : questao.Objetivo;

            if (questao.CodTipoQuestao == 2)
            {
                questao.ChaveDeResposta = !String.IsNullOrEmpty(formCollection["txtChaveDeResposta"])? formCollection["txtChaveDeResposta"].Trim() : questao.ChaveDeResposta;
                questao.Comentario = !String.IsNullOrEmpty(formCollection["txtComentario"]) ? formCollection["txtComentario"].RemoveSpaces() : questao.Comentario;
            }

            if (questao.CodTipoQuestao == 1)
            {
                for (int i = 0; i < questao.Alternativa.Count; i++)
                {
                    questao.Alternativa.ElementAt(i).Enunciado = !String.IsNullOrEmpty(formCollection["txtAlternativaEnunciado" + (i + 1)]) ? formCollection["txtAlternativaEnunciado" + (i + 1)].RemoveSpaces() : questao.Alternativa.ElementAt(i).Enunciado;
                    questao.Alternativa.ElementAt(i).Comentario = !String.IsNullOrEmpty(formCollection["txtAlternativaComentario" + (i + 1)]) ? formCollection["txtAlternativaComentario" + (i + 1)].RemoveSpaces() : questao.Alternativa.ElementAt(i).Comentario;
                }
            }

            if (questao.QuestaoAnexo.Count > 0)
            {
                for (int i = 0; i < questao.QuestaoAnexo.Count; i++)
                {
                    questao.QuestaoAnexo.ElementAt(i).Legenda = !String.IsNullOrEmpty(formCollection["txtAnexoLegenda" + (i + 1)]) ? formCollection["txtAnexoLegenda" + (i + 1)].RemoveSpaces() : questao.QuestaoAnexo.ElementAt(i).Legenda;
                    questao.QuestaoAnexo.ElementAt(i).Fonte = !String.IsNullOrEmpty(formCollection["txtAnexoFonte" + (i + 1)]) ? formCollection["txtAnexoFonte" + (i + 1)].RemoveSpaces() : questao.QuestaoAnexo.ElementAt(i).Fonte;
                }
            }

            Questao.Atualizar(questao);

            return RedirectToAction("Detalhe", new { codigo = questao.CodQuestao });
        }

        //GET: Principal/Questao/Detalhe/4
        public ActionResult Detalhe(string codigo)
        {
            int codQuestao = 0;
            int.TryParse(codigo, out codQuestao);
            Questao model = null;
            if (codQuestao > 0)
            {
                model = Questao.ListarPorCodigo(codQuestao);
            }
            if (model != null)
            {
                return View(model);
            }
            return RedirectToAction("Index");
        }

        //POST: Historico/Questao/Arquivar/50
        [HttpPost]
        public ActionResult Arquivar(string codigo)
        {
            if (!String.IsNullOrEmpty(codigo))
            {
                int codQuestao = 0;
                int.TryParse(codigo, out codQuestao);
                if (codQuestao>0)
                {
                    return Json(Questao.AlternarFlagArquivo(codQuestao));
                }
            }
            return Json(false);
        }

        //GET: Principal/Questao/Gerar/50
        [HttpGet]
        public ActionResult Gerar(string strQte)
        {
            if (!String.IsNullOrEmpty(strQte))
            {
                int qte = int.Parse(strQte);
                List<Questao> lstQuestao = Helpers.DevGerarQuestao.GerarQuestao(qte);
                TempData["lstQuestao"] = lstQuestao;
                return Json(lstQuestao
                    .Select(q=> new
                    {
                        CodQuestao = q.CodQuestao,
                        Professor = q.Professor.Usuario.PessoaFisica.Nome,
                        Disciplina = q.QuestaoTema.First().Tema.Disciplina.Descricao,
                        Dificuldade = new { q.Dificuldade.Descricao, q.Dificuldade.Comentario },
                        Tema = q.QuestaoTema.Select(qt=> new { qt.Tema.Descricao, qt.Tema.Comentario }),
                        Enunciado = q.Enunciado,
                        Objetivo = q.Objetivo,
                        TipoQuestao = q.TipoQuestao.CodTipoQuestao,
                        Alternativa = q.Alternativa.Select(a=>new { a.Enunciado, a.Comentario, a.FlagGabarito }),
                        ChaveDeResposta = q.ChaveDeResposta,
                        Comentario = q.Comentario
                    }), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return View();
            }
        }

        //POST: Principal/Questao/Gerar
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

        // POST: Questao/Apresentar
        [HttpPost]
        public ActionResult Apresentar(string codigo)
        {
            int codQuestao = 0;
            int.TryParse(codigo, out codQuestao);
            Questao model = null;
            if (codQuestao > 0)
            {
                model = Questao.ListarPorCodigo(codQuestao);
            }
            if (model != null)
            {
                return PartialView("_Questao", model);
            }
            return null;
        }
    }
}