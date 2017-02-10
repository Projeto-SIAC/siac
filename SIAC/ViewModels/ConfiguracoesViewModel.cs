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