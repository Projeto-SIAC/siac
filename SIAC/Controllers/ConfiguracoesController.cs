/*
This file is part of SIAC.

Copyright (C) 2016 Felipe Mateus Freire Pontes <felipemfpontes@gmail.com>
Copyright (C) 2016 Francisco Bento da Silva Júnior <francisco.bento.jr@hotmail.com>

SIAC is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details. 
*/
using SIAC.Helpers;
using SIAC.Models;
using SIAC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SIAC.Controllers
{
    [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.SUPERUSUARIO, Categoria.COLABORADOR, Categoria.PROFESSOR })]
    public class ConfiguracoesController : Controller
    {
        public List<UsuarioOpiniao> opinioes => Repositorio.GetInstance().UsuarioOpiniao.ToList();

        // GET: configuracoes/
        public ActionResult Index() => View();

        // GET: configuracoes/opinioes/
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.SUPERUSUARIO, Categoria.COLABORADOR }, Ocupacoes = new[] { Ocupacao.SUPERUSUARIO, Ocupacao.REITOR, Ocupacao.PRO_REITOR, Ocupacao.DIRETOR_GERAL, Ocupacao.DIRETOR })]
        public ActionResult Opinioes(string tab) => View((object)tab);

        // POST: configuracoes/listaropinioes/
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.SUPERUSUARIO, Categoria.COLABORADOR }, Ocupacoes = new[] { Ocupacao.SUPERUSUARIO, Ocupacao.REITOR, Ocupacao.PRO_REITOR, Ocupacao.DIRETOR_GERAL, Ocupacao.DIRETOR })]
        public ActionResult ListarOpinioes(int? pagina, string pesquisa, string ordenar)
        {
            var quantidade = 10;
            var lstOpinioes = opinioes;
            pagina = pagina ?? 1;

            if (!String.IsNullOrWhiteSpace(pesquisa))
            {
                string strPesquisa = pesquisa.Trim().ToLower();
                lstOpinioes = lstOpinioes.Where(q => q.Usuario.Matricula.ToLower().Contains(strPesquisa) || q.Usuario.PessoaFisica.Nome.ToLower().Contains(strPesquisa) || q.Opiniao.ToLower().Contains(strPesquisa)).ToList();
            }

            switch (ordenar)
            {
                case "data_desc":
                    lstOpinioes = lstOpinioes.OrderByDescending(q => q.DtEnvio).ToList();
                    break;

                case "data":
                    lstOpinioes = lstOpinioes.OrderBy(q => q.DtEnvio).ToList();
                    break;

                default:
                    lstOpinioes = lstOpinioes.OrderByDescending(q => q.DtEnvio).ToList();
                    break;
            }

            return PartialView("_ListaOpinioes", lstOpinioes.Skip((quantidade * pagina.Value) - quantidade).Take(quantidade).ToList());
        }

        // GET: configuracoes/parametros/
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.SUPERUSUARIO, Categoria.COLABORADOR }, Ocupacoes = new[] { Ocupacao.SUPERUSUARIO, Ocupacao.REITOR, Ocupacao.PRO_REITOR, Ocupacao.DIRETOR_GERAL, Ocupacao.DIRETOR, Ocupacao.COORDENADOR })]
        public ActionResult Parametros()
        {
            var model = new ConfiguracoesParametrosViewModel();
            model.Parametro = Parametro.Obter();

            model.Disciplinas = Disciplina.ListarOrdenadamente();
            model.Professores = Professor.ListarOrdenadamente();
            model.Temas = Tema.ListarOrdenadamenteComDisciplina();
            model.Alunos = Aluno.ListarOrdenadamente();
            model.Colaboradores = Colaborador.ListarOrdenadamente();
            model.Campi = Campus.ListarOrdenadamente();
            model.Instituicoes = Instituicao.ListarOrdenadamente();
            model.Diretorias = Diretoria.ListarOrdenadamente();
            model.Cursos = Curso.ListarOrdenadamente();
            model.NiveisEnsino = NivelEnsino.ListarOrdenadamente();
            model.Turmas = Turma.ListarOrdenadamente();
            model.Turnos = Turno.ListarOrdenadamente();
            model.Salas = Sala.ListarOrdenadamente();
            model.Matrizes = MatrizCurricular.ListarOrdenadamente();
            model.Horarios = Horario.ListarOrdenadamente();

            return View(model);
        }

        // GET: configuracoes/institucional/
        [Filters.AutenticacaoFilter(SomenteOcupacaoAvi = true)]
        public ActionResult Institucional(string tab)
        {
            if (String.IsNullOrWhiteSpace(tab) || tab.ToLower() != "coordenadores")
            {
                return RedirectToAction("Institucional", new { tab = "Coordenadores" });
            }
            var model = new ConfiguracoesInstitucionalViewModel();
            model.Ocupacoes = Repositorio.GetInstance()
                .Ocupacao
                .Where(o => o.CodOcupacao != Ocupacao.COORDENADOR_AVI
                    && o.CodOcupacao != Ocupacao.SUPERUSUARIO
                    && o.CodOcupacao != Ocupacao.COLABORADOR_SIMULADO
                    && o.CodOcupacao != Ocupacao.COORDENADOR_SIMULADO)
                .ToList();
            model.Coordenadores = Repositorio.GetInstance()
                .Ocupacao
                .Find(Ocupacao.COORDENADOR_AVI)
                .PessoaFisica
                .Where(p => p.Usuario.FirstOrDefault(u => u.Matricula == Sessao.UsuarioMatricula) == null)
                .ToList();
            return View(model);
        }

        // POST: configuracoes/alterarocupacoescoordenadores/
        [HttpPost]
        [Filters.AutenticacaoFilter(SomenteOcupacaoAvi = true)]
        public void AlterarOcupacoesCoordenadores(int[] ocupacoes)
        {
            Parametro.AtualizarOcupacoesCoordenadores(ocupacoes);
        }

        // POST: configuracoes/adicionarocupacaocoordenador/
        [HttpPost]
        [Filters.AutenticacaoFilter(SomenteOcupacaoAvi = true)]
        public void AdicionarOcupacaoCoordenador(int codPessoaFisica)
        {
            PessoaFisica.AdicionarOcupacao(codPessoaFisica, Ocupacao.COORDENADOR_AVI);
        }

        // POST: configuracoes/removerocupacaocoordenador/
        [HttpPost]
        [Filters.AutenticacaoFilter(SomenteOcupacaoAvi = true)]
        public void RemoverOcupacaoCoordenador(int[] codPessoaFisica)
        {
            foreach (var codPessoa in codPessoaFisica)
            {
                PessoaFisica.RemoverOcupacao(codPessoa, Ocupacao.COORDENADOR_AVI);
            }
        }

        // POST: configuracoes/listarpessoa/
        [HttpPost]
        [Filters.AutenticacaoFilter(SomenteOcupacaoAvi = true)]
        public ActionResult ListarPessoa(string pesquisa)
        {
            if (!String.IsNullOrWhiteSpace(pesquisa))
            {
                string strPesquisa = pesquisa.Trim().ToLower();
                var lstPessoas = PessoaFisica.Listar().Where(p =>
                    (p.Usuario.FirstOrDefault(u => u.Colaborador.Count > 0) != null) &&
                    (p.Nome.ToLower().Contains(strPesquisa) ||
                    (!String.IsNullOrEmpty(p.Cpf) && p.Cpf.Contains(strPesquisa)) ||
                    p.Usuario.FirstOrDefault(u => u.Matricula.ToLower().Contains(strPesquisa)) != null)
                );
                return Json(lstPessoas.Select(p => new { CodPessoa = p.CodPessoa, Nome = p.Nome }));
            }
            return null;
        }

        // POST: configuracoes/parametros/
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.SUPERUSUARIO, Categoria.COLABORADOR }, Ocupacoes = new[] { Ocupacao.SUPERUSUARIO, Ocupacao.REITOR, Ocupacao.PRO_REITOR, Ocupacao.DIRETOR_GERAL, Ocupacao.DIRETOR, Ocupacao.COORDENADOR })]
        public ActionResult Parametros(FormCollection formCollection)
        {
            if (formCollection.HasKeys())
            {
                Parametro temp = Parametro.Obter();
                temp.TempoInatividade = int.Parse(formCollection["txtTempoInatividade"]);
                temp.NumeracaoQuestao = int.Parse(formCollection["ddlNumeracaoQuestao"]);
                temp.NumeracaoAlternativa = int.Parse(formCollection["ddlNumeracaoAlternativa"]);
                temp.QteSemestres = int.Parse(formCollection["txtQteSemestre"]);
                temp.TermoResponsabilidade = formCollection["txtTermoResponsabilidade"];
                temp.NotaUsoAcademica = formCollection["txtNotaUsoAcademica"];
                temp.NotaUsoCertificacao = formCollection["txtNotaUsoCertificacao"];
                temp.NotaUsoInstitucional = formCollection["txtNotaUsoInstitucional"];
                temp.NotaUsoReposicao = formCollection["txtNotaUsoReposicao"];
                temp.NotaUsoSimulado = formCollection["txtNotaUsoSimulado"];
                temp.ValorNotaMedia = double.Parse(formCollection["txtValorNotaMedia"].Replace('.', ','));
                Parametro.Atualizar(temp);
            }

            return null;
        }

        // POST: configuracoes/cadastrarprofessor
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.SUPERUSUARIO, Categoria.COLABORADOR }, Ocupacoes = new[] { Ocupacao.SUPERUSUARIO, Ocupacao.REITOR, Ocupacao.PRO_REITOR, Ocupacao.DIRETOR_GERAL, Ocupacao.DIRETOR, Ocupacao.COORDENADOR })]
        public ActionResult CadastrarProfessor(FormCollection formCollection)
        {
            if (formCollection.HasKeys())
            {
                string professorNome = formCollection["txtProfessorNome"];
                string professorMatricula = formCollection["txtProfessorMatricula"];

                int codPessoa = Pessoa.Inserir(new Pessoa() { TipoPessoa = Pessoa.FISICA });

                var pf = new PessoaFisica();
                pf.CodPessoa = codPessoa;
                pf.Nome = professorNome;
                pf.Categoria.Add(Categoria.ListarPorCodigo(Categoria.PROFESSOR));

                int codPessoaFisica = PessoaFisica.Inserir(pf);

                var usuario = new Usuario();
                usuario.Matricula = professorMatricula;
                usuario.CodPessoaFisica = codPessoaFisica;
                usuario.CodCategoria = Categoria.PROFESSOR;
                usuario.Senha = Criptografia.RetornarHash("senha");

                int codUsuario = Usuario.Inserir(usuario);

                var professor = new Professor();
                professor.MatrProfessor = professorMatricula;

                string[] disciplinas = formCollection["ddlProfessorDisciplinas"].Split(',');
                foreach (var disciplina in disciplinas)
                {
                    professor.Disciplina.Add(Disciplina.ListarPorCodigo(int.Parse(disciplina)));
                }

                Professor.Inserir(professor);
            }
            return RedirectToAction("Index");
        }

        // POST: configuracoes/cadastrarcolaborador
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.SUPERUSUARIO, Categoria.COLABORADOR }, Ocupacoes = new[] { Ocupacao.SUPERUSUARIO, Ocupacao.REITOR, Ocupacao.PRO_REITOR, Ocupacao.DIRETOR_GERAL, Ocupacao.DIRETOR, Ocupacao.COORDENADOR })]
        public ActionResult CadastrarColaborador(FormCollection formCollection)
        {
            if (formCollection.HasKeys())
            {
                var colaborador = new Colaborador();
                colaborador.Usuario = new Usuario();
                colaborador.Usuario.PessoaFisica = new PessoaFisica();
                colaborador.Usuario.PessoaFisica.Pessoa = new Pessoa();

                //Pessoa
                colaborador.Usuario.PessoaFisica.Pessoa.TipoPessoa = Pessoa.FISICA;

                //PessoaFisica
                colaborador.Usuario.PessoaFisica.Nome = formCollection["txtColaboradorNome"].Trim();
                colaborador.Usuario.PessoaFisica.Categoria.Add(Categoria.ListarPorCodigo(Categoria.COLABORADOR));

                //Usuario
                colaborador.Usuario.Categoria = Categoria.ListarPorCodigo(3);
                colaborador.Usuario.Matricula = formCollection["txtColaboradorMatricula"].Trim();
                colaborador.Usuario.Senha = Criptografia.RetornarHash("senha");
                colaborador.Usuario.DtCadastro = DateTime.Now;

                colaborador.MatrColaborador = formCollection["txtColaboradorMatricula"].Trim();

                Colaborador.Inserir(colaborador);
            }
            return RedirectToAction("Index");
        }

        // POST: configuracoes/cadastrarcampus
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.SUPERUSUARIO, Categoria.COLABORADOR }, Ocupacoes = new[] { Ocupacao.SUPERUSUARIO, Ocupacao.REITOR, Ocupacao.PRO_REITOR, Ocupacao.DIRETOR_GERAL, Ocupacao.DIRETOR, Ocupacao.COORDENADOR })]
        public ActionResult CadastrarCampus(FormCollection formCollection)
        {
            if (formCollection.HasKeys())
            {
                var campus = new Campus();
                campus.PessoaJuridica = new PessoaJuridica();
                campus.PessoaJuridica.Pessoa = new Pessoa();

                //Pessoa
                campus.PessoaJuridica.Pessoa.TipoPessoa = Pessoa.JURIDICA;

                //PessoaJuridica
                campus.PessoaJuridica.RazaoSocial = formCollection["txtCampusRazaoSocial"].Trim();
                campus.PessoaJuridica.NomeFantasia = formCollection["txtCampusNomeFantasia"].Trim();
                campus.PessoaJuridica.Portal = formCollection["txtCampusPortal"].Trim();

                //Campus
                campus.CodInstituicao = int.Parse(formCollection["ddlCampusInstituicao"]);
                campus.Colaborador = Colaborador.ListarPorCodigo(int.Parse(formCollection["ddlCampusDiretor"]));
                campus.Sigla = formCollection["txtCampusSigla"];

                Campus.Inserir(campus);
                PessoaFisica.AdicionarOcupacao(campus.Colaborador.Usuario.CodPessoaFisica, Ocupacao.DIRETOR_GERAL);
            }
            return RedirectToAction("Index");
        }

        // POST: configuracoes/cadastrardiretoria
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.SUPERUSUARIO, Categoria.COLABORADOR }, Ocupacoes = new[] { Ocupacao.SUPERUSUARIO, Ocupacao.REITOR, Ocupacao.PRO_REITOR, Ocupacao.DIRETOR_GERAL, Ocupacao.DIRETOR, Ocupacao.COORDENADOR })]
        public ActionResult CadastrarDiretoria(FormCollection formCollection)
        {
            if (formCollection.HasKeys())
            {
                var diretoria = new Diretoria();
                diretoria.PessoaJuridica = new PessoaJuridica();
                diretoria.PessoaJuridica.Pessoa = new Pessoa();

                //Pessoa
                diretoria.PessoaJuridica.Pessoa.TipoPessoa = Pessoa.JURIDICA;

                //PessoaJuridica
                diretoria.PessoaJuridica.RazaoSocial = formCollection["txtDiretoriaRazaoSocial"];
                diretoria.PessoaJuridica.NomeFantasia = formCollection["txtDiretoriaNomeFantasia"];
                diretoria.PessoaJuridica.Portal = formCollection["txtDiretoriaPortal"];

                //Diretoria
                string codCampus = formCollection["ddlDiretoriaCampus"];
                diretoria.Campus = Campus.ListarPorCodigo(codCampus);
                diretoria.CodColaboradorDiretor = int.Parse(formCollection["ddlDiretoriaDiretor"]);
                diretoria.Sigla = formCollection["txtDiretoriaSigla"];

                Diretoria.Inserir(diretoria);
            }
            return RedirectToAction("Index");
        }

        // POST: configuracoes/cadastrarcurso
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.SUPERUSUARIO, Categoria.COLABORADOR }, Ocupacoes = new[] { Ocupacao.SUPERUSUARIO, Ocupacao.REITOR, Ocupacao.PRO_REITOR, Ocupacao.DIRETOR_GERAL, Ocupacao.DIRETOR, Ocupacao.COORDENADOR })]
        public ActionResult CadastrarCurso(FormCollection formCollection)
        {
            if (formCollection.HasKeys())
            {
                var curso = new Curso();

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

        // POST: configuracoes/cadastrarturma
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.SUPERUSUARIO, Categoria.COLABORADOR }, Ocupacoes = new[] { Ocupacao.SUPERUSUARIO, Ocupacao.REITOR, Ocupacao.PRO_REITOR, Ocupacao.DIRETOR_GERAL, Ocupacao.DIRETOR, Ocupacao.COORDENADOR })]
        public ActionResult CadastrarTurma(FormCollection formCollection)
        {
            if (formCollection.HasKeys())
            {
                var turma = new Turma();

                //Turma
                turma.CodCurso = int.Parse(formCollection["ddlTurmaCurso"]);
                turma.Periodo = int.Parse(formCollection["txtTurmaPeriodo"]);
                turma.CodTurno = formCollection["ddlTurmaTurno"];
                turma.Nome = formCollection["txtTurmaNome"];

                Turma.Inserir(turma);
            }
            return RedirectToAction("Index");
        }

        // POST: configuracoes/cadastrardisciplina
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.SUPERUSUARIO, Categoria.COLABORADOR }, Ocupacoes = new[] { Ocupacao.SUPERUSUARIO, Ocupacao.REITOR, Ocupacao.PRO_REITOR, Ocupacao.DIRETOR_GERAL, Ocupacao.DIRETOR, Ocupacao.COORDENADOR })]
        public ActionResult CadastrarDisciplina(FormCollection formCollection)
        {
            if (formCollection.HasKeys())
            {
                string disciplinaNome = formCollection["txtDisciplina"];
                string disciplinaSigla = formCollection["txtSigla"];

                var disciplina = new Disciplina();
                disciplina.Descricao = disciplinaNome;
                disciplina.Sigla = disciplinaSigla;
                disciplina.FlagEletivaOptativa = (formCollection["chkEletivaOptativa"] != null) ? true : false;
                disciplina.FlagFlexivel = (formCollection["chkFlexivel"] != null) ? true : false;

                int codDisciplina = Disciplina.Inserir(disciplina);

                string[] temas = formCollection["txtTema"].Split(';');
                int i = 1;
                foreach (var item in temas)
                {
                    string tema = item.Trim();
                    if (!String.IsNullOrWhiteSpace(tema))
                    {
                        var t = new Tema();
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

        // POST: configuracoes/cadastrartema
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.SUPERUSUARIO, Categoria.COLABORADOR }, Ocupacoes = new[] { Ocupacao.SUPERUSUARIO, Ocupacao.REITOR, Ocupacao.PRO_REITOR, Ocupacao.DIRETOR_GERAL, Ocupacao.DIRETOR, Ocupacao.COORDENADOR })]
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
                foreach (var item in temas)
                {
                    string tema = item.Trim();
                    if (!String.IsNullOrWhiteSpace(tema))
                    {
                        var t = new Tema();
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

        // POST: configuracoes/cadastraraluno
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.SUPERUSUARIO, Categoria.COLABORADOR }, Ocupacoes = new[] { Ocupacao.SUPERUSUARIO, Ocupacao.REITOR, Ocupacao.PRO_REITOR, Ocupacao.DIRETOR_GERAL, Ocupacao.DIRETOR, Ocupacao.COORDENADOR })]
        public ActionResult CadastrarAluno(FormCollection formCollection)
        {
            if (formCollection.HasKeys())
            {
                var aluno = new Aluno();
                aluno.Usuario = new Usuario();
                aluno.Usuario.PessoaFisica = new PessoaFisica();
                aluno.Usuario.PessoaFisica.Pessoa = new Pessoa();

                //Pessoa
                aluno.Usuario.PessoaFisica.Pessoa.TipoPessoa = Pessoa.FISICA;

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

        // POST: configuracoes/cadastrarsala
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.SUPERUSUARIO, Categoria.COLABORADOR }, Ocupacoes = new[] { Ocupacao.SUPERUSUARIO, Ocupacao.REITOR, Ocupacao.PRO_REITOR, Ocupacao.DIRETOR_GERAL, Ocupacao.DIRETOR, Ocupacao.COORDENADOR })]
        public ActionResult CadastrarSala(FormCollection formCollection)
        {
            if (formCollection.HasKeys())
            {
                var sala = new Sala();

                //Campus
                string codCampus = formCollection["ddlSalaCampus"];
                //sala.Campus = Campus.ListarPorCodigo(codCampus);
                sala.Descricao = formCollection["txtSalaDescricao"];
                sala.Sigla = formCollection["txtSalaSigla"];

                Sala.Inserir(sala);
            }

            return RedirectToAction("Index");
        }

        // POST: configuracoes/cadastrarmatriz
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.SUPERUSUARIO, Categoria.COLABORADOR }, Ocupacoes = new[] { Ocupacao.SUPERUSUARIO, Ocupacao.REITOR, Ocupacao.PRO_REITOR, Ocupacao.DIRETOR_GERAL, Ocupacao.DIRETOR, Ocupacao.COORDENADOR })]
        public ActionResult CadastrarMatriz(FormCollection formCollection)
        {
            if (formCollection.HasKeys())
            {
                var matrizCurricular = new MatrizCurricular();

                int codCurso = int.Parse(formCollection["ddlMatrizCurso"]);
                matrizCurricular.CodCurso = codCurso;
                matrizCurricular.CodMatriz = MatrizCurricular.ObterCodMatriz(codCurso);

                int qteDisc = int.Parse(formCollection["matrizQte"]);

                for (int i = 1; i <= qteDisc; i++)
                {
                    matrizCurricular.MatrizCurricularDisciplina.Add(new MatrizCurricularDisciplina()
                    {
                        Periodo = int.Parse(formCollection["txtPeriodo" + i]),
                        CodDisciplina = int.Parse(formCollection["ddlDisciplina" + i]),
                        DiscCargaHoraria = int.Parse(formCollection["txtCH" + i])
                    });
                }

                MatrizCurricular.Inserir(matrizCurricular);
            }

            return RedirectToAction("Index");
        }

        // POST: configuracoes/cadastrarhorario
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.SUPERUSUARIO, Categoria.COLABORADOR }, Ocupacoes = new[] { Ocupacao.SUPERUSUARIO, Ocupacao.REITOR, Ocupacao.PRO_REITOR, Ocupacao.DIRETOR_GERAL, Ocupacao.DIRETOR, Ocupacao.COORDENADOR })]
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
                    var h = new Horario();

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