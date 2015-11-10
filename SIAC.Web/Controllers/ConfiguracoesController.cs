using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIAC.Models;
using SIAC.Helpers;

namespace SIAC.Controllers
{
    [Filters.AutenticacaoFilter(Categorias = new[] { 3 })]
    public class ConfiguracoesController : Controller
    {
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
            ViewBag.Cursos = Curso.ListarOrdenadamente();
            ViewBag.NiveisEnsino = NivelEnsino.ListarOrdenadamente();
            ViewBag.Turmas = Turma.ListarOrdenadamente();
            ViewBag.Turnos = Turno.ListarOrdenadamente();
            ViewBag.Salas = Sala.ListarOrdenadamente();
            ViewBag.Matrizes = MatrizCurricular.ListarOrdenadamente();
            ViewBag.Horarios = Horario.ListarOrdenadamente();

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
                temp.NotaUso = formCollection["txtNotaUso"];
                temp.ValorNotaMedia = double.Parse(formCollection["txtValorNotaMedia"].Replace('.', ','));
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
                string codCampus = formCollection["ddlDiretoriaCampus"];
                diretoria.Campus = Campus.ListarPorCodigo(codCampus);
                //diretoria.CodCampus = codCampus;
                //diretoria.CodInstituicao = Campus.ListarPorCodigo(codCampus).CodInstituicao;
                diretoria.CodColaboradorDiretor = int.Parse(formCollection["ddlDiretoriaDiretor"]);
                diretoria.Sigla = formCollection["txtDiretoriaSigla"];

                Diretoria.Inserir(diretoria);
            }
            return RedirectToAction("Index");
        }

        //POST: /Configuracoes/CadastrarCurso
        [HttpPost]
        public ActionResult CadastrarCurso(FormCollection formCollection)
        {
            if (formCollection.HasKeys())
            {
                Curso curso = new Curso();

                //Diretoria
                string codDiretoria = formCollection["ddlCursoDiretoria"];
                curso.Diretoria = Diretoria.ListarPorCodigo(codDiretoria);

                //NivelEnsino
                curso.CodNivelEnsino = int.Parse(formCollection["ddlCursoNivelEnsino"]);

                //Diretor
                curso.CodColabCoordenador = int.Parse(formCollection["ddlCursoCoordenador"]);

                //Curso
                curso.Descricao = formCollection["txtCursoDescricao"];
                curso.Sigla = formCollection["txtCursoSigla"];

                Curso.Inserir(curso);
            }
            return RedirectToAction("Index");
        }

        //POST: Configuracoes/CadastrarTurma
        [HttpPost]
        public ActionResult CadastrarTurma(FormCollection formCollection)
        {
            if (formCollection.HasKeys())
            {
                Turma turma = new Turma();

                //Turma
                turma.CodCurso = int.Parse(formCollection["ddlTurmaCurso"]);
                turma.Periodo = int.Parse(formCollection["txtTurmaPeriodo"]);
                turma.CodTurno = formCollection["ddlTurmaTurno"];
                turma.Nome = formCollection["txtTurmaNome"];

                Turma.Inserir(turma);
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
                    if (!String.IsNullOrWhiteSpace(tema))
                    {
                        Tema t = new Tema();
                        t.CodDisciplina = codDisciplina;
                        t.CodTema = i;
                        t.Descricao = tema;
                        i++;
                        Tema.Inserir(t);
                    }
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

                var codTemas = (from t in Repositorio.GetInstance().Tema
                                where t.CodDisciplina == codDisciplina
                                select t.CodTema).ToList();

                string[] temas = formCollection["txtTemaDescricao"].Split(';');
                int i = codTemas != null && codTemas.Count > 0 ? codTemas.Max() + 1 : 1;
                foreach (string item in temas)
                {
                    string tema = item.Trim();
                    if (!String.IsNullOrWhiteSpace(tema))
                    {
                        Tema t = new Tema();
                        t.CodDisciplina = codDisciplina;
                        t.CodTema = i;
                        t.Descricao = tema;
                        i++;
                        Tema.Inserir(t);
                    }
                }
            }
            return RedirectToAction("Index");
        }

        //POST: Configuracoes/CadastrarAluno
        [HttpPost]
        public ActionResult CadastrarAluno(FormCollection formCollection)
        {
            if (formCollection.HasKeys())
            {
                Aluno aluno = new Aluno();
                aluno.Usuario = new Usuario();
                aluno.Usuario.PessoaFisica = new PessoaFisica();
                aluno.Usuario.PessoaFisica.Pessoa = new Pessoa();

                //Pessoa
                aluno.Usuario.PessoaFisica.Pessoa.TipoPessoa = "F";

                //PessoaFisica
                aluno.Usuario.PessoaFisica.Nome = formCollection["txtAlunoNome"];
                aluno.Usuario.PessoaFisica.Categoria.Add(Categoria.ListarPorCodigo(1));

                //Usuario
                aluno.Usuario.Categoria = Categoria.ListarPorCodigo(1);
                aluno.Usuario.Matricula = formCollection["txtAlunoMatricula"];
                aluno.Usuario.Senha = Criptografia.RetornarHash("senha");
                aluno.Usuario.DtCadastro = DateTime.Now;

                //Curso
                aluno.CodCurso = int.Parse(formCollection["ddlAlunoCurso"]);

                Aluno.Inserir(aluno);
            }

            return RedirectToAction("Index");
        }

        //POST: Configuracoes/CadastrarSala
        [HttpPost]
        public ActionResult CadastrarSala(FormCollection formCollection)
        {
            if (formCollection.HasKeys())
            {
                Sala sala = new Sala();

                //Campus
                string codCampus = formCollection["ddlSalaCampus"];
                sala.Campus = Campus.ListarPorCodigo(codCampus);
                sala.Descricao = formCollection["txtSalaDescricao"];
                sala.Sigla = formCollection["txtSalaSigla"];

                Sala.Inserir(sala);
            }

            return RedirectToAction("Index");
        }

        //POST: Configuracoes/CadastrarMatriz
        [HttpPost]
        public ActionResult CadastrarMatriz(FormCollection formCollection)
        {
            if (formCollection.HasKeys())
            {
                MatrizCurricular matrizCurricular = new MatrizCurricular();

                int codCurso = int.Parse(formCollection["ddlMatrizCurso"]);
                matrizCurricular.CodCurso = codCurso;
                matrizCurricular.CodMatriz = MatrizCurricular.ObterCodMatriz(codCurso);

                int qteDisc = int.Parse(formCollection["matrizQte"]);

                for (int i = 1; i <= qteDisc; i++)
                {
                    matrizCurricular.MatrizCurricularDisciplina.Add(new MatrizCurricularDisciplina() {
                        Periodo = int.Parse(formCollection["txtPeriodo" + i]),
                        CodDisciplina = int.Parse(formCollection["ddlDisciplina" + i]),
                        DiscCargaHoraria = int.Parse(formCollection["txtCH" + i])
                    });
                }

                MatrizCurricular.Inserir(matrizCurricular);
            }

            return RedirectToAction("Index");
        }

        //POST: Configuracoes/CadastrarHorario
        [HttpPost]
        public ActionResult CadastrarHorario(FormCollection formCollection)
        {
            if (formCollection.HasKeys())
            {
                List<Horario> horarios = new List<Horario>();

                string codTurno = formCollection["ddlHorarioTurno"];
                int codGrupo = int.Parse(formCollection["txtHorarioGrupo"]);
                int horarioQte = int.Parse(formCollection["horarioQte"]);

                for (int i = 1; i <= horarioQte; i++)
                {
                    Horario h = new Horario();

                    h.CodTurno = codTurno;
                    h.CodGrupo = codGrupo;
                    h.CodHorario = i;
                    h.HoraInicio = DateTime.Parse(formCollection["txtInicio" + i]);
                    h.HoraTermino = DateTime.Parse(formCollection["txtTermino" + i]);

                    horarios.Add(h);
                }

                Horario.Inserir(horarios);
            }

            return RedirectToAction("Index");
        }
    }
}