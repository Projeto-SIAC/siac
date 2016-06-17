using SIAC.Models;
using System.Collections.Generic;

namespace SIAC.ViewModels
{
    public class ConfiguracoesParametrosViewModel
    {
        public Parametro Parametro { get; set; }
        public List<Disciplina> Disciplinas { get; set; }
        public List<Professor> Professores { get; set; }
        public List<Tema> Temas { get; set; }
        public List<Aluno> Alunos { get; set; }
        public List<Colaborador> Colaboradores { get; set; }
        public List<Campus> Campi { get; set; }
        public List<Instituicao> Instituicoes { get; set; }
        public List<Diretoria> Diretorias { get; set; }
        public List<Curso> Cursos { get; set; }
        public List<NivelEnsino> NiveisEnsino { get; set; }
        public List<Turma> Turmas { get; set; }
        public List<Turno> Turnos { get; set; }
        public List<Sala> Salas { get; set; }
        public List<MatrizCurricular> Matrizes { get; set; }
        public List<Horario> Horarios { get; set; }
    }

    public class ConfiguracoesInstitucionalViewModel
    {
        public List<Ocupacao> Ocupacoes { get; set; }
        public List<PessoaFisica> Coordenadores { get; set; }
    }
}