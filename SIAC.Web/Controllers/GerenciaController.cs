using SIAC.Helpers;
using SIAC.Models;
using SIAC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIAC.Controllers
{
    [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.COLABORADOR, Categoria.PROFESSOR })]
    public class GerenciaController : Controller
    {
        // GET: gerencia
        public ActionResult Index() => View();

        // GET: gerencia/dados
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.COLABORADOR })]
        public ActionResult Dados() => View();

        #region Blocos

        // GET: gerencia/blocos
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.COLABORADOR })]
        public ActionResult Blocos()
        {
            GerenciaBlocosViewModel viewModel = new GerenciaBlocosViewModel();
            viewModel.Campi = Campus.ListarOrdenadamente();
            viewModel.Blocos = Bloco.ListarOrdenadamente();

            return View(viewModel);
        }

        // POST: gerencia/novobloco
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.COLABORADOR })]
        public ActionResult NovoBloco(FormCollection form)
        {
            string lembrete = Lembrete.NEGATIVO;
            string mensagem = "Ocorreu um erro ao tentar cadastrar um novo bloco.";

            if (form.HasKeys())
            {
                string campusCodComposto = form["ddlCampus"].Trim();
                string descricao = form["txtDescricao"].Trim();
                string sigla = form["txtSigla"].Trim();
                string refLocal = form["txtRefLocal"].Trim();
                string observacao = form["txtObservacao"].Trim();
                if (!StringExt.IsNullOrWhiteSpace(campusCodComposto, descricao, sigla))
                {
                    Bloco bloco = new Bloco();
                    bloco.Campus = Campus.ListarPorCodigo(campusCodComposto);
                    bloco.Descricao = descricao;
                    bloco.Sigla = String.IsNullOrWhiteSpace(sigla) ? null : sigla;
                    bloco.RefLocal = String.IsNullOrWhiteSpace(refLocal) ? null : refLocal;
                    bloco.Observacao = String.IsNullOrWhiteSpace(observacao) ? null : observacao;

                    Bloco.Inserir(bloco);

                    lembrete = Lembrete.POSITIVO;
                    mensagem = $"Novo bloco \"{bloco.Descricao}\" cadastrado com sucesso.";
                }
                else
                {
                    mensagem = "É necessário Campus, Descrição e Sigla para cadastrar um novo bloco.";
                }
            }

            Lembrete.AdicionarNotificacao(mensagem, lembrete);
            return RedirectToAction("Blocos");
        }

        // POST: gerencia/carregarbloco
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.COLABORADOR })]
        public ActionResult CarregarBloco(int bloco)
        {
            GerenciaEditarBlocoViewModel viewModel = new GerenciaEditarBlocoViewModel();
            viewModel.Campi = Campus.ListarOrdenadamente();
            viewModel.Bloco = Bloco.ListarPorCodigo(bloco);

            return PartialView("_CarregarBloco", viewModel);
        }

        // POST: gerencia/editarbloco
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.COLABORADOR })]
        public ActionResult EditarBloco(int codigo, FormCollection form)
        {
            string lembrete = Lembrete.NEGATIVO;
            string mensagem = "Ocorreu um erro ao tentar editar um bloco.";

            Bloco bloco = Bloco.ListarPorCodigo(codigo);

            if (bloco != null && form.HasKeys())
            {
                string campusCodComposto = form["ddlCampus"].Trim();
                string descricao = form["txtDescricao"].Trim();
                string sigla = form["txtSigla"].Trim();
                string refLocal = form["txtRefLocal"].Trim();
                string observacao = form["txtObservacao"].Trim();

                if (!StringExt.IsNullOrWhiteSpace(campusCodComposto, descricao, sigla))
                {
                    bloco.Campus = Campus.ListarPorCodigo(campusCodComposto);
                    bloco.Descricao = descricao;
                    bloco.Sigla = String.IsNullOrWhiteSpace(sigla) ? null : sigla;
                    bloco.RefLocal = String.IsNullOrWhiteSpace(refLocal) ? null : refLocal;
                    bloco.Observacao = String.IsNullOrWhiteSpace(observacao) ? null : observacao;

                    Repositorio.GetInstance().SaveChanges();

                    lembrete = Lembrete.POSITIVO;
                    mensagem = $"Bloco \"{bloco.Descricao}\" editado com sucesso.";
                }
                else
                {
                    mensagem = "É necessário Campus, Descrição e Sigla para editar um bloco.";
                }
            }

            Lembrete.AdicionarNotificacao(mensagem, lembrete);
            return RedirectToAction("Blocos");
        }

        // POST: gerencia/excluirbloco
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.COLABORADOR })]
        public void ExcluirBloco(int codigo)
        {
            string lembrete = Lembrete.NEGATIVO;
            string mensagem = "Ocorreu um erro ao tentar excluir um bloco.";

            Bloco bloco = Bloco.ListarPorCodigo(codigo);

            if (bloco != null)
            {
                if (bloco.Sala.Count == 0)
                {
                    Repositorio.GetInstance().Bloco.Remove(bloco);
                    Repositorio.GetInstance().SaveChanges();

                    lembrete = Lembrete.POSITIVO;
                    mensagem = $"Bloco \"{bloco.Descricao}\" excluído com sucesso.";
                }
                else
                {
                    lembrete = Lembrete.NEGATIVO;
                    mensagem = $"É necessário excluir primeiro as salas do Bloco \"{bloco.Descricao}\". Este bloco contém {bloco.Sala.Count} salas relacionadas.";
                }
            }

            Lembrete.AdicionarNotificacao(mensagem, lembrete);
        }

        #endregion

        #region Salas

        // GET: gerencia/salas
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.COLABORADOR })]
        public ActionResult Salas()
        {
            GerenciaSalasViewModel viewModel = new GerenciaSalasViewModel();
            viewModel.Campi = Campus.ListarOrdenadamente();
            viewModel.Blocos = Bloco.ListarOrdenadamente();
            viewModel.Salas = Sala.ListarOrdenadamente();

            return View(viewModel);
        }

        // POST: gerencia/novasala
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.COLABORADOR })]
        public ActionResult NovaSala(FormCollection form)
        {
            string lembrete = Lembrete.NEGATIVO;
            string mensagem = "Ocorreu um erro ao tentar cadastrar uma nova sala.";

            if (form.HasKeys())
            {
                string bloco = form["ddlBloco"].Trim();
                string descricao = form["txtDescricao"].Trim();
                string sigla = form["txtSigla"].Trim();
                string capacidade = form["txtCapacidade"].Trim();
                string refLocal = form["txtRefLocal"].Trim();
                string observacao = form["txtObservacao"].Trim();
                if (!StringExt.IsNullOrWhiteSpace(bloco, descricao, sigla, capacidade))
                {
                    Sala sala = new Sala();
                    sala.Bloco = Bloco.ListarPorCodigo(int.Parse(bloco));
                    sala.Descricao = descricao;
                    sala.Sigla = sigla;
                    sala.Capacidade = int.Parse(capacidade);
                    sala.RefLocal = String.IsNullOrWhiteSpace(refLocal) ? null : refLocal;
                    sala.Observacao = String.IsNullOrWhiteSpace(observacao) ? null : observacao;

                    Sala.Inserir(sala);

                    lembrete = Lembrete.POSITIVO;
                    mensagem = $"Novo sala \"{sala.Descricao}\" cadastrada com sucesso.";
                }
                else
                {
                    mensagem = "É necessário Bloco, Descrição, Sigla e Capacidade para cadastrar uma nova sala.";
                }
            }

            Lembrete.AdicionarNotificacao(mensagem, lembrete);
            return RedirectToAction("Salas");
        }

        // POST: gerencia/carregarsala
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.COLABORADOR })]
        public ActionResult CarregarSala(int sala)
        {
            GerenciaEditarSalaViewModel viewModel = new GerenciaEditarSalaViewModel();
            viewModel.Campi = Campus.ListarOrdenadamente();
            viewModel.Blocos = Bloco.ListarOrdenadamente();
            viewModel.Sala = Sala.ListarPorCodigo(sala);

            return PartialView("_CarregarSala", viewModel);
        }

        // POST: gerencia/editarsala
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.COLABORADOR })]
        public ActionResult EditarSala(int codigo, FormCollection form)
        {
            string lembrete = Lembrete.NEGATIVO;
            string mensagem = "Ocorreu um erro ao tentar editar uma sala.";

            Sala sala = Sala.ListarPorCodigo(codigo);

            if (sala != null && form.HasKeys())
            {
                string bloco = form["ddlBloco"].Trim();
                string descricao = form["txtDescricao"].Trim();
                string sigla = form["txtSigla"].Trim();
                string capacidade = form["txtCapacidade"].Trim();
                string refLocal = form["txtRefLocal"].Trim();
                string observacao = form["txtObservacao"].Trim();
                if (!StringExt.IsNullOrWhiteSpace(bloco, descricao, sigla, capacidade))
                {
                    sala.Bloco = Bloco.ListarPorCodigo(int.Parse(bloco));
                    sala.Descricao = descricao;
                    sala.Sigla = sigla;
                    sala.Capacidade = int.Parse(capacidade);
                    sala.RefLocal = String.IsNullOrWhiteSpace(refLocal) ? null : refLocal;
                    sala.Observacao = String.IsNullOrWhiteSpace(observacao) ? null : observacao;

                    Repositorio.GetInstance().SaveChanges();

                    lembrete = Lembrete.POSITIVO;
                    mensagem = $"Sala \"{sala.Descricao}\" editada com sucesso.";
                }
                else
                {
                    mensagem = "É necessário Bloco, Descrição, Sigla e Capacidade para editar uma sala.";
                }
            }

            Lembrete.AdicionarNotificacao(mensagem, lembrete);
            return RedirectToAction("Salas");
        }

        // POST: gerencia/excluirsala
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.COLABORADOR })]
        public void ExcluirSala(int codigo)
        {
            string lembrete = Lembrete.NEGATIVO;
            string mensagem = "Ocorreu um erro ao tentar excluir uma sala.";

            Sala sala = Sala.ListarPorCodigo(codigo);

            if (sala != null)
            {
                Repositorio.GetInstance().Sala.Remove(sala);
                Repositorio.GetInstance().SaveChanges();

                lembrete = Lembrete.POSITIVO;
                mensagem = $"Sala \"{sala.Descricao}\" excluída com sucesso.";
            }

            Lembrete.AdicionarNotificacao(mensagem, lembrete);
        }

        #endregion

        #region Disciplinas

        // GET: gerencia/disciplinas
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.COLABORADOR })]
        public ActionResult Disciplinas() => View(new GerenciaDisciplinasViewModel()
        {
            Disciplinas = Disciplina.ListarOrdenadamente()
        });

        // POST: gerencia/novadisciplina
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.COLABORADOR })]
        public ActionResult NovaDisciplina(FormCollection form)
        {
            string lembrete = Lembrete.NEGATIVO;
            string mensagem = "Ocorreu um erro ao tentar cadastrar uma nova disciplina.";

            if (form.HasKeys())
            {
                string descricao = form["txtDescricao"].Trim();
                string sigla = form["txtSigla"].Trim();
                if (!StringExt.IsNullOrWhiteSpace(descricao, sigla))
                {
                    Disciplina disciplina = new Disciplina()
                    {
                        Descricao = descricao,
                        Sigla = sigla,
                        Tema = new List<Tema> {
                            new Tema()
                            {
                                CodTema = 1,
                                Descricao = "Simulado"
                            }
                        }
                    };

                    Disciplina.Inserir(disciplina);

                    lembrete = Lembrete.POSITIVO;
                    mensagem = $"Nova disciplina \"{disciplina.Descricao}\" cadastrada com sucesso.";
                }
                else
                {
                    mensagem = "É necessário Descrição e Sigla para cadastrar uma nova disciplina.";
                }
            }

            Lembrete.AdicionarNotificacao(mensagem, lembrete);
            return RedirectToAction("Disciplinas");
        }

        // POST: gerencia/carregardisciplina
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.COLABORADOR })]
        public ActionResult CarregarDisciplina(int disciplina) => PartialView("_CarregarDisciplina", Disciplina.ListarPorCodigo(disciplina));

        // POST: gerencia/editardisciplina
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.COLABORADOR })]
        public ActionResult EditarDisciplina(int codigo, FormCollection form)
        {
            string lembrete = Lembrete.NEGATIVO;
            string mensagem = "Ocorreu um erro ao tentar editar uma disciplina.";

            Disciplina disciplina = Disciplina.ListarPorCodigo(codigo);

            if (disciplina != null && form.HasKeys())
            {
                string descricao = form["txtDescricao"].Trim();
                string sigla = form["txtSigla"].Trim();

                if (!StringExt.IsNullOrWhiteSpace(descricao, sigla))
                {
                    disciplina.Descricao = descricao;
                    disciplina.Sigla = sigla;

                    Repositorio.GetInstance().SaveChanges();

                    lembrete = Lembrete.POSITIVO;
                    mensagem = $"Disciplina \"{disciplina.Descricao}\" editada com sucesso.";
                }
                else
                {
                    mensagem = "É necessário Descrição e Sigla para editar uma disciplina.";
                }
            }

            Lembrete.AdicionarNotificacao(mensagem, lembrete);
            return RedirectToAction("Disciplinas");
        }

        // POST: gerencia/excluirdisciplina
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.COLABORADOR })]
        public void ExcluirDisciplina(int codigo)
        {
            string lembrete = Lembrete.NEGATIVO;
            string mensagem = "Ocorreu um erro ao tentar excluir uma disciplina.";

            Disciplina disciplina = Disciplina.ListarPorCodigo(codigo);

            if (disciplina != null)
            {
                if (disciplina.Tema.Count == 0 || (disciplina.Tema.Count == 1 && disciplina.Tema.First().Descricao.ToLower() == "simulado"))
                {
                    Repositorio.GetInstance().Disciplina.Remove(disciplina);
                    Repositorio.GetInstance().SaveChanges();

                    lembrete = Lembrete.POSITIVO;
                    mensagem = $"Disciplina \"{disciplina.Descricao}\" excluída com sucesso.";
                }
                else
                {
                    lembrete = Lembrete.NEGATIVO;
                    mensagem = $"É necessário excluir primeiro os temas da disciplina \"{disciplina.Descricao}\". Esta disciplina contém {disciplina.Tema.Count} temas relacionados.";
                }
            }

            Lembrete.AdicionarNotificacao(mensagem, lembrete);
        }

        #endregion

        #region Professores

        // GET: gerencia/professores
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.COLABORADOR })]
        public ActionResult Professores() => View(new GerenciaProfessoresViewModel()
        {
            Professores = Professor.ListarOrdenadamente(),
            Disciplinas = Disciplina.ListarOrdenadamente()
        });

        // POST: gerencia/novoprofessor
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.COLABORADOR })]
        public ActionResult NovoProfessor(FormCollection form)
        {
            string lembrete = Lembrete.NEGATIVO;
            string mensagem = "Ocorreu um erro ao tentar cadastrar um(a) novo(a) professor(a).";

            if (form.HasKeys())
            {
                string nome = form["txtNome"].Trim();
                string matricula = form["txtMatricula"].Trim();
                string senha = form["txtSenha"];
                string senhaConfirmacao = form["txtSenhaConfirmacao"];
                string[] disciplinas = form["ddlDisciplina"].Split(',');
                if (!StringExt.IsNullOrWhiteSpace(nome, matricula, senha, senha) && disciplinas.Length > 0)
                {
                    if (senha == senhaConfirmacao)
                    {
                        int codPessoa = Pessoa.Inserir(new Pessoa() { TipoPessoa = Pessoa.FISICA });

                        PessoaFisica pf = new PessoaFisica();
                        pf.CodPessoa = codPessoa;
                        pf.Nome = nome;
                        pf.Categoria.Add(Categoria.ListarPorCodigo(Categoria.PROFESSOR));

                        int codPessoaFisica = PessoaFisica.Inserir(pf);

                        Usuario usuario = new Usuario();
                        usuario.Matricula = matricula;
                        usuario.CodPessoaFisica = codPessoaFisica;
                        usuario.CodCategoria = Categoria.PROFESSOR;
                        usuario.Senha = Criptografia.RetornarHash(senhaConfirmacao);

                        int codUsuario = Usuario.Inserir(usuario);

                        Professor professor = new Professor();
                        professor.MatrProfessor = usuario.Matricula;

                        foreach (string disciplina in disciplinas)
                        {
                            professor.Disciplina.Add(Disciplina.ListarPorCodigo(int.Parse(disciplina)));
                        }

                        Professor.Inserir(professor);

                        lembrete = Lembrete.POSITIVO;
                        mensagem = $"Novo(a) professor(a) \"{pf.Nome}\" cadastrado(a) com sucesso.";
                    }
                    else
                    {
                        mensagem = "A Senha informada deve ser igual à Confirmação da Senha.";
                    }
                }
                else
                {
                    mensagem = "Todos os campos são necessário para cadastrar um(a) novo(a) professor(a).";
                }
            }

            Lembrete.AdicionarNotificacao(mensagem, lembrete);
            return RedirectToAction("Professores");
        }

        // POST: gerencia/carregarprofessor
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.COLABORADOR })]
        public ActionResult CarregarProfessor(string professor) => PartialView("_CarregarProfessor", new GerenciaEditarProfessorViewModel()
        {
            Professor = Professor.ListarPorMatricula(professor),
            Disciplinas = Disciplina.ListarOrdenadamente()
        });

        // POST: gerencia/carregarprofessordisciplinas
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.COLABORADOR })]
        public ActionResult CarregarProfessorDisciplinas(string professor) => PartialView("_CarregarProfessorDisciplinas", Professor.ListarPorMatricula(professor));

        // POST: gerencia/editarprofessor
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.COLABORADOR })]
        public ActionResult EditarProfessor(string codigo, FormCollection form)
        {
            string lembrete = Lembrete.NEGATIVO;
            string mensagem = "Ocorreu um erro ao tentar editar um(a) professor(a).";

            Professor professor = Professor.ListarPorMatricula(codigo);

            if (professor != null && form.HasKeys())
            {
                string nome = form["txtNome"].Trim();
                string[] disciplinas = String.IsNullOrEmpty(form["ddlDisciplina"]) ? new string[0] : form["ddlDisciplina"].Split(',');

                if (!StringExt.IsNullOrWhiteSpace(nome))
                {
                    professor.Usuario.PessoaFisica.Nome = nome;

                    professor.Disciplina.Clear();

                    foreach (string disciplina in disciplinas)
                    {
                        professor.Disciplina.Add(Disciplina.ListarPorCodigo(int.Parse(disciplina)));
                    }

                    Repositorio.GetInstance().SaveChanges();

                    lembrete = Lembrete.POSITIVO;
                    mensagem = $"Professor(a) \"{nome}\" editado(a) com sucesso.";
                }
                else
                {
                    mensagem = "Todos os campos são necessário para editar um(a) professor(a).";
                }
            }

            Lembrete.AdicionarNotificacao(mensagem, lembrete);
            return RedirectToAction("Professores");
        }

        // POST: gerencia/excluirprofessor
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.COLABORADOR })]
        public void ExcluirProfessor(string codigo)
        {
            string lembrete = Lembrete.NEGATIVO;
            string mensagem = "Ocorreu um erro ao tentar excluir um(a) professor(a).";

            Professor professor = Professor.ListarPorMatricula(codigo);

            if (professor != null)
            {
                try
                {
                    Repositorio.GetInstance().Professor.Remove(professor);
                    Repositorio.GetInstance().SaveChanges();

                    lembrete = Lembrete.POSITIVO;
                    mensagem = $"Professor(a) \"{professor.Usuario.PessoaFisica.Nome}\" excluído(a) com sucesso.";
                }
                catch
                {
                    Repositorio.Dispose();
                    Repositorio.Commit();

                    lembrete = Lembrete.NEGATIVO;
                    mensagem = $"Não é possível excluir este(a) professor(a).";
                }
            }

            Lembrete.AdicionarNotificacao(mensagem, lembrete);
        }

        #endregion
    }
}