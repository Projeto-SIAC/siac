using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIAC.Web.Models;
using SIAC.Web.Helpers;

namespace SIAC.Web.Controllers
{
    public class ConfiguracoesController : Controller
    {

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if(Session["Autenticado"] == null)
            {
                filterContext.Result = RedirectToAction("Index", "Dashboard");
            }
            else if(!(bool)Session["Autenticado"])
            {
                filterContext.Result = RedirectToAction("Index", "Dashboard");
            }
            else if((int)Session["UsuarioCategoriaCodigo"] != 3)
            {
                if (TempData["UrlReferrer"] != null)
                {
                    filterContext.Result = Redirect(TempData["UrlReferrer"].ToString());
                }
                else filterContext.Result = RedirectToAction("Index", "Dashboard");
            }

            base.OnActionExecuting(filterContext);
        }

        // GET: /Configuracoes
        public ActionResult Index()
        {
            Parametro model = Parametro.Obter();

            ViewBag.Disciplinas = Disciplina.ListarOrdenadamente();
            ViewBag.Professores = Professor.ListarOrdenadamente();
            ViewBag.Temas = Tema.ListarOrdenadamenteComDisciplina();
            ViewBag.Alunos = Aluno.ListarOrdenadamente();
            ViewBag.Cursos = Curso.ListarOrdenadamente();
            ViewBag.Colaboradores = Colaborador.ListarOrdenadamente();
            ViewBag.Campi = Campus.ListarOrdenadamente();
            ViewBag.Instituicoes = Instituicao.ListarOrdenadamente();
            ViewBag.Diretorias = Diretoria.ListarOrdenadamente();

            return View(model);
        }

        //POST: /Configuracoes
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Index(FormCollection formCollection)
        {
            if (formCollection.HasKeys())
            {
                Parametro temp = Parametro.Obter();
                temp.TempoInatividade = int.Parse(formCollection["txtTempoInatividade"]);
                temp.NumeracaoQuestao = int.Parse(formCollection["ddlNumeracaoQuestao"]);
                temp.NumeracaoAlternativa = int.Parse(formCollection["ddlNumeracaoAlternativa"]);
                temp.QteSemestres = int.Parse(formCollection["txtQteSemestre"]);
                temp.TermoResponsabilidade = formCollection["txtTermoResponsabilidade"];

                Parametro.Atualizar(temp);
            }

            return null;
        }

        //POST: /Configuracoes/CadastrarProfessor
        [HttpPost]
        public ActionResult CadastrarProfessor(FormCollection formCollection)
        {
            if(formCollection.HasKeys())
            {
                string ProfessorNome = formCollection["txtProfessorNome"];
                string ProfessorMatricula = formCollection["txtProfessorMatricula"];

                int codPessoa = Pessoa.Inserir(new Pessoa() { TipoPessoa = "F" });

                PessoaFisica pf = new PessoaFisica();
                pf.CodPessoa = codPessoa;
                pf.Nome = ProfessorNome;
                pf.Categoria.Add(Categoria.ListarPorCodigo(2));

                int codPessoaFisica = PessoaFisica.Inserir(pf);

                Usuario usuario = new Usuario();
                usuario.Matricula = ProfessorMatricula;
                usuario.CodPessoaFisica = codPessoaFisica;
                usuario.CodCategoria = 2;
                usuario.Senha = Criptografia.RetornarHash("senha");

                int codUsuario = Usuario.Inserir(usuario);

                Professor professor = new Professor();
                professor.MatrProfessor = ProfessorMatricula;

                string[] disciplinas = formCollection["ddlProfessorDisciplinas"].Split(',');
                foreach (string item in disciplinas)
                {
                    professor.Disciplina.Add(Disciplina.ListarPorCodigo(int.Parse(item)));
                }

                Professor.Inserir(professor);
            }
            return RedirectToAction("Index");
        }

        //POST: /Configuracoes/CadastrarColaborador
        [HttpPost]
        public ActionResult CadastrarColaborador(FormCollection formCollection)
        {
            if (formCollection.HasKeys())
            {
                Colaborador colaborador = new Colaborador();
                colaborador.Usuario = new Usuario();
                colaborador.Usuario.PessoaFisica = new PessoaFisica();
                colaborador.Usuario.PessoaFisica.Pessoa = new Pessoa();


                //Pessoa
                colaborador.Usuario.PessoaFisica.Pessoa.TipoPessoa = "F";

                //PessoaFisica
                colaborador.Usuario.PessoaFisica.Nome = formCollection["txtColaboradorNome"];
                colaborador.Usuario.PessoaFisica.Categoria.Add(Categoria.ListarPorCodigo(3));

                //Usuario
                colaborador.Usuario.Categoria = Categoria.ListarPorCodigo(3);
                colaborador.Usuario.Matricula = formCollection["txtColaboradorMatricula"];
                colaborador.Usuario.Senha = Criptografia.RetornarHash("senha");
                colaborador.Usuario.DtCadastro = DateTime.Now;

                colaborador.MatrColaborador = formCollection["txtColaboradorMatricula"];

                Colaborador.Inserir(colaborador);
            }
            return RedirectToAction("Index");
        }

        //POST: /Configuracoes/CadastrarCampus
        [HttpPost]
        public ActionResult CadastrarCampus(FormCollection formCollection)
        {
            if (formCollection.HasKeys())
            {
                Campus campus = new Campus();
                campus.PessoaJuridica = new PessoaJuridica();
                campus.PessoaJuridica.Pessoa = new Pessoa();

                //Pessoa
                campus.PessoaJuridica.Pessoa.TipoPessoa = "J";

                //PessoaJuridica
                campus.PessoaJuridica.RazaoSocial = formCollection["txtCampusRazaoSocial"];
                campus.PessoaJuridica.NomeFantasia = formCollection["txtCampusNomeFantasia"];
                campus.PessoaJuridica.Portal = formCollection["txtCampusPortal"];

                //Campus
                campus.CodInstituicao = int.Parse(formCollection["ddlCampusInstituicao"]);
                campus.CodColaboradorDiretor = int.Parse(formCollection["ddlCampusDiretor"]);
                campus.Sigla = formCollection["txtCampusSigla"];

                Campus.Inserir(campus);
            }
            return RedirectToAction("Index");
        }

        //POST: /Configuracoes/CadastrarDiretoria
        [HttpPost]
        public ActionResult CadastrarDiretoria(FormCollection formCollection)
        {
            if (formCollection.HasKeys())
            {
                Diretoria diretoria = new Diretoria();
                diretoria.PessoaJuridica = new PessoaJuridica();
                diretoria.PessoaJuridica.Pessoa = new Pessoa();

                //Pessoa
                diretoria.PessoaJuridica.Pessoa.TipoPessoa = "J";

                //PessoaJuridica
                diretoria.PessoaJuridica.RazaoSocial = formCollection["txtDiretoriaRazaoSocial"];
                diretoria.PessoaJuridica.NomeFantasia = formCollection["txtDiretoriaNomeFantasia"];
                diretoria.PessoaJuridica.Portal = formCollection["txtDiretoriaPortal"];

                //Diretoria
                diretoria.CodCampus = int.Parse(formCollection["ddlDiretoriaCampus"]);

                campus.CodColaboradorDiretor = int.Parse(formCollection["ddlCampusDiretor"]);
                campus.Sigla = formCollection["txtCampusSigla"];

                Campus.Inserir(campus);
            }
            return RedirectToAction("Index");
        }

        //POST: Configuracoes/CadastrarDisciplina
        [HttpPost]
        public ActionResult CadastrarDisciplina(FormCollection formCollection)
        {
            if (formCollection.HasKeys())
            {
                string DisciplinaNome = formCollection["txtDisciplina"];
                string DisciplinaSigla = formCollection["txtSigla"];

                Disciplina disciplina = new Disciplina();
                disciplina.Descricao = DisciplinaNome;
                disciplina.Sigla = DisciplinaSigla;
                disciplina.FlagEletivaOptativa = (formCollection["chkEletivaOptativa"] != null) ? true : false;
                disciplina.FlagFlexivel = (formCollection["chkFlexivel"] != null) ? true : false;

                int codDisciplina = Disciplina.Inserir(disciplina);

                string[] temas = formCollection["txtTema"].Split(';');
                int i = 1;
                foreach (string item in temas)
                {
                    string tema = item.Trim();
                    Tema t = new Tema();
                    t.CodDisciplina = codDisciplina;
                    t.CodTema = i;
                    t.Descricao = tema;
                    i++;
                    Tema.Inserir(t);
                }
            }
            return RedirectToAction("Index");
        }

        //POST: Configuracoes/CadastrarTema
        [HttpPost]
        public ActionResult CadastrarTema(FormCollection formCollection)
        {
            if (formCollection.HasKeys())
            {
                int codDisciplina;
                int.TryParse(formCollection["ddlTemaDisciplina"], out codDisciplina);
                string descricao = formCollection["txtTemaDescricao"];

                Tema tema = new Tema();
                tema.Disciplina = Disciplina.ListarPorCodigo(codDisciplina);
                tema.Descricao = descricao;
                var codTemas = (from t in DataContextSIAC.GetInstance().Tema
                                where t.CodDisciplina == codDisciplina
                                select t.CodTema).ToList();
                tema.CodTema = codTemas != null && codTemas.Count>0 ? codTemas.Max() + 1 : 1;
                Tema.Inserir(tema);
            }
            return RedirectToAction("Index");
        }

        //POST: Configuracoes/CadastrarAluno //FALTA TESTAR
        [HttpPost]
        public ActionResult CadastrarAluno(FormCollection formCollection)
        {
            if (formCollection.HasKeys())
            {
                Aluno aluno = new Aluno();

                //Usuario
                aluno.Usuario = new Usuario();
                aluno.Usuario.Matricula = formCollection["txtAlunoMatricula"];
                aluno.Usuario.Senha = Criptografia.RetornarHash("senha");
                aluno.Usuario.CodCategoria = 1;
                aluno.Usuario.DtCadastro = DateTime.Now;
                
                //PessoaFisica
                aluno.Usuario.PessoaFisica = new PessoaFisica();
                aluno.Usuario.PessoaFisica.Nome = formCollection["txtAlunoNome"];

                //Pessoa
                aluno.Usuario.PessoaFisica.Pessoa.TipoPessoa = "F";

                //Curso
                aluno.CodCurso = int.Parse(formCollection["ddlAlunoCurso"]);

                Aluno.Inserir(aluno);
            }

            return RedirectToAction("Index");
        }

    }
}